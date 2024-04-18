using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorLerp : MonoBehaviour
{
    [Header("Custom Curve Float")]
    public float[] floatarray;
    public float[] smoothened;
    public float smoothness;
    [Header("Custom Curve Vector2")]
    public Vector2[] vectorarray;
    public Vector2[] vectorsmoothened;
    public float vectorsmoothness;
    [Header("Custom Curve Evaluator")]
    public float time;
    public float curvevalue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {

        smoothened = LineCurveWeight.MakeSmoothCurveFloat(floatarray, smoothness);
        vectorsmoothened = LerpTest.MakeSmoothCurveVector2(vectorarray, vectorsmoothness);
        int currentval = 0;
        for (int i = 0; i < vectorsmoothened.Length; i++)
        {

            if (time > vectorsmoothened[i].x)
            {
                currentval += 1;
                continue;
            }
            float currenttime = Mathf.InverseLerp(vectorsmoothened[Mathf.Clamp(currentval - 1, 0, vectorsmoothened.Length)].x, vectorsmoothened[currentval].x, time);

            curvevalue = Mathf.Lerp(vectorsmoothened[Mathf.Clamp(currentval - 1, 0, vectorsmoothened.Length)].y, vectorsmoothened[currentval].y, currenttime);
            // Debug.Log("currentval= " + currentval + " currenttime= " + currenttime + " currentvalue=" + curvevalue);

        }
    }
}
