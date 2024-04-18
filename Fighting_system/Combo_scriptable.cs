using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Combo", menuName = "ScriptableObjects/Combo")]
[System.Serializable]
public class Combo_scriptable : ScriptableObject
{
    //[Header("General Information")]
    public string attackname;
    public Sprite Image;
    public string Description;
    public string Customname;

    public List<float> combotimer;
    public List<ComboInfo> interconnected_combo;
    public AnimationClip animation;
    public float normalizedTimeOffset=-0.5f;
    public float normalizedTransitionDuration=0.5f;
    //public Combo_scriptable[] interconnected_combo2 = new Combo_scriptable[8];
    public MoveType movetype;
    public ControlType type;
    public PriorityTarget Prioritytargets;
    public ComboInfo leftarm_change_attack;
    public ComboInfo rightarm_change_attack;
    public ComboInfo leftarm_change_def;
    public ComboInfo rightarm_change_def;

    public ComboInfo leftfeet_change_attack;
    public ComboInfo rightfeet_change_attack;
    public ComboInfo leftfeet_change_def;
    public ComboInfo rightfeet_change_def;

    public ComboInfo TwoHanded_attack;
    public ComboInfo TwoHanded_def;
    public ComboInfo TwoFooted_attack;
    public ComboInfo TwoFooted_def;
    public ComboInfo MovementChange;
    public bool include_animation_lower_body;
    public bool isKinematic;

    [Tooltip("Must be same as other bodily movesets")]
    public List<int> layer;
    //[Header("Type Charge/PowerUp")]
    public float powerupspeed;
    public bool continuous;
    public bool needreachmaxtransform;
    public bool valuedropping;
    public float drop_speed;
    public float maxpowermultiplier;
    public float minpowermultiplier;
    public float duration;
    public AnimationClip charactertransform;
    public bool Do_Something_within_Range;
    public Combo_scriptable do_If_Near;

    //[Header("Type Attack")]
    public float general_damage;
    public float percent_strength;



    //[Header("Defend")
    public float general_defense;
    public float percent_strength_def;


    //Def/Attack
    public bool Use_Frames;
    public float MaxDistance;
    public bool Wait_reach_distance;
    public float Distance_Offset;
    public bool Use_Weights;
    public bool repeat;
    public int numrepeat;
    public Combo_scriptable Do_After;
    public Vector3 target_offset;
    public float returnToDedefaultTime;
    public bool returnToDedefault;
    public bool returnToDedefaultAftermove;
    public bool repeatable;
    public bool includeBodyParent;     //Rotation/whole body parent etc
    public float ComboAvailable;
    
    
    //[Header("Movement")]
    /*
    public float move_right_max;
    public float move_forward_max;
    public float move_left_max;
    public float move_backward_max;
    public float move_up_max;
    public float move_down_max;
    */
    public List<MovementTiming> move_to_pos;

    //acceleration

    public bool affectedbygravity;
    public bool slowdownwhenlanding;
    public Combo_scriptable slowingdownORland;
    public Combo_scriptable slowingdownair;
    public Combo_scriptable After_Move;
    public bool contrallablewhencontinuing;
    public bool contrallablewhenslowingdown;
    public bool controllablewhenmoving;
    public LayersMovement movementlayer;
    public bool needland;
    public bool Use_Frames_Move;
    public bool HasStartTime;
    public bool towardstargetXZ;
    public bool towardstargetY;
    public bool RotTowardsTargetXZ;
    public bool RotTowardsTargetY;
    public float Verticalspeed;
    public float MaxdistXZ;
    public float MaxdistY;
    public bool SlowDownWhenControlled;
    public bool defaultmove;
    public float maxspeed;
    public bool arcvertical;
    public bool use_previous_velocity;
    public float delaystartcontrol;
    public bool moveduringtransition;
    public bool velocityadd;
    public bool staticvelocity;
    public bool globalDirectPos;
    public bool loopend;
    public float looptime;
    public List<KeyCode> keyholds;
    public List<InverseKinematicTiming> LeftfootIK;
    public List<InverseKinematicTiming> RightfootIK;
    public List<InverseKinematicTiming> LeftArmIK;
    public List<InverseKinematicTiming> RightArmIK;
    public Combo_scriptable.MoveMode movemode;
    public MoveAttack MovementAttack;




    public float startHorizontal;
    public float startVertical;
    public float attackchangestart;
    public bool FootIK;
    public bool useForce;
    public bool returnnttostance;
    public bool loop;
    public bool DistaceFlexible;
    public float defaultstopduration; //speed is 2 so timetorestart=(defaultstopduration-=2*Time.deltaTime);
    //Dodge
    public bool Dodge;

    //[Header("Stance")]
    public bool hold_continuous;
    public bool wait_until_hit;
    public float general_Defence;
    public float Body_Defence_Bonus;

