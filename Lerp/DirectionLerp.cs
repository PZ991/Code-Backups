using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionLerp : MonoBehaviour
{
    public Vector3 currentpos;
    public Vector3 direction;
    public float vectorspeed;
    void Start()
    {
        
    }

    private void OnDrawGizmos()
    {
         currentpos = Vector3.Lerp(currentpos, currentpos + direction, vectorspeed);



    }
}
