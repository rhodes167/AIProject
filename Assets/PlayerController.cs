
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{


    public NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
            if(Physics.Raycast(ray, out hit))
            {
               agent.SetDestination(hit.point);
            }
        }
    }
}
