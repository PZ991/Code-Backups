using System;
using System.Collections;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Progress;
using System.Collections.Generic;
using System.Drawing;

public class VectorDraw : MonoBehaviour
{
    public Mesh oldmesh;
    public Camera cam;
    public float distanceThreshold = 0.5f; // The distance threshold between recorded positions
    public float recordInterval = 0.1f; // The time interval between recordings
    private float currentRecordInterval; // Timer for recording interval
    //public List<Vector3> lastRecordedPosition; // The last recorded position
    public List<Points> points;
    private bool isRecording = false; // Flag to indicate if recording is in progress
    private float Totaldistance; //totaldistance of all points
    public float distance;      //position value
    public bool Generate;       //generate objects to the points
    public Vector3[] originalverts; //original vertices positions
    public GameObject targetobject;// object to copy
    public bool refresh; //refresh originalvertices and others
    public bool reset;  //reapply mesh to curve
    public bool cont;   //continuously apply mesh to curve
    public int numofrepeat; //number the object will be iterated along the curve
    public bool repeat;// does it repeat?
    public int testindex;
    public bool smoothinterpolation;
    public List<GameObject> pointers;
    public List<Mesh> meshIterations;
    private void Start()
    {

    }
    void Update()
    {
        if (points == null)
        {
            points = new List<Points>();
        }
        if (Input.GetMouseButtonDown(0)) // Check if the left mouse button is pressed
        {
            isRecording = true;
            currentRecordInterval = recordInterval;
        }

        if (Input.GetMouseButtonUp(0)) // Check if the left mouse button is released
        {
            isRecording = false;
        }

        if (isRecording)
        {
            currentRecordInterval -= Time.deltaTime; // Decrease the timer
            // Check if the distance threshold is reached or the timer has expired
            if (points.Count == 0)
            {
                RecordMousePosition();
            }
            else
            {
                RaycastHit hit;
                Utility.RayCastToMouse(cam, out hit);
                if (Vector3.Distance(hit.point, points[points.Count - 1].originalposition) >= distanceThreshold || currentRecordInterval <= 0)
                {
                    RecordMousePosition();
                }
            }
        }

    }
    private void OnDrawGizmos()
    {
        //RotateVectorByAngle
        if (points.Count == 0)
            return;

        float distance1 = 0;
        for (int i = 0; i < points.Count - 1; i++)
        {
            distance1 += Vector3.Distance(points[i].originalposition, points[i + 1].originalposition);
            //Debug.DrawLine(points[i].originalposition + transform.position, points[i + 1].originalposition + transform.position);
        }
        Totaldistance = distance1;

        Mesh targetmesh = targetobject.GetComponent<MeshFilter>().mesh;
        Mesh mesh = oldmesh;
        Bounds bound = oldmesh.bounds;

        Vector3[] verts = mesh.vertices;
        originalverts = verts;

        if (refresh)
        {
            foreach (var item in pointers)
            {
                DestroyImmediate(item);
            }
            pointers.Clear();

            for (int i = 0; i < points.Count - 1; i++)
            {
                points[i] = new Points(points[i]);
                /*
                Vector3[] data = new Vector3[originalverts.Length];
                for (int j = 0; j < originalverts.Length; j++)
                {
                    // data[j] = Utility.RotatePointAroundPivot(originalverts[i], targetobject.transform.position, targetobject.transform.forward, Utility.GetDirection(points[i].originalposition, points[i + 1].originalposition));
                    data[j] = originalverts[j];
                }
                */
                //points[i]=new Points(points[i],data);
                GameObject go = new GameObject();
                go.name = "pointer num " + i;
                go.transform.SetParent(transform);
                go.transform.position = points[i].originalposition;
                go.transform.LookAt(points[i + 1].originalposition);
                pointers.Add(go);
                points[i] = new Points(points[i], go.transform.forward, go.transform.right, go.transform.up);

            }
            refresh = false;

        }



        List<Vector3[]> multipliedverts = new List<Vector3[]>();
        for (int i = 0; i < numofrepeat; i++)
        {
            List<Vector3> newverts = new List<Vector3>();

            if (meshIterations != null)
            {
                if (meshIterations.Count > 0)
                {
                    if (meshIterations.Count > i)
                    {
                        foreach (var item in meshIterations[i].vertices)
                        {
                            newverts.Add(item);
                        }
                        multipliedverts.Add(newverts.ToArray());
                        // Debug.Log("verts: " + newverts.Count);

                    }
                    else
                    {
                        foreach (var item in meshIterations[meshIterations.Count - 1].vertices)
                        {
                            newverts.Add(item);
                        }
                        multipliedverts.Add(newverts.ToArray());

                    }
                    continue;
                }
            }
            foreach (var item in originalverts)
            {
                newverts.Add(item);
            }
            multipliedverts.Add(newverts.ToArray());
        }

        List<int[]> multipliedtriangles = new List<int[]>();
        for (int j = 0; j < numofrepeat; j++)
        {
            List<int> newtris = new List<int>();

            if (meshIterations != null)
            {
                if (meshIterations.Count > 0)
                {
                    if (meshIterations.Count > j)
                    {
                        for (int i = 0; i < meshIterations[j].triangles.Length; i++)
                        {
                            int total = 0;
                            for (int k = 0; k < j; k++)
                            {
                                total += multipliedverts[k].Length;
                            }
                            newtris.Add(meshIterations[j].triangles[i] + total);


                        }

                        multipliedtriangles.Add(newtris.ToArray());
                    }
                    else
                    {
                        for (int i = 0; i < meshIterations[meshIterations.Count - 1].triangles.Length; i++)
                        {
                            int total = 0;
                            for (int k = 0; k < j; k++)
                            {
                                total += multipliedverts[k].Length;
                            }
                            newtris.Add(meshIterations[meshIterations.Count - 1].triangles[i] + total);

                        }
                        multipliedtriangles.Add(newtris.ToArray());
                    }
                    continue;
                }
            }

            for (int i = 0; i < mesh.triangles.Length; i++)
            {
                newtris.Add(mesh.triangles[i] + multipliedverts[j].Length * j);

            }
            multipliedtriangles.Add(newtris.ToArray());
        }





        List<Vector3> lastRecordedPosition = new List<Vector3>();
        for (int i = 0; i < points.Count - 1; i++)
        {
            lastRecordedPosition.Add(points[i].originalposition);

        }
        if (!repeat)
        {
            if (!smoothinterpolation)
            {
                Vector3 pos = Utility.GetInterpolatedPositionUnclamped(lastRecordedPosition, Totaldistance, distance);

                Vector3[] modifiedVertices = new Vector3[originalverts.Length];
                for (int i = 0; i < originalverts.Length; i++)
                {
                    float val = Mathf.Lerp(-bound.extents.z, bound.extents.z, Mathf.InverseLerp(-bound.extents.z, bound.extents.z, originalverts[i].z));
                    int posint = Utility.GetInterpolatedPositionIndex(lastRecordedPosition, Totaldistance, distance + (val));
                    Vector3 testpos = Utility.GetInterpolatedPositionUnclamped(lastRecordedPosition, Totaldistance, val);
                    Gizmos.DrawSphere(testpos, 0.1f);
                    // Vector3 pos2 = Utility.GetInterpolatedPosition(lastRecordedPosition, Totaldistance, distance + val);
                    // Vector3 direction = Utility.GetInterpolatedPositionDirection(lastRecordedPosition, Totaldistance, distance + val);
                    // Gizmos.DrawCube(Utility.RotateAroundPivot2(originalverts[i], targetobject.transform.position, Quaternion.FromToRotation(lastRecordedPosition[3], lastRecordedPosition[4]))+pos, Vector3.one*0.1f);
                    //modifiedVertices[i] = Utility.RotateAroundPivot2(originalverts[i], targetobject.transform.position, Quaternion.FromToRotation(lastRecordedPosition[0], lastRecordedPosition[1])) + pos;
                    modifiedVertices[i] = Utility.RotatePointAroundPivot(originalverts[i], targetobject.transform.position, targetobject.transform.forward, Utility.GetDirection(lastRecordedPosition[posint], lastRecordedPosition[posint + 1])) + pos;
                    //  Gizmos.DrawSphere(modifiedVertices[i], 0.1f);

                }

                if (reset || cont)
                {
                    // Gizmos.DrawSphere(originalverts[0], 0.1f);
                    targetmesh.vertices = modifiedVertices;
                    targetmesh.triangles = mesh.triangles;
                    targetmesh.RecalculateBounds();
                    reset = false;
                }
            }
            else
            {

                Vector3[] modifiedVertices = new Vector3[originalverts.Length];

                for (int i = 0; i < originalverts.Length; i++)
                {
                    float val = Mathf.Lerp(-bound.extents.z, bound.extents.z, Mathf.InverseLerp(-bound.extents.z, bound.extents.z, originalverts[i].z));
                    int posint = Utility.GetInterpolatedPositionIndex(lastRecordedPosition, Totaldistance, distance + val);
                    Vector3 pos = Utility.GetInterpolatedPositionUnclamped(lastRecordedPosition, Totaldistance, distance);
                    Vector3 pos2 = Utility.GetInterpolatedPositionUnclamped(lastRecordedPosition, Totaldistance, distance + val);
                    Vector3 p1 = pointers[posint].transform.TransformPoint(new Vector3(originalverts[i].x, originalverts[i].y, 0));
                    Vector3 p2 = pointers[posint + 1].transform.TransformPoint(new Vector3(originalverts[i].x, originalverts[i].y, 0));
                    List<Vector3> poss = new List<Vector3>();
                    poss.Add(p1);
                    poss.Add(p2);
                    Vector3 finl = Vector3.Lerp(p1, p2, Utility.GetInterpolatedPositionUnclampedValue(lastRecordedPosition, Totaldistance, distance + val));




                    // Vector3 pos2 = Utility.GetInterpolatedPosition(lastRecordedPosition, Totaldistance, distance + val);
                    // Vector3 direction = Utility.GetInterpolatedPositionDirection(lastRecordedPosition, Totaldistance, distance + val);
                    // Gizmos.DrawCube(Utility.RotateAroundPivot2(originalverts[i], targetobject.transform.position, Quaternion.FromToRotation(lastRecordedPosition[3], lastRecordedPosition[4]))+pos, Vector3.one*0.1f);
                    //modifiedVertices[i] = Utility.RotateAroundPivot2(originalverts[i], targetobject.transform.position, Quaternion.FromToRotation(lastRecordedPosition[0], lastRecordedPosition[1])) + pos;
                    //Vector3 p1 = Utility.RotatePointAroundPivot(originalverts[i], pos2, targetobject.transform.forward.normalized, Utility.GetDirection(lastRecordedPosition[posint], lastRecordedPosition[posint + 1])) ;
                    // Vector3 p2 = Utility.RotatePointAroundPivot(originalverts[i],pos2, Utility.GetDirection(lastRecordedPosition[posint + 0], lastRecordedPosition[posint + 1]), Utility.GetDirection(lastRecordedPosition[posint + 1], lastRecordedPosition[posint + 2])) ;
                    //  modifiedVertices[i] = Vector3.Lerp(p1, p2, Utility.GetInterpolatedPositionUnclampedValue(lastRecordedPosition, Totaldistance, distance + (val)))+pos;
                    //modifiedVertices[i] = p1 +pos;
                    //Vector3 offset = Vector3.Cross(Utility.GetDirection(lastRecordedPosition[posint], lastRecordedPosition[posint + 1]), Vector3.up).normalized *originalverts[i].x ;

                    Vector3 h1 = Vector3.Lerp(p1, p2, 0.5f);
                    Vector3 h2 = Vector3.Lerp(p2, p1, 0.5f);
                    Vector3 f2 = pointers[posint + 1].transform.TransformPoint(new Vector3(originalverts[i].x, originalverts[i].y, val));
                    Vector3 f1 = pointers[posint].transform.TransformPoint(new Vector3(originalverts[i].x, originalverts[i].y, val));

                    //modifiedVertices[i] = Vector3.Lerp(f1,f2, Utility.GetInterpolatedPositionUnclampedValue(lastRecordedPosition, Totaldistance, distance ));
                    // modifiedVertices[i] = Vector3.Lerp(f1,f2, Utility.GetInterpolatedPositionUnclampedValue(lastRecordedPosition, Totaldistance, distance +val));
                    modifiedVertices[i] = Vector3.Lerp(p1, p2, Utility.GetInterpolatedPositionUnclampedValue(lastRecordedPosition, Totaldistance, distance + val));

                    // Gizmos.DrawSphere(modifiedVertices[i], 0.1f);
                    Gizmos.DrawSphere(p2, 0.1f);
                    //  Gizmos.DrawSphere(p2, 0.1f);
                    // Debug.Log(val);

                }

                if (reset || cont)
                {
                    // Gizmos.DrawSphere(originalverts[0], 0.1f);
                    targetmesh.vertices = modifiedVertices;
                    targetmesh.triangles = mesh.triangles;
                    targetmesh.RecalculateBounds();
                    reset = false;
                }
            }
        }
        else
        {
            if (!smoothinterpolation)
            {
                for (int j = 0; j < numofrepeat; j++)
                {
                    float distance2 = Mathf.Abs(-bound.extents.z - bound.extents.z) * j + distance;
                    Vector3 pos = Utility.GetInterpolatedPositionUnclamped(lastRecordedPosition, Totaldistance, distance2);
                    //  Gizmos.DrawSphere(pos, 0.5f);
                    for (int i = 0; i < originalverts.Length; i++)
                    {
                        float val = Mathf.Lerp(-bound.extents.z, bound.extents.z, Mathf.InverseLerp(-bound.extents.z, bound.extents.z, originalverts[i].z));
                        int posint = Utility.GetInterpolatedPositionIndex(lastRecordedPosition, Totaldistance, distance2 + (val));
                        Debug.Log(Utility.GetInterpolatedPositionUnclampedValue(lastRecordedPosition, Totaldistance, distance2 + (val)));

                        // Vector3 pos2 = Utility.GetInterpolatedPosition(lastRecordedPosition, Totaldistance, distance + val);
                        // Vector3 direction = Utility.GetInterpolatedPositionDirection(lastRecordedPosition, Totaldistance, distance + val);
                        // Gizmos.DrawCube(Utility.RotateAroundPivot2(originalverts[i], targetobject.transform.position, Quaternion.FromToRotation(lastRecordedPosition[3], lastRecordedPosition[4]))+pos, Vector3.one*0.1f);
                        //modifiedVertices[i] = Utility.RotateAroundPivot2(originalverts[i], targetobject.transform.position, Quaternion.FromToRotation(lastRecordedPosition[0], lastRecordedPosition[1])) + pos;
                        Vector3 vectpos = Utility.RotatePointAroundPivot(originalverts[i], targetobject.transform.position, targetobject.transform.forward, Utility.GetDirection(lastRecordedPosition[posint], lastRecordedPosition[posint + 1]));
                        Vector3 vectpos2 = Utility.RotatePointAroundPivot(originalverts[i], targetobject.transform.position, targetobject.transform.forward, Utility.GetDirection(lastRecordedPosition[posint + 1], lastRecordedPosition[posint + 1 + 1]));
                        multipliedverts[j][i] = originalverts[i] + pos;
                        //    Gizmos.DrawSphere(multipliedverts[j][i], 0.1f);
                    }


                }
                Vector3[] data = Utility.CombineListOfArrays(multipliedverts);
                int[] data2 = Utility.CombineListOfArrays(multipliedtriangles);

                if (reset || cont)
                {
                    // Gizmos.DrawSphere(originalverts[0], 0.1f);
                    targetmesh.vertices = data;
                    targetmesh.triangles = data2;
                    targetmesh.RecalculateNormals();
                    targetmesh.RecalculateBounds();
                    reset = false;


                }
            }
            else
            {
                for (int j = 0; j < numofrepeat; j++)
                {
                    Bounds bound2 = bound;
                    if (meshIterations != null)
                    {
                        if (meshIterations.Count > 0)
                        {
                            if (meshIterations.Count > j)
                            {
                                bound2 = meshIterations[j].bounds;
                                float distance2 = Mathf.Abs(-bound2.extents.z - bound2.extents.z) * j + distance;
                                Vector3 pos = Utility.GetInterpolatedPositionUnclamped(lastRecordedPosition, Totaldistance, distance2);
                                for (int i = 0; i < meshIterations[j].vertices.Length; i++)
                                {
                                    float val = Mathf.Lerp(-bound2.extents.z, bound2.extents.z, Mathf.InverseLerp(-bound2.extents.z, bound2.extents.z, meshIterations[j].vertices[i].z));
                                    int posint = Utility.GetInterpolatedPositionIndex(lastRecordedPosition, Totaldistance, distance2 + val);


                                    Vector3 f1 = Utility.TransformPoint(new Vector3(meshIterations[j].vertices[i].x, meshIterations[j].vertices[i].y, 0), points[posint].originalposition, points[posint].forwarddirection, points[posint].updirection, points[posint].rightdirection);
                                    Vector3 f2 = Utility.TransformPoint(new Vector3(meshIterations[j].vertices[i].x, meshIterations[j].vertices[i].y, 0), points[posint + 1].originalposition, points[posint + 1].forwarddirection, points[posint + 1].updirection, points[posint + 1].rightdirection);

                                    float t = Utility.GetInterpolatedPositionUnclampedValue(lastRecordedPosition, Totaldistance, distance2 + val);
                                    //Debug.Log((distance2 + val) + " / " + Totaldistance);
                                    //Debug.DrawLine(f1 + transform.position, f2 + transform.position);
                                    //multipliedverts[j][i] = Vector3.Lerp(f1, f2, Utility.GetInterpolatedPositionUnclampedValue(lastRecordedPosition, Totaldistance, distance2 + val));

                                    //Debug.Log(t);

                                    if (!(t < 0 || (distance2 + val) > Totaldistance))
                                    {
                                        multipliedverts[j][i] = Vector3.Lerp(f1, f2, Utility.GetInterpolatedPositionUnclampedValue(lastRecordedPosition, Totaldistance, distance2 + val));
                                    }
                                    else if (t < 0)
                                    {
                                        //   Debug.DrawLine(f1 + transform.position, f2 + transform.position);


                                        float interpolationRatio2 = Mathf.Abs(distance2 + val) / Vector3.Distance(f1, f2);

                                        multipliedverts[j][i] = ((f1 - f2) * (interpolationRatio2 * Vector3.Distance(f1, f2))) + f1;

                                        //multipliedverts[j][i] = (f1 - f2) * (Mathf.Abs(distance2 + val)) + f1;
                                    }
                                    else if ((distance2 + val) > Totaldistance)
                                    {
                                        //Debug.DrawLine(f1 + transform.position, f2 + transform.position);
                                        float remainingDistance2 = (distance2 + val) - Totaldistance;

                                        float interpolationRatio2 = remainingDistance2 / Vector3.Distance(f1, f2);

                                        multipliedverts[j][i] = ((f2 - f1) * (interpolationRatio2 * Vector3.Distance(f1, f2))) + f2;

                                        //multipliedverts[j][i] = (lastRecordedPosition[lastRecordedPosition.Count - 1] - lastRecordedPosition[lastRecordedPosition.Count - 2]) * (Mathf.Abs(distance2 + val)) + lastRecordedPosition[lastRecordedPosition.Count - 1];
                                    }


                                }
                            }
                            else
                            {
                                bound2 = meshIterations[meshIterations.Count - 1].bounds;
                                float distance2 = Mathf.Abs(-bound2.extents.z - bound2.extents.z) * j + distance;
                                Vector3 pos = Utility.GetInterpolatedPositionUnclamped(lastRecordedPosition, Totaldistance, distance2);
                                for (int i = 0; i < meshIterations[meshIterations.Count - 1].vertices.Length; i++)
                                {
                                    float val = Mathf.Lerp(-bound2.extents.z, bound2.extents.z, Mathf.InverseLerp(-bound2.extents.z, bound2.extents.z, meshIterations[meshIterations.Count - 1].vertices[i].z));
                                    int posint = Utility.GetInterpolatedPositionIndex(lastRecordedPosition, Totaldistance, distance2 + val);


                                    Vector3 f1 = Utility.TransformPoint(new Vector3(meshIterations[meshIterations.Count - 1].vertices[i].x, meshIterations[meshIterations.Count - 1].vertices[i].y, 0), points[posint].originalposition, points[posint].forwarddirection, points[posint].updirection, points[posint].rightdirection);
                                    Vector3 f2 = Utility.TransformPoint(new Vector3(meshIterations[meshIterations.Count - 1].vertices[i].x, meshIterations[meshIterations.Count - 1].vertices[i].y, 0), points[posint + 1].originalposition, points[posint + 1].forwarddirection, points[posint + 1].updirection, points[posint + 1].rightdirection);

                                    multipliedverts[j][i] = Vector3.Lerp(f1, f2, Utility.GetInterpolatedPositionUnclampedValue(lastRecordedPosition, Totaldistance, distance2 + val));

                                }
                            }
                        }
                        else
                        {
                            float distance2 = Mathf.Abs(-bound2.extents.z - bound2.extents.z) * j + distance;
                            Vector3 pos = Utility.GetInterpolatedPositionUnclamped(lastRecordedPosition, Totaldistance, distance2);
                            for (int i = 0; i < originalverts.Length; i++)
                            {
                                float val = Mathf.Lerp(-bound2.extents.z, bound2.extents.z, Mathf.InverseLerp(-bound2.extents.z, bound2.extents.z, originalverts[i].z));
                                int posint = Utility.GetInterpolatedPositionIndex(lastRecordedPosition, Totaldistance, distance2 + val);


                                Vector3 f1 = Utility.TransformPoint(new Vector3(originalverts[i].x, originalverts[i].y, 0), points[posint].originalposition, points[posint].forwarddirection, points[posint].updirection, points[posint].rightdirection);
                                Vector3 f2 = Utility.TransformPoint(new Vector3(originalverts[i].x, originalverts[i].y, 0), points[posint + 1].originalposition, points[posint + 1].forwarddirection, points[posint + 1].updirection, points[posint + 1].rightdirection);

                                multipliedverts[j][i] = Vector3.Lerp(f1, f2, Utility.GetInterpolatedPositionUnclampedValue(lastRecordedPosition, Totaldistance, distance2 + val));

                            }
                        }
                    }
                }
                Vector3[] data = Utility.CombineListOfArrays(multipliedverts);
                int[] data2 = Utility.CombineListOfArrays(multipliedtriangles);

                if (reset || cont)
                {
                    //   Debug.Log("totalverts: "+data.Length);
                    //    Debug.Log("totaltris: " + data2.Length);

                    targetmesh.vertices = data;
                    targetmesh.triangles = data2;
                    targetmesh.RecalculateNormals();
                    targetmesh.RecalculateBounds();
                    reset = false;

                }
            }
        }



    }
    public static void RenderCurve(List<Points> points)
    {



    }
    void RecordMousePosition()
    {
        RaycastHit hit;
        Utility.RayCastToMouse(cam, out hit);
        points.Add(new Points(hit.point, Vector3.one, 0, Vector3.zero, Vector3.zero, Vector3.zero));
        //  Debug.Log("Mouse Position Recorded: " + hit.point);
        currentRecordInterval = recordInterval;
    }

