using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class TriangleMaterial : MonoBehaviour
{

    public Camera cam;
    public float radius = 1f;
    public bool Red;
    public bool Green;
    public bool Blue;
    public MeshFilter meshFilter;

    public Vector3 vertexpositions;
    public Dictionary<Vector3, List<int>> verticestriangles = new Dictionary<Vector3, List<int>>();
    public List<Vector3> verticesavailable = new List<Vector3>();
    public bool refreshdictionary;
    public bool checkdictionary;

    public TMP_InputField opacitytext;
    public Slider opacityslider;

    public Toggle red;
    public Toggle green;
    public Toggle blue;

    private void Start()
    {
        ResetDictionary();
    }

    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawLine(cam.transform.position, hit.point);
                if (meshFilter != null)
                {
                    Mesh mesh = meshFilter.mesh;


                    Vector3 hitPoint = hit.point;
                    int[] verticesInRadius = GetVerticesInRadiusV2(verticesavailable.ToArray(), hitPoint, radius);
                    Color[] data = mesh.colors;
                    foreach (var item in verticesInRadius)
                    {
                        Debug.Log("Vertices in radius: " + string.Join(", ", item));
                        Color col;
                        if (Red)
                        {
                            col.r = Mathf.Lerp(0, 1, opacityslider.value * 0.001f);
                            data[item] = new Color(Mathf.Clamp(data[item].r + col.r, 0, 1), Mathf.Clamp(data[item].g - col.r, 0, 1), Mathf.Clamp(data[item].b - col.r, 0, 1));

                        }
                        else if (Blue)
                        {
                            col.b = Mathf.Lerp(0, 1, opacityslider.value * 0.01f);
                            data[item] = new Color(Mathf.Clamp(data[item].r - col.b, 0, 1), Mathf.Clamp(data[item].g - col.b, 0, 1), Mathf.Clamp(data[item].b + col.b, 0, 1));

                        }
                        else if (Green)
                        {
                            col.g = Mathf.Lerp(0, 1, opacityslider.value * 0.01f);
                            data[item] = new Color(Mathf.Clamp(data[item].r - col.g, 0, 1), Mathf.Clamp(data[item].g + col.g, 0, 1), Mathf.Clamp(data[item].b - col.g, 0, 1));

                        }

                    }
                    mesh.SetColors(data);
                    // Now you have the indices of the vertices within the radius.
                }
            }
        }
    }

    int[] GetVerticesInRadius(Vector3[] vertices, Vector3 center, float radius)
    {
        // List to store the indices of vertices within the radius.
        List<int> verticesInRadius = new List<int>();

        for (int i = 0; i < vertices.Length; i++)
        {
            float distance = Vector3.Distance(transform.TransformPoint(vertices[i]), center);
            if (distance <= radius)
            {
                verticesInRadius.Add(i);
            }
        }

        return verticesInRadius.ToArray();
    }

    int[] GetVerticesInRadiusV2(Vector3[] vertices, Vector3 center, float radius)
    {
        // List to store the indices of vertices within the radius.
        List<int> verticesInRadius = new List<int>();

        for (int i = 0; i < vertices.Length; i++)
        {
            float distance = Vector3.Distance(transform.TransformPoint(vertices[i]), center);
            if (distance <= radius)
            {
                if (verticestriangles.TryGetValue(vertices[i], out var triangleIndices))
                {
                    verticesInRadius.AddRange(triangleIndices);
                }
            }
        }

        return verticesInRadius.ToArray();
    }

    public void SetBooleans(int indexRGB)
    {
        if (indexRGB == 0)
        {
            Red = red.isOn;
            if (red.isOn)
            {
                Green = false;
                Blue = false;

            }
        }
        else if (indexRGB == 1)
        {
            Green = green.isOn;
            if (green.isOn)
            {
                Red = false;

                Blue = false;
            }
        }
        else if (indexRGB == 2)
        {

            Blue = blue.isOn;
            if (blue.isOn)
            {
                Red = false;
                Green = false;
            }

        }
    }
    public void SetOpacityFromSlider()
    {
        opacitytext.text = opacityslider.value.ToString();
    }
    public void SetOpacityFromText()
    {

        opacitytext.text = Mathf.Clamp(float.Parse(opacitytext.text), 0, 100).ToString();
        opacityslider.value = float.Parse(opacitytext.text);
    }


    public void ResetDictionary()
    {
        verticestriangles = new Dictionary<Vector3, List<int>>();
        verticesavailable = new List<Vector3>();
        Mesh mesh2 = meshFilter.mesh;
        Vector3[] vertices2 = mesh2.vertices;

        int[] triangles = mesh2.triangles;
        Vector3[] triangleVertices = new Vector3[triangles.Length];

        for (int i = 0; i < triangles.Length / 3; i++)
        {
            int vertexIndex1 = triangles[i * 3];
            int vertexIndex2 = triangles[i * 3 + 1];
            int vertexIndex3 = triangles[i * 3 + 2];

            // Store the vertices in the triangleVertices array
            if (!verticestriangles.ContainsKey(vertices2[vertexIndex1]))
            {
                verticestriangles.Add(vertices2[vertexIndex1], new List<int> { (vertexIndex1) });
                verticesavailable.Add(vertices2[vertexIndex1]);
            }
            else if (verticestriangles.ContainsKey(vertices2[vertexIndex1]))
            {
                if (!verticestriangles[vertices2[vertexIndex1]].Contains(vertexIndex1))
                    verticestriangles[vertices2[vertexIndex1]].Add(vertexIndex1);
            }

            if (!verticestriangles.ContainsKey(vertices2[vertexIndex2]))
            {
                verticestriangles.Add(vertices2[vertexIndex2], new List<int> { (vertexIndex2) });
                verticesavailable.Add(vertices2[vertexIndex2]);

            }
            else if (verticestriangles.ContainsKey(vertices2[vertexIndex2]))
            {
                if (!verticestriangles[vertices2[vertexIndex2]].Contains(vertexIndex2))
                    verticestriangles[vertices2[vertexIndex2]].Add(vertexIndex2);
            }

            if (!verticestriangles.ContainsKey(vertices2[vertexIndex3]))
            {
                verticesavailable.Add(vertices2[vertexIndex3]);

                verticestriangles.Add(vertices2[vertexIndex3], new List<int> { (vertexIndex3) });
            }
            else if (verticestriangles.ContainsKey(vertices2[vertexIndex3]))
            {
                if (!verticestriangles[vertices2[vertexIndex3]].Contains(vertexIndex3))
                    verticestriangles[vertices2[vertexIndex3]].Add(vertexIndex3);
            }

        }
    }
    private void OnDrawGizmos()
    {
        if (refreshdictionary)
        {
            ResetDictionary();
            refreshdictionary = false;
        }
        if (checkdictionary)
        {
            foreach (var item in verticestriangles)
            {
                string collection = "";
                foreach (var item2 in verticestriangles[item.Key])
                {
                    collection += " /" + item2;
                }
                Debug.Log(item.Key + "Collection of: " + collection);
            }
            checkdictionary = false;
        }
        Mesh mesh = meshFilter.mesh;
        Vector3[] vertices = mesh.vertices;
        Color[] data = mesh.colors;
        Color[] data2 = mesh.colors;

        if (data.Length != vertices.Length)
        {
            data = new Color[vertices.Length];
            data2 = new Color[vertices.Length];

        }

        for (int i = 0; i < vertices.Length; i++)
        {

            if (data[i].a == 1)
                data[i].a = 0;
            if (data2[i].a == 0)
                data2[i].a = 1;
            if (data[i] == new Color(0, 0, 0) || data[i] == new Color(0, 0, 0, 0))
                data[i] = Color.red;
            Gizmos.color = data2[i];
            //Gizmos.DrawSphere(vertices[i], 0.05f);
        }
        meshFilter.mesh.SetColors(data);
    }

    public static List<Vector3[]> GetTriangleVertices(MeshFilter meshFilter)
    {
        Mesh mesh2 = meshFilter.mesh;
        Vector3[] vertices2 = mesh2.vertices;
        int[] triangles = mesh2.triangles;
        List<Vector3[]> data = new List<Vector3[]>();
        for (int i = 0; i < triangles.Length / 3; i++)
        {
            Vector3[] triangleVertices = new Vector3[3];

            int vertexIndex1 = triangles[i * 3];
            int vertexIndex2 = triangles[i * 3 + 1];
            int vertexIndex3 = triangles[i * 3 + 2];

            // Store the vertices in the triangleVertices array
            triangleVertices[0] = vertices2[vertexIndex1];
            triangleVertices[1] = vertices2[vertexIndex2];
            triangleVertices[2] = vertices2[vertexIndex3];
            data.Add(triangleVertices);
        }
        return data;
    }
}
