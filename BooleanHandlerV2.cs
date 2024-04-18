using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class BooleanHandlerV2 : MonoBehaviour
{
    // Start is called before the first frame update
    public BooleanOperatorV4 targetObject;
    //public List<BooleanOperatorV3> booleans;
    public BooleanOperatorV4 booleans;
    public bool process;
    public List<TriangleData> triangles = new List<TriangleData>();
    public List<Vector3> forbiddenpoints = new List<Vector3>();

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDrawGizmos()
    {

        var keys = new List<Vector3>(targetObject.availableConnections.Keys);

        foreach (var key in keys)
        {
            targetObject.availableConnections[key] = new(0, targetObject.availableConnections[key].Item2);
        }
        foreach (var item in targetObject.triangleDatas)
        {
            item.normalpoints.Clear();
            item.unpassablepoints.Clear();
        }

        foreach (var item2 in targetObject.triangleDatas)
        {
            Mesh mesh2 = Utility.GetMesh(targetObject.gameObject);
            Vector3[] vertices2 = mesh2.vertices;
            Transform transform = targetObject.transform;
            foreach (var item3 in item2.Edges)
            {

                RaycastHit[] hit = Physics.RaycastAll(transform.TransformPoint(item3.points[0]), transform.TransformPoint(item3.points[1]) - transform.TransformPoint(item3.points[0]), Vector3.Distance(transform.TransformPoint(item3.points[0]), transform.TransformPoint(item3.points[1])));
                RaycastHit[] hit2 = Physics.RaycastAll(transform.TransformPoint(item3.points[1]), transform.TransformPoint(item3.points[0]) - transform.TransformPoint(item3.points[1]), Vector3.Distance(transform.TransformPoint(item3.points[1]), transform.TransformPoint(item3.points[0])));

                HashSet<RaycastHit> data = new HashSet<RaycastHit>();

                data.UnionWith(hit);
                data.UnionWith(hit2);

                List<RaycastHit> sortedData = data
                    .OrderBy(point =>
                        Mathf.InverseLerp(0, 1, Vector3.Distance(transform.TransformPoint(item3.points[1]), point.point) /
                                                 Vector3.Distance(transform.TransformPoint(item3.points[0]), transform.TransformPoint(item3.points[1]))))
                    .ToList();

                List<Vector3> sortedData2 = sortedData.Select(item => item.point).ToList();
                Gizmos.color = Color.white;

                /*
                foreach (var item4 in sortedData)
                {
                    Gizmos.DrawSphere(item4.point, 0.01f);
                    // Gizmos.DrawRay(item4.point, item4.normal);
                    Debug.Log(item4.triangleIndex);
                }
                */
                if (hit.Length > 0 && hit2.Length > 0)
                {
                    sortedData2.Insert(0, transform.TransformPoint(item3.points[1]));

                    sortedData2.Insert(sortedData.Count - 1, transform.TransformPoint(item3.points[0]));
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
                        {
                            Gizmos.color = Color.red;
                            if (item2.normalpoints.Contains(sortedData[i].point) && !item2.unpassablepoints.Contains(sortedData[i].point))
                            {
                                item2.normalpoints.Remove(sortedData[i].point);

                            }
                            else if (!item2.unpassablepoints.Contains(sortedData[i].point))
                            {
                                item2.unpassablepoints.Add(sortedData[i].point);

                            }
                        }
                        else
                        {
                            Gizmos.color = Color.green;

                            if (!item2.normalpoints.Contains(sortedData[i].point) && !item2.unpassablepoints.Contains(sortedData[i].point))
                                item2.normalpoints.Add(sortedData[i].point);
                        }

                        //Gizmos.DrawLine(sortedData2[i], sortedData2[i + 1]);



                    }
                }
                else if (hit.Length == 1)
                {

                    Gizmos.color = Color.red;
                    if (!item2.normalpoints.Contains(transform.TransformPoint(item3.points[0])) && !item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[0])))
                        item2.normalpoints.Add(transform.TransformPoint(item3.points[0]));
                    if (item2.normalpoints.Contains(transform.TransformPoint(item3.points[1])) && !item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[1])))
                    {
                        item2.normalpoints.Remove(transform.TransformPoint(item3.points[1]));

                    }
                    else if (!item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[1])))
                    {
                        item2.unpassablepoints.Add(transform.TransformPoint(item3.points[1]));

                    }
                    if (item2.normalpoints.Contains(hit[0].point) && !item2.unpassablepoints.Contains(hit[0].point))
                    {
                        item2.normalpoints.Remove(hit[0].point);

                    }
                    else if (!item2.unpassablepoints.Contains(hit[0].point))
                    {
                        item2.unpassablepoints.Add(hit[0].point);

                    }
                    /*
                    Gizmos.DrawLine(transform.TransformPoint(item3.points[1]), hit[0].point);
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(hit[0].point, transform.TransformPoint(item3.points[0]));
                    */
                    targetObject.availableConnections[item3.points[1]] = new(targetObject.availableConnections[item3.points[1]].Item1 + 1, targetObject.availableConnections[item3.points[1]].Item2);
                }
                else if (hit2.Length == 1)
                {
                    Gizmos.color = Color.green;
                    if (!item2.normalpoints.Contains(transform.TransformPoint(item3.points[1])) && !item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[1])))
                        item2.normalpoints.Add(transform.TransformPoint(item3.points[1]));
                    if (item2.normalpoints.Contains(transform.TransformPoint(item3.points[0])) && !item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[0])))
                    {
                        item2.normalpoints.Remove(transform.TransformPoint(item3.points[0]));

                    }
                    else if (!item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[0])))
                    {
                        item2.unpassablepoints.Add(transform.TransformPoint(item3.points[0]));

                    }
                    if (item2.normalpoints.Contains(hit2[0].point) && !item2.unpassablepoints.Contains(hit2[0].point))
                    {
                        item2.normalpoints.Remove(hit2[0].point);

                    }
                    else if (!item2.unpassablepoints.Contains(hit2[0].point))
                    {
                        item2.unpassablepoints.Add(hit2[0].point);

                    }
                    /*
                    Gizmos.DrawLine(transform.TransformPoint(item3.points[1]), hit2[0].point);
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(hit2[0].point, transform.TransformPoint(item3.points[0]));
                    */
                    targetObject.availableConnections[item3.points[0]] = new(targetObject.availableConnections[item3.points[0]].Item1 + 1, targetObject.availableConnections[item3.points[1]].Item2);
                    //targetObject.availableVertices[item3.points[0]] = new(targetObject.availableVertices[item3.points[0]].Item1, targetObject.availableVertices[item3.points[0]].Item2 + 1);

                }
                else
                {
                    bool zero = targetObject.availableConnections[item3.points[0]].Item1 > 0;
                    bool one = targetObject.availableConnections[item3.points[1]].Item1 > 0;
                    if (!(zero && one))
                    {
                        Gizmos.color = Color.green;
                        if (!item2.normalpoints.Contains(transform.TransformPoint(item3.points[0])) && !item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[0])))
                            item2.normalpoints.Add(transform.TransformPoint(item3.points[0]));
                        if (!item2.normalpoints.Contains(transform.TransformPoint(item3.points[1])) && !item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[1])))
                            item2.normalpoints.Add(transform.TransformPoint(item3.points[1]));
                    }
                    else
                    {

                        if (item2.normalpoints.Contains(transform.TransformPoint(item3.points[0])) && !item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[0])))
                        {
                            item2.normalpoints.Remove(transform.TransformPoint(item3.points[0]));

                        }
                        else if (!item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[0])))
                        {
                            item2.unpassablepoints.Add(transform.TransformPoint(item3.points[0]));

                        }
                        if (item2.normalpoints.Contains(transform.TransformPoint(item3.points[1])) && !item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[1])))
                        {
                            item2.normalpoints.Remove(transform.TransformPoint(item3.points[1]));

                        }
                        else if (!item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[1])))
                        {
                            item2.unpassablepoints.Add(transform.TransformPoint(item3.points[1]));

                        }
                        Gizmos.color = Color.red;

                    }
                    // Gizmos.DrawLine(transform.TransformPoint(item3.points[1]), transform.TransformPoint(item3.points[0]));

                }

            }
        }
        foreach (var item2 in booleans.triangleDatas)
        {
            Mesh mesh2 = Utility.GetMesh(booleans.gameObject);
            Vector3[] vertices2 = mesh2.vertices;
            Transform transform = booleans.transform;
            foreach (var item3 in item2.Edges)
            {

                RaycastHit[] hit = Physics.RaycastAll(transform.TransformPoint(item3.points[0]), transform.TransformPoint(item3.points[1]) - transform.TransformPoint(item3.points[0]), Vector3.Distance(transform.TransformPoint(item3.points[0]), transform.TransformPoint(item3.points[1])));
                RaycastHit[] hit2 = Physics.RaycastAll(transform.TransformPoint(item3.points[1]), transform.TransformPoint(item3.points[0]) - transform.TransformPoint(item3.points[1]), Vector3.Distance(transform.TransformPoint(item3.points[1]), transform.TransformPoint(item3.points[0])));

                HashSet<RaycastHit> data = new HashSet<RaycastHit>();

                data.UnionWith(hit);
                data.UnionWith(hit2);

                List<RaycastHit> sortedData = data
                    .OrderBy(point =>
                        Mathf.InverseLerp(0, 1, Vector3.Distance(transform.TransformPoint(item3.points[1]), point.point) /
                                                 Vector3.Distance(transform.TransformPoint(item3.points[0]), transform.TransformPoint(item3.points[1]))))
                    .ToList();

                List<Vector3> sortedData2 = sortedData.Select(item => item.point).ToList();
                Gizmos.color = Color.white;

                /*
                foreach (var item4 in sortedData)
                {
                    Gizmos.DrawSphere(item4.point, 0.01f);
                    // Gizmos.DrawRay(item4.point, item4.normal);
                    Debug.Log(item4.triangleIndex);
                }
                */
                if (hit.Length > 0 && hit2.Length > 0)
                {
                    sortedData2.Insert(0, transform.TransformPoint(item3.points[1]));

                    sortedData2.Insert(sortedData.Count - 1, transform.TransformPoint(item3.points[0]));
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
                        {
                            Gizmos.color = Color.red;
                            if (item2.normalpoints.Contains(sortedData[i].point) && !item2.unpassablepoints.Contains(sortedData[i].point))
                            {
                                item2.normalpoints.Remove(sortedData[i].point);

                            }
                            else if (!item2.unpassablepoints.Contains(sortedData[i].point))
                            {
                                item2.unpassablepoints.Add(sortedData[i].point);

                            }
                        }
                        else
                        {
                            Gizmos.color = Color.green;

                            if (!item2.normalpoints.Contains(sortedData[i].point) && !item2.unpassablepoints.Contains(sortedData[i].point))
                                item2.normalpoints.Add(sortedData[i].point);
                        }

                        //Gizmos.DrawLine(sortedData2[i], sortedData2[i + 1]);



                    }
                }
                else if (hit.Length == 1)
                {

                    Gizmos.color = Color.red;
                    if (!item2.normalpoints.Contains(transform.TransformPoint(item3.points[0])) && !item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[0])))
                        item2.normalpoints.Add(transform.TransformPoint(item3.points[0]));
                    if (item2.normalpoints.Contains(transform.TransformPoint(item3.points[1])) && !item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[1])))
                    {
                        item2.normalpoints.Remove(transform.TransformPoint(item3.points[1]));

                    }
                    else if (!item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[1])))
                    {
                        item2.unpassablepoints.Add(transform.TransformPoint(item3.points[1]));

                    }
                    if (item2.normalpoints.Contains(hit[0].point) && !item2.unpassablepoints.Contains(hit[0].point))
                    {
                        item2.normalpoints.Remove(hit[0].point);

                    }
                    else if (!item2.unpassablepoints.Contains(hit[0].point))
                    {
                        item2.unpassablepoints.Add(hit[0].point);

                    }
                    /*
                    Gizmos.DrawLine(transform.TransformPoint(item3.points[1]), hit[0].point);
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(hit[0].point, transform.TransformPoint(item3.points[0]));
                    */
                    booleans.availableConnections[item3.points[1]] = new(booleans.availableConnections[item3.points[1]].Item1 + 1, booleans.availableConnections[item3.points[1]].Item2);
                }
                else if (hit2.Length == 1)
                {
                    Gizmos.color = Color.green;
                    if (!item2.normalpoints.Contains(transform.TransformPoint(item3.points[1])) && !item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[1])))
                        item2.normalpoints.Add(transform.TransformPoint(item3.points[1]));
                    if (item2.normalpoints.Contains(transform.TransformPoint(item3.points[0])) && !item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[0])))
                    {
                        item2.normalpoints.Remove(transform.TransformPoint(item3.points[0]));

                    }
                    else if (!item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[0])))
                    {
                        item2.unpassablepoints.Add(transform.TransformPoint(item3.points[0]));

                    }
                    if (item2.normalpoints.Contains(hit2[0].point) && !item2.unpassablepoints.Contains(hit2[0].point))
                    {
                        item2.normalpoints.Remove(hit2[0].point);

                    }
                    else if (!item2.unpassablepoints.Contains(hit2[0].point))
                    {
                        item2.unpassablepoints.Add(hit2[0].point);

                    }
                    /*
                    Gizmos.DrawLine(transform.TransformPoint(item3.points[1]), hit2[0].point);
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(hit2[0].point, transform.TransformPoint(item3.points[0]));
                    */
                    booleans.availableConnections[item3.points[0]] = new(booleans.availableConnections[item3.points[0]].Item1 + 1, booleans.availableConnections[item3.points[1]].Item2);
                    //targetObject.availableVertices[item3.points[0]] = new(targetObject.availableVertices[item3.points[0]].Item1, targetObject.availableVertices[item3.points[0]].Item2 + 1);

                }
                else
                {
                    bool zero = booleans.availableConnections[item3.points[0]].Item1 > 0;
                    bool one = booleans.availableConnections[item3.points[1]].Item1 > 0;
                    if (!(zero && one))
                    {
                        Gizmos.color = Color.green;
                        if (!item2.normalpoints.Contains(transform.TransformPoint(item3.points[0])) && !item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[0])))
                            item2.normalpoints.Add(transform.TransformPoint(item3.points[0]));
                        if (!item2.normalpoints.Contains(transform.TransformPoint(item3.points[1])) && !item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[1])))
                            item2.normalpoints.Add(transform.TransformPoint(item3.points[1]));
                    }
                    else
                    {

                        if (item2.normalpoints.Contains(transform.TransformPoint(item3.points[0])) && !item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[0])))
                        {
                            item2.normalpoints.Remove(transform.TransformPoint(item3.points[0]));

                        }
                        else if (!item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[0])))
                        {
                            item2.unpassablepoints.Add(transform.TransformPoint(item3.points[0]));

                        }
                        if (item2.normalpoints.Contains(transform.TransformPoint(item3.points[1])) && !item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[1])))
                        {
                            item2.normalpoints.Remove(transform.TransformPoint(item3.points[1]));

                        }
                        else if (!item2.unpassablepoints.Contains(transform.TransformPoint(item3.points[1])))
                        {
                            item2.unpassablepoints.Add(transform.TransformPoint(item3.points[1]));

                        }
                        Gizmos.color = Color.red;

                    }
                    // Gizmos.DrawLine(transform.TransformPoint(item3.points[1]), transform.TransformPoint(item3.points[0]));

                }

            }
        }


        if (process)
        {
            triangles.Clear();
            forbiddenpoints = new List<Vector3>();
            foreach (var item in targetObject.triangleDatas)
            {
                foreach (var item2 in item.normalpoints)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(item2, 0.04f);
                }
                foreach (var item2 in item.unpassablepoints)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(item2, 0.04f);
                    forbiddenpoints.Add(item2);
                }
            }
            foreach (var item in booleans.triangleDatas)
            {
                foreach (var item2 in item.normalpoints)
                {
                    forbiddenpoints.Add(item2);
                }
            }
            foreach (var item in targetObject.triangleDatas)
            {

                Gizmos.color = Color.white;

                List<TriangleData> newtris = new List<TriangleData>();

                Vector3 SupraV1 = item.Edges[0].points[0];
                Vector3 SupraV2 = item.Edges[1].points[1];
                Vector3 SupraV3 = item.Edges[2].points[0];

                Vector3 p12 = SupraV2 - SupraV1;
                Vector3 p13 = SupraV3 - SupraV1;
                Vector3 p23 = SupraV3 - SupraV2;

                Vector3 newV1 = -p12 - p13 + SupraV1;
                Vector3 newV2 = p13 + p23 + SupraV3;
                Vector3 newV3 = p12 - p23 + SupraV2;
                newtris.Add(new TriangleData(newV1, newV2, newV3));
                TriangulateVertices(newV1, newV2, newV3, item.normalpoints, item.unpassablepoints, forbiddenpoints, newtris, true, true);
                foreach (var item2 in newtris)
                {
                    triangles.Add(item2);
                }
            }
            foreach (var item in booleans.triangleDatas)
            {

                Gizmos.color = Color.white;

                List<TriangleData> newtris = new List<TriangleData>();

                Vector3 SupraV1 = item.Edges[0].points[0];
                Vector3 SupraV2 = item.Edges[1].points[1];
                Vector3 SupraV3 = item.Edges[2].points[0];

                Vector3 p12 = SupraV2 - SupraV1;
                Vector3 p13 = SupraV3 - SupraV1;
                Vector3 p23 = SupraV3 - SupraV2;

                Vector3 newV1 = -p12 - p13 + SupraV1;
                Vector3 newV2 = p13 + p23 + SupraV3;
                Vector3 newV3 = p12 - p23 + SupraV2;
                newtris.Add(new TriangleData(newV1, newV2, newV3));
                TriangulateVertices(newV1, newV2, newV3, item.unpassablepoints, item.normalpoints, forbiddenpoints, newtris, true, true);
                foreach (var item2 in newtris)
                {
                    triangles.Add(item2);
                }
            }

            Debug.Log(forbiddenpoints.Count);

            process = false;
        }

        foreach (var item in triangles)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(item.Edges[0].points[0], item.Edges[0].points[1]);
            Gizmos.DrawLine(item.Edges[1].points[0], item.Edges[1].points[1]);
            Gizmos.DrawLine(item.Edges[2].points[0], item.Edges[2].points[1]);
        }
    }

    //instead of list only check new triangle of vertices
    public void TriangulateVertices(Vector3 SupraV1, Vector3 SupraV2, Vector3 SupraV3, List<Vector3> normalpoints, List<Vector3> unpassablepoints, List<Vector3> forbidden, List<TriangleData> Triangles, bool removeSupra, bool removepoints)
    {
        List<Vector3> processingpoints = new List<Vector3>();
        foreach (var item in normalpoints)
        {
            processingpoints.Add(item);
        }
        foreach (var item in unpassablepoints)
        {
            processingpoints.Add(item);
        }

        foreach (var item in processingpoints)
        {
            List<TriangleData> insidetriangle = new List<TriangleData>();
            foreach (var item2 in Triangles)
            {
                if (Vector3.Distance(item2.circumcirclepos, item) < item2.circumdistance)
                {
                    insidetriangle.Add(item2);

                }

            }
            List<Vector3[]> availablededges = new List<Vector3[]>();
            List<Vector3[]> bannededges = new List<Vector3[]>();

            foreach (var item2 in insidetriangle)
            {
                foreach (var item3 in item2.Edges)
                {
                    availablededges.Add(item3.points);
                }
                Triangles.Remove(item2);

            }
            List<Vector3[]> newavailablededges = RemoveDuplicates(availablededges);


            foreach (var item2 in newavailablededges)
            {
                Triangles.Add(new TriangleData(item2[0], item2[1], item));

            }
        }
        if (removeSupra)
        {
            List<TriangleData> data = new List<TriangleData>();
            foreach (var item in Triangles)
            {

                foreach (var item2 in item.Edges)
                {
                    foreach (var item3 in item2.points)
                    {
                        if (!data.Contains(item))
                        {
                            if (item3.Equals(SupraV1) || item3.Equals(SupraV2) || item3.Equals(SupraV3))
                                data.Add(item);
                        }
                        else
                            break;
                    }
                }
            }
            foreach (var item in data)
            {
                Triangles.Remove(item);
            }
        }
        if (removepoints)
        {
            List<TriangleData> data = new List<TriangleData>();
            foreach (var item in Triangles)
            {
                /*
                if (processingpoints.Contains((item.Edges[0].points[0])) && processingpoints.Contains((item.Edges[1].points[1])) && processingpoints.Contains((item.Edges[2].points[0])))
                {
                    data.Add(item);
                }
                */
                if (!data.Contains(item))
                {
                    if ((forbidden.Contains(item.Edges[0].points[0]) && forbidden.Contains(item.Edges[0].points[1])) &&
                        (forbidden.Contains(item.Edges[1].points[0]) && forbidden.Contains(item.Edges[1].points[1])) &&
                        (forbidden.Contains(item.Edges[2].points[0]) && forbidden.Contains(item.Edges[2].points[1])))
                    {

                        data.Add(item);
                    }
                }

            }
            foreach (var item in data)
            {
                Triangles.Remove(item);
            }
        }
    }

    public static void CalculateTriangleData(Vector3 p1, Vector3 p2, Vector3 p3, out Vector3 normal, out Vector3 finalpos)
    {
        Vector3 midpoint12 = (p1 + p2) / 2;
        Vector3 dir12 = p2 - p1;

        Vector3 midpoint23 = (p3 + p2) / 2;
        Vector3 dir23 = p2 - p3;

        Vector3 midpoint13 = (p1 + p3) / 2;

        Vector3 dir13 = p1 - p3;
        normal = Vector3.Cross(p2 - p1, p3 - p1);

        Vector3 normal12 = Vector3.Cross(normal, dir12);
        Vector3 normal23 = Vector3.Cross(normal, dir23);
        Vector3 normal13 = Vector3.Cross(normal, dir13);

        Utility.LineIntersection(out finalpos, midpoint12, normal12, midpoint23, normal23);

    }

    public static void CalculateTriangleData(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 normal, out Vector3 finalpos)
    {
        Vector3 midpoint12 = (p1 + p2) / 2;
        Vector3 dir12 = p2 - p1;

        Vector3 midpoint23 = (p3 + p2) / 2;
        Vector3 dir23 = p2 - p3;

        Vector3 midpoint13 = (p1 + p3) / 2;

        Vector3 dir13 = p1 - p3;

        Vector3 normal12 = Vector3.Cross(normal, dir12);
        Vector3 normal23 = Vector3.Cross(normal, dir23);
        Vector3 normal13 = Vector3.Cross(normal, dir13);

        Utility.LineIntersection(out finalpos, midpoint12, normal12, midpoint23, normal23);

    }

    public static List<Vector3[]> RemoveDuplicates(List<Vector3[]> vectorArraysList)
    {
        List<Vector3[]> uniqueArrays = new List<Vector3[]>(vectorArraysList.Count);
        List<Vector3[]> bannedArrays = new List<Vector3[]>(vectorArraysList.Count);

        foreach (Vector3[] array in vectorArraysList)
        {
            bool isDuplicate = false;

            // Check if the array already exists in the uniqueArrays list
            foreach (Vector3[] uniqueArray in uniqueArrays)
            {
                if (ArraysHaveSameElements(array, uniqueArray))
                {
                    isDuplicate = true;
                    bannedArrays.Add(array);
                    Debug.Log("yes");
                    break;
                }
            }
            bool isbanned = false;
            foreach (var banneditem in bannedArrays)
            {
                if (ArraysHaveSameElements(array, banneditem))
                {
                    isbanned = true;
                    Debug.Log("ban");
                }
            }
            // If it's not a duplicate, add it to the uniqueArrays list
            if (!isDuplicate && !isbanned)
            {

                uniqueArrays.Add(array);
            }
            else
            {
                for (int i = 0; i < uniqueArrays.Count; i++)
                {
                    if (ArraysHaveSameElements(array, uniqueArrays[i]))
                    {
                        uniqueArrays.RemoveAt(i);
                        break;
                    }
                }

            }
        }

        // Replace the original list with the uniqueArrays list
        return uniqueArrays;
    }

    // Function to check if two Vector3[] arrays have the same elements
    private static bool ArraysHaveSameElements(Vector3[] array1, Vector3[] array2)
    {
        if (array1.Length != array2.Length)
            return false;

        HashSet<Vector3> set1 = new HashSet<Vector3>(array1);
        HashSet<Vector3> set2 = new HashSet<Vector3>(array2);

        return set1.SetEquals(set2);
    }

    [System.Serializable]
    public struct TriangleData
    {
        public EdgeData[] Edges;
        public Vector3 normal;
        public Vector3 circumcirclepos;
        public float circumdistance;
        public List<Vector3> normalpoints;
        public List<Vector3> unpassablepoints;
        public TriangleData(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Edges = new EdgeData[3];
            Edges[0] = new EdgeData(p1, p2);
            Edges[1] = new EdgeData(p1, p3);
            Edges[2] = new EdgeData(p2, p3);
            normalpoints = new List<Vector3>();
            unpassablepoints = new List<Vector3>();
            CalculateTriangleData(p1, p2, p3, out normal, out circumcirclepos);
            circumdistance = Vector3.Distance(p1, circumcirclepos);
        }
        public TriangleData(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 normal)
        {
            Edges = new EdgeData[3];
            Edges[0] = new EdgeData(p1, p2);
            Edges[1] = new EdgeData(p1, p3);
            Edges[2] = new EdgeData(p2, p3);
            this.normal = normal;
            normalpoints = new List<Vector3>();
            unpassablepoints = new List<Vector3>();
            CalculateTriangleData(p1, p2, p3, normal, out circumcirclepos);
            circumdistance = Vector3.Distance(p1, circumcirclepos);
        }


    }
    [System.Serializable]

    public struct EdgeData
    {
        public Vector3[] points;
        public Vector3[] ReverseValue()
        {
            /*
            List<Vector3> sub = new List<Vector3>();
            for (int i = points.Length - 1; i > 0; i--)
            {
                sub.Add(points[i]);
            }
            ;
            */
            Array.Reverse(points);
            return points;
        }
        public EdgeData(Vector3 p1, Vector3 p2)
        {
            points = new Vector3[] { p1, p2 };
        }
    }


}
