using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class CustomNavAIV2 : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform start;
    public Transform end;
    public Transform reference;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrawGizmos()
    {
        /*
        NavMeshPath path = new NavMeshPath();

        // Calculate the path
        if (NavMesh.CalculatePath(start.position, end.position, NavMesh.AllAreas, path))
        {
            // Retrieve the corners of the path
            Vector3[] corners = path.corners;

            // Iterate over the corners and check for objects along the path
            for (int i = 0; i < corners.Length - 1; i++)
            {
                Vector3 from = corners[i];
                Vector3 to = corners[i + 1];

                RaycastHit[] hits = Physics.RaycastAll(from, Vector3.down,1);

                foreach (RaycastHit hit in hits)
                {
                    GameObject obj = hit.collider.gameObject;
                    Debug.Log("Object on path: " + obj.name);
                     
                }
                Gizmos.DrawSphere(corners[i], 0.5f);
            }
        }
        */
        
        if(end!=null)
        {
            Vector3 vertex1 = new Vector3(0, 0, 0); // Replace x1, y1, z1 with the coordinates of vertex 1
            Vector3 vertex2 = new Vector3(0, 5, 0); // Replace x2, y2, z2 with the coordinates of vertex 2
            Vector3 vertex3 = new Vector3(0, 0, 5); // Replace x3, y3, z3 with the coordinates of vertex 3
            Vector3 randomPoint = end.position;
           
          
            if (end != null)
            {
                // start.GetComponent<Collider>().ClosestPoint
                // Gizmos.DrawLine(end.position, Utility.ClosestPointOnMesh(start.GetComponent<MeshFilter>().mesh,end.position));
                // Gizmos.DrawSphere(Utility.ClosestPointOnMesh(start.GetComponent<MeshFilter>().mesh, end.position), 2);
                List<Vector3[]> vertices = GetColliderTriangles(start.GetComponent<Collider>());
                // List<Vector3> vertices2 = new List<Vector3>();
                //  start.GetComponent<MeshFilter>().mesh.GetVertices(vertices2) ;
                Vector3 closestPoint =Vector3.zero;
                for (int i = 0; i < vertices.Count; i++)
                {
                    vertex1 =  start.position + start.rotation * Vector3.Scale(vertices[i][0], start.lossyScale);
                    vertex2 =  start.position + start.rotation * Vector3.Scale(vertices[i][1], start.lossyScale);
                    vertex3 = start.position + start.rotation * Vector3.Scale(vertices[i][2], start.lossyScale);
                     Vector3 newclosestPoint;

                    Vector3 normal = Vector3.Cross(vertex2 - vertex1, vertex3 - vertex1).normalized;
                    float distance = Vector3.Dot(normal, vertex1);
                    Vector3 projectedPoint = randomPoint - (normal * (Vector3.Dot(normal, randomPoint) - distance));
                    bool isInsideTriangle = IsPointInsideTriangle(projectedPoint, vertex1, vertex2, vertex3);


                    if (i != 0)
                    {
                        if (isInsideTriangle)
                        {
                            newclosestPoint = projectedPoint;
                        }
                        else
                        {
                            newclosestPoint = FindClosestPoint(vertex1, vertex2, vertex1, vertex3, vertex2, vertex3, randomPoint);
                        }
                        if (Vector3.Distance(newclosestPoint, end.position) < Vector3.Distance(closestPoint, end.position))
                        {
                            closestPoint = newclosestPoint;
                        }
                    }
                    else
                    {
                        if (isInsideTriangle)
                        {
                            closestPoint = projectedPoint;
                        }
                        else
                        {
                            closestPoint = FindClosestPoint(vertex1, vertex2, vertex1, vertex3, vertex2, vertex3, randomPoint);
                        }
                    }
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(vertex1, 0.25f);
                    Gizmos.color = Color.blue;

                    Gizmos.DrawSphere(vertex2, 0.25f);
                    Gizmos.color = Color.yellow;

                    Gizmos.DrawSphere(vertex3, 0.25f);

                }
               

                

                Gizmos.DrawSphere(closestPoint, 0.25f);
                Gizmos.DrawLine(closestPoint, randomPoint); 

            }

           


        }



    }
    bool IsPointInsideTriangle(Vector3 point, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        Vector3 e1 = v2 - v1;
        Vector3 e2 = v3 - v1;
        Vector3 e3 = point - v1;

        float dot11 = Vector3.Dot(e1, e1);
        float dot12 = Vector3.Dot(e1, e2);
        float dot13 = Vector3.Dot(e1, e3);
        float dot22 = Vector3.Dot(e2, e2);
        float dot23 = Vector3.Dot(e2, e3);

        float invDenom = 1.0f / (dot11 * dot22 - dot12 * dot12);
        float u = (dot22 * dot13 - dot12 * dot23) * invDenom;
        float v = (dot11 * dot23 - dot12 * dot13) * invDenom;

        return (u >= 0) && (v >= 0) && (u + v <= 1);
    }



    public static Vector3 FindClosestPoint(Vector3 edge1Start, Vector3 edge1End, Vector3 edge2Start, Vector3 edge2End, Vector3 edge3Start, Vector3 edge3End, Vector3 randomPoint)
    {
        Vector3 closestPoint = Vector3.zero;
        float closestDistance = float.MaxValue;

        // Calculate the closest point on edge 1
        Vector3 closestPointEdge1 = CalculateClosestPointOnEdge(edge1Start, edge1End, randomPoint);
        float distanceEdge1 = Vector3.Distance(randomPoint, closestPointEdge1);

        // Check if this is the closest point so far
        if (distanceEdge1 < closestDistance)
        {
            closestDistance = distanceEdge1;
            closestPoint = closestPointEdge1;
        }

        // Calculate the closest point on edge 2
        Vector3 closestPointEdge2 = CalculateClosestPointOnEdge(edge2Start, edge2End, randomPoint);
        float distanceEdge2 = Vector3.Distance(randomPoint, closestPointEdge2);

        // Check if this is the closest point so far
        if (distanceEdge2 < closestDistance)
        {
            closestDistance = distanceEdge2;
            closestPoint = closestPointEdge2;
        }

        // Calculate the closest point on edge 3
        Vector3 closestPointEdge3 = CalculateClosestPointOnEdge(edge3Start, edge3End, randomPoint);
        float distanceEdge3 = Vector3.Distance(randomPoint, closestPointEdge3);

        // Check if this is the closest point so far
        if (distanceEdge3 < closestDistance)
        {
            closestDistance = distanceEdge3;
            closestPoint = closestPointEdge3;
        }

        return closestPoint;
    }

    public static Vector3 CalculateClosestPointOnEdge(Vector3 edgeStart, Vector3 edgeEnd, Vector3 randomPoint)
    {
        // Calculate the vector along the edge
        Vector3 edgeVector = edgeEnd - edgeStart;

        // Calculate the vector from the edge's start point to the random point
        Vector3 randomVector = randomPoint - edgeStart;

        // Calculate the distance along the edge to the closest point
        float t = Vector3.Dot(randomVector, edgeVector) / Vector3.Dot(edgeVector, edgeVector);
        t = Mathf.Clamp(t, 0, 1); // Ensure the closest point is within the edge's range

        // Calculate the closest point on the edge
        Vector3 pointOnEdge = edgeStart + t * edgeVector;

        return pointOnEdge;
    }


    public List<Vector3[]> GetColliderTriangles(Collider targetCollider)
    {
        List<Vector3[]> triangles = new List<Vector3[]>();

        MeshCollider meshCollider = targetCollider as MeshCollider;
        if (meshCollider != null)
        {
            Mesh mesh = meshCollider.sharedMesh;
            if (mesh != null)
            {
                Vector3[] vertices = mesh.vertices;
                int[] trianglesArray = mesh.triangles;

                for (int i = 0; i < trianglesArray.Length; i += 3)
                {
                    Vector3[] triangle = new Vector3[3];

                    triangle[0] = vertices[trianglesArray[i]];
                    triangle[1] = vertices[trianglesArray[i + 1]];
                    triangle[2] = vertices[trianglesArray[i + 2]];

                    triangles.Add(triangle);
                }
            }
        }

        return triangles;
    }
}
