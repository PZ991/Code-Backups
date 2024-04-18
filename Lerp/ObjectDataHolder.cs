using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDataHolder : MonoBehaviour
{
    public bool Selected;
    public bool moving;
    public LerpData data;
    public LerpTest datamanager;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (!moving)
        {
            var dataimport = datamanager.lerpdataimport;
            for (int i = 0; i < dataimport.Count; i++)
            {
                if (dataimport[i] == data.data)
                {
                    transform.position = new Vector3(datamanager.floatval[i][0], datamanager.floatval[i][1], datamanager.floatval[i][2]);

                }
            }
        }
        */
    }
}
