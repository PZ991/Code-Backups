using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceOffsetChecker :MonoBehaviour
{
    public Transform OriginalPosition;
    public Transform EndBone;
    public Combo_scriptable Combo;
    public Player_Model_Scriptable model;
    public void CheckDist()
    {

        
            Vector3 handpos;
            
                OriginalPosition = GameObject.Find("ShoulderL").transform;
                handpos = GameObject.Find("Bone.016").transform.position;
                Vector3 shoulderpos = GameObject.Find("Bone.010").transform.position;

                if (handpos != null)
                {
                    Combo.MaxDistance = Mathf.Clamp(Mathf.InverseLerp(0, model.LeftArmReach, (Vector3.Distance(shoulderpos, handpos))), 0, 1);
                    if (Combo.MaxDistance > 0.99f || Combo.MaxDistance == 1)

                        Combo.Distance_Offset = ((Vector3.Distance(OriginalPosition.position, handpos))) - (Mathf.Lerp(0, model.LeftArmReach, Combo.MaxDistance));
                    else
                        Combo.Distance_Offset = 0;
                   
                }
            
        
    }
}
