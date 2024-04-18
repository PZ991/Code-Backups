using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTexture : MonoBehaviour
{
    public Color color;
    public Camera cam;
    public Vector2 add;
    public List<int> index;
    public Mesh mesh;
    public float radius;
    public List<Vector2> textureCoordsInRadius;
    public bool faceclamp;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // Get the current mouse position in screen coordinates.
            Vector3 mousePosition = Input.mousePosition;

            // Cast a ray from the camera to the mouse position to find the corresponding world point.
            Ray ray = cam.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                #region Pixeltexture

                Debug.DrawLine(hit.point, cam.transform.position, Color.magenta);
                // Get the texture on the object's material at the hit point.
                Renderer renderer = hit.transform.GetComponent<Renderer>();
                Material material = renderer.material;

                // Get the texture coordinates (UV) at the hit point.
                Vector2 textureCoord = hit.textureCoord;

                // Get the main texture from the material (assuming it's a Texture2D).
                Texture2D texture = (Texture2D)material.mainTexture;

                // Calculate the pixel coordinates (multiply by texture width and height).
                int pixelX = (int)(textureCoord.x * texture.width);
                int pixelY = (int)(textureCoord.y * texture.height);
                //  Debug.Log(textureCoord.x + "/" + textureCoord.y);
                Color pixelColor = texture.GetPixel(pixelX, pixelY);

                #region Radius
                textureCoordsInRadius = new List<Vector2>();

                for (float x = textureCoord.x - radius; x <= textureCoord.x + radius; x += 0.00f)
                {
                    for (float y = textureCoord.y - radius; y <= textureCoord.y + radius; y += 0.001f)
                    {
                        if (x >= 1 || x <= 0)
                            continue;
                        if (y >= 1 || y <= 0)
                            continue;
                        float distance = Vector2.Distance(new Vector2(x, y), textureCoord);
                        if (distance > radius)
                            continue;
                        if (faceclamp)
                        {
                            int[] triangles = mesh.triangles;
                            Vector2[] uvs = mesh.uv;
                            int uvIndex1 = triangles[hit.triangleIndex * 3];
                            int uvIndex2 = triangles[hit.triangleIndex * 3 + 1];
                            int uvIndex3 = triangles[hit.triangleIndex * 3 + 2];
                            Vector2 uv1 = uvs[uvIndex1];
                            Vector2 uv2 = uvs[uvIndex2];
                            Vector2 uv3 = uvs[uvIndex3];
                            Debug.DrawLine(cam.transform.position, uv1, Color.green);
                            Debug.DrawLine(cam.transform.position, uv2, Color.green);
                            Debug.DrawLine(cam.transform.position, uv3, Color.green);
                            if (!Utility.IsPointInTriangle(new Vector2(x, y), uv1, uv2, uv3))
                                continue;

                        }

                        if (distance <= radius)
                        {
                            textureCoordsInRadius.Add(new Vector2(x, y));
                        }
                    }
                }



                #endregion


                Debug.DrawLine(textureCoord, cam.transform.position, Color.magenta);

                // Debug.Log("Pixel Color: " + pixelColor);

                // Sample the pixel color at the calculated pixel coordinates.
                color = texture.GetPixel(pixelX, pixelY);

                #endregion
            }
        }
    }
    private void OnDrawGizmos()
    {

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector2[] uvs = mesh.uv;
        int[] triangles = mesh.triangles;

        if (Physics.Raycast(ray, out hit))
        {
            Gizmos.DrawSphere(hit.point, radius * 5);
        }


        List<int> indexediting = new List<int>();
        for (int k = 0; k < index.Count; k++)
        {


            int triangleIndex = index[k];
            // Get the UVs of the triangle
            if (!indexediting.Contains(triangles[triangleIndex * 3]))
            {

                int uvIndex1 = triangles[triangleIndex * 3];
                uvs[uvIndex1] += add;
                indexediting.Add(uvIndex1);
            }
            if (!indexediting.Contains(triangles[triangleIndex * 3 + 1]))
            {
                int uvIndex2 = triangles[triangleIndex * 3 + 1];
                uvs[uvIndex2] += add;
                indexediting.Add(uvIndex2);

            }
            if (!indexediting.Contains(triangles[triangleIndex * 3 + 2]))
            {
                int uvIndex3 = triangles[triangleIndex * 3 + 2];
                uvs[uvIndex3] += add;
                indexediting.Add(uvIndex3);

            }
        }


        Debug.Log(uvs.Length);
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Gizmos.color = Color.blue;

            int vertexIndex1 = triangles[i];
            int vertexIndex2 = triangles[i + 1];
            int vertexIndex3 = triangles[i + 2];

            Vector2 uv1 = uvs[vertexIndex1];
            Vector2 uv2 = uvs[vertexIndex2];
            Vector2 uv3 = uvs[vertexIndex3];
            Gizmos.DrawLine(uv3, uv1);
            Gizmos.DrawLine(uv1, uv2);
            Gizmos.DrawLine(uv3, uv2);
        }
        for (int n = 0; n < indexediting.Count; n++)
        {

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.TransformPoint(mesh.vertices[indexediting[n]]), 0.1f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(mesh.uv[indexediting[n]], 0.03f);

        }

        for (int i = 0; i < uvs.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(uvs[i], 0.01f);

        }
    }

}
