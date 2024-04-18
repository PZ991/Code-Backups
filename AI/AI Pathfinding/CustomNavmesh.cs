using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CustomNavmesh : MonoBehaviour
{
    private Transform unit;
    private Transform target;
    public NavMeshAgent agent;
    public bool process;
    public NavMeshSurface surface;
    public int showvertex;

    public bool Reload;
    public bool changed;
    public bool finished;
    public bool processing;
    public List<List<Vector3>> PositionSet = new List<List<Vector3>>();
    int currentLayer = 3;

    public List<LineRenderer> rends;
    public Material red;
    public Material green;

    public List<NavmeshAgentAI> agents;
    public List<NavmeshInfo> infos;
    // Start is called before the first frame update
    void Start()
    {
        surface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        if (agents.Count > 0)
        {
            if (agents[0].targets != null)
            {
                if (processing && !finished)
                {
                    //get the path
                    NavMeshPath path = new NavMeshPath();
                    NavMesh.CalculatePath(agents[0].transform.position, agents[0].targets.position, NavMesh.AllAreas, path);
                    List<Vector3> tempos = new List<Vector3>();
                    List<Vector3> tempos2 = new List<Vector3>(path.corners);

                    //check if current path is already in the PositionSet
                    if (!CheckMatch(PositionSet, tempos2))
                    {
                        //each position of the corner on the navmesh
                        foreach (var item in path.corners)
                        {
                            //Debug.Log(item);
                            tempos.Add(item);       

                            //get all collider near the point
                            NavMeshHit hit;
                            Collider[] cols = new Collider[1]; 
                            if (NavMesh.SamplePosition(item, out hit, 1, NavMesh.AllAreas))
                            {
                                //Debug.DrawRay(hit.position, Vector3.up,Color.black,100);
                                //Gizmos.DrawSphere(hit.position, 1f);
                                cols = Physics.OverlapSphere(hit.position, .1f);
                               // Debug.Log("");

                               //check if set and set the weight
                                if (cols.Length > 0)
                                {
                                    //Debug.Log("HIT");
                                    if (cols[0].GetComponent<NavmeshInfo>() != null)
                                    {
                                        if (cols[0].GetComponent<NavmeshInfo>().isSet == false)
                                        {
                                            //Debug.Log(cols[0].name);
                                            //cols[0].GetComponent<NavMeshModifier>().area = currentLayer;
                                            cols[0].GetComponent<NavmeshInfo>().Calculate(agents[0].weights, currentLayer);
                                            currentLayer += 1;
                                        }
                                        // cols[0].GetComponent<NavmeshInfo>().Weight;
                                        //calculate personal weight
                                    }
                                }
                            }

                        }
                        Reload = true;

                        PositionSet.Add(tempos);
                        processing = true;
                    }
                    else                                            //if the list of vectors is the same as already existed in the list
                    {
                        finished = true;
                        Reload = false;
                        processing = false;
                        agents[0].path = path;
                        agents[0].changedestination = false;
                        agents.RemoveAt(0);
                    }


                }
                
                if (Reload)
                {
                    surface.BuildNavMesh();
                    Reload = false;
                    processing = true;
                   // Debug.Log("reload");
                }
            }
            else 
            {
                agents.RemoveAt(0);
            }
        }
        else
        {
            finished = true;
            Reload = false;
            processing = false;
        }

        // if processing is done
        if(finished==true)
        {
            foreach (var item in infos)
            {
                infos.Clear();
            }
            infos.Clear();
            surface.BuildNavMesh();
        }
    }

    public async void ReturnPath()
    {

    }
    private void OnDrawGizmos()
    {
        if (agents.Count > 0)
        {
            if (agents[0].targets != null)
            {
                unit = agents[0].transform;
                target = agents[0].targets;
                NavMeshPath path = new NavMeshPath();
                NavMesh.CalculatePath(unit.position, target.position, NavMesh.AllAreas, path);
                List<Vector3> tempos2 = new List<Vector3>(path.corners);
                if (rends.Count != PositionSet.Count)
                {
                    foreach (var item in rends)
                    {
                        Destroy(item.gameObject);

                    }
                    rends.Clear();
                }
                if (PositionSet.Count > 0)
                {
                    foreach (var item in PositionSet)
                    {
                        if (rends.Count != PositionSet.Count)
                        {
                            LineRenderer line = new GameObject().AddComponent<LineRenderer>() as LineRenderer;
                            line.positionCount = item.Count;
                            for (int i = 0; i < item.Count; i++)
                            {
                                line.SetPosition(i, item[i]);

                            }



                            rends.Add(line);
                        }
                        else
                        {
                            foreach (var item2 in item)
                            {
                                if (tempos2.Contains(item2))
                                    Gizmos.color = Color.green;
                                else
                                    Gizmos.color = Color.red;
                                Gizmos.DrawSphere(item2, 0.5f);
                            }
                        }
                    }
                }
                for (int i = 0; i < PositionSet.Count; i++)
                {
                    List<Vector3> pos = new List<Vector3>();
                    for (int k = 0; k < rends[i].positionCount; k++)
                    {
                        pos.Add(rends[i].GetPosition(i));
                    }
                    if (CheckMatch(tempos2, pos))
                    {
                        rends[i].material = green;

                    }
                    else
                    {
                        rends[i].material = red;
                    }
                }
            }
        }






        //// NavMeshTriangulation navMeshTriangulation = NavMesh.CalculateTriangulation();
        /*
        foreach (var item in navMeshTriangulation.vertices)
        {

            Gizmos.color = Color.black;
            Gizmos.DrawSphere(item, 0.5f);

        }
        */
        //Debug.Log(navMeshTriangulation.vertices.Length-1);

        /*
        for (int i = 0; i < navMeshTriangulation.vertices.Length; i++)
        
        {
            if(i==showvertex)
            {
                Gizmos.color = Color.grey;

                Gizmos.DrawRay(navMeshTriangulation.vertices[i], Vector3.up * 10);

            }
            else
            Gizmos.color = Color.black;

            Gizmos.DrawSphere(navMeshTriangulation.vertices[i], 0.5f);

        }
        */


        //Debug.Log(NavMesh.GetAreaCost(0));
        //path.corners = new Vector3[];
        /*
        if (process)
        {
            NavMeshPath path = new NavMeshPath() ;
            NavMesh.CalculatePath(unit.position, target.position, NavMesh.AllAreas, path);
            foreach (var item in path.corners)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(item, .5f);


                NavMeshHit hit;
                Collider[] cols = new Collider[1];
                if (NavMesh.SamplePosition(item, out hit, 1, NavMesh.AllAreas))
                {
                    //Gizmos.DrawSphere(hit.position, 1f);
                    cols = Physics.OverlapSphere(hit.position, .5f);
                    if (cols.Length > 0)
                    {
                        if (cols[0].GetComponent<NavMeshSurface>() != null)
                        {
                            Debug.Log(cols[0].GetComponent<NavMeshSurface>().defaultArea);
                            Debug.Log(cols[0].name);
                            if (cols[0].name=="Plane.007")
                            {

                            }
                        }
                    }
                }

            }
            //agent.SetPath(path);
        }
        */

    }

    //Check if list is the same
    bool CheckMatch(List<List<Vector3>> l0, List<Vector3> l2)
    {
        //checks if the list of list contains the list specified
        
        bool probable = false;
        for (int k = 0; k < l0.Count; k++)
        {
            if (l0[k].Count != l2.Count)
            {
                probable = false;
                continue;
            }
            //l0[k].Sort(); l2.Sort();
            for (int i = 0; i < l0[k].Count; i++)
            {
                if (!l0[k].Contains(l2[k]))
                {
                    probable = false;
                    break;
                }
                else
                    probable = true;
            }
        }
        //Debug.Log(probable);
        return probable;
    }
    bool CheckMatch(List<Vector3> l1,List<Vector3> l2)
    {
        //checks if both list is the same
        if (l1.Count != l2.Count)
            return false;
        for (int i = 0; i < l1.Count; i++)
        {
            if (!l1.Contains( l2[i]))
                return false;
        }
        return true;
    }
}

