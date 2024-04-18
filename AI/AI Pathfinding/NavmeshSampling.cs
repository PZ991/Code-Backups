using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavmeshSampling : MonoBehaviour
{
    public Transform agent;
    public Transform target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        NavMeshHit hit = new NavMeshHit();
        NavMesh.FindClosestEdge(agent.position, out hit, NavMesh.AllAreas);
        //NavMesh.SamplePosition(agent.position, out hit,10, NavMesh.AllAreas);       //where unit is standing
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hit.position, 0.5f);
    }
}