    //[Header("Custom KeyBind")]
    public KeyCode key;

    //[Header("Effects")]
    public List<Particle_Effect> effects;
    //public List<EffectMode> effectmode;

    //[Header("Spawn")]
    public List<EntityScriptable> Entities;

    public List<SummonTiming> summons;

    public List<CommandValue> commands;
    public List<string> transformpath;
    public List<float> commandtime;
    

    public bool AllowObjectPickup;
    public Vector2 minmaxAllow;
    public List<int> layerAffectedPickup;
    public float DistanceReach;
    public float DistanceTouch;
    public float ReachSpeed;
    public bool extendwhennear;
    public bool only_first_IK;
    public float AttachDistance;
    //Cancel REACH and ANGULAR CANCELATION REACH

    //public List<int[]> specificpointTarget;  //array for randomized points


    public bool differentiateLegGround;
    public bool leftleg;
    public AnimationClip alternateclip;
    //Interaction with certain object boolean, boolean with object, Commands into objects

    //[Header("Camera")]
    public List<CameraTiming> camtime;

    public bool nowaitingtransition;
    public bool incomingtransition;
    public float incomingnormalizedTimeOffset = -0.5f;
    public float incomingnormalizedTransitionDuration = 0.5f;

    public bool returntodefaultcamafter;
    public float returntime;
    public bool maintaincamposition;
    public bool maintaincamrotation;

    public bool use_defaultcam;
    public bool animateoncecam;
    public bool smoothtransitioncam;
    public bool cameraweightsonly;


    public List<CameraClamp> cameraClamps;
    public bool use_defaultcamsetting;
    public Combo_scriptable(Combo_scriptable copy)
    {
        attackname = copy.attackname;
        Image = copy.Image;
        Description = copy.Description;
        Customname = copy.Customname;
        combotimer = copy.combotimer;
        interconnected_combo = copy.interconnected_combo;
        //interconnected_combo2 = combo.interconnected_combo2;
        key = copy.key;
        animation = copy.animation;
    }
public enum ControlType { Humanoid, Quad, Multiple }
    public enum MoveType { Charge_Powerup, Attack, Movement, Stance, Defend, Dance }

    public enum LayersHumanoid { WholeBody, LeftArm, RightARm, LeftLeg, RightLeg, Head, Spine, Waist }
    public enum LayersMovement { HeadOnly, UpperBody, LowerBody, WholeBody, Whole_ExcludeArms, UpperBodyNoArms }

    public enum LayersQuadrouped { WholeBody2, FrontLeftFeet, FrontRightFeet, BackLeftFeet, BackRightFeet, FrontSpine, BackSpine, Head2 }
    public enum LayersMultiple { WholeBody3,Head3, Spine3, Layer1, Layer2, Layer3, Layer4, Layer5, Layer6, Layer7, Layer8  }

    public enum OtherLayers { Tail }
    public enum PriorityTarget { Upper_Body_Attack_Defend, Lower_Body_Attack_Defend, Head, Body, None }
    public enum MoveMode { Speed, Direction, Position }
    public enum MoveAttackAI { AttackonDistanceComputedHeavy,AttackonDistanceCompute, AttackonDistanceManual }
    public enum TargetMode { Nearest,Farthest, Lowest_Value,Cursor_Nearest,Cursor_Farthest,Cursor_Lowest_Value }
}
[System.Serializable]
public struct MoveAttack
{
    public Combo_scriptable.MoveAttackAI AImode;
    public Combo_scriptable Move;
    public bool needTarget;
    public Combo_scriptable.TargetMode targetMode;
    public float distanceTarget;
    public float StartTimeSubMove;
    public Combo_scriptable Attack;
    public List<int> Moveattacklayers;
    public bool movefirst;

    public MoveAttack(Combo_scriptable.MoveAttackAI mode, Combo_scriptable combo, bool target, Combo_scriptable.TargetMode targetmode,float distance, Combo_scriptable MovementonAttack2, List<int> Moveattacklayers2, bool movefirst2, float StartTimeSubMove2)
    {
        AImode = mode; 
        Move = combo;
        needTarget = target;
        targetMode = targetmode;
        distanceTarget = distance;
        Attack = MovementonAttack2;
        Moveattacklayers = Moveattacklayers2;
        movefirst = movefirst2;
         StartTimeSubMove= StartTimeSubMove2;
    }
}



[System.Serializable]
public struct InverseKinematicTiming
{

