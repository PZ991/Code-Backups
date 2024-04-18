using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiLineIterator : MonoBehaviour
{

    public List<Vector3> testpos = new List<Vector3>();
    public List<Vector3> testpos2 = new List<Vector3>();
    public List<Vector3> testpos3 = new List<Vector3>();
    public float testXinfluence;
    public float testYinfluence;
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
        testXinfluence = Mathf.Clamp01(testXinfluence);
        testYinfluence = Mathf.Clamp01(testYinfluence);

        Gizmos.color = Color.yellow;
        int startindex = Mathf.FloorToInt(testXinfluence * (testpos.Count - 1));
        int endindex = Mathf.CeilToInt(testXinfluence * (testpos.Count - 1));
        Vector3 fpos1 = Vector3.Lerp(testpos[startindex], testpos[endindex], testXinfluence * (testpos.Count - 1) - startindex);
        Gizmos.DrawCube(fpos1, Vector3.one * 0.5f);

        Gizmos.color = Color.green;
        int startindex2 = Mathf.FloorToInt(testXinfluence * (testpos2.Count - 1));
        int endindex2 = Mathf.CeilToInt(testXinfluence * (testpos2.Count - 1));
        Vector3 fpos2 = Vector3.Lerp(testpos2[startindex2], testpos2[endindex2], testXinfluence * (testpos2.Count - 1) - startindex2);
        Gizmos.DrawCube(fpos2, Vector3.one * 0.5f);

        Gizmos.color = Color.blue;
        int startindex3 = Mathf.FloorToInt(testXinfluence * (testpos3.Count - 1));
        int endindex3 = Mathf.CeilToInt(testXinfluence * (testpos3.Count - 1));
        Vector3 fpos3 = Vector3.Lerp(testpos3[startindex3], testpos3[endindex3], testXinfluence * (testpos3.Count - 1) - startindex3);
        Gizmos.DrawCube(fpos3, Vector3.one * 0.5f);

        //3 = count of Y list
        int startindex4 = Mathf.FloorToInt(testYinfluence * (3 - 1));
        int endindex4 = Mathf.CeilToInt(testYinfluence * (3 - 1));
        Vector3 p1 = Vector3.zero;
        Vector3 p2 = Vector3.zero;
        if (startindex4 == 0)
        {
            p1 = fpos1;
        }
        else if (startindex4 == 1)
        {
            p1 = fpos2;
        }
        else if (startindex4 == 2)
        {
            p1 = fpos3;
        }
        if (endindex4 == 0)
        {
            p2 = fpos1;
        }
        else if (endindex4 == 1)
        {
            p2 = fpos2;
        }
        else if (endindex4 == 2)
        {
            p2 = fpos3;
        }

        Gizmos.color = Color.white;
        Gizmos.DrawCube(Vector3.Lerp(p1, p2, testYinfluence * (3 - 1) - startindex4), Vector3.one * 0.5f);


        Gizmos.color = Color.black;
        Gizmos.DrawLine(p1, p2);
        for (int i = 1; i < testpos.Count; i++)
        {
            Gizmos.DrawSphere(testpos[i], 0.1f);
            Gizmos.DrawLine(testpos[i - 1], testpos[i]);

            if (i == 1)
                Gizmos.DrawSphere(testpos[i - 1], 0.1f);

        }
        for (int i = 1; i < testpos2.Count; i++)
        {
            Gizmos.DrawSphere(testpos2[i], 0.1f);
            Gizmos.DrawLine(testpos2[i - 1], testpos2[i]);

            if (i == 1)
                Gizmos.DrawSphere(testpos2[i - 1], 0.1f);

        }
        for (int i = 1; i < testpos3.Count; i++)
        {
            Gizmos.DrawSphere(testpos3[i], 0.1f);
            Gizmos.DrawLine(testpos3[i - 1], testpos3[i]);

            if (i == 1)
                Gizmos.DrawSphere(testpos3[i - 1], 0.1f);

        }

    }
}
