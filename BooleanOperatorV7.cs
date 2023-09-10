using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public bool resetDictionary;
    public bool resetTris;
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
        if (resetTris)
        {
            availableTriangles = new Dictionary<int, List<Vector3>>();
            verticestriangle = new Dictionary<Vector3, Dictionary<Vector3, Dictionary<Vector3, int>>>();
            resetTris = false;
        }

    }
    public void ResetDictionary()
    {
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
            Vector3 triangleNormal = (normalA + normalB + normalC).normalized;
            triangleDatas.Add(new BooleanHandlerV3.TriangleData(V1, V2, V3, triangleNormal));
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
            #region Edge connections

            if (!availableedges.ContainsKey(V1))
            {

                availableedges.Add(V1, new Dictionary<Vector3, List<RaycastHit>> { { V2, RaycastLine(V1, V2, triangleNormal) } });

                availableedges[V1].Add(V3, RaycastLine(V1, V3, triangleNormal));
            }
            else
            {

                if (!availableedges[V1].ContainsKey(V2))
                    availableedges[V1].Add(V2, RaycastLine(V1, V2, triangleNormal));
                if (!availableedges[V1].ContainsKey(V3))
                    availableedges[V1].Add(V3, RaycastLine(V1, V3, triangleNormal));


            }

            if (!availableedges.ContainsKey(V2))
            {

                availableedges.Add(V2, new Dictionary<Vector3, List<RaycastHit>> { { V1, RaycastLine(V2, V1, triangleNormal) } });

                availableedges[V2].Add(V3, RaycastLine(V2, V3, triangleNormal));
            }
            else
            {

                if (!availableedges[V2].ContainsKey(V1))
                    availableedges[V2].Add(V1, RaycastLine(V2, V1, triangleNormal));
                if (!availableedges[V2].ContainsKey(V3))
                    availableedges[V2].Add(V3, RaycastLine(V2, V3, triangleNormal));


            }

            if (!availableedges.ContainsKey(V3))
            {

                availableedges.Add(V3, new Dictionary<Vector3, List<RaycastHit>> { { V1, RaycastLine(V3, V1, triangleNormal) } });

                availableedges[V3].Add(V2, RaycastLine(V3, V2, triangleNormal));
            }
            else
            {

                if (!availableedges[V3].ContainsKey(V1))
                    availableedges[V3].Add(V1, RaycastLine(V1, V3, triangleNormal));
                if (!availableedges[V3].ContainsKey(V2))
                    availableedges[V3].Add(V2, RaycastLine(V3, V2, triangleNormal));


            }



            #endregion


            #region Vertex Triangles

            #region 1
            if (!verticestriangle.ContainsKey(V1))
            {
                verticestriangle.Add(V1,
                    new Dictionary<Vector3, Dictionary<Vector3, int>> { { V2,
                            new Dictionary<Vector3, int> { { V3, i } } } });
            }
            else
            {
                if (!verticestriangle[V1].ContainsKey(V2))
                {
                    verticestriangle[V1].Add(V2, new Dictionary<Vector3, int> { { V3, i } });
                }
                else
                {
                    if (!verticestriangle[V1][V2].ContainsKey(V3))
                    {
                        verticestriangle[V1][V2].Add(V3, i);

                    }

                }
            }

            if (!verticestriangle.ContainsKey(V1))
            {
                verticestriangle.Add(V1,
                    new Dictionary<Vector3, Dictionary<Vector3, int>> { { V3,
                            new Dictionary<Vector3, int> { { V2, i } } } });
            }
            else
            {
                if (!verticestriangle[V1].ContainsKey(V3))
                {
                    verticestriangle[V1].Add(V3, new Dictionary<Vector3, int> { { V2, i } });
                }
                else
                {
                    if (!verticestriangle[V1][V3].ContainsKey(V2))
                    {
                        verticestriangle[V1][V3].Add(V2, i);

                    }

                }
            }
            #endregion

            #region 2
            if (!verticestriangle.ContainsKey(V2))
            {
                verticestriangle.Add(V2,
                    new Dictionary<Vector3, Dictionary<Vector3, int>> { {V1,
                            new Dictionary<Vector3, int> { { V3, i } } } });
            }
            else
            {
                if (!verticestriangle[V2].ContainsKey(V1))
                {
                    verticestriangle[V2].Add(V1, new Dictionary<Vector3, int> { { V3, i } });
                }
                else
                {
                    if (!verticestriangle[V2][V1].ContainsKey(V3))
                    {
                        verticestriangle[V2][V1].Add(V3, i);

                    }

                }
            }

            if (!verticestriangle.ContainsKey(V2))
            {
                verticestriangle.Add(V2,
                    new Dictionary<Vector3, Dictionary<Vector3, int>> { { V3,
                            new Dictionary<Vector3, int> { {V1, i } } } });
            }
            else
            {
                if (!verticestriangle[V2].ContainsKey(V3))
                {
                    verticestriangle[V2].Add(V3, new Dictionary<Vector3, int> { { V1, i } });
                }
                else
                {
                    if (!verticestriangle[V2][V3].ContainsKey(V1))
                    {
                        verticestriangle[V2][V3].Add(V1, i);

                    }

                }
            }
            #endregion

            #region 3
            if (!verticestriangle.ContainsKey(V3))
            {
                verticestriangle.Add(V3,
                    new Dictionary<Vector3, Dictionary<Vector3, int>> { {V1,
                            new Dictionary<Vector3, int> { { V2, i } } } });
            }
            else
            {
                if (!verticestriangle[V3].ContainsKey(V1))
                {
                    verticestriangle[V3].Add(V1, new Dictionary<Vector3, int> { { V2, i } });
                }
                else
                {
                    if (!verticestriangle[V3][V1].ContainsKey(V2))
                    {
                        verticestriangle[V3][V1].Add(V2, i);

                    }

                }
            }

            if (!verticestriangle.ContainsKey(V3))
            {
                verticestriangle.Add(V3,
                    new Dictionary<Vector3, Dictionary<Vector3, int>> { { V2,
                            new Dictionary<Vector3, int> { {V1, i } } } });
            }
            else
            {
                if (!verticestriangle[V3].ContainsKey(V2))
                {
                    verticestriangle[V3].Add(V2, new Dictionary<Vector3, int> { { V1, i } });
                }
                else
                {
                    if (!verticestriangle[V3][V2].ContainsKey(V1))
                    {
                        verticestriangle[V3][V2].Add(V1, i);

                    }

                }
            }
            #endregion


            #endregion
        }
    }

    public List<RaycastHit> RaycastLine(Vector3 p1, Vector3 p2, Vector3 normal)
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
    public static List<RaycastHit> RaycastLine(Transform transform, Vector3 p1, Vector3 p2, out RaycastHit[] hit, out RaycastHit[] hit2, Collider colliderToRemove)
    {
        hit = Physics.RaycastAll(p1, p2 - p1, Vector3.Distance(p1, p2));
        hit2 = Physics.RaycastAll(p2, p1 - p2, Vector3.Distance(p2, p1));
        HashSet<RaycastHit> data = new HashSet<RaycastHit>();
        data.UnionWith(hit);
        data.UnionWith(hit2);
        List<RaycastHit> sortedData = data
            .OrderBy(point =>
                Mathf.InverseLerp(0, 1, Vector3.Distance(p2, point.point) / Vector3.Distance(p1, p2)))
            .ToList();

        // Remove the specific collider from the list
        sortedData.RemoveAll(hitResult => hitResult.collider == colliderToRemove);

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
