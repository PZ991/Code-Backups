using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MatchVertexOrientation : MonoBehaviour
{
    public Camera cam;

    public Dictionary<int, List<Vector3>> availableTriangles;

    public Dictionary<Vector3, (int, List<int>)> availableVertices = new Dictionary<Vector3, (int, List<int>)>();

    public Dictionary<int, List<Vector3>> availableTriangles2;

    public Dictionary<Vector3, List<int>> availableVertices2 = new Dictionary<Vector3, List<int>>();

    public List<List<Vector3>> vertexlines = new List<List<Vector3>>();

    public int selectedline;
    public GameObject targetObject;
    public GameObject target;
    public Mesh oldmesh;

    public bool resetmesh;
    public bool setmesh;

    [Range(0, 1)]
    public float maxslider;
    [Range(1, 0)]
    public float mininfluence;
    bool changed;
    public Orientation orientation;

    public Transform VertexSelector;

    // Start is called before the first frame update
    void Start()
    {
        ResetDictionary();
        ResetDictionary2();
        vertexlines.Add(new List<Vector3>());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            selectedline = Mathf.Clamp(selectedline - 1, 0, vertexlines.Count);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            selectedline = Mathf.Clamp(selectedline + 1, 0, vertexlines.Count);

        }

        RaycastHit hit;
        if (Utility.RayCastToMouse(cam, out hit))
        {
            foreach (var item in availableVertices)
            {
                if (Vector3.Distance(item.Key, hit.point) < 0.5f)
                {
                    VertexSelector.localPosition = item.Key;
                    break;
                }
                else
                {
                    VertexSelector.localPosition = hit.point;
                }
            }
            if (Input.GetMouseButtonDown(0))
            {

                if (vertexlines.Count - 1 < selectedline)
                    vertexlines.Add(new List<Vector3>());
                foreach (var item in availableVertices)
                {
                    if (Vector3.Distance(item.Key, hit.point) < 0.5f)
                    {
                        if (!vertexlines[selectedline].Contains(item.Key))
                        {
                            vertexlines[selectedline].Add(item.Key);
                        }
                        break;
                    }
                }
            }
        }
        if (resetmesh)
        {
            Utility.SetMesh(target, oldmesh);
            resetmesh = false;
        }

        if (setmesh)
        {
            ResetDictionary();
            ResetDictionary2();
            Vector3 min, max;
            List<Vector3> verts = new List<Vector3>();
            foreach (var item in availableVertices2)
            {
                verts.Add(item.Key);
            }
            Utility.FindMinMax(verts.ToArray(), out min, out max);

            float maxdist = 0;
            Vector3[] data = Utility.GetBoundingBoxCorners(targetObject.GetComponent<MeshFilter>().mesh.bounds);
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Utility.ScaleMoveRotateVector(targetObject.transform, data[i]);
            }
            if (orientation == Orientation.Z)
            {

                float start = max.z;
                float end = min.z;
                float slidemax = Mathf.Lerp(start, end, maxslider);
                Vector3[] verts2 = Utility.GetMesh(target).vertices;
                foreach (var item in availableVertices2)
                {


                    if (item.Key.z <= slidemax)
                    {
                        //Debug.DrawRay(pos2 - targetObject.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.DrawRay(item.Key - target.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.Log(Mathf.InverseLerp(0, 1, item.Key.z));
                        //float slideinfluence = Mathf.Lerp(item.Key.x, pos1.x, Mathf.Lerp(1, mininfluence, Mathf.InverseLerp(0, slidemax, item.Key.x)));
                        // verts2[item.Value.Item1] = new Vector3(slideinfluence, item.Key.y, item.Key.z);

                        float slideinfluence = Mathf.Lerp(mininfluence, 1, Mathf.InverseLerp(max.z, slidemax, item.Key.z));
                        float influenceposX = Mathf.InverseLerp(max.x, min.x, item.Key.x);
                        float influenceposY = Mathf.InverseLerp(max.y, min.y, item.Key.y);

                        int startindex4 = Mathf.FloorToInt(influenceposY * (vertexlines.Count - 1));
                        int endindex4 = Mathf.CeilToInt(influenceposY * (vertexlines.Count - 1));

                        Vector3 p1 = Vector3.zero;
                        Vector3 p2 = Vector3.zero;

                        int startindex = Mathf.FloorToInt(influenceposX * (vertexlines[startindex4].Count - 1));
                        int endindex = Mathf.CeilToInt(influenceposX * (vertexlines[startindex4].Count - 1));
                        int startindex2 = Mathf.FloorToInt(influenceposX * (vertexlines[endindex4].Count - 1));
                        int endindex2 = Mathf.CeilToInt(influenceposX * (vertexlines[endindex4].Count - 1));

                        p1 = Vector3.Lerp(vertexlines[startindex4][startindex], vertexlines[startindex4][endindex], influenceposX * (vertexlines[startindex4].Count - 1) - startindex);
                        p2 = Vector3.Lerp(vertexlines[endindex4][startindex2], vertexlines[endindex4][endindex2], influenceposX * (vertexlines[endindex4].Count - 1) - startindex2);

                        Vector3 finalpos = Vector3.Lerp(p1, p2, influenceposY * (3 - 1) - startindex4);

                        // Debug.Log(Mathf.InverseLerp(max.z, slidemax, item.Key.z));
                        //Vector3 pos = Vector3.Lerp(data[7], data[3], influenceposX);
                        //Vector3 pos2 = Vector3.Lerp(data[5], data[1], influenceposX);
                        //Vector3 finalpos = Vector3.Lerp(pos, pos2, influenceposY);

                        //verts2[item.Value[0]] = Vector3.Lerp(target.transform.InverseTransformPoint(item.Key), target.transform.InverseTransformPoint(pos2), slideinfluence);
                        foreach (var item2 in item.Value)
                        {

                            verts2[item2] = Vector3.Lerp(target.transform.InverseTransformPoint(item.Key), target.transform.InverseTransformPoint(finalpos), slideinfluence);


                        }

                    }


                }
                Utility.SetVertices(target, verts2);
            }
            if (orientation == Orientation.X)
            {

                float start = max.x;
                float end = min.x;
                float slidemax = Mathf.Lerp(start, end, maxslider);
                Vector3[] verts2 = Utility.GetMesh(target).vertices;
                foreach (var item in availableVertices2)
                {


                    if (item.Key.x <= slidemax)
                    {
                        //Debug.DrawRay(pos2 - targetObject.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.DrawRay(item.Key - target.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.Log(Mathf.InverseLerp(0, 1, item.Key.z));
                        //float slideinfluence = Mathf.Lerp(item.Key.x, pos1.x, Mathf.Lerp(1, mininfluence, Mathf.InverseLerp(0, slidemax, item.Key.x)));
                        // verts2[item.Value.Item1] = new Vector3(slideinfluence, item.Key.y, item.Key.z);

                        float slideinfluence = Mathf.Lerp(mininfluence, 1, Mathf.InverseLerp(max.x, slidemax, item.Key.x));
                        float influenceposZ = Mathf.InverseLerp(max.z, min.z, item.Key.z);
                        float influenceposY = Mathf.InverseLerp(max.y, min.y, item.Key.y);
                        Debug.Log(Mathf.InverseLerp(max.z, slidemax, item.Key.z));
                        Vector3 pos = Vector3.Lerp(data[7], data[6], influenceposZ);
                        Vector3 pos2 = Vector3.Lerp(data[5], data[4], influenceposZ);
                        Vector3 finalpos = Vector3.Lerp(pos, pos2, influenceposY);

                        //verts2[item.Value[0]] = Vector3.Lerp(target.transform.InverseTransformPoint(item.Key) , target.transform.InverseTransformPoint(pos2), slideinfluence);
                        foreach (var item2 in item.Value)
                        {

                            verts2[item2] = Vector3.Lerp(target.transform.InverseTransformPoint(item.Key), target.transform.InverseTransformPoint(finalpos), slideinfluence);


                        }

                    }


                }
                Utility.SetVertices(target, verts2);
            }
            if (orientation == Orientation.Y)
            {

                float start = max.y;
                float end = min.y;
                float slidemax = Mathf.Lerp(start, end, maxslider);
                Vector3[] verts2 = Utility.GetMesh(target).vertices;
                foreach (var item in availableVertices2)
                {


                    if (item.Key.y <= slidemax)
                    {
                        //Debug.DrawRay(pos2 - targetObject.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.DrawRay(item.Key - target.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.Log(Mathf.InverseLerp(0, 1, item.Key.z));
                        //float slideinfluence = Mathf.Lerp(item.Key.x, pos1.x, Mathf.Lerp(1, mininfluence, Mathf.InverseLerp(0, slidemax, item.Key.x)));
                        // verts2[item.Value.Item1] = new Vector3(slideinfluence, item.Key.y, item.Key.z);

                        float slideinfluence = Mathf.Lerp(mininfluence, 1, Mathf.InverseLerp(max.y, slidemax, item.Key.y));
                        float influenceposZ = Mathf.InverseLerp(max.z, min.z, item.Key.z);
                        float influenceposY = Mathf.InverseLerp(max.x, min.x, item.Key.x);
                        Debug.Log(Mathf.InverseLerp(max.z, slidemax, item.Key.z));
                        Vector3 pos2 = Vector3.Lerp(data[3], data[2], influenceposZ);
                        Vector3 pos = Vector3.Lerp(data[7], data[6], influenceposZ);
                        Vector3 finalpos = Vector3.Lerp(pos, pos2, influenceposY);

                        //verts2[item.Value[0]] = Vector3.Lerp(target.transform.InverseTransformPoint(item.Key) , target.transform.InverseTransformPoint(pos2), slideinfluence);
                        foreach (var item2 in item.Value)
                        {

                            verts2[item2] = Vector3.Lerp(target.transform.InverseTransformPoint(item.Key), target.transform.InverseTransformPoint(finalpos), slideinfluence);


                        }

                    }


                }
                Utility.SetVertices(target, verts2);
            }
            if (orientation == Orientation.N_Y)
            {

                float start = min.y;
                float end = max.y;
                float slidemax = Mathf.Lerp(start, end, maxslider);
                Vector3[] verts2 = Utility.GetMesh(target).vertices;
                foreach (var item in availableVertices2)
                {


                    if (item.Key.y >= slidemax)
                    {
                        //Debug.DrawRay(pos2 - targetObject.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.DrawRay(item.Key - target.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.Log(Mathf.InverseLerp(0, 1, item.Key.z));
                        //float slideinfluence = Mathf.Lerp(item.Key.x, pos1.x, Mathf.Lerp(1, mininfluence, Mathf.InverseLerp(0, slidemax, item.Key.x)));
                        // verts2[item.Value.Item1] = new Vector3(slideinfluence, item.Key.y, item.Key.z);

                        float slideinfluence = Mathf.Lerp(mininfluence, 1, Mathf.InverseLerp(min.y, slidemax, item.Key.y));
                        float influenceposZ = Mathf.InverseLerp(max.z, min.z, item.Key.z);
                        float influenceposY = Mathf.InverseLerp(max.x, min.x, item.Key.x);
                        Debug.Log(Mathf.InverseLerp(max.z, slidemax, item.Key.z));
                        Vector3 pos2 = Vector3.Lerp(data[1], data[0], influenceposZ);
                        Vector3 pos = Vector3.Lerp(data[5], data[4], influenceposZ);
                        Vector3 finalpos = Vector3.Lerp(pos, pos2, influenceposY);

                        //verts2[item.Value[0]] = Vector3.Lerp(target.transform.InverseTransformPoint(item.Key) , target.transform.InverseTransformPoint(pos2), slideinfluence);
                        foreach (var item2 in item.Value)
                        {

                            verts2[item2] = Vector3.Lerp(target.transform.InverseTransformPoint(item.Key), target.transform.InverseTransformPoint(finalpos), slideinfluence);


                        }

                    }


                }
                Utility.SetVertices(target, verts2);
            }
            if (orientation == Orientation.N_X)
            {

                float start = min.x;
                float end = max.x;
                float slidemax = Mathf.Lerp(start, end, maxslider);
                Vector3[] verts2 = Utility.GetMesh(target).vertices;
                foreach (var item in availableVertices2)
                {


                    if (item.Key.x >= slidemax)
                    {
                        //Debug.DrawRay(pos2 - targetObject.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.DrawRay(item.Key - target.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.Log(Mathf.InverseLerp(0, 1, item.Key.z));
                        //float slideinfluence = Mathf.Lerp(item.Key.x, pos1.x, Mathf.Lerp(1, mininfluence, Mathf.InverseLerp(0, slidemax, item.Key.x)));
                        // verts2[item.Value.Item1] = new Vector3(slideinfluence, item.Key.y, item.Key.z);

                        float slideinfluence = Mathf.Lerp(mininfluence, 1, Mathf.InverseLerp(min.x, slidemax, item.Key.x));
                        float influenceposZ = Mathf.InverseLerp(max.z, min.z, item.Key.z);
                        float influenceposY = Mathf.InverseLerp(max.y, min.y, item.Key.y);
                        Debug.Log(Mathf.InverseLerp(max.z, slidemax, item.Key.z));
                        Vector3 pos = Vector3.Lerp(data[3], data[2], influenceposZ);
                        Vector3 pos2 = Vector3.Lerp(data[1], data[0], influenceposZ);
                        Vector3 finalpos = Vector3.Lerp(pos, pos2, influenceposY);

                        //verts2[item.Value[0]] = Vector3.Lerp(target.transform.InverseTransformPoint(item.Key) , target.transform.InverseTransformPoint(pos2), slideinfluence);
                        foreach (var item2 in item.Value)
                        {

                            verts2[item2] = Vector3.Lerp(target.transform.InverseTransformPoint(item.Key), target.transform.InverseTransformPoint(finalpos), slideinfluence);


                        }

                    }


                }
                Utility.SetVertices(target, verts2);
            }
            if (orientation == Orientation.N_Z)
            {

                float start = min.z;
                float end = max.z;
                float slidemax = Mathf.Lerp(start, end, maxslider);
                Vector3[] verts2 = Utility.GetMesh(target).vertices;
                foreach (var item in availableVertices2)
                {


                    if (item.Key.z >= slidemax)
                    {
                        //Debug.DrawRay(pos2 - targetObject.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.DrawRay(item.Key - target.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.Log(Mathf.InverseLerp(0, 1, item.Key.z));
                        //float slideinfluence = Mathf.Lerp(item.Key.x, pos1.x, Mathf.Lerp(1, mininfluence, Mathf.InverseLerp(0, slidemax, item.Key.x)));
                        // verts2[item.Value.Item1] = new Vector3(slideinfluence, item.Key.y, item.Key.z);

                        float slideinfluence = Mathf.Lerp(mininfluence, 1, Mathf.InverseLerp(min.z, slidemax, item.Key.z));
                        float influenceposX = Mathf.InverseLerp(max.x, min.x, item.Key.x);
                        float influenceposY = Mathf.InverseLerp(max.y, min.y, item.Key.y);
                        Debug.Log(Mathf.InverseLerp(max.z, slidemax, item.Key.z));
                        Vector3 pos = Vector3.Lerp(data[6], data[2], influenceposX);
                        Vector3 pos2 = Vector3.Lerp(data[4], data[0], influenceposX);
                        Vector3 finalpos = Vector3.Lerp(pos, pos2, influenceposY);

                        //verts2[item.Value[0]] = Vector3.Lerp(target.transform.InverseTransformPoint(item.Key), target.transform.InverseTransformPoint(pos2), slideinfluence);
                        foreach (var item2 in item.Value)
                        {

                            verts2[item2] = Vector3.Lerp(target.transform.InverseTransformPoint(item.Key), target.transform.InverseTransformPoint(finalpos), slideinfluence);


                        }

                    }


                }
                Utility.SetVertices(target, verts2);
            }
            setmesh = false;
        }

    }
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.black;
        for (int i = 0; i < vertexlines.Count; i++)
        {
            for (int j = 1; j < vertexlines[i].Count; j++)
            {
                Gizmos.DrawSphere(vertexlines[i][j], 0.1f);
                Gizmos.DrawLine(vertexlines[i][j - 1], vertexlines[i][j]);

                if (j == 1)
                    Gizmos.DrawSphere(vertexlines[i][j - 1], 0.1f);
            }
        }
    }
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

        }
    }
    public void ResetDictionary2()
    {
        availableVertices2 = new Dictionary<Vector3, List<int>>();
        availableTriangles2 = new Dictionary<int, List<Vector3>>();
        Mesh mesh2 = Utility.GetMesh(target);
        Vector3[] vertices2 = mesh2.vertices;

        int[] triangles = mesh2.triangles;
        Vector3[] triangleVertices = new Vector3[triangles.Length];
        for (int i = 0; i < triangles.Length / 3; i++)
        {
            int vertexIndex1 = triangles[i * 3];
            int vertexIndex2 = triangles[i * 3 + 1];
            int vertexIndex3 = triangles[i * 3 + 2];

            availableTriangles2.Add(i, new List<Vector3>() { Utility.ScaleMoveRotateVector(target.transform, vertices2[vertexIndex1]), Utility.ScaleMoveRotateVector(target.transform, vertices2[vertexIndex2]), Utility.ScaleMoveRotateVector(target.transform, vertices2[vertexIndex3]) });

            // Store the vertices in the triangleVertices array
            if (!availableVertices2.ContainsKey(Utility.ScaleMoveRotateVector(target.transform, vertices2[vertexIndex1])))
            {
                availableVertices2.Add(Utility.ScaleMoveRotateVector(target.transform, vertices2[vertexIndex1]), new List<int> { (vertexIndex1) });
            }
            else if (availableVertices2.ContainsKey(Utility.ScaleMoveRotateVector(target.transform, vertices2[vertexIndex1])))
            {
                if (!availableVertices2[Utility.ScaleMoveRotateVector(target.transform, vertices2[vertexIndex1])].Contains(vertexIndex1))
                    availableVertices2[Utility.ScaleMoveRotateVector(target.transform, vertices2[vertexIndex1])].Add(vertexIndex1);
            }

            if (!availableVertices2.ContainsKey(Utility.ScaleMoveRotateVector(target.transform, vertices2[vertexIndex2])))
            {
                availableVertices2.Add(Utility.ScaleMoveRotateVector(target.transform, vertices2[vertexIndex2]), new List<int> { (vertexIndex2) });

            }
            else if (availableVertices2.ContainsKey(Utility.ScaleMoveRotateVector(target.transform, vertices2[vertexIndex2])))
            {
                if (!availableVertices2[Utility.ScaleMoveRotateVector(target.transform, vertices2[vertexIndex2])].Contains(vertexIndex2))
                    availableVertices2[Utility.ScaleMoveRotateVector(target.transform, vertices2[vertexIndex2])].Add(vertexIndex2);
            }

            if (!availableVertices2.ContainsKey(Utility.ScaleMoveRotateVector(target.transform, vertices2[vertexIndex3])))
            {
                availableVertices2.Add(Utility.ScaleMoveRotateVector(target.transform, vertices2[vertexIndex3]), new List<int> { (vertexIndex3) });
            }
            else if (availableVertices2.ContainsKey(Utility.ScaleMoveRotateVector(target.transform, vertices2[vertexIndex3])))
            {
                if (!availableVertices2[Utility.ScaleMoveRotateVector(target.transform, vertices2[vertexIndex3])].Contains(vertexIndex3))
                    availableVertices2[Utility.ScaleMoveRotateVector(target.transform, vertices2[vertexIndex3])].Add(vertexIndex3);
            }

        }
    }

}
