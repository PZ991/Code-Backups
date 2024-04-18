using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class AIShadowWeights : MonoBehaviour
{
    // Start is called before the first frame update
    public TwoBoneIKConstraint parentIK;
    public TwoBoneIKConstraint shadowIK;
    public MultiAimConstraint parentaim;
    public MultiAimConstraint shadowaim;
    public Transform parent;
    public Transform shadow;
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if(parent!=null&&shadow!=null&&parentIK!=null&&shadowIK!=null)
        {
            shadow.position = parent.position;
            shadow.rotation = parent.rotation;
            shadowIK.weight = parentIK.weight;
        }
        else if (parent != null && shadow != null && parentaim != null && shadowaim != null)
        {
            shadow.position = parent.position;
            shadow.rotation = parent.rotation;
            shadowaim.weight = parentaim.weight;
        }
    }
}
