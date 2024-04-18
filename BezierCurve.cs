using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 p0;
    public Vector3 c0;
    public Vector3 c1;
    public Vector3 p1;
    public float iterator = 0.05f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(p0+transform.position,Vector3.one* 0.2f);
        Gizmos.DrawCube(p1 + transform.position, Vector3.one* 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(c0 + transform.position,Vector3.one* 0.2f);
        Gizmos.DrawCube(c1 + transform.position,Vector3.one* 0.2f);
        Gizmos.color = Color.white ;

        Gizmos.DrawLine(p0 + transform.position, c0 + transform.position);
        Gizmos.DrawLine(p1 + transform.position, c1 + transform.position);

        for (float i = 0; i <= 1; i+=iterator)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(Utility.CubicCurve(p0, c0, c1, p1, i)+transform.position,Utility.CubicCurve(p0, c0, c1, p1, i+iterator) + transform.position);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Utility.CubicCurve(p0, c0, c1, p1,i) + transform.position, 0.1f);
        }
    }
}
