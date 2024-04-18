using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BooleanOperatorV3 : MonoBehaviour
{
    // Triangle index & Vertex Positions (Triangles)
    public Dictionary<int, List<Vector3>> availableTriangles;

    //Vertex number of inside collisions and Positions & Index that have the same position
    public Dictionary<Vector3, (int, List<int>)> availableVertices = new Dictionary<Vector3, (int, List<int>)>();

    //Connection of vertices to other vertices (Edge)
    public Dictionary<Vector3, (int, List<Vector3>)> availableConnections = new Dictionary<Vector3, (int, List<Vector3>)>();

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

    }
    public void ResetDictionary()
    {
        availableVertices = new Dictionary<Vector3, (int, List<int>)>();
        availableTriangles = new Dictionary<int, List<Vector3>>();
        availableConnections = new Dictionary<Vector3, (int, List<Vector3>)>();
        Mesh mesh2 = Utility.GetMesh(targetObject);
        Vector3[] vertices2 = mesh2.vertices;

        int[] triangles = mesh2.triangles;
        Vector3[] triangleVertices = new Vector3[triangles.Length];
        for (int i = 0; i < triangles.Length / 3; i++)
        {
            int vertexIndex1 = triangles[i * 3];
            int vertexIndex2 = triangles[i * 3 + 1];
            int vertexIndex3 = triangles[i * 3 + 2];

            availableTriangles.Add(i, new List<Vector3>() { vertices2[vertexIndex1], vertices2[vertexIndex2], vertices2[vertexIndex3] });

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

            #region Vertices Similarity
            // Store the vertices in the triangleVertices array
            if (!availableVertices.ContainsKey(vertices2[vertexIndex1]))
            {
                availableVertices.Add(vertices2[vertexIndex1], (0, new List<int> { (vertexIndex1) }));
            }
            else if (availableVertices.ContainsKey(vertices2[vertexIndex1]))
            {
                if (!availableVertices[vertices2[vertexIndex1]].Item2.Contains(vertexIndex1))
                    availableVertices[vertices2[vertexIndex1]].Item2.Add(vertexIndex1);
            }

            if (!availableVertices.ContainsKey(vertices2[vertexIndex2]))
            {
                availableVertices.Add(vertices2[vertexIndex2], (0, new List<int> { (vertexIndex2) }));

            }
            else if (availableVertices.ContainsKey(vertices2[vertexIndex2]))
            {
                if (!availableVertices[vertices2[vertexIndex2]].Item2.Contains(vertexIndex2))
                    availableVertices[vertices2[vertexIndex2]].Item2.Add(vertexIndex2);
            }

            if (!availableVertices.ContainsKey(vertices2[vertexIndex3]))
            {
                availableVertices.Add(vertices2[vertexIndex3], (0, new List<int> { (vertexIndex3) }));
            }
            else if (availableVertices.ContainsKey(vertices2[vertexIndex3]))
            {
                if (!availableVertices[vertices2[vertexIndex3]].Item2.Contains(vertexIndex3))
                    availableVertices[vertices2[vertexIndex3]].Item2.Add(vertexIndex3);
            }
            #endregion
        }
    }
    /*
    public void ResetDictionary()
    {
        availableVertices = new Dictionary<Vector3, (int, List<int>)>();
        availableTriangles = new Dictionary<int, List<Vector3>>();
        Mesh mesh2 = Utility.GetMesh(targetObject);
        Vector3[] vertices2 = mesh2.vertices;

        int[] triangles = mesh2.triangles;
        Vector3[] triangleVertices = new Vector3[triangles.Length];
        for (int i = 0; i < triangles.Length / 3; i++)
        {
            int vertexIndex1 = triangles[i * 3];
            int vertexIndex2 = triangles[i * 3 + 1];
            int vertexIndex3 = triangles[i * 3 + 2];

            availableTriangles.Add(i, new List<Vector3>() { Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex1]), Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex2]), Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex3]) });

            #region Edge connections

            if(!availableConnections.ContainsKey(Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex1])))
            {
                availableConnections.Add(Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex1]),new List<Vector3> { vertices2[vertexIndex2], vertices2[vertexIndex3] });

            }
            else
            {
                if (!availableConnections[Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex1])].Contains(vertices2[vertexIndex1]))
                    availableConnections[Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex1])].Add(vertices2[vertexIndex1]);
                else if (!availableConnections[Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex1])].Contains(vertices2[vertexIndex3]))
                    availableConnections[Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex1])].Add(vertices2[vertexIndex3]);

            }
            if(!availableConnections.ContainsKey(Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex2])))
            {
                availableConnections.Add(Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex2]),new List<Vector3> { vertices2[vertexIndex1], vertices2[vertexIndex3] });

            }
            else
            {
                if (!availableConnections[Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex2])].Contains(vertices2[vertexIndex1]))
                    availableConnections[Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex2])].Add(vertices2[vertexIndex1]);
                else if (!availableConnections[Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex2])].Contains(vertices2[vertexIndex3]))
                    availableConnections[Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex2])].Add(vertices2[vertexIndex3]);

            }

            if(!availableConnections.ContainsKey(Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex3])))
            {
                availableConnections.Add(Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex3]),new List<Vector3> { vertices2[vertexIndex1], vertices2[vertexIndex2] });

            }
            else
            {
                if (!availableConnections[Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex3])].Contains(vertices2[vertexIndex1]))
                    availableConnections[Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex3])].Add(vertices2[vertexIndex1]);
                else if (!availableConnections[Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex3])].Contains(vertices2[vertexIndex2]))
                    availableConnections[Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex3])].Add(vertices2[vertexIndex2]);

            }

            #endregion

            #region Vertices Similarity
            // Store the vertices in the triangleVertices array
            if (!availableVertices.ContainsKey(Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex1])))
            {
                availableVertices.Add(Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex1]), (vertexIndex1, new List<int> { (vertexIndex1) }));
            }
            else if (availableVertices.ContainsKey(Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex1])))
            {
                if (!availableVertices[Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex1])].Item2.Contains(vertexIndex1))
                    availableVertices[Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex1])].Item2.Add(vertexIndex1);
            }

            if (!availableVertices.ContainsKey(Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex2])))
            {
                availableVertices.Add(Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex2]), (vertexIndex2, new List<int> { (vertexIndex2) }));

            }
            else if (availableVertices.ContainsKey(Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex2])))
            {
                if (!availableVertices[Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex2])].Item2.Contains(vertexIndex2))
                    availableVertices[Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex2])].Item2.Add(vertexIndex2);
            }

            if (!availableVertices.ContainsKey(Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex3])))
            {
                availableVertices.Add(Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex3]), (vertexIndex3, new List<int> { (vertexIndex3) }));
            }
            else if (availableVertices.ContainsKey(Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex3])))
            {
                if (!availableVertices[Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex3])].Item2.Contains(vertexIndex3))
                    availableVertices[Utility.ScaleMoveRotateVector(targetObject.transform, vertices2[vertexIndex3])].Item2.Add(vertexIndex3);
            }
            #endregion
        }
    }
    */
}
