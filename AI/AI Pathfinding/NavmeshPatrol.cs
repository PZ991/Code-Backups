using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavmeshPatrol : MonoBehaviour
{
    bool patrolwaiting;
    public float totalwaittime;
    public float switchprobability;
    public List<Transform> waypoint;

    public NavMeshAgent agent;
    int patrolindex;
    bool traveling;
    bool waiting;
    bool patrolforward;
    float waittimer;
    public Transform destination;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(traveling && agent.remainingDistance<=0.1f)
        {
            traveling = false;
            if(patrolwaiting)
            {
                waiting = true;
                waittimer = 0f;
            }
            else
            {
                ChangeIndex();
                SetDistination();

            }
        }
        if(waiting)
        {
            waittimer += Time.deltaTime;
            if(waittimer>=totalwaittime)
            {
                waiting = false;
                ChangeIndex();
                SetDistination();
            }
        }
    }
    public void ChangeIndex() 
    {
        if(Random.Range(0f,1f)<=switchprobability)
        {
            patrolforward =! patrolforward;
        }
        if (patrolforward)
        {
            /*
              

            patrolindex++;
            if(curretpatrolindex>= waypoint.Count)
            {
            patrolindex=0;
            }
             */
             
            patrolindex = (patrolindex + 1) % waypoint.Count;
        }
        else
        {
            /*
             patrolindex--;
            if(curretpatrolindex<0)
            {
            curretpatrolindex=waypoint.Cout-1;
            }
             */
            if (--patrolindex<0)
            {
                patrolindex = waypoint.Count - 1;
            }
        }
    }
    public void SetDistination()
    {
        if(waypoint!=null)
        {
            Vector3 targetvector = waypoint[patrolindex].transform.position;
            agent.SetDestination(targetvector);
            traveling = true;
        
        }
    }
}
