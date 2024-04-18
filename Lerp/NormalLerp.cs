using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalLerp : MonoBehaviour
{
    [Header("Lerp")]
    [Range(0, 20)]
    public float value;
    public float lerpvalue;
    public float invlerpvalue;

    
    

    
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
        lerpvalue = Mathf.Lerp(0, 20, value);
        invlerpvalue = Mathf.InverseLerp(0, 20, value);

    }
}
