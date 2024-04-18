using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distance_Checker : MonoBehaviour
{
    
    public bool Check;

    public GameObject MainMeshModel;
    public Player_Model_Scriptable model;
    //[Header("Humanoid")]
    public Transform shoulderL;
    public Transform shoulderR;
    public Transform ArmL;
    public Transform ArmR;
    public Transform LegL;
    public Transform LegR;
    public Transform FeetL;
    public Transform FeetR;
    public Transform Hip;
    public Transform Head;

    //[Header("Quadrouped")]
    public Transform FrontShoulderL;
    public Transform FrontShoulderR;
    public Transform FrontFeetL;
    public Transform FrontFeetR;
    public Transform BackHindL;
    public Transform BackHindR;
    public Transform BackFeetL;
    public Transform BackFeetR;
    public Transform BackBone;
    public Transform HeadQ;

    //[Header("Multiple")]
    public Transform HeadSpineStart;
    public Transform HeadSpineEnd;
    public Transform Layer1Start;
    public Transform Layer1End;
    public Transform Layer2Start;
    public Transform Layer2End;
    public Transform Layer3Start;
    public Transform Layer3End;
    public Transform Layer4Start;
    public Transform Layer4End;
    public Transform Layer5Start;
    public Transform Layer5End;
    public Transform Layer6Start;
    public Transform Layer6End;
    public Transform Layer7Start;
    public Transform Layer7End;
    public Transform Layer8Start;
    public Transform Layer8End;


    void Start()
    {
       /// StartChecking();
    }
    public void StartChecking()
    {
        
        if (model.type == Player_Model_Scriptable.ControlType.Humanoid)
        {
            model.LeftArmReach = CheckDistance(shoulderL.position, ArmL.position);
            model.RightArmReach = CheckDistance(shoulderR.position, ArmR.position);
            model.LeftFeetReach = CheckDistance(LegL.position, FeetL.position);
            model.RightFeetReach = CheckDistance(LegR.position, FeetR.position);
            model.HeadSpineReach = CheckDistance(Hip.position, Head.position);

        }
        else if (model.type ==Player_Model_Scriptable.ControlType.Quad)
        {
            model.FrontLeftReach = CheckDistance(FrontShoulderL.position, FrontFeetL.position);
            model.FrontRightReach = CheckDistance(FrontShoulderR.position, FrontFeetR.position);
            model.BackLeftReach = CheckDistance(BackHindL.position, BackFeetL.position);
            model.BackRightReach = CheckDistance(BackHindR.position, BackFeetR.position);
            model.HeadSpineReachQ = CheckDistance(BackBone.position, HeadQ.position);
        }
        else
        {
            if(HeadSpineStart.position!=null &&HeadSpineEnd.position!=null)
            model.HeadSpineReachM = CheckDistance(HeadSpineStart.position, HeadSpineEnd.position);
            if (Layer1Start.position != null && Layer1End.position != null)
                model.Layer1Reach = CheckDistance(Layer1Start.position, Layer1End.position);
            if (Layer2Start.position != null && Layer2End.position != null)

                model.Layer2Reach = CheckDistance(Layer2Start.position, Layer2End.position);
            if (Layer3Start.position != null && Layer3End.position != null)

                model.Layer3Reach = CheckDistance(Layer3Start.position, Layer3End.position);
            if (Layer4Start.position != null && Layer4End.position != null)

                model.Layer4Reach = CheckDistance(Layer4Start.position, Layer4End.position);
            if (Layer5Start.position != null && Layer5End.position != null)

                model.Layer5Reach = CheckDistance(Layer5Start.position, Layer5End.position);
            if (Layer6Start.position != null && Layer6End.position != null)

                model.Layer6Reach = CheckDistance(Layer6Start.position, Layer6End.position);
            if (Layer7Start.position != null && Layer7End.position != null)

                model.Layer7Reach = CheckDistance(Layer7Start.position, Layer7End.position);
            if (Layer8Start.position != null && Layer8End.position != null)

                model.Layer8Reach = CheckDistance(Layer8Start.position, Layer8End.position);
        }
        
    }
    
    public float CheckDistance(Vector3 obj1, Vector3 obj2)
    {
        float distance;
       return distance = Vector3.Distance(obj1, obj2);

    }
    private void OnDrawGizmos()
    {


    }
}