    public float timing;
    public float value;
    public InverseKinematicTiming(float timing2, float value2)
    {
        timing = timing2;
        value = value2;
    }
}
[System.Serializable]
public struct MovementTiming
{
    public Vector3 Direction;
    public float Directiontiming;
    public float speedtiming;
    public float horizontalspeed;
    public float verticalspeed;
    public bool allowmovement; 
    public float directionchangespeed;
    public bool faceDirection;
    public MovementTiming(Vector3 direction, float directiontiming, float speedtime2, float horizontal, float vertical, bool allowmovement2, float directionchangespeed2, bool facedirect)
    {
        Direction = direction;
        Directiontiming = directiontiming;
        speedtiming = speedtime2;
        horizontalspeed = horizontal;
        verticalspeed = vertical;
        allowmovement = allowmovement2;
        directionchangespeed = directionchangespeed2;
        faceDirection = facedirect;
        

    }
    public MovementTiming(float directiontiming, float speedtime2, bool allowmovement2,float horizontal, float vertical)
    {
        Directiontiming = directiontiming;
        speedtiming = speedtime2;
        allowmovement = allowmovement2;
        horizontalspeed = horizontal;
        verticalspeed = vertical;

        Direction = Vector3.zero;
        horizontalspeed = 0;
        verticalspeed = 0;
        allowmovement = false;
        directionchangespeed = 0;
        faceDirection = false;
    }
     public MovementTiming(float directiontiming, float speedtime2, bool allowmovement2,float horizontal, float directionchangespeed2,Vector3 direction)
    {
        Directiontiming = directiontiming;
        speedtiming = speedtime2;
        allowmovement = allowmovement2;
        horizontalspeed = horizontal;
        Direction = direction;
        directionchangespeed = directionchangespeed2;

        verticalspeed = 0;
        horizontalspeed = 0;
        verticalspeed = 0;
        allowmovement = false;
        faceDirection = false;
    }
    public MovementTiming(float directiontiming, float speedtime2, bool allowmovement2,float horizontal, Vector3 direction, bool facedirect)
    {
        Directiontiming = directiontiming;
        speedtiming = speedtime2;
        allowmovement = allowmovement2;
        horizontalspeed = horizontal;
        Direction = direction;
        faceDirection = facedirect;

        directionchangespeed = 0;
        verticalspeed = 0;
        horizontalspeed = 0;
        verticalspeed = 0;
        allowmovement = false;
    }

}
[System.Serializable]

public struct EffectTiming
{
    public float effect_start_time;
    public bool start;
    public string effect_transform_parent;
    public float effect_start_time_parent_position;
    public bool effect_follow_parent_position;

    public string effect_rotation_parent;
    public float effect_start_time_parent_rotation;
    public bool effect_follow_parent_rotation;

    public Vector3 position;
    public Quaternion rotation;
    public EffectTiming(float starttime, bool start2, string transformparent, float timetransform, bool followtransform, string rotationparent, float timerotate, bool followrotation,Vector3 pos,Quaternion rotation2)
    {
        effect_start_time = starttime;
        start = start2;
        effect_transform_parent = transformparent;
        effect_start_time_parent_position = timetransform;
        effect_follow_parent_position = followtransform;

        effect_rotation_parent = rotationparent;
        effect_start_time_parent_rotation = timerotate;
        effect_follow_parent_rotation = followrotation;
        position = pos;
        rotation = rotation2;
    }
}

[System.Serializable]
public struct SummonTiming
{
    public float time;
    public ObjectKnowledge summon;
    public string RelativePath;
    public string parent;
    public SummonTiming(float Time, ObjectKnowledge obj, string Path,string parentpath)
    {
        time = Time;
        summon = obj;
        RelativePath = Path;
        parent = parentpath;
    }
}
[System.Serializable]
public struct CameraTiming
{
    public Vector3 CamPos;
    public Quaternion CamRot;
    public float Time;
    public float weight;
    public CameraTiming(Vector3 pos, Quaternion rot, float time,float weights)
    {
        CamPos = pos;
        CamRot = rot;
        Time = time;
        weight = weights;
    }
}
[System.Serializable]
public struct CameraClamp
{
    public Vector2 Xminmax;
    public Vector2 Yminmax;
    public Vector2 threshold;
    public float time;
    public CameraClamp(Vector2 Xminmax2, Vector2 Yminmax2, Vector2 threshold2, float time2)
    {
        Xminmax = Xminmax2;
        Yminmax = Yminmax2;
        threshold = threshold2;
        time = time2;
    }
}


[System.Serializable]
public struct ComboInfo
{
    public string name;
    public string path;
    public bool custom;
    public Combo_scriptable combo;
    public ComboInfo(string n, string p, bool c, Combo_scriptable co)
    {
        name = n;
        path = p;
        custom = c;
        combo = co;
    }
    public ComboInfo(ComboInfo copy, Combo_scriptable co)
    {
        name = copy.name;
        path = copy.path;
        custom = copy.custom;
        combo = co;
    }

}


