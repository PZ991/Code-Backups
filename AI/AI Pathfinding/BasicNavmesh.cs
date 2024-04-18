using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BasicNavmesh : MonoBehaviour
{
    public Transform destination;
    public NavMeshAgent agent;
    public bool change;
    // Start is called before the first frame update
    void Start()
    {
        SetDestination();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetDestination()
    {
        if(destination!=null)
        {
            Vector3 targetvector = destination.transform.position;

            if (change)
            {
                agent.SetAreaCost(4, 10);
                Debug.Log(this.name + agent.GetAreaCost(4));
            }
            Debug.Log(this.name+agent.GetAreaCost(4));

            agent.SetDestination(targetvector);
        }
    }
}
