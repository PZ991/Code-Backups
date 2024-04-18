using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class BooleanHandler : MonoBehaviour
{
    public BooleanOperatorV3 targetObject;
    //public List<BooleanOperatorV3> booleans;
    public BooleanOperatorV3 booleans;
    public int displayVertex;
    public int displayVertex1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDrawGizmos()
    {

        foreach (var item2 in targetObject.availableConnections)
        {
            Mesh mesh2 = Utility.GetMesh(targetObject.gameObject);
            Vector3[] vertices2 = mesh2.vertices;
            Transform transform = targetObject.transform;
            foreach (var item in targetObject.availableConnections[item2.Key].Item2)
            {
                RaycastHit[] hit = Physics.RaycastAll(transform.TransformPoint(item2.Key), transform.TransformPoint(item) - transform.TransformPoint(item2.Key), Vector3.Distance(transform.TransformPoint(item), transform.TransformPoint(item2.Key)));
                RaycastHit[] hit2 = Physics.RaycastAll(transform.TransformPoint(item), transform.TransformPoint(item2.Key) - transform.TransformPoint(item), Vector3.Distance(transform.TransformPoint(item), transform.TransformPoint(item2.Key)));

                HashSet<RaycastHit> data = new HashSet<RaycastHit>();

                data.UnionWith(hit);
                data.UnionWith(hit2);

                List<RaycastHit> sortedData = data
                    .OrderBy(point =>
                        Mathf.InverseLerp(0, 1, Vector3.Distance(transform.TransformPoint(item2.Key), point.point) /
                                                 Vector3.Distance(transform.TransformPoint(item2.Key), transform.TransformPoint(item))))
                    .ToList();

                List<Vector3> sortedData2 = sortedData.Select(item => item.point).ToList();
                Gizmos.color = Color.white;

                foreach (var item3 in sortedData)
                {
                    Gizmos.DrawSphere(item3.point, 0.01f);
                    Gizmos.DrawRay(item3.point, item3.normal);
                    Debug.Log(item3.triangleIndex);
                }

                if (hit.Length > 0 && hit2.Length > 0)
                {
                    sortedData2.Insert(0, transform.TransformPoint(item2.Key));

                    sortedData2.Insert(sortedData.Count - 1, transform.TransformPoint(item));
                    bool inside = false;
                    int insides = 0;
                    for (int i = 0; i < sortedData2.Count - 1; i++)
                    {

                        //Debug.Log(plane.GetSide(vertices2[displayVertex1]));
                        // Plane plane = new Plane(sortedData[i].normal, sortedData2[i + 1]);

                        if (i > 0 && i < sortedData.Count - 1)
                        {

                            Plane plane = new Plane(sortedData[i - 1].normal, sortedData[i - 1].point);
                            Plane plane2 = new Plane(sortedData[i].normal, sortedData[i].point);

                            bool inside1 = !plane2.GetSide(sortedData2[i]);
                            bool inside2 = !plane.GetSide(sortedData2[i + 1]);
                            if (!inside1)
                                insides++;
                            else
                                insides--;
                            if (!inside2)
                                insides++;
                            else
                                insides--;
                            insides = Mathf.Clamp(insides, 0, 9999);
                            inside = insides == 0;
                        }
                        else if (i == 0)
                        {
                            Plane plane = new Plane(sortedData[i].normal, sortedData[i].point);

                            inside = !plane.GetSide(sortedData2[i]);
                        }
                        else if (i >= sortedData.Count - 1)
                        {
                            Plane plane = new Plane(sortedData[sortedData.Count - 1].normal, sortedData[sortedData.Count - 1].point);

                            inside = !plane.GetSide(sortedData2[i]);
                        }
                        if (inside)
                            Gizmos.color = Color.red;
                        else
                            Gizmos.color = Color.green;

                        Gizmos.DrawLine(sortedData2[i], sortedData2[i + 1]);



                    }
                }
                else if (hit.Length == 1)
                {

                    Gizmos.color = Color.red;

                    Gizmos.DrawLine(transform.TransformPoint(item), hit[0].point);
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(hit[0].point, transform.TransformPoint(item2.Key));
                }
                else if (hit2.Length == 1)
                {
                    Gizmos.color = Color.green;

                    Gizmos.DrawLine(transform.TransformPoint(item), hit2[0].point);
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(hit2[0].point, transform.TransformPoint(item2.Key));

                }
                else
                {
                    if (targetObject.availableConnections[item].Item1 == 0 && targetObject.availableConnections[item2.Key].Item1 == 0)
                    {
                        Gizmos.color = Color.green;

                        Gizmos.DrawLine(transform.TransformPoint(item), transform.TransformPoint(item2.Key));
                    }
                    else
                    {
                        Gizmos.color = Color.red;

                        Gizmos.DrawLine(transform.TransformPoint(item), transform.TransformPoint(item2.Key));
                    }

                }

            }
        }
        foreach (var item2 in booleans.availableConnections)

        {
            Mesh mesh2 = Utility.GetMesh(booleans.gameObject);
            Vector3[] vertices2 = mesh2.vertices;
            Transform transform = booleans.transform;
            foreach (var item in booleans.availableConnections[item2.Key].Item2)
            {
                RaycastHit[] hit = Physics.RaycastAll(transform.TransformPoint(item2.Key), transform.TransformPoint(item) - transform.TransformPoint(item2.Key), Vector3.Distance(transform.TransformPoint(item), transform.TransformPoint(item2.Key)));
                RaycastHit[] hit2 = Physics.RaycastAll(transform.TransformPoint(item), transform.TransformPoint(item2.Key) - transform.TransformPoint(item), Vector3.Distance(transform.TransformPoint(item), transform.TransformPoint(item2.Key)));

                HashSet<RaycastHit> data = new HashSet<RaycastHit>();

                data.UnionWith(hit);
                data.UnionWith(hit2);

                List<RaycastHit> sortedData = data
                    .OrderBy(point =>
                        Mathf.InverseLerp(0, 1, Vector3.Distance(transform.TransformPoint(item2.Key), point.point) /
                                                 Vector3.Distance(transform.TransformPoint(item2.Key), transform.TransformPoint(item))))
                    .ToList();

                List<Vector3> sortedData2 = sortedData.Select(item => item.point).ToList();
                Gizmos.color = Color.white;

                foreach (var item3 in sortedData)
                {
                    Gizmos.DrawSphere(item3.point, 0.01f);
                    Gizmos.DrawRay(item3.point, item3.normal);
                    Debug.Log(item3.triangleIndex);
                }

                if (hit.Length > 0 && hit2.Length > 0)
                {
                    sortedData2.Insert(0, transform.TransformPoint(item2.Key));

                    sortedData2.Insert(sortedData.Count - 1, transform.TransformPoint(item));
                    bool inside = false;
                    int insides = 0;
                    for (int i = 0; i < sortedData2.Count - 1; i++)
                    {

                        //Debug.Log(plane.GetSide(vertices2[displayVertex1]));
                        // Plane plane = new Plane(sortedData[i].normal, sortedData2[i + 1]);

                        if (i > 0 && i < sortedData.Count - 1)
                        {

                            Plane plane = new Plane(sortedData[i - 1].normal, sortedData[i - 1].point);
                            Plane plane2 = new Plane(sortedData[i].normal, sortedData[i].point);

                            bool inside1 = !plane2.GetSide(sortedData2[i]);
                            bool inside2 = !plane.GetSide(sortedData2[i + 1]);
                            if (!inside1)
                                insides++;
                            else
                                insides--;
                            if (!inside2)
                                insides++;
                            else
                                insides--;
                            insides = Mathf.Clamp(insides, 0, 9999);
                            inside = insides == 0;
                        }
                        else if (i == 0)
                        {
                            Plane plane = new Plane(sortedData[i].normal, sortedData[i].point);

                            inside = !plane.GetSide(sortedData2[i]);
                        }
                        else if (i >= sortedData.Count - 1)
                        {
                            Plane plane = new Plane(sortedData[sortedData.Count - 1].normal, sortedData[sortedData.Count - 1].point);

                            inside = !plane.GetSide(sortedData2[i]);
                        }
                        if (inside)
                            Gizmos.color = Color.red;
                        else
                            Gizmos.color = Color.green;

                        Gizmos.DrawLine(sortedData2[i], sortedData2[i + 1]);



                    }
                }
                else if (hit.Length == 1)
                {

                    Gizmos.color = Color.red;

                    Gizmos.DrawLine(transform.TransformPoint(item), hit[0].point);
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(hit[0].point, transform.TransformPoint(item2.Key));
                }
                else if (hit2.Length == 1)
                {
                    Gizmos.color = Color.green;

                    Gizmos.DrawLine(transform.TransformPoint(item), hit2[0].point);
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(hit2[0].point, transform.TransformPoint(item2.Key));
                }
                else
                {
                    Gizmos.color = Color.green;

                    Gizmos.DrawLine(transform.TransformPoint(item), transform.TransformPoint(item2.Key));


                }

            }
        }

    }
}