    public Vector3 GetRotatedPosition(Vector3 position, Vector3 pivot, Vector3 direction)
    {
        // Calculate the offset from the pivot
        Vector3 offset = position - pivot;

        // Rotate the offset along the pivot's direction
        Quaternion rotation = Quaternion.Euler(direction);
        Vector3 rotatedOffset = rotation * offset;

        // Calculate the rotated position by adding the rotated offset to the pivot
        Vector3 rotatedPosition = pivot + rotatedOffset;

        return rotatedPosition;
    }

    [System.Serializable]
    public struct Points
    {
        public Vector3 originalposition;
        public Vector3 forwarddirection;
        public Vector3 rightdirection;
        public Vector3 updirection;
        public Vector3 scale;
        // public Vector3[] data;
        public float twist;
        public Points(Vector3 pos, Vector3 scale, float twist, Vector3 forward, Vector3 right, Vector3 up)
        {
            this.originalposition = pos;
            this.scale = scale;
            this.twist = twist;
            this.forwarddirection = forward;
            this.rightdirection = right;
            this.updirection = up;
        }
        public Points(Points point)
        {
            this.originalposition = point.originalposition;
            this.scale = point.scale;
            this.twist = point.twist;
            this.updirection = point.updirection;
            this.rightdirection = point.rightdirection;
            this.forwarddirection = point.forwarddirection;
        }
        /*
       public Points(Points point, Vector3[] data)
        {
            this.originalposition = point.originalposition;
            this.scale = point.scale;
            this.updirection = point.updirection;
            this.rightdirection = point.rightdirection;
            this.forwarddirection = point.forwarddirection;
            this.data = data;
            if (this.data == null)
                data = new Vector3[0];
            this.twist = point.twist;
        }
        */
        public Points(Points point, Vector3 forward, Vector3 right, Vector3 up)
        {
            this.originalposition = point.originalposition;
            this.scale = point.scale;
            /*
            this.data = point.data;
            if (this.data == null)
                this.data = new Vector3[0];
            this.data=data;
            */
            this.twist = point.twist;
            this.updirection = up;
            this.rightdirection = right;
            this.forwarddirection = forward;
        }


    }
}
