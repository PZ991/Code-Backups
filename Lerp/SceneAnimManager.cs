using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAnimManager : MonoBehaviour
{
    public GameObject[] Animatable;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Animatable = GameObject.FindGameObjectsWithTag("Animatable");
        for (int i = 0; i < Animatable.Length; i++)
        {
            if(Animatable[i].GetComponent<ObjectDataHolder>()==null)
            {

            }
        }
    }
}
