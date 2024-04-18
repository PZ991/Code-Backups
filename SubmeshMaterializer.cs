using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SubmeshMaterializer : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Material> materials;
    public int currentindex;
    public Camera cam;
    public int index;
    public bool process;
    public Mesh originalmesh;
    public MeshRenderer displayer;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Mesh mesh2 = originalmesh;
        Mesh mesh = Utility.GetMesh(gameObject);
        int[] triangles = mesh.triangles;
        int[] triangles2 = mesh2.triangles;
        Vector3[] vertices = mesh.vertices;



        MeshRenderer meshRenderer = Utility.GetMeshRenderer(gameObject);
        if (materials.Count != meshRenderer.materials.Length)
        {
            meshRenderer.materials = materials.ToArray();
            mesh.subMeshCount = materials.Count;
        }
        if (Input.GetKeyDown(KeyCode.O))
            currentindex = Mathf.Clamp(currentindex + 1, 0, materials.Count - 1);
        if (Input.GetKeyDown(KeyCode.P))
            currentindex = Mathf.Clamp(currentindex - 1, 0, materials.Count - 1);
        displayer.material = meshRenderer.materials[currentindex];
        Debug.Log(mesh.triangles.Length);
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;

            if (Utility.RayCastToMouse(cam, out hit))
            {
                if (hit.triangleIndex != index)
                {
                    int triangleIndex = hit.triangleIndex;

                    int index1 = triangles2[triangleIndex * 3];
                    int index2 = triangles2[triangleIndex * 3 + 1];
                    int index3 = triangles2[triangleIndex * 3 + 2];
                    Vector3 vertex1 = vertices[index1];
                    Vector3 vertex2 = vertices[index2];
                    Vector3 vertex3 = vertices[index3];

                    List<List<int>> submeshdata = new List<List<int>>();

                    List<List<Vector3>> submeshVertices = new List<List<Vector3>>();

                    for (int i = 0; i < mesh.subMeshCount; i++)
                    {
                        List<int> submeshTriangles = new List<int>(mesh.GetTriangles(i));
                        submeshdata.Add(submeshTriangles);

                        List<Vector3> submeshVerts = new List<Vector3>();
                        foreach (int vertexIndex in submeshTriangles)
                        {
                            submeshVerts.Add(mesh.vertices[vertexIndex]);
                        }

                        // Add the submesh vertices data to the submeshVertices list.
                        submeshVertices.Add(submeshVerts);
                    }

                    int totalsubmesh = 0;
                    for (int i = 0; i < submeshVertices.Count; i++)
                    {
                        for (int j = 0; j < submeshVertices[i].Count; j += 3)
                        {

                            if ((submeshVertices[i][j] == vertex1) && (submeshVertices[i][j + 1] == vertex2) && (submeshVertices[i][j + 2] == vertex3))
                            //if (Utility.IsPointInTriangle(hit.point, submeshVertices[i][j], submeshVertices[i][j + 1], submeshVertices[i][j + 2]))
                            {
                                if (i != currentindex)
                                {
                                    submeshdata[currentindex].Insert(0, (submeshdata[i][j + 2]));
                                    submeshdata[currentindex].Insert(0, (submeshdata[i][j + 1]));
                                    submeshdata[currentindex].Insert(0, submeshdata[i][j]);
                                    submeshdata[i].RemoveAt(j + 2);
                                    submeshdata[i].RemoveAt(j + 1);
                                    submeshdata[i].RemoveAt(j);


                                }
                            }

                        }
                        totalsubmesh += submeshdata[i].Count;
                    }
                    // Set the modified submesh data (triangles) for each submesh
                    //mesh.triangles = triangles2.ToArray();
                    for (int i = 0; i < submeshdata.Count; i++)
                    {
                        mesh.SetTriangles(submeshdata[i].ToArray(), i);
                    }

                    index = hit.triangleIndex;
                }
            }

        }
    }
    private void OnDrawGizmos()
    {
        Mesh mesh2 = originalmesh;
        Mesh mesh = Utility.GetMesh(gameObject);
        int triangleIndex = index;
        int[] triangles = mesh.triangles;
        int[] triangles2 = mesh2.triangles;
        Vector3[] vertices = mesh.vertices;

        RaycastHit hit;
        if (Utility.RayCastToMouse(cam, out hit))
        {
            int triangleIndex2 = hit.triangleIndex;

            int index12 = triangles2[triangleIndex2 * 3];
            int index22 = triangles2[triangleIndex2 * 3 + 1];
            int index32 = triangles2[triangleIndex2 * 3 + 2];
            Vector3 vertex13 = vertices[index12];
            Vector3 vertex23 = vertices[index22];
            Vector3 vertex33 = vertices[index32];
            Gizmos.DrawSphere(vertex13, 0.1f);
            Gizmos.DrawSphere(vertex23, 0.1f);
            Gizmos.DrawSphere(vertex33, 0.1f);
        }
        /*

        int index1 = triangles[triangleIndex * 3];
        int index2 = triangles[triangleIndex * 3 + 1];
        int index3 = triangles[triangleIndex * 3 + 2];
        List<int> triangles2 = new List<int>(triangles);
        Vector3 vertex1 = vertices[index1];
        Vector3 vertex2 = vertices[index2];
        Vector3 vertex3 = vertices[index3];
        */
        // int[] dat = mesh.GetTriangles(currentindex);
        int index1 = triangles2[triangleIndex * 3];
        int index2 = triangles2[triangleIndex * 3 + 1];
        int index3 = triangles2[triangleIndex * 3 + 2];
        Vector3 vertex1 = vertices[index1];
        Vector3 vertex2 = vertices[index2];
        Vector3 vertex3 = vertices[index3];
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(vertex1, 0.1f);
        Gizmos.DrawSphere(vertex2, 0.1f);
        Gizmos.DrawSphere(vertex3, 0.1f);


        if (process)
        {
            /*
            if (materials.Count != meshRenderer.materials.Length)
            {
                meshRenderer.materials = materials.ToArray();
                mesh.subMeshCount = materials.Count;
            }
            */

            List<List<int>> submeshdata = new List<List<int>>();

            List<List<Vector3>> submeshVertices = new List<List<Vector3>>();

            for (int i = 0; i < mesh.subMeshCount; i++)
            {
                List<int> submeshTriangles = new List<int>(mesh.GetTriangles(i));
                submeshdata.Add(submeshTriangles);

                List<Vector3> submeshVerts = new List<Vector3>();
                foreach (int vertexIndex in submeshTriangles)
                {
                    submeshVerts.Add(mesh.vertices[vertexIndex]);
                }

                // Add the submesh vertices data to the submeshVertices list.
                submeshVertices.Add(submeshVerts);
            }

            int totalsubmesh = 0;
            for (int i = 0; i < submeshVertices.Count; i++)
            {
                for (int j = 0; j < submeshVertices[i].Count; j += 3)
                {

                    if ((submeshVertices[i][j] == vertex1) && (submeshVertices[i][j + 1] == vertex2) && (submeshVertices[i][j + 2] == vertex3))
                    //if (Utility.IsPointInTriangle(hit.point, submeshVertices[i][j], submeshVertices[i][j + 1], submeshVertices[i][j + 2]))
                    {

                        submeshdata[currentindex].Insert(0, (submeshdata[i][j + 2]));
                        submeshdata[currentindex].Insert(0, (submeshdata[i][j + 1]));
                        submeshdata[currentindex].Insert(0, submeshdata[i][j]);
                        submeshdata[i].RemoveAt(j + 2);
                        submeshdata[i].RemoveAt(j + 1);
                        submeshdata[i].RemoveAt(j);



                    }

                }
                totalsubmesh += submeshdata[i].Count;
            }
            // Set the modified submesh data (triangles) for each submesh
            //mesh.triangles = triangles2.ToArray();
            for (int i = 0; i < submeshdata.Count; i++)
            {
                mesh.SetTriangles(submeshdata[i].ToArray(), i);
            }

            process = false;


        }
        else
        {
            List<List<int>> submeshdata = new List<List<int>>();

            List<List<Vector3>> submeshVertices = new List<List<Vector3>>();

            for (int i = 0; i < mesh.subMeshCount; i++)
            {
                List<int> submeshTriangles = new List<int>(mesh.GetTriangles(i));
                submeshdata.Add(submeshTriangles);

                List<Vector3> submeshVerts = new List<Vector3>();
                foreach (int vertexIndex in submeshTriangles)
                {
                    submeshVerts.Add(mesh.vertices[vertexIndex]);
                }

                // Add the submesh vertices data to the submeshVertices list.
                submeshVertices.Add(submeshVerts);
            }

            int totalsubmesh = 0;
            for (int i = 0; i < submeshdata.Count; i++)
            {
                for (int j = 0; j < submeshdata[i].Count; j += 3)
                {

                    if ((submeshVertices[i][j] == vertex1) && (submeshVertices[i][j + 1] == vertex2) && (submeshVertices[i][j + 2] == vertex3))
                    //if (Utility.IsPointInTriangle(hit.point, submeshVertices[i][j], submeshVertices[i][j + 1], submeshVertices[i][j + 2]))
                    {

                        Debug.Log("submesh: " + i + " indexes: " + j + "/" + (j + 1) + "/" + (j + 2) + " from real indexes: " + index1 + "/" + index2 + "/" + index3 + " offset count: " + totalsubmesh);
                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(vertex1, 0.2f);
                        Gizmos.DrawSphere(vertex2, 0.2f);
                        Gizmos.DrawSphere(vertex3, 0.2f);



                    }

                    /*
                    if (Utility.IsPointInTriangle(hit., transform.TransformPoint(submeshVertices[i][j]), transform.TransformPoint(submeshVertices[i][j + 1]), transform.TransformPoint(submeshVertices[i][j + 2])))
                    {

                        Gizmos.color = Color.green;
                        Gizmos.DrawSphere(transform.TransformPoint(submeshVertices[i][j]), 0.2f);
                        Gizmos.DrawSphere(transform.TransformPoint(submeshVertices[i][j + 1]), 0.2f);
                        Gizmos.DrawSphere(transform.TransformPoint(submeshVertices[i][j + 2]), 0.2f);



                    }
                    */

                }
                totalsubmesh += submeshdata[i].Count;
            }
        }
    }
}
