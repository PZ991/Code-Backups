using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class BrushInstancer : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Material> mats;
    public GameObject brush;
    public Camera cam;
    public float brushsize;
    public RenderTexture render;

    public List<GameObject> processor;

    public bool process;
    public Texture2D texture2D;

    public MeshRenderer finalrend;

    public int currentbrushindex;
    public float height;
    void Start()
    {
        finalrend.material.mainTexture = CreateBlackTexture(5, 5);

    }
    private Texture2D CreateBlackTexture(int width, int height)
    {
        Texture2D blackTexture = new Texture2D(width, height);

        // Create a black color.
        Color black = new Color(0, 0, 0, 0);

        // Set each pixel to the black color.
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = black;
        }

        // Apply the pixel data to the texture.
        blackTexture.SetPixels(pixels);

        // Make sure to call Apply() to actually apply the changes to the texture.
        blackTexture.Apply();

        return blackTexture;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            currentbrushindex = Mathf.Clamp(currentbrushindex - 1, 0, mats.Count - 1);
            Clearremove();

        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            currentbrushindex = Mathf.Clamp(currentbrushindex + 1, 0, mats.Count - 1);
            Clearremove();

        }
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;

            if (Utility.RayCastToMouse(cam, out hit))
            {
                height = Mathf.Max(hit.point.y + Vector3.up.y * 0.01f, height);
                var go = Instantiate(brush, new Vector3(hit.point.x, height, hit.point.z), Quaternion.identity);
                go.transform.localScale = Vector3.one * brushsize;
                processor.Add(go);





            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            foreach (var go in processor)
            {
                Material mat = new Material(mats[currentbrushindex]);
                Vector3 scale = go.transform.localScale;
                scale.y = scale.z;
                scale.z = 0;
                mat.mainTextureScale = scale;

                Vector3 pos = go.transform.position;
                pos.x = -pos.x;
                pos.y = pos.z;
                pos.y = -pos.y;
                pos.z = 0;
                mat.mainTextureOffset = Vector3.Scale(scale, pos);
                go.GetComponent<MeshRenderer>().material = mat;
            }
            StartCoroutine(Sav());

        }
    }
    private void LateUpdate()
    {

    }
    private void OnDrawGizmos()
    {

    }
    public void Save()
    {

        RenderTexture.active = render;

        texture2D = new Texture2D(render.width, render.height);
        texture2D.ReadPixels(new Rect(0, 0, render.width, render.height), 0, 0);
        texture2D.Apply();

        // var data = texture2d.EncodeToPNG();

        // File.WriteAllBytes(Application.dataPath + "/savedcanvas.png", data);
    }
    public void Clearremove()
    {/*
        foreach (var item in processor)
        {
            Destroy(item);
        }
        */
        processor.Clear();
    }
    public IEnumerator Sav()
    {
        yield return new WaitForEndOfFrame();
        Save();

        finalrend.material.mainTexture = texture2D;



    }
}
