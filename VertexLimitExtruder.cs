using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class VertexLimitExtruder : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 min;
    private Vector3 max;
    public Mesh mesh;
    public GameObject targetobject;
    public Orientation orientation;
    [Range(0, 1)]
    public float minSlider;
    [Range(0, 1)]
    public float maxSlider;

    public float range;
    public bool usemin;
    public bool set;
    public bool load;
    private void OnDrawGizmos()
    {
        if (targetobject != null)
        {
            if (mesh != null)
            {
                Vector3[] vertices = Utility.GetMesh(targetobject).vertices;

                for (int i = 0; i < vertices.Length; i++)
                {
                    // vertices[i] = Utility.ScaleMoveRotateVector(transform, vertices[i]);
                }
                Utility.FindMinMax(vertices, out min, out max);

                Gizmos.color = Color.black;
                Gizmos.DrawSphere(min, 0.2f);
                Gizmos.DrawSphere(max, 0.2f);
                float maxdist = 0;
                List<int> newdata = new List<int>();

                if (orientation == Orientation.X)
                {

                    float start = max.x;
                    float end = min.x;
                    float slidemax = Mathf.Lerp(start, end, minSlider);
                    float slidemin = Mathf.Lerp(start, end, maxSlider);
                    if (slidemax >= slidemin)
                    {
                        if (usemin)

                            maxdist = Mathf.Infinity;
                        for (int i = 0; i < vertices.Length; i++)
                        {


                            if (vertices[i].x >= slidemin && vertices[i].x <= slidemax)
                            {
                                Gizmos.color = Color.red;
                                Gizmos.DrawSphere(Utility.ScaleMoveRotateVector(transform, vertices[i]), 0.1f);
                                Vector3 start2 = transform.position + transform.right.normalized * mesh.bounds.extents.x * transform.localScale.x;
                                Vector3 end2 = transform.position + -transform.right.normalized * mesh.bounds.extents.x * transform.localScale.x;
                                RaycastHit hit;
                                if (Physics.Raycast(Utility.ScaleMoveRotateVector(transform, vertices[i]), start2 - end2, out hit, range))
                                {
                                    if (!usemin)
                                    {
                                        if (hit.distance > maxdist)
                                        {
                                            Gizmos.color = Color.green;
                                            maxdist = hit.distance;
                                        }
                                    }
                                    else
                                    {
                                        if (hit.distance < maxdist)
                                        {
                                            Gizmos.color = Color.green;
                                            maxdist = hit.distance;
                                        }
                                    }


                                }
                                newdata.Add(i);
                                Gizmos.color = Color.yellow;
                                Gizmos.DrawSphere(Utility.MoveVectorAlongDirection(Utility.ScaleMoveRotateVector(transform, vertices[i]), (start2 - end2).normalized * maxdist), 0.1f);



                            }
                            else
                            {
                                Gizmos.color = Color.blue;
                                Gizmos.DrawSphere(Utility.ScaleMoveRotateVector(transform, vertices[i]), 0.1f);
                            }

                        }
                    }
                }

                else if (orientation == Orientation.Y)
                {
                    float start = max.y;
                    float end = min.y;
                    float slidemax = Mathf.Lerp(start, end, minSlider);
                    float slidemin = Mathf.Lerp(start, end, maxSlider);
                    if (slidemax >= slidemin)
                    {
                        if (usemin)

                            maxdist = Mathf.Infinity;
                        for (int i = 0; i < vertices.Length; i++)
                        {


                            if (vertices[i].y >= slidemin && vertices[i].y <= slidemax)
                            {
                                Gizmos.color = Color.red;
                                Gizmos.DrawSphere(Utility.ScaleMoveRotateVector(transform, vertices[i]), 0.1f);
                                Vector3 start2 = transform.position + transform.up.normalized * mesh.bounds.extents.y * transform.localScale.y;
                                Vector3 end2 = transform.position + -transform.up.normalized * mesh.bounds.extents.y * transform.localScale.y;
                                RaycastHit hit;
                                if (Physics.Raycast(Utility.ScaleMoveRotateVector(transform, vertices[i]), start2 - end2, out hit, range))
                                {
                                    if (!usemin)
                                    {
                                        if (hit.distance > maxdist)
                                        {
                                            Gizmos.color = Color.green;
                                            maxdist = hit.distance;
                                        }
                                    }
                                    else
                                    {
                                        if (hit.distance < maxdist)
                                        {
                                            Gizmos.color = Color.green;
                                            maxdist = hit.distance;
                                        }
                                    }


                                }
                                newdata.Add(i);
                                Gizmos.color = Color.yellow;
                                Gizmos.DrawSphere(Utility.MoveVectorAlongDirection(Utility.ScaleMoveRotateVector(transform, vertices[i]), (start2 - end2).normalized * maxdist), 0.1f);



                            }
                            else
                            {
                                Gizmos.color = Color.blue;
                                Gizmos.DrawSphere(Utility.ScaleMoveRotateVector(transform, vertices[i]), 0.1f);
                            }

                        }
                    }
                }

                else if (orientation == Orientation.Z)
                {
                    float start = max.z;
                    float end = min.z;
                    float slidemax = Mathf.Lerp(start, end, minSlider);
                    float slidemin = Mathf.Lerp(start, end, maxSlider);
                    if (slidemax >= slidemin)
                    {
                        if (usemin)

                            maxdist = Mathf.Infinity;
                        for (int i = 0; i < vertices.Length; i++)
                        {


                            if (vertices[i].z >= slidemin && vertices[i].z <= slidemax)
                            {
                                Gizmos.color = Color.red;
                                Gizmos.DrawSphere(Utility.ScaleMoveRotateVector(transform, vertices[i]), 0.1f);
                                Vector3 start2 = transform.position + transform.forward.normalized * mesh.bounds.extents.z * transform.localScale.z;
                                Vector3 end2 = transform.position + -transform.forward.normalized * mesh.bounds.extents.z * transform.localScale.z;
                                RaycastHit hit;
                                if (Physics.Raycast(Utility.ScaleMoveRotateVector(transform, vertices[i]), start2 - end2, out hit, range))
                                {
                                    if (!usemin)
                                    {
                                        if (hit.distance > maxdist)
                                        {
                                            Gizmos.color = Color.green;
                                            maxdist = hit.distance;
                                        }
                                    }
                                    else
                                    {
                                        if (hit.distance < maxdist)
                                        {
                                            Gizmos.color = Color.green;
                                            maxdist = hit.distance;
                                        }
                                    }


                                }
                                newdata.Add(i);
                                Gizmos.color = Color.yellow;
                                Gizmos.DrawSphere(Utility.MoveVectorAlongDirection(Utility.ScaleMoveRotateVector(transform, vertices[i]), (start2 - end2).normalized * maxdist), 0.1f);



                            }
                            else
                            {
                                Gizmos.color = Color.blue;
                                Gizmos.DrawSphere(Utility.ScaleMoveRotateVector(transform, vertices[i]), 0.1f);
                            }

                        }
                    }
                }

                else if (orientation == Orientation.N_X)
                {

                    float start = min.x;
                    float end = max.x;
                    float slidemax = Mathf.Lerp(start, end, maxSlider);
                    float slidemin = Mathf.Lerp(start, end, minSlider);
                    if (slidemax >= slidemin)
                    {
                        if (usemin)

                            maxdist = Mathf.Infinity;
                        for (int i = 0; i < vertices.Length; i++)
                        {


                            if (vertices[i].x >= slidemin && vertices[i].x <= slidemax)
                            {
                                Gizmos.color = Color.red;
                                Gizmos.DrawSphere(Utility.ScaleMoveRotateVector(transform, vertices[i]), 0.1f);
                                Vector3 start2 = transform.position + -transform.right.normalized * mesh.bounds.extents.x * transform.localScale.x;
                                Vector3 end2 = transform.position + transform.right.normalized * mesh.bounds.extents.x * transform.localScale.x;
                                RaycastHit hit;
                                if (Physics.Raycast(Utility.ScaleMoveRotateVector(transform, vertices[i]), start2 - end2, out hit, range))
                                {
                                    if (!usemin)
                                    {
                                        if (hit.distance > maxdist)
                                        {
                                            Gizmos.color = Color.green;
                                            maxdist = hit.distance;
                                        }
                                    }
                                    else
                                    {
                                        if (hit.distance < maxdist)
                                        {
                                            Gizmos.color = Color.green;
                                            maxdist = hit.distance;
                                        }
                                    }


                                }
                                newdata.Add(i);
                                Gizmos.color = Color.yellow;
                                Gizmos.DrawSphere(Utility.MoveVectorAlongDirection(Utility.ScaleMoveRotateVector(transform, vertices[i]), (start2 - end2).normalized * maxdist), 0.1f);



                            }
                            else
                            {
                                Gizmos.color = Color.blue;
                                Gizmos.DrawSphere(Utility.ScaleMoveRotateVector(transform, vertices[i]), 0.1f);
                            }

                        }
                    }
                }

                else if (orientation == Orientation.N_Y)
                {

                    float start = min.y;
                    float end = max.y;
                    float slidemax = Mathf.Lerp(start, end, maxSlider);
                    float slidemin = Mathf.Lerp(start, end, minSlider);
                    if (slidemax >= slidemin)
                    {
                        if (usemin)

                            maxdist = Mathf.Infinity;
                        for (int i = 0; i < vertices.Length; i++)
                        {


                            if (vertices[i].y >= slidemin && vertices[i].y <= slidemax)
                            {
                                Gizmos.color = Color.red;
                                Gizmos.DrawSphere(Utility.ScaleMoveRotateVector(transform, vertices[i]), 0.1f);
                                Vector3 start2 = transform.position + -transform.up.normalized * mesh.bounds.extents.y * transform.localScale.y;
                                Vector3 end2 = transform.position + transform.up.normalized * mesh.bounds.extents.y * transform.localScale.y;
                                RaycastHit hit;
                                if (Physics.Raycast(Utility.ScaleMoveRotateVector(transform, vertices[i]), start2 - end2, out hit, range))
                                {
                                    if (!usemin)
                                    {
                                        if (hit.distance > maxdist)
                                        {
                                            Gizmos.color = Color.green;
                                            maxdist = hit.distance;
                                        }
                                    }
                                    else
                                    {
                                        if (hit.distance < maxdist)
                                        {
                                            Gizmos.color = Color.green;
                                            maxdist = hit.distance;
                                        }
                                    }


                                }
                                newdata.Add(i);
                                Gizmos.color = Color.yellow;
                                Gizmos.DrawSphere(Utility.MoveVectorAlongDirection(Utility.ScaleMoveRotateVector(transform, vertices[i]), (start2 - end2).normalized * maxdist), 0.1f);



                            }
                            else
                            {
                                Gizmos.color = Color.blue;
                                Gizmos.DrawSphere(Utility.ScaleMoveRotateVector(transform, vertices[i]), 0.1f);
                            }

                        }
                    }
                }

                else if (orientation == Orientation.N_Z)
                {

                    float start = min.z;
                    float end = max.z;
                    float slidemax = Mathf.Lerp(start, end, maxSlider);
                    float slidemin = Mathf.Lerp(start, end, minSlider);
                    if (slidemax >= slidemin)
                    {
                        if (usemin)

                            maxdist = Mathf.Infinity;
                        for (int i = 0; i < vertices.Length; i++)
                        {


                            if (vertices[i].z >= slidemin && vertices[i].z <= slidemax)
                            {
                                Gizmos.color = Color.red;
                                Gizmos.DrawSphere(Utility.ScaleMoveRotateVector(transform, vertices[i]), 0.1f);
                                Vector3 start2 = transform.position + -transform.forward.normalized * mesh.bounds.extents.z * transform.localScale.z;
                                Vector3 end2 = transform.position + transform.forward.normalized * mesh.bounds.extents.z * transform.localScale.z;
                                RaycastHit hit;
                                if (Physics.Raycast(Utility.ScaleMoveRotateVector(transform, vertices[i]), start2 - end2, out hit, range))
                                {
                                    if (!usemin)
                                    {
                                        if (hit.distance > maxdist)
                                        {
                                            Gizmos.color = Color.green;
                                            maxdist = hit.distance;
                                        }
                                    }
                                    else
                                    {
                                        if (hit.distance < maxdist)
                                        {
                                            Gizmos.color = Color.green;
                                            maxdist = hit.distance;
                                        }
                                    }


                                }
                                newdata.Add(i);
                                Gizmos.color = Color.yellow;
                                Gizmos.DrawSphere(Utility.MoveVectorAlongDirection(Utility.ScaleMoveRotateVector(transform, vertices[i]), (start2 - end2).normalized * maxdist), 0.1f);



                            }
                            else
                            {
                                Gizmos.color = Color.blue;
                                Gizmos.DrawSphere(Utility.ScaleMoveRotateVector(transform, vertices[i]), 0.1f);
                            }

                        }
                    }
                }



                if (set)
                {
                    if (orientation == Orientation.N_X)
                    {
                        for (int i = 0; i < newdata.Count; i++)
                        {
                            Debug.Log(newdata[i]);
                            vertices[newdata[i]] = Utility.MoveVectorAlongDirection(vertices[newdata[i]], -Vector3.right * maxdist);

                        }
                    }
                    else if (orientation == Orientation.X)
                    {
                        for (int i = 0; i < newdata.Count; i++)
                        {
                            Debug.Log(newdata[i]);
                            vertices[newdata[i]] = Utility.MoveVectorAlongDirection(vertices[newdata[i]], Vector3.right * maxdist);

                        }
                    }
                    else if (orientation == Orientation.Y)
                    {
                        for (int i = 0; i < newdata.Count; i++)
                        {
                            Debug.Log(newdata[i]);
                            vertices[newdata[i]] = Utility.MoveVectorAlongDirection(vertices[newdata[i]], Vector3.up * maxdist);

                        }
                    }
                    else if (orientation == Orientation.Z)
                    {
                        for (int i = 0; i < newdata.Count; i++)
                        {
                            Debug.Log(newdata[i]);
                            vertices[newdata[i]] = Utility.MoveVectorAlongDirection(vertices[newdata[i]], Vector3.forward * maxdist);

                        }
                    }
                    else if (orientation == Orientation.N_Y)
                    {
                        for (int i = 0; i < newdata.Count; i++)
                        {
                            Debug.Log(newdata[i]);
                            vertices[newdata[i]] = Utility.MoveVectorAlongDirection(vertices[newdata[i]], -Vector3.up * maxdist);

                        }
                    }
                    else if (orientation == Orientation.N_Z)
                    {
                        for (int i = 0; i < newdata.Count; i++)
                        {
                            Debug.Log(newdata[i]);
                            vertices[newdata[i]] = Utility.MoveVectorAlongDirection(vertices[newdata[i]], -Vector3.forward * maxdist);

                        }
                    }

                    Utility.SetVertices(targetobject, vertices);
                    mesh.RecalculateBounds();
                    set = false;
                }
                if (load)
                {
                    targetobject.GetComponent<MeshFilter>().mesh = mesh;
                    load = false;
                }

            }
        }
    }


}
