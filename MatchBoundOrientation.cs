using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchBoundOrientation : MonoBehaviour
{
    public Camera cam;

    public Dictionary<int, List<Vector3>> availableTriangles;

    public Dictionary<Vector3, (int, List<int>)> availableVertices = new Dictionary<Vector3, (int, List<int>)>();

    public Dictionary<int, List<Vector3>> availableTriangles2;

    public Dictionary<Vector3, List<int>> availableVertices2 = new Dictionary<Vector3, List<int>>();


    public GameObject targetObject;
    public GameObject target;
    public Mesh targetoldmesh;
    public Mesh targetobjectoldmesh;
    public bool autodetectorientation;
    public bool autoscale;
    public bool autoraycast;
    public bool autoraycasttotarget;
    public bool autoraycasttotargetcenter;
    public bool resetmesh;
    public bool setmesh;

    [Range(0, 1)]
    public float maxslider;
    [Range(1, 0)]
    public float mininfluence;
    bool changed;
    public Orientation orientation;
    public float power;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (resetmesh)
        {
            Utility.SetMesh(target, targetoldmesh);
            resetmesh = false;
        }

        if (setmesh)
        {
            //Fix Adjust Maxslider on N-
            //make auto detect orientation

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
            Vector3[] data = Utility.GetBoundingBoxCorners(targetobjectoldmesh.bounds);
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Utility.ScaleMoveRotateVector(targetObject.transform, data[i]);
                Debug.DrawRay(data[i], Vector3.up, Color.black, 10);
            }
            if (autodetectorientation)
            {
                orientation = Utility.GetNearestDirectionCardinalOrientation(targetObject.transform, target.transform.position - targetObject.transform.position);
            }
            if (orientation == Orientation.Z)
            {

                float start = min.z;
                float end = max.z;
                float slidemax = Mathf.Lerp(start, end, maxslider);
                Vector3[] verts2 = Utility.GetMesh(target).vertices;
                foreach (var item in availableVertices2)
                {
                    //7351
                    //3276
                    int first = 7, second = 3, third = 5, fourth = 1;
                    float minX = min.x, minY = min.y, minZ = min.z;
                    float maxX = max.x, maxY = max.y, maxZ = max.z;
                    float targetX = item.Key.x, targetY = item.Key.y, targetZ = item.Key.z;
                    Vector3 dirZ = Utility.GetNearestDirectionCardinal(targetObject.transform, target.transform.forward);
                    Vector3 dirX = Utility.GetNearestDirectionCardinal(targetObject.transform, target.transform.right);
                    Vector3 dirY = Utility.GetNearestDirectionCardinal(targetObject.transform, target.transform.up);
                    bool powX = true, powY = true, powZ = false;
                    bool inverse = false;
                    if (dirX == -targetObject.transform.forward)
                    {
                        if (dirY == targetObject.transform.right)
                        {
                            first = 5;
                            second = 7;
                            third = 1;
                            fourth = 3;
                        }
                        else if (dirY == -targetObject.transform.up)
                        {
                            first = 1;
                            second = 5;
                            third = 3;
                            fourth = 7;
                        }
                        else if (dirY == targetObject.transform.up)
                        {
                            first = 7;
                            second = 3;
                            third = 5;
                            fourth = 1;
                        }

                        else
                        {
                            first = 3;
                            second = 1;
                            third = 7;
                            fourth = 5;
                        }
                        minX = min.x;
                        minY = min.y;
                        minZ = min.z;
                        maxX = max.x;
                        maxY = max.z;
                        maxZ = max.y;
                        targetX = item.Key.z;
                        targetY = item.Key.y;
                        targetZ = item.Key.x;
                        powX = false;
                        powY = true;
                        powZ = true;
                        inverse = true;
                    }
                    else if (dirX == targetObject.transform.forward)
                    {
                        if (dirY == targetObject.transform.right)
                        {
                            first = 7;
                            second = 5;
                            third = 3;
                            fourth = 1;
                        }
                        else if (dirY == -targetObject.transform.up)
                        {
                            first = 5;
                            second = 1;
                            third = 7;
                            fourth = 3;
                        }
                        else if (dirY == targetObject.transform.up)
                        {
                            first = 3;
                            second = 7;
                            third = 1;
                            fourth = 5;
                        }

                        else
                        {
                            first = 1;
                            second = 3;
                            third = 5;
                            fourth = 7;
                        }
                        minX = min.x;
                        minY = min.y;
                        minZ = min.z;
                        maxX = max.x;
                        maxY = max.z;
                        maxZ = max.y;
                        targetX = item.Key.z;
                        targetY = item.Key.y;
                        targetZ = item.Key.x;
                        powX = true;
                        powY = false;
                        powZ = true;
                    }
                    else if (dirY == -targetObject.transform.forward)
                    {
                        Debug.Log("upside");

                        if (dirZ == -targetObject.transform.right)
                        {
                            first = 3;
                            second = 7;
                            third = 1;
                            fourth = 5;
                        }
                        else if (dirZ == targetObject.transform.up)
                        {
                            first = 7;
                            second = 5;
                            third = 3;
                            fourth = 1;
                        }
                        else if (dirZ == -targetObject.transform.up)
                        {
                            first = 1;
                            second = 3;
                            third = 5;
                            fourth = 7;
                        }

                        else
                        {
                            first = 5;
                            second = 1;
                            third = 7;
                            fourth = 3;
                        }
                        minX = min.y;
                        minY = min.x;
                        minZ = min.z;
                        maxX = max.y;
                        maxY = max.x;
                        maxZ = max.z;
                        targetX = item.Key.z;
                        targetY = item.Key.x;
                        targetZ = item.Key.y;
                        powX = true;
                        powY = false;
                        powZ = true;
                        inverse = true;

                    }
                    else if (dirY == targetObject.transform.forward)
                    {
                        if (dirZ == -targetObject.transform.right)
                        {
                            first = 1;
                            second = 5;
                            third = 3;
                            fourth = 7;
                        }
                        else if (dirZ == targetObject.transform.up)
                        {
                            first = 3;
                            second = 1;
                            third = 7;
                            fourth = 5;
                        }
                        else if (dirZ == -targetObject.transform.up)
                        {
                            first = 5;
                            second = 7;
                            third = 1;
                            fourth = 3;
                        }

                        else
                        {
                            first = 7;
                            second = 3;
                            third = 5;
                            fourth = 1;
                        }
                        minX = min.y;
                        minY = min.x;
                        minZ = min.z;
                        maxX = max.y;
                        maxY = max.x;
                        maxZ = max.z;
                        targetX = item.Key.z;
                        targetY = item.Key.x;
                        targetZ = item.Key.y;
                        powX = false;
                        powY = true;
                        powZ = true;
                    }
                    else if (dirZ == -targetObject.transform.forward)
                    {
                        if (dirX == -targetObject.transform.right)
                        {
                            first = 3;
                            second = 7;
                            third = 1;
                            fourth = 5;
                        }
                        else if (dirX == targetObject.transform.up)
                        {
                            first = 7;
                            second = 5;
                            third = 3;
                            fourth = 1;
                        }
                        else if (dirX == -targetObject.transform.up)
                        {
                            first = 1;
                            second = 3;
                            third = 5;
                            fourth = 7;
                        }

                        else
                        {
                            first = 5;
                            second = 1;
                            third = 7;
                            fourth = 3;
                        }
                        minX = min.x;
                        minY = min.y;
                        minZ = min.z;
                        maxX = max.x;
                        maxY = max.y;
                        maxZ = max.z;
                        targetX = item.Key.x;
                        targetY = item.Key.y;
                        targetZ = item.Key.z;
                        inverse = true;
                    }

                    else
                    {
                        if (dirX == -targetObject.transform.right)
                        {
                            first = 1;
                            second = 5;
                            third = 3;
                            fourth = 7;
                        }
                        else if (dirX == targetObject.transform.up)
                        {
                            first = 3;
                            second = 1;
                            third = 7;
                            fourth = 5;
                        }
                        else if (dirX == -targetObject.transform.up)
                        {
                            first = 5;
                            second = 7;
                            third = 1;
                            fourth = 3;
                        }

                        else
                        {
                            Debug.Log("This");
                            first = 7;
                            second = 3;
                            third = 5;
                            fourth = 1;
                        }
                        minX = min.x;
                        minY = min.y;
                        minZ = min.z;
                        maxX = max.x;
                        maxY = max.y;
                        maxZ = max.z;
                        targetX = item.Key.x;
                        targetY = item.Key.y;
                        targetZ = item.Key.z;
                    }

                    if (inverse)
                    {
                        start = max.z;
                        end = min.z;
                        slidemax = Mathf.Lerp(start, end, maxslider);
                    }
                    if (inverse ? targetZ >= slidemax : targetZ <= slidemax)
                    {
                        //Debug.DrawRay(pos2 - targetObject.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.DrawRay(item.Key - target.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.Log(Mathf.InverseLerp(0, 1, item.Key.z));
                        //float slideinfluence = Mathf.Lerp(item.Key.x, pos1.x, Mathf.Lerp(1, mininfluence, Mathf.InverseLerp(0, slidemax, item.Key.x)));
                        // verts2[item.Value.Item1] = new Vector3(slideinfluence, item.Key.y, item.Key.z);
                        float slideinfluence = Mathf.Lerp(1, mininfluence, Mathf.InverseLerp(inverse ? maxZ : minZ, slidemax, targetZ));

                        //float slideinfluence = Mathf.Lerp(1, mininfluence, Mathf.InverseLerp(min.z, slidemax, item.Key.z));
                        float powslideinfluence = Mathf.Clamp01(Mathf.Pow(slideinfluence, power));
                        float influenceposX = Mathf.InverseLerp(maxX, minX, targetX);
                        float influenceposY = Mathf.InverseLerp(maxY, minY, targetY);
                        Debug.Log(Mathf.InverseLerp(max.z, slidemax, targetZ));
                        Vector3 pos = Vector3.Lerp(data[first], data[second], influenceposX);
                        Vector3 pos2 = Vector3.Lerp(data[third], data[fourth], influenceposX);
                        Vector3 finalpos = Vector3.Lerp(pos, pos2, influenceposY);

                        //verts2[item.Value[0]] = Vector3.Lerp(target.transform.InverseTransformPoint(item.Key), target.transform.InverseTransformPoint(pos2), slideinfluence);
                        foreach (var item2 in item.Value)
                        {


                            float x = Mathf.Lerp(target.transform.InverseTransformPoint(Utility.ScaleMoveRotateVector(target.transform, item.Key)).x, target.transform.InverseTransformPoint(finalpos).x, powX ? powslideinfluence : slideinfluence);
                            float y = Mathf.Lerp(target.transform.InverseTransformPoint(Utility.ScaleMoveRotateVector(target.transform, item.Key)).y, target.transform.InverseTransformPoint(finalpos).y, powY ? powslideinfluence : slideinfluence);
                            float z = Mathf.Lerp(target.transform.InverseTransformPoint(Utility.ScaleMoveRotateVector(target.transform, item.Key)).z, target.transform.InverseTransformPoint(finalpos).z, powZ ? powslideinfluence : slideinfluence);
                            verts2[item2] = new Vector3(x, y, z);


                        }

                    }


                }
                Utility.SetVertices(target, verts2);
            }
            if (orientation == Orientation.X)
            {

                float start = min.x;
                float end = max.x;
                float slidemax = Mathf.Lerp(start, end, maxslider);
                Vector3[] verts2 = Utility.GetMesh(target).vertices;
                foreach (var item in availableVertices2)
                {
                    //3,2,7,6
                    int first = 7, second = 5, third = 6, fourth = 4;
                    float minX = min.x, minY = min.y, minZ = min.z;
                    float maxX = max.x, maxY = max.y, maxZ = max.z;
                    float targetX = item.Key.x, targetY = item.Key.y, targetZ = item.Key.z;
                    Vector3 dirZ = Utility.GetNearestDirectionCardinal(targetObject.transform, target.transform.forward);
                    Vector3 dirX = Utility.GetNearestDirectionCardinal(targetObject.transform, target.transform.right);
                    Vector3 dirY = Utility.GetNearestDirectionCardinal(targetObject.transform, target.transform.up);
                    bool powX = false, powY = true, powZ = true;
                    bool inverse = false;
                    if (dirX == -targetObject.transform.right)
                    {
                        if (dirZ == -targetObject.transform.forward)
                        {
                            first = 6;
                            second = 7;
                            third = 4;
                            fourth = 5;
                        }
                        else if (dirZ == -targetObject.transform.up)
                        {
                            first = 4;
                            second = 6;
                            third = 5;
                            fourth = 7;
                        }
                        else if (dirZ == targetObject.transform.up)
                        {
                            first = 7;
                            second = 5;
                            third = 6;
                            fourth = 4;
                        }

                        else
                        {
                            first = 5;
                            second = 4;
                            third = 7;
                            fourth = 6;
                        }
                        minX = min.x;
                        minY = min.z;
                        minZ = min.y;
                        maxX = max.x;
                        maxY = max.z;
                        maxZ = max.y;
                        targetX = item.Key.x;
                        targetY = item.Key.z;
                        targetZ = item.Key.y;
                        powX = false;
                        powY = true;
                        powZ = true;
                        inverse = true;
                    }
                    else if (dirX == targetObject.transform.right)
                    {
                        if (dirZ == targetObject.transform.forward)
                        {
                            first = 7;
                            second = 6;
                            third = 5;
                            fourth = 4;
                        }
                        else if (dirZ == targetObject.transform.up)
                        {
                            first = 6;
                            second = 4;
                            third = 7;
                            fourth = 5;
                        }
                        else if (dirZ == -targetObject.transform.up)
                        {
                            first = 5;
                            second = 7;
                            third = 4;
                            fourth = 6;
                        }

                        else
                        {
                            first = 4;
                            second = 5;
                            third = 6;
                            fourth = 7;
                        }
                        minX = min.x;
                        minY = min.z;
                        minZ = min.y;
                        maxX = max.x;
                        maxY = max.z;
                        maxZ = max.y;
                        targetX = item.Key.x;
                        targetY = item.Key.z;
                        targetZ = item.Key.y;
                        powX = false;
                        powY = true;
                        powZ = true;
                    }
                    else if (dirZ == targetObject.transform.right)
                    {
                        Debug.Log("upside");

                        if (dirX == targetObject.transform.up)
                        {
                            first = 7;
                            second = 6;
                            third = 5;
                            fourth = 4;
                        }
                        else if (dirX == targetObject.transform.forward)
                        {
                            first = 5;
                            second = 7;
                            third = 4;
                            fourth = 6;
                        }
                        else if (dirX == -targetObject.transform.forward)
                        {
                            first = 6;
                            second = 4;
                            third = 7;
                            fourth = 5;
                        }

                        else
                        {
                            first = 4;
                            second = 5;
                            third = 6;
                            fourth = 7;
                        }
                        minX = min.y;
                        minY = min.x;
                        minZ = min.z;
                        maxX = max.y;
                        maxY = max.x;
                        maxZ = max.z;
                        targetX = item.Key.z;
                        targetY = item.Key.y;
                        targetZ = item.Key.x;
                        powX = true;
                        powY = true;
                        powZ = false;
                    }
                    else if (dirZ == -targetObject.transform.right)
                    {
                        Debug.Log("upside");

                        if (dirX == targetObject.transform.up)
                        {
                            first = 6;
                            second = 7;
                            third = 4;
                            fourth = 5;
                        }
                        else if (dirX == targetObject.transform.forward)
                        {
                            first = 7;
                            second = 5;
                            third = 6;
                            fourth = 4;
                        }
                        else if (dirX == -targetObject.transform.forward)
                        {
                            first = 4;
                            second = 6;
                            third = 5;
                            fourth = 7;
                        }

                        else
                        {
                            first = 5;
                            second = 4;
                            third = 7;
                            fourth = 6;
                        }
                        minX = min.y;
                        minY = min.x;
                        minZ = min.z;
                        maxX = max.y;
                        maxY = max.x;
                        maxZ = max.z;
                        targetX = item.Key.z;
                        targetY = item.Key.y;
                        targetZ = item.Key.x;
                        powX = true;
                        powY = true;
                        powZ = false;
                        inverse = true;
                    }
                    else if (dirY == -targetObject.transform.right)
                    {
                        if (dirX == targetObject.transform.forward)
                        {
                            first = 5;
                            second = 4;
                            third = 7;
                            fourth = 6;
                        }
                        else if (dirX == -targetObject.transform.up)
                        {
                            first = 4;
                            second = 6;
                            third = 5;
                            fourth = 7;
                        }
                        else if (dirX == targetObject.transform.up)
                        {
                            first = 7;
                            second = 5;
                            third = 6;
                            fourth = 4;
                        }

                        else
                        {
                            first = 6;
                            second = 7;
                            third = 4;
                            fourth = 5;
                        }
                        minX = min.x;
                        minY = min.y;
                        minZ = min.z;
                        maxX = max.x;
                        maxY = max.y;
                        maxZ = max.z;
                        targetX = item.Key.y;
                        targetY = item.Key.x;
                        targetZ = item.Key.z;
                        inverse = true;
                        powX = true;
                        powY = false;
                        powZ = true;
                    }
                    else
                    {
                        if (dirX == targetObject.transform.up)
                        {
                            first = 6;
                            second = 4;
                            third = 7;
                            fourth = 5;
                        }
                        else if (dirX == -targetObject.transform.forward)
                        {
                            first = 4;
                            second = 5;
                            third = 6;
                            fourth = 7;
                        }
                        else if (dirX == targetObject.transform.forward)
                        {
                            first = 7;
                            second = 6;
                            third = 5;
                            fourth = 4;
                        }

                        else
                        {
                            first = 5;
                            second = 7;
                            third = 4;
                            fourth = 6;
                        }
                        minX = min.x;
                        minY = min.y;
                        minZ = min.z;
                        maxX = max.x;
                        maxY = max.y;
                        maxZ = max.z;
                        targetX = item.Key.y;
                        targetY = item.Key.x;
                        targetZ = item.Key.z;
                        powX = true;
                        powY = false;
                        powZ = true;
                    }

                    if (inverse)
                    {
                        start = max.x;
                        end = min.x;
                        slidemax = Mathf.Lerp(start, end, maxslider);
                    }
                    if (inverse ? targetX >= slidemax : targetX <= slidemax)
                    {

                        //Debug.DrawRay(pos2 - targetObject.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.DrawRay(item.Key - target.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.Log(Mathf.InverseLerp(0, 1, item.Key.z));
                        //float slideinfluence = Mathf.Lerp(item.Key.x, pos1.x, Mathf.Lerp(1, mininfluence, Mathf.InverseLerp(0, slidemax, item.Key.x)));
                        // verts2[item.Value.Item1] = new Vector3(slideinfluence, item.Key.y, item.Key.z);
                        float slideinfluence = Mathf.Lerp(1, mininfluence, Mathf.InverseLerp(inverse ? maxX : minX, slidemax, targetX));

                        // float slideinfluence = Mathf.Lerp(1, mininfluence, Mathf.InverseLerp(min.x, slidemax, item.Key.x));
                        float powslideinfluence = Mathf.Clamp01(Mathf.Pow(slideinfluence, power));
                        float influenceposZ = Mathf.InverseLerp(maxZ, minZ, targetZ);
                        float influenceposY = Mathf.InverseLerp(maxY, minY, targetY);
                        Vector3 pos = Vector3.Lerp(data[first], data[second], influenceposY);
                        Vector3 pos2 = Vector3.Lerp(data[third], data[fourth], influenceposY);
                        Vector3 finalpos = Vector3.Lerp(pos, pos2, influenceposZ);

                        //verts2[item.Value[0]] = Vector3.Lerp(target.transform.InverseTransformPoint(item.Key) , target.transform.InverseTransformPoint(pos2), slideinfluence);
                        foreach (var item2 in item.Value)
                        {

                            float x = Mathf.Lerp(target.transform.InverseTransformPoint(Utility.ScaleMoveRotateVector(target.transform, item.Key)).x, target.transform.InverseTransformPoint(finalpos).x, powX ? powslideinfluence : slideinfluence);
                            float y = Mathf.Lerp(target.transform.InverseTransformPoint(Utility.ScaleMoveRotateVector(target.transform, item.Key)).y, target.transform.InverseTransformPoint(finalpos).y, powY ? powslideinfluence : slideinfluence);
                            float z = Mathf.Lerp(target.transform.InverseTransformPoint(Utility.ScaleMoveRotateVector(target.transform, item.Key)).z, target.transform.InverseTransformPoint(finalpos).z, powZ ? powslideinfluence : slideinfluence);

                            verts2[item2] = new Vector3(x, y, z);

                        }

                    }


                }
                Utility.SetVertices(target, verts2);
            }
            if (orientation == Orientation.Y)
            {

                float start = min.y;
                float end = max.y;
                float slidemax = Mathf.Lerp(start, end, maxslider);
                Vector3[] verts2 = Utility.GetMesh(target).vertices;
                foreach (var item in availableVertices2)
                {

                    int first = 3, second = 2, third = 7, fourth = 6;
                    float minX = min.x, minY = min.y, minZ = min.z;
                    float maxX = max.x, maxY = max.y, maxZ = max.z;
                    float targetX = item.Key.x, targetY = item.Key.y, targetZ = item.Key.z;
                    Vector3 dirZ = Utility.GetNearestDirectionCardinal(targetObject.transform, target.transform.forward);
                    Vector3 dirX = Utility.GetNearestDirectionCardinal(targetObject.transform, target.transform.right);
                    Vector3 dirY = Utility.GetNearestDirectionCardinal(targetObject.transform, target.transform.up);
                    bool powX = true, powY = false, powZ = true;
                    bool inverse = false;
                    if (dirZ == -targetObject.transform.up)
                    {
                        if (dirX == -targetObject.transform.forward)
                        {
                            first = 7;
                            second = 3;
                            third = 6;
                            fourth = 2;
                        }
                        else if (dirX == -targetObject.transform.right)
                        {
                            first = 6;
                            second = 7;
                            third = 2;
                            fourth = 3;
                        }
                        else if (dirX == targetObject.transform.right)
                        {
                            first = 3;
                            second = 2;
                            third = 7;
                            fourth = 6;
                        }

                        else
                        {
                            first = 2;
                            second = 6;
                            third = 3;
                            fourth = 7;
                        }
                        minX = min.x;
                        minY = min.z;
                        minZ = min.y;
                        maxX = max.x;
                        maxY = max.z;
                        maxZ = max.y;
                        targetX = item.Key.x;
                        targetY = item.Key.z;
                        targetZ = item.Key.y;
                        powX = true;
                        powY = true;
                        powZ = false;
                        inverse = true;
                    }
                    else if (dirZ == targetObject.transform.up)
                    {
                        if (dirX == -targetObject.transform.forward)
                        {
                            first = 3;
                            second = 7;
                            third = 2;
                            fourth = 6;
                        }
                        else if (dirX == -targetObject.transform.right)
                        {
                            first = 7;
                            second = 6;
                            third = 3;
                            fourth = 2;
                        }
                        else if (dirX == targetObject.transform.right)
                        {
                            first = 2;
                            second = 3;
                            third = 6;
                            fourth = 7;
                        }

                        else
                        {
                            first = 6;
                            second = 2;
                            third = 7;
                            fourth = 3;
                        }
                        minX = min.x;
                        minY = min.z;
                        minZ = min.y;
                        maxX = max.x;
                        maxY = max.z;
                        maxZ = max.y;
                        targetX = item.Key.x;
                        targetY = item.Key.z;
                        targetZ = item.Key.y;
                        powX = true;
                        powY = true;
                        powZ = false;
                    }
                    else if (dirX == targetObject.transform.up)
                    {
                        Debug.Log("upside");

                        if (dirZ == -targetObject.transform.forward)
                        {
                            first = 2;
                            second = 3;
                            third = 6;
                            fourth = 7;
                        }
                        else if (dirZ == -targetObject.transform.right)
                        {
                            first = 3;
                            second = 7;
                            third = 2;
                            fourth = 6;
                        }
                        else if (dirZ == targetObject.transform.right)
                        {
                            first = 6;
                            second = 2;
                            third = 7;
                            fourth = 3;
                        }

                        else
                        {
                            first = 7;
                            second = 6;
                            third = 3;
                            fourth = 2;
                        }
                        minX = min.y;
                        minY = min.x;
                        minZ = min.z;
                        maxX = max.y;
                        maxY = max.x;
                        maxZ = max.z;
                        targetX = item.Key.y;
                        targetY = item.Key.x;
                        targetZ = item.Key.z;
                        powX = false;
                        powY = true;
                        powZ = true;
                    }
                    else if (dirX == -targetObject.transform.up)
                    {
                        Debug.Log("upside");

                        if (dirZ == -targetObject.transform.forward)
                        {
                            first = 6;
                            second = 7;
                            third = 2;
                            fourth = 3;
                        }
                        else if (dirZ == -targetObject.transform.right)
                        {
                            first = 2;
                            second = 6;
                            third = 3;
                            fourth = 7;
                        }
                        else if (dirZ == targetObject.transform.right)
                        {
                            first = 7;
                            second = 3;
                            third = 6;
                            fourth = 2;
                        }

                        else
                        {
                            first = 3;
                            second = 2;
                            third = 7;
                            fourth = 6;
                        }
                        minX = min.y;
                        minY = min.x;
                        minZ = min.z;
                        maxX = max.y;
                        maxY = max.x;
                        maxZ = max.z;
                        targetX = item.Key.y;
                        targetY = item.Key.x;
                        targetZ = item.Key.z;
                        powX = false;
                        powY = true;
                        powZ = true;
                        inverse = true;
                    }
                    else if (dirY == -targetObject.transform.up)
                    {
                        if (dirZ == -targetObject.transform.forward)
                        {
                            first = 2;
                            second = 3;
                            third = 6;
                            fourth = 7;
                        }
                        else if (dirZ == -targetObject.transform.right)
                        {
                            first = 3;
                            second = 7;
                            third = 2;
                            fourth = 6;
                        }
                        else if (dirZ == targetObject.transform.right)
                        {
                            first = 6;
                            second = 2;
                            third = 7;
                            fourth = 3;
                        }

                        else
                        {
                            first = 7;
                            second = 6;
                            third = 3;
                            fourth = 2;
                        }
                        minX = min.x;
                        minY = min.y;
                        minZ = min.z;
                        maxX = max.x;
                        maxY = max.y;
                        maxZ = max.z;
                        targetX = item.Key.x;
                        targetY = item.Key.y;
                        targetZ = item.Key.z;
                        inverse = true;
                    }

                    else
                    {
                        if (dirZ == -targetObject.transform.forward)
                        {
                            first = 6;
                            second = 7;
                            third = 2;
                            fourth = 3;
                        }
                        else if (dirZ == -targetObject.transform.right)
                        {
                            first = 2;
                            second = 6;
                            third = 3;
                            fourth = 7;
                        }
                        else if (dirZ == targetObject.transform.right)
                        {
                            first = 7;
                            second = 3;
                            third = 6;
                            fourth = 2;
                        }

                        else
                        {
                            first = 3;
                            second = 2;
                            third = 7;
                            fourth = 6;
                        }
                        minX = min.x;
                        minY = min.y;
                        minZ = min.z;
                        maxX = max.x;
                        maxY = max.y;
                        maxZ = max.z;
                        targetX = item.Key.x;
                        targetY = item.Key.y;
                        targetZ = item.Key.z;
                    }

                    if (inverse)
                    {
                        start = max.y;
                        end = min.y;
                        slidemax = Mathf.Lerp(start, end, maxslider);
                    }
                    if (inverse ? targetY >= slidemax : targetY <= slidemax)
                    {
                        //Debug.DrawRay(pos2 - targetObject.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.DrawRay(item.Key - target.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.Log(Mathf.InverseLerp(0, 1, item.Key.z));
                        //float slideinfluence = Mathf.Lerp(item.Key.x, pos1.x, Mathf.Lerp(1, mininfluence, Mathf.InverseLerp(0, slidemax, item.Key.x)));
                        // verts2[item.Value.Item1] = new Vector3(slideinfluence, item.Key.y, item.Key.z);

                        //float slideinfluence = Mathf.Lerp(1, mininfluence, Mathf.InverseLerp(minY, slidemax, targetY));
                        float slideinfluence = Mathf.Lerp(1, mininfluence, Mathf.InverseLerp(inverse ? maxY : minY, slidemax, targetY));
                        float powslideinfluence = Mathf.Clamp01(Mathf.Pow(slideinfluence, power));
                        float influenceposZ = Mathf.InverseLerp(maxZ, minZ, targetZ);
                        float influenceposY = Mathf.InverseLerp(maxX, minX, targetX);
                        Vector3 pos2 = Vector3.Lerp(data[first], data[second], influenceposZ);
                        Vector3 pos = Vector3.Lerp(data[third], data[fourth], influenceposZ);
                        Vector3 finalpos = Vector3.Lerp(pos, pos2, influenceposY);

                        //verts2[item.Value[0]] = Vector3.Lerp(target.transform.InverseTransformPoint(item.Key) , target.transform.InverseTransformPoint(pos2), slideinfluence);
                        foreach (var item2 in item.Value)
                        {

                            float x = Mathf.Lerp(target.transform.InverseTransformPoint(Utility.ScaleMoveRotateVector(target.transform, item.Key)).x, target.transform.InverseTransformPoint(finalpos).x, powX ? powslideinfluence : slideinfluence);
                            float y = Mathf.Lerp(target.transform.InverseTransformPoint(Utility.ScaleMoveRotateVector(target.transform, item.Key)).y, target.transform.InverseTransformPoint(finalpos).y, powY ? powslideinfluence : slideinfluence);
                            float z = Mathf.Lerp(target.transform.InverseTransformPoint(Utility.ScaleMoveRotateVector(target.transform, item.Key)).z, target.transform.InverseTransformPoint(finalpos).z, powZ ? powslideinfluence : slideinfluence);
                            //  verts2[item2] = Vector3.Lerp(target.transform.InverseTransformPoint(item.Key), target.transform.InverseTransformPoint(finalpos), slideinfluence);
                            verts2[item2] = new Vector3(x, y, z);

                        }

                    }


                }
                Utility.SetVertices(target, verts2);
            }
            if (orientation == Orientation.N_Y)
            {

                float start = max.y;
                float end = min.y;
                float slidemax = Mathf.Lerp(start, end, maxslider);
                Vector3[] verts2 = Utility.GetMesh(target).vertices;
                foreach (var item in availableVertices2)
                {
                    //3276
                    //1054
                    int first = 1, second = 0, third = 5, fourth = 4;
                    float minX = min.x, minY = min.y, minZ = min.z;
                    float maxX = max.x, maxY = max.y, maxZ = max.z;
                    float targetX = item.Key.x, targetY = item.Key.y, targetZ = item.Key.z;
                    Vector3 dirZ = Utility.GetNearestDirectionCardinal(targetObject.transform, target.transform.forward);
                    Vector3 dirX = Utility.GetNearestDirectionCardinal(targetObject.transform, target.transform.right);
                    Vector3 dirY = Utility.GetNearestDirectionCardinal(targetObject.transform, target.transform.up);
                    bool powX = true, powY = false, powZ = true;
                    bool inverse = false;
                    if (dirZ == -targetObject.transform.up)
                    {
                        if (dirX == -targetObject.transform.forward)
                        {
                            first = 5;
                            second = 1;
                            third = 4;
                            fourth = 0;
                        }
                        else if (dirX == -targetObject.transform.right)
                        {
                            first = 4;
                            second = 5;
                            third = 0;
                            fourth = 1;
                        }
                        else if (dirX == targetObject.transform.right)
                        {
                            first = 1;
                            second = 0;
                            third = 5;
                            fourth = 4;
                        }

                        else
                        {
                            first = 0;
                            second = 4;
                            third = 1;
                            fourth = 5;
                        }
                        minX = min.x;
                        minY = min.z;
                        minZ = min.y;
                        maxX = max.x;
                        maxY = max.z;
                        maxZ = max.y;
                        targetX = item.Key.x;
                        targetY = item.Key.z;
                        targetZ = item.Key.y;
                        powX = true;
                        powY = true;
                        powZ = false;
                        inverse = true;
                    }
                    else if (dirZ == targetObject.transform.up)
                    {
                        if (dirX == -targetObject.transform.forward)
                        {
                            first = 1;
                            second = 5;
                            third = 0;
                            fourth = 4;
                        }
                        else if (dirX == -targetObject.transform.right)
                        {
                            first = 5;
                            second = 4;
                            third = 1;
                            fourth = 0;
                        }
                        else if (dirX == targetObject.transform.right)
                        {
                            first = 0;
                            second = 1;
                            third = 4;
                            fourth = 5;
                        }

                        else
                        {
                            first = 4;
                            second = 0;
                            third = 5;
                            fourth = 1;
                        }
                        minX = min.x;
                        minY = min.z;
                        minZ = min.y;
                        maxX = max.x;
                        maxY = max.z;
                        maxZ = max.y;
                        targetX = item.Key.x;
                        targetY = item.Key.z;
                        targetZ = item.Key.y;
                        powX = true;
                        powY = true;
                        powZ = false;
                    }
                    else if (dirX == targetObject.transform.up)
                    {
                        Debug.Log("upside");

                        if (dirZ == -targetObject.transform.forward)
                        {
                            first = 0;
                            second = 1;
                            third = 4;
                            fourth = 5;
                        }
                        else if (dirZ == -targetObject.transform.right)
                        {
                            first = 1;
                            second = 5;
                            third = 0;
                            fourth = 4;
                        }
                        else if (dirZ == targetObject.transform.right)
                        {
                            first = 4;
                            second = 0;
                            third = 5;
                            fourth = 1;
                        }

                        else
                        {
                            first = 5;
                            second = 4;
                            third = 1;
                            fourth = 0;
                        }
                        minX = min.y;
                        minY = min.x;
                        minZ = min.z;
                        maxX = max.y;
                        maxY = max.x;
                        maxZ = max.z;
                        targetX = item.Key.y;
                        targetY = item.Key.x;
                        targetZ = item.Key.z;
                        powX = false;
                        powY = true;
                        powZ = true;
                    }
                    else if (dirX == -targetObject.transform.up)
                    {
                        Debug.Log("upside");

                        if (dirZ == -targetObject.transform.forward)
                        {
                            first = 4;
                            second = 5;
                            third = 0;
                            fourth = 1;
                        }
                        else if (dirZ == -targetObject.transform.right)
                        {
                            first = 0;
                            second = 4;
                            third = 1;
                            fourth = 5;
                        }
                        else if (dirZ == targetObject.transform.right)
                        {
                            first = 5;
                            second = 1;
                            third = 4;
                            fourth = 0;
                        }

                        else
                        {
                            first = 1;
                            second = 0;
                            third = 5;
                            fourth = 4;
                        }
                        minX = min.y;
                        minY = min.x;
                        minZ = min.z;
                        maxX = max.y;
                        maxY = max.x;
                        maxZ = max.z;
                        targetX = item.Key.y;
                        targetY = item.Key.x;
                        targetZ = item.Key.z;
                        powX = false;
                        powY = true;
                        powZ = true;
                        inverse = true;
                    }
                    else if (dirY == -targetObject.transform.up)
                    {
                        if (dirZ == -targetObject.transform.forward)
                        {
                            first = 0;
                            second = 1;
                            third = 4;
                            fourth = 5;
                        }
                        else if (dirZ == -targetObject.transform.right)
                        {
                            first = 1;
                            second = 5;
                            third = 0;
                            fourth = 4;
                        }
                        else if (dirZ == targetObject.transform.right)
                        {
                            first = 4;
                            second = 0;
                            third = 5;
                            fourth = 1;
                        }

                        else
                        {
                            first = 5;
                            second = 4;
                            third = 1;
                            fourth = 0;
                        }
                        minX = min.x;
                        minY = min.y;
                        minZ = min.z;
                        maxX = max.x;
                        maxY = max.y;
                        maxZ = max.z;
                        targetX = item.Key.x;
                        targetY = item.Key.y;
                        targetZ = item.Key.z;
                        inverse = true;
                    }

                    else
                    {
                        if (dirZ == -targetObject.transform.forward)
                        {
                            first = 4;
                            second = 5;
                            third = 0;
                            fourth = 1;
                        }
                        else if (dirZ == -targetObject.transform.right)
                        {
                            first = 0;
                            second = 4;
                            third = 1;
                            fourth = 5;
                        }
                        else if (dirZ == targetObject.transform.right)
                        {
                            first = 5;
                            second = 1;
                            third = 4;
                            fourth = 0;
                        }

                        else
                        {
                            first = 1;
                            second = 0;
                            third = 5;
                            fourth = 4;
                        }
                        minX = min.x;
                        minY = min.y;
                        minZ = min.z;
                        maxX = max.x;
                        maxY = max.y;
                        maxZ = max.z;
                        targetX = item.Key.x;
                        targetY = item.Key.y;
                        targetZ = item.Key.z;
                    }

                    if (inverse)
                    {
                        start = max.y;
                        end = min.y;
                        slidemax = Mathf.Lerp(start, end, maxslider);
                    }
                    if (inverse ? targetY <= slidemax : targetY >= slidemax)
                    {
                        //Debug.DrawRay(pos2 - targetObject.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.DrawRay(item.Key - target.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.Log(Mathf.InverseLerp(0, 1, item.Key.z));
                        //float slideinfluence = Mathf.Lerp(item.Key.x, pos1.x, Mathf.Lerp(1, mininfluence, Mathf.InverseLerp(0, slidemax, item.Key.x)));
                        // verts2[item.Value.Item1] = new Vector3(slideinfluence, item.Key.y, item.Key.z);
                        float slideinfluence = Mathf.Lerp(1, mininfluence, Mathf.InverseLerp(inverse ? minY : maxY, slidemax, targetY));
                        // float slideinfluence = Mathf.Lerp(1, mininfluence, Mathf.InverseLerp(max.y, slidemax, item.Key.y));
                        float powslideinfluence = Mathf.Clamp01(Mathf.Pow(slideinfluence, power));
                        float influenceposZ = Mathf.InverseLerp(maxZ, minZ, targetZ);
                        float influenceposX = Mathf.InverseLerp(maxX, minX, targetX);
                        Vector3 pos2 = Vector3.Lerp(data[first], data[second], influenceposZ);
                        Vector3 pos = Vector3.Lerp(data[third], data[fourth], influenceposZ);
                        Vector3 finalpos = Vector3.Lerp(pos, pos2, influenceposX);

                        //verts2[item.Value[0]] = Vector3.Lerp(target.transform.InverseTransformPoint(item.Key) , target.transform.InverseTransformPoint(pos2), slideinfluence);
                        foreach (var item2 in item.Value)
                        {

                            float x = Mathf.Lerp(target.transform.InverseTransformPoint(Utility.ScaleMoveRotateVector(target.transform, item.Key)).x, target.transform.InverseTransformPoint(finalpos).x, powX ? powslideinfluence : slideinfluence);
                            float y = Mathf.Lerp(target.transform.InverseTransformPoint(Utility.ScaleMoveRotateVector(target.transform, item.Key)).y, target.transform.InverseTransformPoint(finalpos).y, powY ? powslideinfluence : slideinfluence);
                            float z = Mathf.Lerp(target.transform.InverseTransformPoint(Utility.ScaleMoveRotateVector(target.transform, item.Key)).z, target.transform.InverseTransformPoint(finalpos).z, powZ ? powslideinfluence : slideinfluence);
                            //  verts2[item2] = Vector3.Lerp(target.transform.InverseTransformPoint(item.Key), target.transform.InverseTransformPoint(finalpos), slideinfluence);
                            verts2[item2] = new Vector3(x, y, z);


                        }

                    }


                }
                Utility.SetVertices(target, verts2);
            }
            if (orientation == Orientation.N_X)
            {

                float start = max.x;
                float end = min.x;
                float slidemax = Mathf.Lerp(start, end, maxslider);
                Vector3[] verts2 = Utility.GetMesh(target).vertices;
                foreach (var item in availableVertices2)
                {
                    //7564
                    //3120
                    int first = 3, second = 1, third = 2, fourth = 0;
                    float minX = min.x, minY = min.y, minZ = min.z;
                    float maxX = max.x, maxY = max.y, maxZ = max.z;
                    float targetX = item.Key.x, targetY = item.Key.y, targetZ = item.Key.z;
                    Vector3 dirZ = Utility.GetNearestDirectionCardinal(targetObject.transform, target.transform.forward);
                    Vector3 dirX = Utility.GetNearestDirectionCardinal(targetObject.transform, target.transform.right);
                    Vector3 dirY = Utility.GetNearestDirectionCardinal(targetObject.transform, target.transform.up);
                    bool powX = false, powY = true, powZ = true;
                    bool inverse = false;
                    if (dirX == -targetObject.transform.right)
                    {
                        if (dirZ == -targetObject.transform.forward)
                        {
                            first = 2;
                            second = 3;
                            third = 0;
                            fourth = 1;
                        }
                        else if (dirZ == -targetObject.transform.up)
                        {
                            first = 0;
                            second = 2;
                            third = 1;
                            fourth = 3;
                        }
                        else if (dirZ == targetObject.transform.up)
                        {
                            first = 3;
                            second = 1;
                            third = 2;
                            fourth = 0;
                        }

                        else
                        {
                            first = 1;
                            second = 0;
                            third = 3;
                            fourth = 2;
                        }
                        minX = min.x;
                        minY = min.z;
                        minZ = min.y;
                        maxX = max.x;
                        maxY = max.z;
                        maxZ = max.y;
                        targetX = item.Key.x;
                        targetY = item.Key.z;
                        targetZ = item.Key.y;
                        powX = false;
                        powY = true;
                        powZ = true;
                        inverse = true;
                    }
                    else if (dirX == targetObject.transform.right)
                    {
                        if (dirZ == targetObject.transform.forward)
                        {
                            first = 3;
                            second = 2;
                            third = 1;
                            fourth = 0;
                        }
                        else if (dirZ == targetObject.transform.up)
                        {
                            first = 2;
                            second = 0;
                            third = 3;
                            fourth = 1;
                        }
                        else if (dirZ == -targetObject.transform.up)
                        {
                            first = 1;
                            second = 3;
                            third = 0;
                            fourth = 2;
                        }

                        else
                        {
                            first = 0;
                            second = 1;
                            third = 2;
                            fourth = 3;
                        }
                        minX = min.x;
                        minY = min.z;
                        minZ = min.y;
                        maxX = max.x;
                        maxY = max.z;
                        maxZ = max.y;
                        targetX = item.Key.x;
                        targetY = item.Key.z;
                        targetZ = item.Key.y;
                        powX = false;
                        powY = true;
                        powZ = true;
                    }
                    else if (dirZ == targetObject.transform.right)
                    {
                        Debug.Log("upside");

                        if (dirX == targetObject.transform.up)
                        {
                            first = 3;
                            second = 2;
                            third = 1;
                            fourth = 0;
                        }
                        else if (dirX == targetObject.transform.forward)
                        {
                            first = 1;
                            second = 3;
                            third = 0;
                            fourth = 2;
                        }
                        else if (dirX == -targetObject.transform.forward)
                        {
                            first = 2;
                            second = 0;
                            third = 3;
                            fourth = 1;
                        }

                        else
                        {
                            first = 0;
                            second = 1;
                            third = 2;
                            fourth = 3;
                        }
                        minX = min.y;
                        minY = min.x;
                        minZ = min.z;
                        maxX = max.y;
                        maxY = max.x;
                        maxZ = max.z;
                        targetX = item.Key.z;
                        targetY = item.Key.y;
                        targetZ = item.Key.x;
                        powX = true;
                        powY = true;
                        powZ = false;
                    }
                    else if (dirZ == -targetObject.transform.right)
                    {
                        Debug.Log("upside");

                        if (dirX == targetObject.transform.up)
                        {
                            first = 2;
                            second = 3;
                            third = 0;
                            fourth = 1;
                        }
                        else if (dirX == targetObject.transform.forward)
                        {
                            first = 3;
                            second = 1;
                            third = 2;
                            fourth = 0;
                        }
                        else if (dirX == -targetObject.transform.forward)
                        {
                            first = 0;
                            second = 2;
                            third = 1;
                            fourth = 3;
                        }

                        else
                        {
                            first = 1;
                            second = 0;
                            third = 3;
                            fourth = 2;
                        }
                        minX = min.y;
                        minY = min.x;
                        minZ = min.z;
                        maxX = max.y;
                        maxY = max.x;
                        maxZ = max.z;
                        targetX = item.Key.z;
                        targetY = item.Key.y;
                        targetZ = item.Key.x;
                        powX = true;
                        powY = true;
                        powZ = false;
                        inverse = true;
                    }
                    else if (dirY == -targetObject.transform.right)
                    {
                        if (dirX == targetObject.transform.forward)
                        {
                            first = 1;
                            second = 0;
                            third = 3;
                            fourth = 2;
                        }
                        else if (dirX == -targetObject.transform.up)
                        {
                            first = 0;
                            second = 2;
                            third = 1;
                            fourth = 3;
                        }
                        else if (dirX == targetObject.transform.up)
                        {
                            first = 3;
                            second = 1;
                            third = 2;
                            fourth = 0;
                        }

                        else
                        {
                            first = 2;
                            second = 3;
                            third = 0;
                            fourth = 1;
                        }
                        minX = min.x;
                        minY = min.y;
                        minZ = min.z;
                        maxX = max.x;
                        maxY = max.y;
                        maxZ = max.z;
                        targetX = item.Key.y;
                        targetY = item.Key.x;
                        targetZ = item.Key.z;
                        inverse = true;
                        powX = true;
                        powY = false;
                        powZ = true;
                    }
                    else
                    {
                        if (dirX == targetObject.transform.up)
                        {
                            first = 2;
                            second = 0;
                            third = 3;
                            fourth = 1;
                        }
                        else if (dirX == -targetObject.transform.forward)
                        {
                            first = 0;
                            second = 1;
                            third = 2;
                            fourth = 3;
                        }
                        else if (dirX == targetObject.transform.forward)
                        {
                            first = 3;
                            second = 2;
                            third = 1;
                            fourth = 0;
                        }

                        else
                        {
                            first = 1;
                            second = 3;
                            third = 0;
                            fourth = 2;
                        }
                        minX = min.x;
                        minY = min.y;
                        minZ = min.z;
                        maxX = max.x;
                        maxY = max.y;
                        maxZ = max.z;
                        targetX = item.Key.y;
                        targetY = item.Key.x;
                        targetZ = item.Key.z;
                        powX = true;
                        powY = false;
                        powZ = true;
                    }

                    if (inverse)
                    {
                        start = max.x;
                        end = min.x;
                        slidemax = Mathf.Lerp(start, end, maxslider);
                    }
                    if (inverse ? targetX <= slidemax : targetX >= slidemax)

                    {
                        //Debug.DrawRay(pos2 - targetObject.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.DrawRay(item.Key - target.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.Log(Mathf.InverseLerp(0, 1, item.Key.z));
                        //float slideinfluence = Mathf.Lerp(item.Key.x, pos1.x, Mathf.Lerp(1, mininfluence, Mathf.InverseLerp(0, slidemax, item.Key.x)));
                        // verts2[item.Value.Item1] = new Vector3(slideinfluence, item.Key.y, item.Key.z);

                        float slideinfluence = Mathf.Lerp(1, mininfluence, Mathf.InverseLerp(inverse ? minX : maxX, slidemax, targetX));
                        float powslideinfluence = Mathf.Clamp01(Mathf.Pow(slideinfluence, power));

                        float influenceposZ = Mathf.InverseLerp(maxZ, minZ, targetZ);
                        float influenceposY = Mathf.InverseLerp(maxY, minY, targetY);
                        Vector3 pos = Vector3.Lerp(data[first], data[second], influenceposY);
                        Vector3 pos2 = Vector3.Lerp(data[third], data[fourth], influenceposY);
                        Vector3 finalpos = Vector3.Lerp(pos, pos2, influenceposZ);

                        //verts2[item.Value[0]] = Vector3.Lerp(target.transform.InverseTransformPoint(item.Key) , target.transform.InverseTransformPoint(pos2), slideinfluence);
                        foreach (var item2 in item.Value)
                        {
                            float x = Mathf.Lerp(target.transform.InverseTransformPoint(Utility.ScaleMoveRotateVector(target.transform, item.Key)).x, target.transform.InverseTransformPoint(finalpos).x, powX ? powslideinfluence : slideinfluence);
                            float y = Mathf.Lerp(target.transform.InverseTransformPoint(Utility.ScaleMoveRotateVector(target.transform, item.Key)).y, target.transform.InverseTransformPoint(finalpos).y, powY ? powslideinfluence : slideinfluence);
                            float z = Mathf.Lerp(target.transform.InverseTransformPoint(Utility.ScaleMoveRotateVector(target.transform, item.Key)).z, target.transform.InverseTransformPoint(finalpos).z, powZ ? powslideinfluence : slideinfluence);

                            verts2[item2] = new Vector3(x, y, z);


                        }

                    }


                }
                Utility.SetVertices(target, verts2);
            }
            if (orientation == Orientation.N_Z)
            {

                float start = max.z;
                float end = min.z;
                float slidemax = Mathf.Lerp(start, end, maxslider);
                Vector3[] verts2 = Utility.GetMesh(target).vertices;
                foreach (var item in availableVertices2)
                {
                    //7351
                    //6240
                    int first = 7, second = 3, third = 5, fourth = 1;
                    float minX = min.x, minY = min.y, minZ = min.z;
                    float maxX = max.x, maxY = max.y, maxZ = max.z;
                    float targetX = item.Key.x, targetY = item.Key.y, targetZ = item.Key.z;
                    Vector3 dirZ = Utility.GetNearestDirectionCardinal(targetObject.transform, target.transform.forward);
                    Vector3 dirX = Utility.GetNearestDirectionCardinal(targetObject.transform, target.transform.right);
                    Vector3 dirY = Utility.GetNearestDirectionCardinal(targetObject.transform, target.transform.up);
                    bool powX = true, powY = true, powZ = false;
                    bool inverse = false;
                    if (dirX == -targetObject.transform.forward)
                    {
                        if (dirY == targetObject.transform.right)
                        {
                            first = 4;
                            second = 6;
                            third = 0;
                            fourth = 2;
                        }
                        else if (dirY == -targetObject.transform.up)
                        {
                            first = 0;
                            second = 4;
                            third = 2;
                            fourth = 6;
                        }
                        else if (dirY == targetObject.transform.up)
                        {
                            first = 6;
                            second = 2;
                            third = 4;
                            fourth = 0;
                        }

                        else
                        {
                            first = 2;
                            second = 0;
                            third = 6;
                            fourth = 4;
                        }
                        minX = min.x;
                        minY = min.y;
                        minZ = min.z;
                        maxX = max.x;
                        maxY = max.z;
                        maxZ = max.y;
                        targetX = item.Key.z;
                        targetY = item.Key.y;
                        targetZ = item.Key.x;
                        powX = false;
                        powY = true;
                        powZ = true;
                        inverse = true;
                    }
                    else if (dirX == targetObject.transform.forward)
                    {
                        if (dirY == targetObject.transform.right)
                        {
                            first = 6;
                            second = 4;
                            third = 2;
                            fourth = 0;
                        }
                        else if (dirY == -targetObject.transform.up)
                        {
                            first = 4;
                            second = 0;
                            third = 6;
                            fourth = 2;
                        }
                        else if (dirY == targetObject.transform.up)
                        {
                            first = 2;
                            second = 6;
                            third = 0;
                            fourth = 4;
                        }

                        else
                        {
                            first = 0;
                            second = 2;
                            third = 4;
                            fourth = 6;
                        }
                        minX = min.x;
                        minY = min.y;
                        minZ = min.z;
                        maxX = max.x;
                        maxY = max.z;
                        maxZ = max.y;
                        targetX = item.Key.z;
                        targetY = item.Key.y;
                        targetZ = item.Key.x;
                        powX = false;
                        powY = true;
                        powZ = true;
                    }
                    else if (dirY == -targetObject.transform.forward)
                    {
                        Debug.Log("upside");

                        if (dirZ == -targetObject.transform.right)
                        {
                            first = 2;
                            second = 6;
                            third = 0;
                            fourth = 4;
                        }
                        else if (dirZ == targetObject.transform.up)
                        {
                            first = 6;
                            second = 4;
                            third = 2;
                            fourth = 0;
                        }
                        else if (dirZ == -targetObject.transform.up)
                        {
                            first = 0;
                            second = 2;
                            third = 4;
                            fourth = 6;
                        }

                        else
                        {
                            first = 4;
                            second = 0;
                            third = 6;
                            fourth = 2;
                        }
                        minX = min.y;
                        minY = min.x;
                        minZ = min.z;
                        maxX = max.y;
                        maxY = max.x;
                        maxZ = max.z;
                        targetX = item.Key.z;
                        targetY = item.Key.x;
                        targetZ = item.Key.y;
                        powX = true;
                        powY = false;
                        powZ = true;
                        inverse = true;

                    }
                    else if (dirY == targetObject.transform.forward)
                    {
                        if (dirZ == -targetObject.transform.right)
                        {
                            first = 0;
                            second = 4;
                            third = 2;
                            fourth = 6;
                        }
                        else if (dirZ == targetObject.transform.up)
                        {
                            first = 2;
                            second = 0;
                            third = 6;
                            fourth = 4;
                        }
                        else if (dirZ == -targetObject.transform.up)
                        {
                            first = 4;
                            second = 6;
                            third = 0;
                            fourth = 2;
                        }

                        else
                        {
                            first = 6;
                            second = 2;
                            third = 4;
                            fourth = 0;
                        }
                        minX = min.y;
                        minY = min.x;
                        minZ = min.z;
                        maxX = max.y;
                        maxY = max.x;
                        maxZ = max.z;
                        targetX = item.Key.z;
                        targetY = item.Key.x;
                        targetZ = item.Key.y;
                        powX = true;
                        powY = false;
                        powZ = true;
                    }
                    else if (dirZ == -targetObject.transform.forward)
                    {
                        if (dirX == -targetObject.transform.right)
                        {
                            first = 2;
                            second = 6;
                            third = 0;
                            fourth = 4;
                        }
                        else if (dirX == targetObject.transform.up)
                        {
                            first = 7;
                            second = 4;
                            third = 2;
                            fourth = 0;
                        }
                        else if (dirX == -targetObject.transform.up)
                        {
                            first = 0;
                            second = 2;
                            third = 4;
                            fourth = 6;
                        }

                        else
                        {
                            first = 4;
                            second = 0;
                            third = 6;
                            fourth = 2;
                        }
                        minX = min.x;
                        minY = min.y;
                        minZ = min.z;
                        maxX = max.x;
                        maxY = max.y;
                        maxZ = max.z;
                        targetX = item.Key.x;
                        targetY = item.Key.y;
                        targetZ = item.Key.z;
                        powX = true;
                        powY = true;
                        powZ = false;
                        inverse = true;
                    }

                    else
                    {
                        if (dirX == -targetObject.transform.right)
                        {
                            first = 0;
                            second = 4;
                            third = 2;
                            fourth = 6;
                        }
                        else if (dirX == targetObject.transform.up)
                        {
                            first = 2;
                            second = 0;
                            third = 6;
                            fourth = 4;
                        }
                        else if (dirX == -targetObject.transform.up)
                        {
                            first = 4;
                            second = 6;
                            third = 0;
                            fourth = 2;
                        }

                        else
                        {
                            Debug.Log("This");
                            first = 6;
                            second = 2;
                            third = 4;
                            fourth = 0;
                        }
                        minX = min.x;
                        minY = min.y;
                        minZ = min.z;
                        maxX = max.x;
                        maxY = max.y;
                        maxZ = max.z;
                        targetX = item.Key.x;
                        targetY = item.Key.y;
                        targetZ = item.Key.z;
                    }

                    if (inverse)
                    {
                        start = max.z;
                        end = min.z;
                        slidemax = Mathf.Lerp(start, end, maxslider);
                    }
                    if (inverse ? targetZ <= slidemax : targetZ >= slidemax)
                    {
                        //Debug.DrawRay(pos2 - targetObject.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.DrawRay(item.Key - target.transform.position, Vector3.up * 10, Color.black, 10);
                        //Debug.Log(Mathf.InverseLerp(0, 1, item.Key.z));
                        //float slideinfluence = Mathf.Lerp(item.Key.x, pos1.x, Mathf.Lerp(1, mininfluence, Mathf.InverseLerp(0, slidemax, item.Key.x)));
                        // verts2[item.Value.Item1] = new Vector3(slideinfluence, item.Key.y, item.Key.z);

                        float slideinfluence = Mathf.Lerp(1, mininfluence, Mathf.InverseLerp(inverse ? minZ : maxZ, slidemax, targetZ));
                        //float powslideinfluence = Mathf.Clamp01(Mathf.Pow(slideinfluence, 2) * power);
                        float powslideinfluence = Mathf.Clamp01(Mathf.Pow(slideinfluence, power));
                        float influenceposX = 0;
                        float influenceposX2 = 0;
                        float influenceposY = 0;

                        influenceposX = Mathf.InverseLerp(maxX, minX, targetX);
                        influenceposY = Mathf.InverseLerp(maxY, minY, targetY);



                        Debug.Log(slideinfluence);
                        Vector3 pos = Vector3.Lerp(data[first], data[second], influenceposX);
                        Vector3 pos2 = Vector3.Lerp(data[third], data[fourth], influenceposX);
                        Vector3 finalpos = Vector3.Lerp(pos, pos2, influenceposY);

                        //verts2[item.Value[0]] = Vector3.Lerp(target.transform.InverseTransformPoint(item.Key), target.transform.InverseTransformPoint(pos2), slideinfluence);
                        foreach (var item2 in item.Value)
                        {
                            if (autoscale)
                            {
                                float x = Mathf.Lerp(target.transform.InverseTransformPoint(Utility.ScaleMoveRotateVector(target.transform, item.Key)).x, target.transform.InverseTransformPoint(finalpos).x, powX ? powslideinfluence : slideinfluence);
                                float y = Mathf.Lerp(target.transform.InverseTransformPoint(Utility.ScaleMoveRotateVector(target.transform, item.Key)).y, target.transform.InverseTransformPoint(finalpos).y, powY ? powslideinfluence : slideinfluence);
                                float z = Mathf.Lerp(target.transform.InverseTransformPoint(Utility.ScaleMoveRotateVector(target.transform, item.Key)).z, target.transform.InverseTransformPoint(finalpos).z, powZ ? powslideinfluence : slideinfluence);
                                verts2[item2] = new Vector3(x, y, z);
                            }
                            else if (autoraycast)
                            {
                                RaycastHit hit;
                                if (Physics.Raycast(Utility.ScaleMoveRotateVector(target.transform, item.Key), target.transform.forward, out hit))
                                {
                                    verts2[item2] = Vector3.Lerp(target.transform.InverseTransformPoint(Utility.ScaleMoveRotateVector(target.transform, item.Key)), target.transform.InverseTransformPoint(hit.point), slideinfluence);
                                }
                            }
                            else if (autoraycasttotarget)
                            {
                                RaycastHit hit;
                                if (Physics.Raycast(Utility.ScaleMoveRotateVector(target.transform, item.Key), targetObject.transform.position - target.transform.position, out hit))
                                {
                                    verts2[item2] = Vector3.Lerp(target.transform.InverseTransformPoint(Utility.ScaleMoveRotateVector(target.transform, item.Key)), target.transform.InverseTransformPoint(hit.point), slideinfluence);
                                }
                            }
                            else if (autoraycasttotargetcenter)
                            {
                                RaycastHit hit;
                                if (Physics.Raycast(Utility.ScaleMoveRotateVector(target.transform, item.Key), targetObject.transform.position - Utility.ScaleMoveRotateVector(target.transform, item.Key), out hit))
                                {
                                    verts2[item2] = Vector3.Lerp(target.transform.InverseTransformPoint(Utility.ScaleMoveRotateVector(target.transform, item.Key)), target.transform.InverseTransformPoint(hit.point), slideinfluence);
                                }
                            }

                            else
                            {
                                Plane plane = new Plane(data[first], data[second], data[fourth]);
                                //verts2[item2] = plane.ClosestPointOnPlane(target.transform.InverseTransformPoint(Utility.ScaleMoveRotateVector(target.transform, item.Key)));
                                verts2[item2] = Vector3.Lerp(target.transform.InverseTransformPoint(Utility.ScaleMoveRotateVector(target.transform, item.Key)), target.transform.InverseTransformPoint(plane.ClosestPointOnPlane(Utility.ScaleMoveRotateVector(target.transform, item.Key))), slideinfluence);

                            }

                        }

                    }


                }
                Utility.SetVertices(target, verts2);
            }
            setmesh = false;
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

    /*
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
    */
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

            availableTriangles2.Add(i, new List<Vector3>() { vertices2[vertexIndex1], vertices2[vertexIndex2], vertices2[vertexIndex3] });

            // Store the vertices in the triangleVertices array
            if (!availableVertices2.ContainsKey(vertices2[vertexIndex1]))
            {
                availableVertices2.Add(vertices2[vertexIndex1], new List<int> { (vertexIndex1) });
            }
            else if (availableVertices2.ContainsKey(vertices2[vertexIndex1]))
            {
                if (!availableVertices2[vertices2[vertexIndex1]].Contains(vertexIndex1))
                    availableVertices2[vertices2[vertexIndex1]].Add(vertexIndex1);
            }

            if (!availableVertices2.ContainsKey(vertices2[vertexIndex2]))
            {
                availableVertices2.Add(vertices2[vertexIndex2], new List<int> { (vertexIndex2) });

            }
            else if (availableVertices2.ContainsKey(vertices2[vertexIndex2]))
            {
                if (!availableVertices2[vertices2[vertexIndex2]].Contains(vertexIndex2))
                    availableVertices2[vertices2[vertexIndex2]].Add(vertexIndex2);
            }

            if (!availableVertices2.ContainsKey(vertices2[vertexIndex3]))
            {
                availableVertices2.Add(vertices2[vertexIndex3], new List<int> { (vertexIndex3) });
            }
            else if (availableVertices2.ContainsKey(vertices2[vertexIndex3]))
            {
                if (!availableVertices2[vertices2[vertexIndex3]].Contains(vertexIndex3))
                    availableVertices2[vertices2[vertexIndex3]].Add(vertexIndex3);
            }

        }
    }

}
