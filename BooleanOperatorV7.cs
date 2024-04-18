using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class BooleanOperatorV7 : MonoBehaviour
{
    // Triangle index & Vertex that are hits
    public Dictionary<int, List<Vector3>> availableTriangles = new Dictionary<int, List<Vector3>>();

    //number of connectionsinside & Vertex Positions & Index that have the same position (Vertex)
    public Dictionary<Vector3, List<int>> availableVertices = new Dictionary<Vector3, List<int>>();
    public Dictionary<Vector3, Vector3> vectornormals = new Dictionary<Vector3, Vector3>();

    public Dictionary<Vector3, Dictionary<Vector3, List<RaycastHit>>> availableedges = new Dictionary<Vector3, Dictionary<Vector3, List<RaycastHit>>>();
    public Dictionary<Vector3, Dictionary<Vector3, Dictionary<Vector3, int>>> verticestriangle = new Dictionary<Vector3, Dictionary<Vector3, Dictionary<Vector3, int>>>();
    public List<BooleanHandlerV3.TriangleData> triangleDatas = new List<BooleanHandlerV3.TriangleData>();
    public GameObject targetObject;
    public bool reset;
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
            CompleteReset();
            UpdateCollisions();

            reset = false;
        }


    }

    public void CompleteReset()
    {
        availableTriangles = new Dictionary<int, List<Vector3>>();
        verticestriangle = new Dictionary<Vector3, Dictionary<Vector3, Dictionary<Vector3, int>>>();

        availableVertices = new Dictionary<Vector3, List<int>>();
        availableedges = new Dictionary<Vector3, Dictionary<Vector3, List<RaycastHit>>>();
        triangleDatas = new List<BooleanHandlerV3.TriangleData>();
        vectornormals = new Dictionary<Vector3, Vector3>();
        Mesh mesh2 = Utility.GetMesh(targetObject);
        Vector3[] vertices2 = mesh2.vertices;
        Vector3[] normals = mesh2.normals;

        int[] triangles = mesh2.triangles;
        Vector3[] triangleVertices = new Vector3[triangles.Length];
        for (int i = 0; i < triangles.Length / 3; i++)
        {
            #region Initialize
            int vertexIndex1 = triangles[i * 3];
            int vertexIndex2 = triangles[i * 3 + 1];
            int vertexIndex3 = triangles[i * 3 + 2];
            Vector3 normalA = normals[vertexIndex1];
            Vector3 normalB = normals[vertexIndex2];
            Vector3 normalC = normals[vertexIndex3];
            Vector3 V1 = transform.TransformPoint(vertices2[vertexIndex1]);
            Vector3 V2 = transform.TransformPoint(vertices2[vertexIndex2]);
            Vector3 V3 = transform.TransformPoint(vertices2[vertexIndex3]);
            //Vector3 triangleNormal = (normalA + normalB + normalC).normalized;
            Vector3 vertexNormal = mesh2.normals[vertexIndex1];

            triangleDatas.Add(new BooleanHandlerV3.TriangleData(V1, V2, V3, vertexNormal));
            /*
            if (!availableTriangles.ContainsKey(i))
                availableTriangles.Add(i, new List<Vector3>() { V1, V2, V3 });
            else
            {
                if (!availableTriangles[i].Contains(V1))
                {
                    availableTriangles[i].Add(V1);
                }
                if (!availableTriangles[i].Contains(V2))
                {
                    availableTriangles[i].Add(V2);
                }
                if (!availableTriangles[i].Contains(V3))
                {
                    availableTriangles[i].Add(V3);
                }

            }
            */

            #region Vertex Available
            if (!availableVertices.ContainsKey(V1))
            {
                availableVertices.Add(V1, new List<int> { vertexIndex1 });
            }
            else
            {
                availableVertices[V1].Add(vertexIndex1);
            }
            if (!availableVertices.ContainsKey(V2))
            {
                availableVertices.Add(V2, new List<int> { vertexIndex2 });

            }
            else
            {
                availableVertices[V2].Add(vertexIndex2);

            }
            if (!availableVertices.ContainsKey(V3))
            {
                availableVertices.Add(V3, new List<int> { vertexIndex3 });

            }
            else
            {
                availableVertices[V3].Add(vertexIndex3);

            }

            #endregion


            #region Normals
            if (!vectornormals.ContainsKey(V1))
            {
                vectornormals.Add(V1, normals[vertexIndex1].normalized);
            }
            else
            {
                vectornormals[V1] = (vectornormals[V1] + normals[vertexIndex1].normalized).normalized;
            }
            if (!vectornormals.ContainsKey(V2))
            {
                vectornormals.Add(V2, normals[vertexIndex2].normalized);

            }
            else
            {
                vectornormals[V2] = (vectornormals[V2] + normals[vertexIndex2].normalized).normalized;

            }
            if (!vectornormals.ContainsKey(V3))
            {
                vectornormals.Add(V3, normals[vertexIndex3].normalized);

            }
            else
            {
                vectornormals[V3] = (vectornormals[V3] + normals[vertexIndex3].normalized).normalized;

            }

            #endregion


            #endregion

            #region Normals
            foreach (var item in availableedges)
            {
                foreach (var item2 in availableedges[item.Key])
                {
                    foreach (var item3 in availableedges[item.Key][item2.Key])
                    {
                        if (!availableVertices.ContainsKey(item3.point))
                        {
                            availableVertices.Add(item3.point, new List<int> { availableVertices.Count });
                            vectornormals.Add(item3.point, item3.normal.normalized);

                        }
                        else
                        {
                            availableVertices[item3.point].Add(availableVertices.Count);
                            vectornormals[item3.point] = (vectornormals[item3.point] + item3.normal.normalized).normalized;

                        }
                    }
                }
            }
            #endregion
            /*
            int sum = 0;
            foreach (var edge in availableedges)
                foreach (var item in availableedges[edge.Key])
                {
                    sum++;
                }

            Debug.Log(sum);
            */
        }


    }


    public void UpdateCollisions(bool self = true, bool others = true)
    {
        Mesh mesh2 = Utility.GetMesh(targetObject);
        Vector3[] vertices2 = mesh2.vertices;
        Vector3[] normals = mesh2.normals;

        int[] triangles = mesh2.triangles;
        for (int i = 0; i < triangles.Length / 3; i++)
        {
            int vertexIndex1 = triangles[i * 3];
            int vertexIndex2 = triangles[i * 3 + 1];
            int vertexIndex3 = triangles[i * 3 + 2];
            Vector3 V1 = transform.TransformPoint(vertices2[vertexIndex1]);
            Vector3 V2 = transform.TransformPoint(vertices2[vertexIndex2]);
            Vector3 V3 = transform.TransformPoint(vertices2[vertexIndex3]);
            Vector3 normalA = normals[vertexIndex1];
            Vector3 normalB = normals[vertexIndex2];
            Vector3 normalC = normals[vertexIndex3];
            Vector3 triangleNormal = (normalA + normalB + normalC).normalized;
            if (self)
                UpdateColliderSelf(i, triangles, vertices2, normals);
            if (others)
                UpdateColliderBoolean(i, triangles, vertices2, normals);

            #region Vertex Triangles

            AddVertex(V1, V2, V3, i);
            AddVertex(V2, V1, V3, i);
            AddVertex(V3, V1, V2, i);

            #endregion
        }
    }
    public void UpdateColliderBoolean(int i, int[] triangles, Vector3[] vertices2, Vector3[] normals)
    {
        int vertexIndex1 = triangles[i * 3];
        int vertexIndex2 = triangles[i * 3 + 1];
        int vertexIndex3 = triangles[i * 3 + 2];
        Vector3 V1 = transform.TransformPoint(vertices2[vertexIndex1]);
        Vector3 V2 = transform.TransformPoint(vertices2[vertexIndex2]);
        Vector3 V3 = transform.TransformPoint(vertices2[vertexIndex3]);
        Vector3 normalA = normals[vertexIndex1];
        Vector3 normalB = normals[vertexIndex2];
        Vector3 normalC = normals[vertexIndex3];
        Vector3 triangleNormal = (normalA + normalB + normalC).normalized;

        if (!availableedges.ContainsKey(V1))
        {

            availableedges.Add(V1, new Dictionary<Vector3, List<RaycastHit>> { { V2, RaycastLineHit(V1, V2, triangleNormal) } });

            availableedges[V1].Add(V3, RaycastLineHit(V1, V3, triangleNormal));
        }
        else
        {

            if (!availableedges[V1].ContainsKey(V2))
                availableedges[V1].Add(V2, RaycastLineHit(V1, V2, triangleNormal));
            if (!availableedges[V1].ContainsKey(V3))
                availableedges[V1].Add(V3, RaycastLineHit(V1, V3, triangleNormal));


        }

        if (!availableedges.ContainsKey(V2))
        {

            availableedges.Add(V2, new Dictionary<Vector3, List<RaycastHit>> { { V1, RaycastLineHit(V2, V1, triangleNormal) } });

            availableedges[V2].Add(V3, RaycastLineHit(V2, V3, triangleNormal));
        }
        else
        {

            if (!availableedges[V2].ContainsKey(V1))
                availableedges[V2].Add(V1, RaycastLineHit(V2, V1, triangleNormal));
            if (!availableedges[V2].ContainsKey(V3))
                availableedges[V2].Add(V3, RaycastLineHit(V2, V3, triangleNormal));


        }

        if (!availableedges.ContainsKey(V3))
        {

            availableedges.Add(V3, new Dictionary<Vector3, List<RaycastHit>> { { V1, RaycastLineHit(V3, V1, triangleNormal) } });

            availableedges[V3].Add(V2, RaycastLineHit(V3, V2, triangleNormal));
        }
        else
        {

            if (!availableedges[V3].ContainsKey(V1))
                availableedges[V3].Add(V1, RaycastLineHit(V3, V1, triangleNormal));
            if (!availableedges[V3].ContainsKey(V2))
                availableedges[V3].Add(V2, RaycastLineHit(V3, V2, triangleNormal));


        }

    }
    public void UpdateColliderSelf(int i, int[] triangles, Vector3[] vertices2, Vector3[] normals)
    {
        int vertexIndex1 = triangles[i * 3];
        int vertexIndex2 = triangles[i * 3 + 1];
        int vertexIndex3 = triangles[i * 3 + 2];
        Vector3 V1 = transform.TransformPoint(vertices2[vertexIndex1]);
        Vector3 V2 = transform.TransformPoint(vertices2[vertexIndex2]);
        Vector3 V3 = transform.TransformPoint(vertices2[vertexIndex3]);
        Vector3 normalA = normals[vertexIndex1];
        Vector3 normalB = normals[vertexIndex2];
        Vector3 normalC = normals[vertexIndex3];
        Vector3 triangleNormal = (normalA + normalB + normalC).normalized;

        if (!availableedges.ContainsKey(V1))
        {

            availableedges.Add(V1, new Dictionary<Vector3, List<RaycastHit>> { { V2, RaycastLineHit(V1, V2, triangleNormal) } });

            availableedges[V1].Add(V3, RaycastLineSelf(V1, V3, triangleNormal));
        }
        else
        {

            if (!availableedges[V1].ContainsKey(V2))
                availableedges[V1].Add(V2, RaycastLineSelf(V1, V2, triangleNormal));
            if (!availableedges[V1].ContainsKey(V3))
                availableedges[V1].Add(V3, RaycastLineSelf(V1, V3, triangleNormal));


        }

        if (!availableedges.ContainsKey(V2))
        {

            availableedges.Add(V2, new Dictionary<Vector3, List<RaycastHit>> { { V1, RaycastLineSelf(V2, V1, triangleNormal) } });

            availableedges[V2].Add(V3, RaycastLineSelf(V2, V3, triangleNormal));
        }
        else
        {

            if (!availableedges[V2].ContainsKey(V1))
                availableedges[V2].Add(V1, RaycastLineSelf(V2, V1, triangleNormal));
            if (!availableedges[V2].ContainsKey(V3))
                availableedges[V2].Add(V3, RaycastLineSelf(V2, V3, triangleNormal));


        }

        if (!availableedges.ContainsKey(V3))
        {

            availableedges.Add(V3, new Dictionary<Vector3, List<RaycastHit>> { { V1, RaycastLineSelf(V3, V1, triangleNormal) } });

            availableedges[V3].Add(V2, RaycastLineSelf(V3, V2, triangleNormal));
        }
        else
        {

            if (!availableedges[V3].ContainsKey(V1))
                availableedges[V3].Add(V1, RaycastLineSelf(V3, V1, triangleNormal));
            if (!availableedges[V3].ContainsKey(V2))
                availableedges[V3].Add(V2, RaycastLineSelf(V3, V2, triangleNormal));


        }

    }

    private void AddVertex(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, int i)
    {
        if (!verticestriangle.ContainsKey(vertex1))
        {
            verticestriangle[vertex1] = new Dictionary<Vector3, Dictionary<Vector3, int>>();
        }

        if (!verticestriangle[vertex1].ContainsKey(vertex2))
        {
            verticestriangle[vertex1][vertex2] = new Dictionary<Vector3, int>();
        }

        if (!verticestriangle[vertex1][vertex2].ContainsKey(vertex3))
        {
            verticestriangle[vertex1][vertex2][vertex3] = i;
        }
    }

    public List<RaycastHit> RaycastLineBoth(Vector3 p1, Vector3 p2, Vector3 normal)
    {
        RaycastHit[] hit = Physics.RaycastAll(p1, p2 - p1, Vector3.Distance(p1, p2));
        RaycastHit[] hit2 = Physics.RaycastAll(p2, p1 - p2, Vector3.Distance(p2, p1));
        HashSet<RaycastHit> data = new HashSet<RaycastHit>();
        data.UnionWith(hit);
        data.UnionWith(hit2);
        List<RaycastHit> sortedData = data
            .OrderBy(point =>
                Mathf.InverseLerp(0, 1, Vector3.Distance(p2, point.point) / Vector3.Distance(p1, p2)))
            .ToList();

        // Remove the specific collider from the list
        sortedData.RemoveAll(hitResult => hitResult.collider == this.GetComponent<Collider>());
        string dat = "";
        if (!(sortedData.Count > 0))
            return sortedData;
        foreach (var item in sortedData)
        {
            dat += "////" + item.point;
            Plane plane = new Plane(item.normal, item.point);
            normal = Vector3.ProjectOnPlane(normal, item.normal);
            BooleanOperatorV7 boolean = item.collider.GetComponent<BooleanOperatorV7>();
            if (boolean != null)
            {
                if (boolean.vectornormals.ContainsKey(item.point))
                {
                    boolean.vectornormals[item.point] = (normal + boolean.vectornormals[item.point]);

                }
                else
                {
                    boolean.vectornormals.Add(item.point, normal);

                }
                if (boolean.availableTriangles.ContainsKey(item.triangleIndex))
                {
                    if (!boolean.availableTriangles[item.triangleIndex].Contains(item.point))
                    {
                        boolean.availableTriangles[item.triangleIndex].Add(item.point);

                    }



                }
                else
                {
                    boolean.availableTriangles.Add(item.triangleIndex, new List<Vector3> { item.point });

                }
            }

            if (!vectornormals.ContainsKey(item.point))
            {
                vectornormals.Add(item.point, vectornormals[p1] + vectornormals[p2]);
            }
            else
            {
                vectornormals[item.point] = (normal + vectornormals[item.point]);
            }

        }
        Debug.Log(p1 + "///" + p2 + dat);
        return sortedData;
    }
    public List<RaycastHit> RaycastLineSelf(Vector3 p1, Vector3 p2, Vector3 normal)
    {
        RaycastHit[] hit = Physics.RaycastAll(p1, p2 - p1, Vector3.Distance(p1, p2));
        RaycastHit[] hit2 = Physics.RaycastAll(p2, p1 - p2, Vector3.Distance(p2, p1));
        HashSet<RaycastHit> data = new HashSet<RaycastHit>();
        data.UnionWith(hit);
        data.UnionWith(hit2);
        List<RaycastHit> sortedData = data
            .OrderBy(point =>
                Mathf.InverseLerp(0, 1, Vector3.Distance(p2, point.point) / Vector3.Distance(p1, p2)))
            .ToList();

        // Remove the specific collider from the list
        sortedData.RemoveAll(hitResult => hitResult.collider == this.GetComponent<Collider>());
        string dat = "";
        if (!(sortedData.Count > 0))
            return sortedData;
        foreach (var item in sortedData)
        {


            if (!vectornormals.ContainsKey(item.point))
            {
                vectornormals.Add(item.point, vectornormals[p1] + vectornormals[p2]);
            }
            else
            {
                vectornormals[item.point] = (normal + vectornormals[item.point]);
            }

        }
        return sortedData;
    }
    public List<RaycastHit> RaycastLineHit(Vector3 p1, Vector3 p2, Vector3 normal)
    {
        RaycastHit[] hit = Physics.RaycastAll(p1, p2 - p1, Vector3.Distance(p1, p2));
        RaycastHit[] hit2 = Physics.RaycastAll(p2, p1 - p2, Vector3.Distance(p2, p1));
        HashSet<RaycastHit> data = new HashSet<RaycastHit>();
        data.UnionWith(hit);
        data.UnionWith(hit2);
        List<RaycastHit> sortedData = data
            .OrderBy(point =>
                Mathf.InverseLerp(0, 1, Vector3.Distance(p2, point.point) / Vector3.Distance(p1, p2)))
            .ToList();

        // Remove the specific collider from the list
        sortedData.RemoveAll(hitResult => hitResult.collider == this.GetComponent<Collider>());
        if (!(sortedData.Count > 0))
            return sortedData;
        foreach (var item in sortedData)
        {
            Plane plane = new Plane(item.normal, item.point);
            normal = Vector3.ProjectOnPlane(normal, item.normal);
            BooleanOperatorV7 boolean = item.collider.GetComponent<BooleanOperatorV7>();
            if (boolean != null)
            {
                if (boolean.vectornormals.ContainsKey(item.point))
                {
                    boolean.vectornormals[item.point] = (normal + boolean.vectornormals[item.point]);

                }
                else
                {
                    boolean.vectornormals.Add(item.point, normal);

                }
                if (boolean.availableTriangles.ContainsKey(item.triangleIndex))
                {
                    if (!boolean.availableTriangles[item.triangleIndex].Contains(item.point))
                    {
                        boolean.availableTriangles[item.triangleIndex].Add(item.point);

                    }



                }
                else
                {
                    boolean.availableTriangles.Add(item.triangleIndex, new List<Vector3> { item.point });

                }
            }



        }
        return sortedData;
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
