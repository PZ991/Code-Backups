using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooleanOperatorV2 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject targetmesh;
    public GameObject subtractingmesh;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnDrawGizmos()
    {
        Mesh mesh = Utility.GetMesh(targetmesh);

        List<Vector3> hitpoints = new List<Vector3>();
        for (int i = 0; i < mesh.triangles.Length; i+=3)
        {
        Vector3[] vertices = Utility.GetTriangleVertices(mesh, i/3);
/*

            for (int j = 0; j < vertices.Length; j++)
            {
                // Rotate the vertex
                vertices[j] = targetmesh.transform.rotation * vertices[j];

                // Scale the vertex
                vertices[j] = Vector3.Scale(vertices[j], targetmesh.transform.localScale );

                // Translate the vertex
                vertices[j] = targetmesh.transform.position +  (vertices[j] );
            }
             */
            for (int j = 0; j < vertices.Length; j++)
            {
                vertices[j] = Utility.ScaleMoveRotateVector(targetmesh.transform, vertices[j]);
            }
            Gizmos.color = Color.black;
            Gizmos.DrawLine(vertices[0], vertices[1]);
            Gizmos.DrawLine(vertices[0], vertices[2]);
            Gizmos.DrawLine(vertices[1], vertices[2]);
            Collider cols=targetmesh.GetComponent<Collider>();
            cols.enabled = false;
            RaycastHit hit;

            
            if(Physics.Linecast(vertices[0], vertices[1],out hit))
            {
                Gizmos.DrawSphere(hit.point, 0.01f);
                if (!hitpoints.Contains(hit.point))
                    hitpoints.Add(hit.point);
            }
            if(Physics.Linecast(vertices[1], vertices[2],out hit))
            {
                Gizmos.DrawSphere(hit.point, 0.01f);
                if (!hitpoints.Contains(hit.point))
                    hitpoints.Add(hit.point);
            }
            if(Physics.Linecast(vertices[0], vertices[2],out hit))
            {
                Gizmos.DrawSphere(hit.point, 0.01f);
                if (!hitpoints.Contains(hit.point))
                    hitpoints.Add(hit.point);

            }


            if (Physics.Linecast(vertices[1], vertices[0],out hit))
            {
                Gizmos.DrawSphere(hit.point, 0.01f);
                if (!hitpoints.Contains(hit.point))
                    hitpoints.Add(hit.point);

            }
            if (Physics.Linecast(vertices[2], vertices[1],out hit))
            {
                Gizmos.DrawSphere(hit.point, 0.01f);
                if (!hitpoints.Contains(hit.point))
                    hitpoints.Add(hit.point);

            }
            if (Physics.Linecast(vertices[2], vertices[0],out hit))
            {
                Gizmos.DrawSphere(hit.point, 0.01f);
                if(!hitpoints.Contains(hit.point))
                hitpoints.Add(hit.point);

            }
            cols.enabled = true;


        }
        Gizmos.color = Color.red;
        Debug.Log(hitpoints.Count);
        /*
        for (int i = 0; i < hitpoints.Count; i++)
        {
            RaycastHit hit;
            for (int j = 0; j< hitpoints.Count; j++)
            {
                if (j == i)
                    continue;
                Debug.Log(Physics.Linecast(hitpoints[i], hitpoints[j], out hit));
                if (Physics.Linecast(hitpoints[i], hitpoints[j], out hit))
                {
                    
           // Gizmos.DrawLine(hitpoints[i], hitpoints[j]);

                }
            }

        }
        */
    }
}
