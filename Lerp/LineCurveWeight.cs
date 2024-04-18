using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCurveWeight : MonoBehaviour
{



    



    [Header("BezierLerp")]
    public Transform transformBegin;
    public Transform transformEnd;
    
    public Transform vectortargetlerpstart;
    public Transform vectortargetlerpend;

    public float bezier;
    public Transform testlerp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public static float[] MakeSmoothCurveFloat(float[] arrayToCurve, float smoothness)
    {

        List<float> points;
        List<float> curvedPoints;
        int pointsLength = 0;
        int curvedLength = 0;

        if (smoothness < 1.0f) smoothness = 1.0f;

        pointsLength = arrayToCurve.Length;

        curvedLength = (pointsLength * Mathf.RoundToInt(smoothness)) - 1;
        curvedPoints = new List<float>(curvedLength);

        float t = 0.0f;
        for (int pointInTimeOnCurve = 0; pointInTimeOnCurve < curvedLength + 1; pointInTimeOnCurve++)
        {
            t = Mathf.InverseLerp(0, curvedLength, pointInTimeOnCurve);

            points = new List<float>(arrayToCurve);

            for (int j = pointsLength - 1; j > 0; j--)
            {
                for (int i = 0; i < j; i++)
                {
                    points[i] = (1 - t) * points[i] + t * points[i + 1];
                }
            }

            curvedPoints.Add(points[0]);
        }

        return (curvedPoints.ToArray());
    }

     Vector3 GetBezierPosition(float t)
    {
        Vector3 p0 = transformBegin.position;
        Vector3 p1 = p0 + transformBegin.forward;
        Vector3 p3 = transformEnd.position;
        Vector3 p2 = p3 - -transformEnd.forward;

        // here is where the magic happens!
        return Mathf.Pow(1f - t, 3f) * p0 + 3f * Mathf.Pow(1f - t, 2f) * t * p1 + 3f * (1f - t) * Mathf.Pow(t, 2f) * p2 + Mathf.Pow(t, 3f) * p3;
    }

    private void OnDrawGizmos()
    {
        #region Lerp1
        

        Gizmos.color = Color.black;
        Gizmos.DrawCube(transformBegin.position, Vector3.one * 0.1f);
        Gizmos.DrawCube(transformEnd.position, Vector3.one * 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(vectortargetlerpstart.position, Vector3.one * 0.1f);
        Gizmos.DrawCube(vectortargetlerpend.position, Vector3.one * 0.1f);
        transformBegin.LookAt(vectortargetlerpstart);
        transformEnd.LookAt(vectortargetlerpend);

        /*
        for (int i = 0; i < 10; i++)
        {
            Vector3 pos1 = GetBezierPosition(0.1f * i);
            Vector3 pos2 = GetBezierPosition(0.1f * (i+1));
            Gizmos.color = Color.red;
            Gizmos.DrawLine(pos1, pos2);
          //  Gizmos.color = Color.green;
           // Gizmos.DrawCube(pos1, Vector3.one*0.1f);
        }
        */

        Vector3[] pos = new Vector3[4];
        pos[0] = transformBegin.position;
        pos[3] = transformEnd.position;
        pos[1] = vectortargetlerpstart.position;
        pos[2] = vectortargetlerpend.position;
        Vector3[] smoothvector = LerpTest.MakeSmoothCurveVector3(pos, 5);
        for (int i = 1; i < smoothvector.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(smoothvector[i - 1], smoothvector[i]);
            Gizmos.DrawCube(smoothvector[i - 1], Vector3.one * 0.01f);
        }


        //Xcurve.AddKey(new Keyframe(0.5f, (transformBegin.position.x + transformEnd.position.x) / 2, intangent, outtangent));// inweight, outweight));

        // onetan=Mathf.Tan(Vector2.Angle(new Vector2(0, transformBegin.position.x), new Vector2(1, transformEnd.position.x)));
        //  intan=Mathf.Tan(Vector2.Angle(new Vector2( transformBegin.position.x,0), (new Vector2((transformBegin.position.x + transformEnd.position.x) / 2, 0.5f))));
        //  outtan=Mathf.Tan(Vector2.Angle(new Vector2(0.5f, 0.5f), new Vector2(  transformEnd.position.x,1)));

        #endregion

        #region Lerp2
        testlerp.position = GetBezierPosition(bezier);

        #endregion
    }
}
