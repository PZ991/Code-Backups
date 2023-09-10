using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using static Parabox.CSG.CSG;
using static UnityEditor.Progress;

public class BooleanHandlerV3 : MonoBehaviour
{
    // Start is called before the first frame update
    public BooleanOperatorV7 targetboolean;
    public BooleanOperatorV7 boolean;
    public Dictionary<Vector3, Dictionary<Vector3, List<bool>>> processedpoints = new Dictionary<Vector3, Dictionary<Vector3, List<bool>>>();
    public Dictionary<Vector3, List<Vector3>> processingpointsdone = new Dictionary<Vector3, List<Vector3>>();
    public List<TriangleData> triangles = new List<TriangleData>();
    public List<Vector3> forbiddenpoints = new List<Vector3>();

    public bool process;
    public bool process2;
    public bool show;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDrawGizmos()
    {
        if (process)
        {
            processedpoints = new Dictionary<Vector3, Dictionary<Vector3, List<bool>>>();
            processingpointsdone = new Dictionary<Vector3, List<Vector3>>();
            Vector3 startingvertex = Vector3.zero;
            bool found = false;
            foreach (var item in targetboolean.availableedges)
            {
                // int boolval = 0;
                //Gizmos.DrawSphere(item.Key, 0.05f);
                if (found)
                    break;
                foreach (var item2 in targetboolean.availableedges[item.Key])
                {
                    List<bool> ins = new List<bool>();

                    ins = PointInside(item.Key, item2.Key, targetboolean.availableedges[item.Key][item2.Key], false);

                    //Debug.Log(ins.Count);
                    if (ins[0] == true)
                    {
                        startingvertex = item.Key;
                        found = true;
                        break;

                    }
                    else if (ins[ins.Count - 1] == true)
                    {
                        startingvertex = item2.Key;
                        found = true;
                        break;
                    }
                    if (!processingpointsdone.ContainsKey(item.Key))
                    {
                        processingpointsdone.Add(item.Key, new List<Vector3> { item2.Key });
                        processedpoints.Add(item.Key, new Dictionary<Vector3, List<bool>> { { item2.Key, new List<bool> { false, false } } });
                    }
                    else if (!processingpointsdone[item.Key].Contains(item2.Key))
                    {
                        processingpointsdone[item.Key].Add(item2.Key);
                        processedpoints[item.Key].Add(item2.Key, new List<bool> { false, false });
                    }
                    // Gizmos.DrawSphere(item2.Key, 0.05f);

                }

            }
            if (found)
            {
                Debug.Log("Found");
                processingpointsdone.Clear();
                processedpoints.Clear();

                selectprocesses(startingvertex, true);

                Gizmos.DrawSphere(startingvertex, 0.5f);

            }



            forbiddenpoints = new List<Vector3>();


            foreach (var item in processingpointsdone)
            {
                foreach (var item2 in processedpoints[item.Key])
                {
                    if (processedpoints[item.Key][item2.Key][0] == false)
                        Gizmos.color = Color.green;
                    else
                    {
                        Gizmos.color = Color.red;

                        forbiddenpoints.Add(item.Key);
                    }

                    Gizmos.DrawSphere(item.Key, 0.1f);

                    if (processedpoints[item.Key][item2.Key][(processedpoints[item.Key][item2.Key].Count - 1)] == false)
                        Gizmos.color = Color.green;
                    else
                    {
                        Gizmos.color = Color.red;

                        forbiddenpoints.Add(item2.Key);
                    }
                    Gizmos.DrawSphere(item2.Key, 0.1f);

                    if (targetboolean.availableedges[item.Key][item2.Key].Count > 0)
                    {
                        for (int i = 0; i < targetboolean.availableedges[item.Key][item2.Key].Count; i++)
                        {
                            /*
                            if (processedpoints[item.Key][item2.Key][i] == false)
                                Gizmos.color = Color.green;
                            else
                                */
                            Gizmos.color = Color.red;

                            Gizmos.DrawSphere(targetboolean.availableedges[item.Key][item2.Key][i].point, 0.1f);
                            forbiddenpoints.Add(targetboolean.availableedges[item.Key][item2.Key][i].point);

                        }
                    }
                }
            }




            triangles.Clear();

            foreach (var item in targetboolean.triangleDatas)
            {

                Gizmos.color = Color.white;

                List<TriangleData> newtris = new List<TriangleData>();

                Vector3 SupraV1 = item.points[0];
                Vector3 SupraV2 = item.points[1];
                Vector3 SupraV3 = item.points[2];

                Vector3 p12 = SupraV2 - SupraV1;
                Vector3 p13 = SupraV3 - SupraV1;
                Vector3 p23 = SupraV3 - SupraV2;

                Vector3 newV1 = -p12 - p13 + SupraV1;
                Vector3 newV2 = p13 + p23 + SupraV3;
                Vector3 newV3 = p12 - p23 + SupraV2;
                newtris.Add(new TriangleData(newV1, newV2, newV3));
                List<Vector3> newpoints = new List<Vector3>();

                newpoints.Add(SupraV1);
                newpoints.Add(SupraV2);
                newpoints.Add(SupraV3);
                forbiddenpoints.Add(newV1);
                forbiddenpoints.Add(newV2);
                forbiddenpoints.Add(newV3);
                foreach (var item2 in targetboolean.availableedges[item.points[0]][item.points[1]])
                {
                    if (!newpoints.Contains(item2.point))
                        newpoints.Add(item2.point);
                }
                foreach (var item2 in targetboolean.availableedges[item.points[2]][item.points[1]])
                {
                    if (!newpoints.Contains(item2.point))

                        newpoints.Add(item2.point);
                }
                foreach (var item2 in targetboolean.availableedges[item.points[0]][item.points[2]])
                {
                    if (!newpoints.Contains(item2.point))

                        newpoints.Add(item2.point);
                }

                TriangulateVertices(targetboolean, newV1, newV2, newV3, newpoints, forbiddenpoints, newtris, true, true);
                foreach (var item2 in newtris)
                {
                    triangles.Add(item2);
                }
            }
            Debug.Log(forbiddenpoints.Count);


            process = false;

        }

        if (process2)
        {
            processedpoints = new Dictionary<Vector3, Dictionary<Vector3, List<bool>>>();
            processingpointsdone = new Dictionary<Vector3, List<Vector3>>();
            Vector3 startingvertex = Vector3.zero;
            bool found = false;
            foreach (var item in boolean.availableedges)
            {
                // int boolval = 0;
                //Gizmos.DrawSphere(item.Key, 0.05f);
                if (found)
                    break;
                foreach (var item2 in boolean.availableedges[item.Key])
                {
                    List<bool> ins = new List<bool>();

                    ins = PointInside(item.Key, item2.Key, boolean.availableedges[item.Key][item2.Key], false);

                    //Debug.Log(ins.Count);
                    if (ins[0] == true)
                    {
                        startingvertex = item.Key;
                        found = true;
                        break;

                    }
                    else if (ins[ins.Count - 1] == true)
                    {
                        startingvertex = item2.Key;
                        found = true;
                        break;
                    }
                    if (!processingpointsdone.ContainsKey(item.Key))
                    {
                        processingpointsdone.Add(item.Key, new List<Vector3> { item2.Key });
                        processedpoints.Add(item.Key, new Dictionary<Vector3, List<bool>> { { item2.Key, new List<bool> { false, false } } });
                    }
                    else if (!processingpointsdone[item.Key].Contains(item2.Key))
                    {
                        processingpointsdone[item.Key].Add(item2.Key);
                        processedpoints[item.Key].Add(item2.Key, new List<bool> { false, false });
                    }
                    // Gizmos.DrawSphere(item2.Key, 0.05f);

                }

            }
            if (found)
            {
                Debug.Log("Found");
                processingpointsdone.Clear();
                processedpoints.Clear();

                selectprocesses(boolean, startingvertex, true);

                Gizmos.DrawSphere(startingvertex, 0.5f);

            }



            forbiddenpoints = new List<Vector3>();


            foreach (var item in processingpointsdone)
            {
                foreach (var item2 in processedpoints[item.Key])
                {
                    if (processedpoints[item.Key][item2.Key][0] == false)
                        Gizmos.color = Color.green;
                    else
                    {
                        Gizmos.color = Color.red;

                        forbiddenpoints.Add(item.Key);
                    }

                    Gizmos.DrawSphere(item.Key, 0.1f);

                    if (processedpoints[item.Key][item2.Key][(processedpoints[item.Key][item2.Key].Count - 1)] == false)
                        Gizmos.color = Color.green;
                    else
                    {
                        Gizmos.color = Color.red;

                        forbiddenpoints.Add(item2.Key);
                    }
                    Gizmos.DrawSphere(item2.Key, 0.1f);

                    if (boolean.availableedges[item.Key][item2.Key].Count > 0)
                    {
                        for (int i = 0; i < boolean.availableedges[item.Key][item2.Key].Count; i++)
                        {
                            /*
                            if (processedpoints[item.Key][item2.Key][i] == false)
                                Gizmos.color = Color.green;
                            else
                                */
                            Gizmos.color = Color.red;

                            Gizmos.DrawSphere(boolean.availableedges[item.Key][item2.Key][i].point, 0.1f);
                            forbiddenpoints.Add(boolean.availableedges[item.Key][item2.Key][i].point);

                        }
                    }
                }
            }




            triangles.Clear();

            foreach (var item in boolean.triangleDatas)
            {

                Gizmos.color = Color.white;

                List<TriangleData> newtris = new List<TriangleData>();

                Vector3 SupraV1 = item.points[0];
                Vector3 SupraV2 = item.points[1];
                Vector3 SupraV3 = item.points[2];


                Vector3 p12 = SupraV2 - SupraV1;
                Vector3 p13 = SupraV3 - SupraV1;
                Vector3 p23 = SupraV3 - SupraV2;

                Vector3 newV1 = -p12 - p13 + SupraV1;
                Vector3 newV2 = p13 + p23 + SupraV3;
                Vector3 newV3 = p12 - p23 + SupraV2;
                newtris.Add(new TriangleData(newV1, newV2, newV3));
                List<Vector3> newpoints = new List<Vector3>();

                newpoints.Add(SupraV1);
                newpoints.Add(SupraV2);
                newpoints.Add(SupraV3);
                foreach (var item2 in boolean.availableedges[item.points[0]][item.points[1]])
                {
                    if (!newpoints.Contains(item2.point))
                        newpoints.Add(item2.point);
                }
                foreach (var item2 in boolean.availableedges[item.points[2]][item.points[1]])
                {
                    if (!newpoints.Contains(item2.point))

                        newpoints.Add(item2.point);
                }
                foreach (var item2 in boolean.availableedges[item.points[0]][item.points[2]])
                {
                    if (!newpoints.Contains(item2.point))

                        newpoints.Add(item2.point);
                }

                int index = boolean.verticestriangle[item.points[0]][item.points[1]][item.points[2]];
                if (boolean.availableTriangles.ContainsKey(index))
                {
                    foreach (var item2 in boolean.availableTriangles[index])
                    {
                        newpoints.Add(item2);

                        forbiddenpoints.Add(item2);
                        //Gizmos.color = Color.red;

                        Gizmos.DrawSphere(item2, 0.1f);
                        Debug.DrawRay(item2, boolean.vectornormals[item2]);
                    }
                }
                TriangulateVertices(boolean, newV1, newV2, newV3, newpoints, forbiddenpoints, newtris, true, false);
                foreach (var item2 in newtris)
                {
                    triangles.Add(item2);
                }
            }
            Debug.Log(forbiddenpoints.Count);


            process2 = false;

        }

        if (show)
        {
            //Debug.Log(processingpointsdone.Count);
            foreach (var item in triangles)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(item.points[0], item.points[1]);
                Gizmos.DrawLine(item.points[0], item.points[2]);
                Gizmos.DrawLine(item.points[2], item.points[1]);
                // Gizmos.color = Color.blue;
                //Gizmos.DrawRay(item.points[0], V7.vectornormals[item.points[0]]);
                //Gizmos.DrawRay(item.points[1], V7.vectornormals[item.points[1]]);
                //Gizmos.DrawRay(item.points[2], V7.vectornormals[item.points[2]]);
            }

        }
    }

    public void selectprocesses(Vector3 point, bool inside)
    {
        foreach (var item in targetboolean.availableedges[point])
        {
            if (processingpointsdone.ContainsKey(point))
            {
                if (processingpointsdone[point].Contains(item.Key))
                    continue;
            }
            List<bool> ins = new List<bool>();

            ins = PointInside(point, item.Key, targetboolean.availableedges[point][item.Key], inside);


            if (!processingpointsdone.ContainsKey(point))
            {
                processingpointsdone.Add(point, new List<Vector3> { item.Key });
                processedpoints.Add(point, new Dictionary<Vector3, List<bool>> { { item.Key, ins } });

            }
            else if (!processingpointsdone[point].Contains(item.Key))
            {
                processingpointsdone[point].Add(item.Key);
                processedpoints[point].Add(item.Key, ins);

            }
            //Debug.Log("this");
            selectprocesses(item.Key, ins[ins.Count - 1]);

            /*
            if (processingpointsdone.ContainsKey(item.Key))
            {
                if (processingpointsdone[item.Key].Contains(point))
                    continue;
                else
                {
                    processingpointsdone[item.Key].Add(point);
                    ins.Reverse();
                    processedpoints[item.Key].Add(point, ins);
                }

            }
            else
            {
                processingpointsdone.Add(item.Key, new List<Vector3> { point });
                ins.Reverse();
                processedpoints.Add(item.Key, new Dictionary<Vector3, List<bool>> { { point, ins } });
            }
            */
        }
    }

    //fix when collision is more than 1
    public void selectprocesses(BooleanOperatorV7 boolop, Vector3 point, bool inside)
    {
        foreach (var item in boolop.availableedges[point])
        {
            if (processingpointsdone.ContainsKey(point))
            {
                if (processingpointsdone[point].Contains(item.Key))
                    continue;
            }
            List<bool> ins = new List<bool>();

            ins = PointInside(point, item.Key, boolop.availableedges[point][item.Key], inside);


            if (!processingpointsdone.ContainsKey(point))
            {
                processingpointsdone.Add(point, new List<Vector3> { item.Key });
                processedpoints.Add(point, new Dictionary<Vector3, List<bool>> { { item.Key, ins } });

            }
            else if (!processingpointsdone[point].Contains(item.Key))
            {
                processingpointsdone[point].Add(item.Key);
                processedpoints[point].Add(item.Key, ins);

            }
            //Debug.Log("this");
            selectprocesses(boolop, item.Key, ins[ins.Count - 1]);

            /*
            if (processingpointsdone.ContainsKey(item.Key))
            {
                if (processingpointsdone[item.Key].Contains(point))
                    continue;
                else
                {
                    processingpointsdone[item.Key].Add(point);
                    ins.Reverse();
                    processedpoints[item.Key].Add(point, ins);
                }

            }
            else
            {
                processingpointsdone.Add(item.Key, new List<Vector3> { point });
                ins.Reverse();
                processedpoints.Add(item.Key, new Dictionary<Vector3, List<bool>> { { point, ins } });
            }
            */
        }
    }

    public static List<bool> PointInside(Vector3 p1, Vector3 p2, List<RaycastHit> sortedData, bool alreadyinside)
    {
        List<Vector3> sortedData2 = sortedData.Select(item => item.point).ToList();
        List<bool> isinside = new List<bool>();
        //Debug.Log(sortedData.Count);
        if (sortedData.Count > 1)
        {
            sortedData2.Insert(0, p2);

            sortedData2.Insert(sortedData2.Count - 1, p1);
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
                    isinside.Add(true);
                }
                else
                {
                    Gizmos.color = Color.green;
                    isinside.Add(false);

                }

                //Gizmos.DrawLine(sortedData2[i], sortedData2[i + 1]);



            }
        }
        else if (sortedData.Count == 1)
        {
            Plane plane = new Plane(sortedData[0].normal, sortedData[0].point);
            isinside.Add(!plane.GetSide(p1));
            isinside.Add(!plane.GetSide(p2));

        }
        else
        {
            if (alreadyinside)
            {
                isinside.Add(true);
                isinside.Add(true);
            }
            else
            {
                isinside.Add(false);
                isinside.Add(false);
            }


        }
        return isinside;
    }
    public static List<bool> PointInside(Vector3 p1, Vector3 p2, RaycastHit[] hit, RaycastHit[] hit2, List<RaycastHit> sortedData, bool alreadyinside)
    {
        List<Vector3> sortedData2 = sortedData.Select(item => item.point).ToList();
        List<bool> isinside = new List<bool>();
        if (hit.Length > 0 && hit2.Length > 0)
        {
            sortedData2.Insert(0, p2);

            sortedData2.Insert(sortedData2.Count - 1, p1);
            bool inside = false;
            int insides = 0;
            for (int i = 0; i < sortedData2.Count - 1; i++)
            {

                //Debug.Log(plane.GetSide(vertices2[displayVertex1]));
                // Plane plane = new Plane(sortedData[i].normal, sortedData2[i + 1]);

                if (i > 0 && i < sortedData2.Count - 1)
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
                    isinside.Add(true);
                }
                else
                {
                    Gizmos.color = Color.green;
                    isinside.Add(false);

                }

                //Gizmos.DrawLine(sortedData2[i], sortedData2[i + 1]);



            }
        }
        else if (hit.Length == 1)
        {

            Gizmos.color = Color.red;
            isinside.Add(false);
            isinside.Add(true);
            isinside.Add(true);

        }
        else if (hit2.Length == 1)
        {
            isinside.Add(true);
            isinside.Add(true);
            isinside.Add(false);


        }
        else
        {
            if (alreadyinside)
            {
                isinside.Add(true);
                isinside.Add(true);
            }
            else
            {
                isinside.Add(false);
                isinside.Add(false);
            }


        }
        return isinside;
    }

    public void TriangulateVertices(BooleanOperatorV7 V7, Vector3 SupraV1, Vector3 SupraV2, Vector3 SupraV3, List<Vector3> normalpoints, List<Vector3> forbidden, List<TriangleData> Triangles, bool removeSupra, bool removepoints)
    {


        foreach (var item in normalpoints)
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
                Vector3[] edge1 = new Vector3[] { item2.points[0], item2.points[1] };
                Vector3[] edge2 = new Vector3[] { item2.points[1], item2.points[2] };
                Vector3[] edge3 = new Vector3[] { item2.points[2], item2.points[0] };
                availablededges.Add(edge1);
                availablededges.Add(edge2);
                availablededges.Add(edge3);

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


                foreach (var item3 in item.points)
                {

                    if (ApproximateVector(item3, SupraV1) || ApproximateVector(item3, SupraV2) || ApproximateVector(item3, SupraV3))
                        // if (item3.Equals(SupraV1) || item3.Equals(SupraV2) || item3.Equals(SupraV3))
                        data.Add(item);
                    // else
                    //Debug.Log(item3 + "/////" + SupraV1 + "/" + SupraV2 + "/" + SupraV3 + "/");

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
                    if ((forbidden.Contains(item.points[0])) &&
                        (forbidden.Contains(item.points[1])) &&
                        (forbidden.Contains(item.points[2])))
                    {

                        if ((Vector3.Angle(V7.vectornormals[item.points[0]], (item.circumcirclepos - item.points[0]).normalized) > 90) ||
                         (Vector3.Angle(V7.vectornormals[item.points[1]], (item.circumcirclepos - item.points[1]).normalized) > 90) ||
                         (Vector3.Angle(V7.vectornormals[item.points[2]], (item.circumcirclepos - item.points[2]).normalized) > 90))

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

    public static bool ApproximateVector(Vector3 first, Vector3 second)
    {
        bool x = Mathf.Approximately(first.x, second.x);
        bool y = Mathf.Approximately(first.y, second.y);
        bool z = Mathf.Approximately(first.z, second.z);
        return (x && y && z);
    }

    [System.Serializable]
    public struct TriangleData
    {
        public Vector3[] points;
        public Vector3 normal;
        public Vector3 circumcirclepos;
        public float circumdistance;
        public List<Vector3> normalpoints;
        public TriangleData(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            points = new Vector3[3];
            points[0] = p1;
            points[1] = p2;
            points[2] = p3;
            normalpoints = new List<Vector3>();
            CalculateTriangleData(p1, p2, p3, out normal, out circumcirclepos);
            circumdistance = Vector3.Distance(p1, circumcirclepos);
        }
        public TriangleData(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 normal)
        {
            points = new Vector3[3];
            points[0] = p1;
            points[1] = p2;
            points[2] = p3;
            this.normal = normal;
            normalpoints = new List<Vector3>();
            CalculateTriangleData(p1, p2, p3, normal, out circumcirclepos);
            circumdistance = Vector3.Distance(p1, circumcirclepos);
        }


    }

}
