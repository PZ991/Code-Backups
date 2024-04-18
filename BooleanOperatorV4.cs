using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BooleanOperatorV4 : MonoBehaviour
{
    // Triangle index & Vertex Positions (Triangles)
    public Dictionary<int, List<Vector3>> availableTriangles;

    //number of connectionsinside & Vertex Positions & Index that have the same position (Vertex)
    public Dictionary<Vector3, List<int>> availableVertices = new Dictionary<Vector3, List<int>>();
    public Dictionary<Vector3, (int, List<Vector3>)> availableConnections = new Dictionary<Vector3, (int, List<Vector3>)>();
    public List<BooleanHandlerV2.TriangleData> triangleDatas = new List<BooleanHandlerV2.TriangleData>();
    public GameObject targetObject;
    public bool resetDictionary;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDrawGizmos()
    {
        if (resetDictionary)
        {
            ResetDictionary();
            resetDictionary = false;
        }
        foreach (var item in triangleDatas)
        {
            foreach (var item2 in item.Edges)
            {

                // Gizmos.DrawLine(item2.points[0], item2.points[1]);

            }
        }
    }
    public void ResetDictionary()
    {
        availableVertices = new Dictionary<Vector3, List<int>>();
        availableTriangles = new Dictionary<int, List<Vector3>>();
        availableConnections = new Dictionary<Vector3, (int, List<Vector3>)>();
        triangleDatas = new List<BooleanHandlerV2.TriangleData>();
        Mesh mesh2 = Utility.GetMesh(targetObject);
        Vector3[] vertices2 = mesh2.vertices;
        Vector3[] normals = mesh2.normals;

        int[] triangles = mesh2.triangles;
        Vector3[] triangleVertices = new Vector3[triangles.Length];
        for (int i = 0; i < triangles.Length / 3; i++)
        {
            int vertexIndex1 = triangles[i * 3];
            int vertexIndex2 = triangles[i * 3 + 1];
            int vertexIndex3 = triangles[i * 3 + 2];
            Vector3 normalA = normals[vertexIndex1];
            Vector3 normalB = normals[vertexIndex2];
            Vector3 normalC = normals[vertexIndex3];
            Vector3 triangleNormal = (normalA + normalB + normalC).normalized;
            triangleDatas.Add(new BooleanHandlerV2.TriangleData(vertices2[vertexIndex1], vertices2[vertexIndex2], vertices2[vertexIndex3], triangleNormal));
            availableTriangles.Add(i, new List<Vector3>() { vertices2[vertexIndex1], vertices2[vertexIndex2], vertices2[vertexIndex3] });

            if (!availableVertices.ContainsKey(vertices2[vertexIndex1]))
                availableVertices.Add(vertices2[vertexIndex1], new List<int> { vertexIndex1 });
            else
                availableVertices[vertices2[vertexIndex1]].Add(vertexIndex1);
            if (!availableVertices.ContainsKey(vertices2[vertexIndex2]))
                availableVertices.Add(vertices2[vertexIndex2], new List<int> { vertexIndex2 });
            else
                availableVertices[vertices2[vertexIndex2]].Add(vertexIndex2);
            if (!availableVertices.ContainsKey(vertices2[vertexIndex3]))
                availableVertices.Add(vertices2[vertexIndex3], new List<int> { vertexIndex3 });
            else
                availableVertices[vertices2[vertexIndex3]].Add(vertexIndex3);

            #region Edge connections

            if (!availableConnections.ContainsKey(vertices2[vertexIndex1]))
            {

                availableConnections.Add(vertices2[vertexIndex1], (0, new List<Vector3> { vertices2[vertexIndex2], vertices2[vertexIndex3] }));

            }


            else
            {

                if (!availableConnections[vertices2[vertexIndex1]].Item2.Contains(vertices2[vertexIndex2]))
                    availableConnections[vertices2[vertexIndex1]].Item2.Add(vertices2[vertexIndex2]);
                else if (!availableConnections[vertices2[vertexIndex1]].Item2.Contains(vertices2[vertexIndex3]))
                    availableConnections[vertices2[vertexIndex1]].Item2.Add(vertices2[vertexIndex3]);


            }

            if (!availableConnections.ContainsKey(vertices2[vertexIndex2]))
            {
                availableConnections.Add(vertices2[vertexIndex2], (0, new List<Vector3> { vertices2[vertexIndex1], vertices2[vertexIndex3] }));

            }

            else
            {
                if (!availableConnections[vertices2[vertexIndex2]].Item2.Contains(vertices2[vertexIndex1]))
                    availableConnections[vertices2[vertexIndex2]].Item2.Add(vertices2[vertexIndex1]);
                else if (!availableConnections[vertices2[vertexIndex2]].Item2.Contains(vertices2[vertexIndex3]))
                    availableConnections[vertices2[vertexIndex2]].Item2.Add(vertices2[vertexIndex3]);


            }

            if (!availableConnections.ContainsKey(vertices2[vertexIndex3]))
            {
                availableConnections.Add(vertices2[vertexIndex3], (0, new List<Vector3> { vertices2[vertexIndex1], vertices2[vertexIndex2] }));

            }

            else
            {
                if (!availableConnections[vertices2[vertexIndex3]].Item2.Contains(vertices2[vertexIndex1]))
                    availableConnections[vertices2[vertexIndex3]].Item2.Add(vertices2[vertexIndex1]);
                else if (!availableConnections[vertices2[vertexIndex3]].Item2.Contains(vertices2[vertexIndex2]))
                    availableConnections[vertices2[vertexIndex3]].Item2.Add(vertices2[vertexIndex2]);


            }



            #endregion

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

}
