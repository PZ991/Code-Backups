using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NavmeshAgentAI : MonoBehaviour
{
    public List<PrefData> weights= new List<PrefData>();
    public NavMeshPath path;
    public CustomNavmesh manager;
    public Transform targets;
    public bool changedestination;
    public bool localdestination;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(changedestination)
        {
            if(!localdestination)
            {

            }
        }
    }
}
[System.Serializable]
public struct PrefData
{
    public string name;
    public float weight;
}
