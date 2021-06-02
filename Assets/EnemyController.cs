using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum State //possible enemy states
    {
        Patrol,
        Idle,
        Chase
    }

    public State state;

    public bool patrolTargetNorth; //current patrol destination for enemy
    public NavMeshAgent enemy;
	public float timer; //for waiting idle at patrol nodes
	public float waitTime;
	public float playerDistance; //distance between player and enemy
	public float targetDistance; //distance between enemy and patrol node destination
	public float alertRange = 3; //"vision" distance - range at which player is detected
	public float escapeRange = 5; //range at which player can escape
	public Material calm; //enemy colour changes
	public Material alert;


	// Start is called before the first frame update
	void Start()
    {
        state = State.Patrol;
		patrolTargetNorth = false; //set patrol target to the bottom one
		timer = 0;
		waitTime = 1.5f;
    }

	// Update is called once per frame
	private void Update()
    {

		playerDistance = Vector3.Distance(GameObject.Find("Player").transform.position, gameObject.transform.position); //find distance between player and enemy
		//print("distance to player:" + playerDistance);


		switch (state)
        {
            case State.Idle: //when in "Idle" state

				enemy.SetDestination(gameObject.transform.position); //set navigation destination to current location - stop moving
				if(timer < waitTime) //if not completed waiting
				{
					timer += Time.deltaTime; //count up wait time
					if(playerDistance < alertRange) //if player is close enough
					{
						state = State.Chase; //chase player
						GetComponent<Renderer>().material = alert; //change to alert colour - red
					}
				}
				else
				{
					patrolTargetNorth = !patrolTargetNorth; //change patrol target destination
					state = State.Patrol; //resume patrol
					timer = 0; //reset timer
				}
				
				break;


            case State.Patrol: //when in "Patrol" state

				if (patrolTargetNorth) //if next destination is the top one
				{
					enemy.SetDestination(GameObject.Find("NorthBase").transform.position); //set navigation destination
					targetDistance = Vector3.Distance(GameObject.Find("NorthBase").transform.position, gameObject.transform.position); //update distance to destination
					//print("distance to dest:" + targetDistance);
					if (targetDistance < 0.68) //if in range - on top of target
					{
						state = State.Idle; //return to Idle state
						//print("IDLE TRIGGER Distance");
					}
				}
                else
                {
					enemy.SetDestination(GameObject.Find("SouthBase").transform.position);
					targetDistance = Vector3.Distance(GameObject.Find("SouthBase").transform.position, gameObject.transform.position);
					//print("distance to dest:" + targetDistance);
					if (targetDistance < 0.68)
					{
						state = State.Idle;
						//print("IDLE TRIGGER Distance");
					}
				}

				if (playerDistance < alertRange) //if player is close enough
				{
					state = State.Chase; //start chase
					GetComponent<Renderer>().material = alert; //change to alert colour - red
				}

				break;


            case State.Chase: //when in "Chase" state

				enemy.SetDestination(GameObject.Find("Player").transform.position); //set player location to target destination

				if (playerDistance > escapeRange) //if player is far enough away
				{
					state = State.Patrol; //resume patrol
					GetComponent<Renderer>().material = calm; //revert to calm colour - white/grey
				}

				break;
        }
    }
}
