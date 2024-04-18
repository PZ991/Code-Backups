using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BooleanOperatorV5 : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 SupraV1;
    public Vector3 SupraV2;
    public Vector3 SupraV3;
    public List<Vector3> normalpoints = new List<Vector3>();
    public List<Vector3> unpassablepoints = new List<Vector3>();
    public List<TriangleData> Triangles = new List<TriangleData>();
    public bool reset;
    public bool Triangulate;
    public bool removeSupra;
    public bool removepoints;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDrawGizmos()
    {

        if (reset)
        {
            Triangles = new List<TriangleData>();
            Triangles.Add(new TriangleData(SupraV1, SupraV2, SupraV3));
            reset = false;
        }
        if (Triangulate)
        {
            TriangulateVertices(SupraV1, SupraV2, SupraV3, normalpoints, unpassablepoints, Triangles, removeSupra, removepoints);
        }

        foreach (var item in normalpoints)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(item, 0.05f);

        }
        foreach (var item in unpassablepoints)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(item, 0.05f);

        }

        if (Triangles != null)
            foreach (var item in Triangles)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(item.Edges[0].points[0], 0.05f);
                Gizmos.DrawSphere(item.Edges[1].points[1], 0.05f);
                Gizmos.DrawSphere(item.Edges[2].points[0], 0.05f);

                Gizmos.DrawLine(item.Edges[0].points[0], item.Edges[0].points[1]);
                Gizmos.DrawLine(item.Edges[1].points[0], item.Edges[1].points[1]);
                Gizmos.DrawLine(item.Edges[2].points[0], item.Edges[2].points[1]);
                Gizmos.color = Color.red;
                //Gizmos.DrawSphere(item.circumcirclepos, 0.08f);

                Vector3 p12 = SupraV2 - SupraV1;
                Vector3 p13 = SupraV3 - SupraV1;
                Vector3 p23 = SupraV3 - SupraV2;

                Vector3 newV1 = -p12 - p13 + SupraV1;
                Vector3 newV2 = p13 + p23 + SupraV3;
                Vector3 newV3 = p12 - p23 + SupraV2;

                Gizmos.DrawLine(-p12, -p13);
                Debug.Log(p13);
                // Gizmos.DrawLine(SupraV1, SupraV3);
                //V0
                Gizmos.DrawSphere(newV1, 0.05f);
                Gizmos.DrawSphere(newV2, 0.05f);
                Gizmos.DrawSphere(newV3, 0.05f);
                //Gizmos.DrawSphere(Utility.MoveVectorAlongDirection(item.Edges[0].points[0], p12 + p13), 0.05f);
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

    public void CalculateTriangleData(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 normal, out Vector3 finalpos)
    {
        Vector3 midpoint12 = (SupraV1 + SupraV2) / 2;
        Vector3 dir12 = SupraV2 - SupraV1;

        Vector3 midpoint23 = (SupraV3 + SupraV2) / 2;
        Vector3 dir23 = SupraV2 - SupraV3;

        Vector3 midpoint13 = (SupraV1 + SupraV3) / 2;

        Vector3 dir13 = SupraV1 - SupraV3;

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
    public static void TriangulateVertices(Vector3 SupraV1, Vector3 SupraV2, Vector3 SupraV3, List<Vector3> normalpoints, List<Vector3> unpassablepoints, List<TriangleData> Triangles, bool removeSupra, bool removepoints)
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


            Debug.Log(newavailablededges.Count);
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
                    if ((unpassablepoints.Contains(item.Edges[0].points[0]) && unpassablepoints.Contains(item.Edges[0].points[1])) &&
                        (unpassablepoints.Contains(item.Edges[1].points[0]) && unpassablepoints.Contains(item.Edges[1].points[1])) &&
                        (unpassablepoints.Contains(item.Edges[2].points[0]) && unpassablepoints.Contains(item.Edges[2].points[1])))
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

    [System.Serializable]
    public struct TriangleData
    {
        public EdgeData[] Edges;
        public Vector3 normal;
        public Vector3 circumcirclepos;
        public float circumdistance;

        public TriangleData(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Edges = new EdgeData[3];
            Edges[0] = new EdgeData(p1, p2);
            Edges[1] = new EdgeData(p1, p3);
            Edges[2] = new EdgeData(p2, p3);

            CalculateTriangleData(p1, p2, p3, out normal, out circumcirclepos);
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
