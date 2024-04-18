using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Combo_scriptable))]
public class CombooscriptableEditor : Editor
{
    private SerializedProperty comboname;
    private SerializedProperty combosprite;
    private SerializedProperty Description;
    private SerializedProperty CustomName;
    private SerializedProperty combotimer;
    private SerializedProperty interconnected_combo;
    private SerializedProperty clip;
    private SerializedProperty key;
    private Combo_scriptable.MoveType movetype;
    private SerializedProperty Prioritytargets;
    public SerializedProperty include_animation_lower_body;
    public SerializedProperty leftarm_change_attack;
    public SerializedProperty rightarm_change_attack;
    public SerializedProperty leftarm_change_def;
    public SerializedProperty rightarm_change_def;
    public SerializedProperty TwoHanded_attack;
    public SerializedProperty TwoHanded_def;
    public SerializedProperty leftfeet_change_attack;
    public SerializedProperty rightfeet_change_attack;
    public SerializedProperty leftfeet_change_def;
    public SerializedProperty rightfeet_change_def;
    public SerializedProperty TwoFooted_attack;
    public SerializedProperty TwoFooted_def;
    public SerializedProperty MovementChange;
    public SerializedProperty use_previous_velocity;
    public SerializedProperty isKinematic;
    public SerializedProperty normalizedTimeOffset ;
    public SerializedProperty normalizedTransitionDuration;
    public SerializedProperty includeBodyParent;
    public SerializedProperty ComboAvailable;

    //attack
    private SerializedProperty gen_damage;
    private SerializedProperty percent_strength;

    private SerializedProperty Use_Frames;


    //Defend
    private SerializedProperty general_defense;
    private SerializedProperty percent_strength_def;

    private SerializedProperty repeatable;
    private SerializedProperty MaxDistance;
    private SerializedProperty Wait_reach_distance;
    private SerializedProperty Distance_Offset;
    private SerializedProperty Use_Weights;
    public SerializedProperty repeat;
    public SerializedProperty numrepeat;
    private SerializedProperty use_both_hands;
    private SerializedProperty use_both_feet;
    public SerializedProperty Do_After;
    public SerializedProperty target_offset;
    private SerializedProperty returnToDedefaultTime;
    private SerializedProperty returnToDedefault;
    private SerializedProperty returnToDedefaultAftermove;
    private SerializedProperty includeSpine;
    public SerializedProperty StartTimeSubMove;
    public SerializedProperty MovementonAttack;
    public SerializedProperty Moveattacklayers;

    //powerup
    private SerializedProperty powerupspeed;
    private SerializedProperty continuous;
    private SerializedProperty duration;
    private SerializedProperty charactertransform;
    private SerializedProperty type;
    private SerializedProperty needreachmaxtransform;
    private SerializedProperty valuedropping;
    private SerializedProperty drop_speed;
    private SerializedProperty maxpowermultiplier;
    private SerializedProperty minpowermultiplier;

    //Movement
    /*
    private SerializedProperty move_right_max;
    private SerializedProperty move_forward_max;
    private SerializedProperty move_left_max;
    private SerializedProperty move_backward_max;
    private SerializedProperty move_up_max;
    private SerializedProperty move_down_max;
    */

    private SerializedProperty After_Move;
    private SerializedProperty Dodge;
    public SerializedProperty affectedbygravity;
    public SerializedProperty slowdownwhenlanding;
    public SerializedProperty slowingdownORland;
    public SerializedProperty contrallablewhencontinuing;
    public SerializedProperty contrallablewhenslowingdown;
    public SerializedProperty controllablewhenmoving;
    public SerializedProperty movementlayer;
    public SerializedProperty needland;
    public SerializedProperty Use_Frames_Move;
    public SerializedProperty HasStartTime;
    public SerializedProperty towardstargetXZ;
    public SerializedProperty towardstargetY;
    public SerializedProperty rotTowardsTargetXZ;
    public SerializedProperty rotTowardsTargetY;
    public SerializedProperty MaxdistXZ;
    public SerializedProperty MaxdistY;
    public SerializedProperty SlowDownWhenControlled;
    public SerializedProperty defaultmove;
    public SerializedProperty arcvertical;
    public SerializedProperty delaystartcontrol;
    public SerializedProperty moveduringtransition;
    public SerializedProperty velocityadd;
    public SerializedProperty staticvelocity;
    
    public SerializedProperty MovementAttack;

    public SerializedProperty attackchangestart;
    public SerializedProperty FootIK;
    public SerializedProperty useForce;
    public SerializedProperty returnnttostance;
    public SerializedProperty loop;
    public SerializedProperty DistaceFlexible;
    public SerializedProperty defaultstopduration;
    public SerializedProperty slowingdownair;

    public SerializedProperty LeftFootIK;
    public SerializedProperty RightFootIK;
    public SerializedProperty LeftArmIK;
    public SerializedProperty RightArmIK;
    public SerializedProperty loopend;
    public SerializedProperty looptime;
    public SerializedProperty globalDirectPos;
    public SerializedProperty keyholds;
    //Stance
    private SerializedProperty hold_continuous;
    private SerializedProperty Body_Defence_Bonus;
    private SerializedProperty wait_until_hit;
    private SerializedProperty general_Defence_Bonus;
    
    //Effect
    public SerializedProperty effects;
    public SerializedProperty effecttiming;

    public SerializedProperty AllowObjectPickup;
    public SerializedProperty minmaxAllow;
    public SerializedProperty use_defaultcamsetting;


    //public SerializedProperty effectmode;


    //Type
    public bool Humanoid;
    public bool Quad;
    public bool Multiple;
    public SerializedProperty layer;

    //humanoid
    public bool LeftArm, RightARm, Head, LeftLeg, RightLeg, Spine, WholeBody, Waist, OnlyWholeBody;
    //quad
    public bool FrontLeftFeet, FrontRightFeet, BackLeftFeet, BackRightFeet, FrontSpine, BackSpine, WholeBody2, Head2, OnlyWholeBody2;
    //multiple
    public bool Head3, Spine3, Layer1, Layer2, Layer3, Layer4, Layer5, Layer6, Layer7, Layer8, WholeBody3, OnlyWholeBody3;

    public int leftfootcount;
    public int rightfootcount;
    public int leftarmcount;
    public int rightarmcount;

    public int movementcount;
    public List<int> effectcount;
    public int maineffectcount;

    public List<List<int>> effectcount2;
    public int maineffectcount2;
        public int entitycount;


    public bool frameused;
    public bool frameused_def;
    public bool frameused_move;

    bool foldoutLL;
    bool foldoutRL;
    bool foldoutLA;
    bool foldoutRA;
    bool foldoutM;
    List<bool> foldoutE;
    bool foldoutET;

    List<List<bool>> foldoutE2;
    List<bool> foldoutET2;
    bool foldoutEN;

    bool view_timing_move_only;
    bool view_timing_IK_only;
    bool view_position_only;
    bool view_effect_position_only;
    bool view_effect_rotation_only;
    bool view_effect_timing_only;
    bool view_effect_all;

    bool view_effect_position_only2;
    bool view_effect_rotation_only2;
    bool view_effect_timing_only2;
    bool view_effect_all2;

    bool view_speed_only;
    bool view_value_only;
    bool cancel_move;
    bool cancel;
    int summoncount;
    bool dropdownsummon;
    bool dropdowncamclamp;

    public int targettypecommand;
    public bool dropdowncommand;
    public List<bool> dropdowncommandvalues = new List<bool>();

    public bool dropdowncam;

    public SerializedProperty layerAffectedPickup;
    public SerializedProperty DistanceReach;
    public SerializedProperty DistanceTouch;
    public SerializedProperty ReachSpeed;
    public SerializedProperty extendwhennear;
    public SerializedProperty only_first_IK;

    public SerializedProperty differentiateLegGround;
    public SerializedProperty leftleg;
    public SerializedProperty alternateclip;
    public int startsmooth, endsmooth;
    public float smoothnesscam;
    public SerializedProperty AttachDistance;
    public SerializedProperty nowaitingtransition;
    public SerializedProperty incomingtransition;
    public SerializedProperty incomingnormalizedTimeOffset ;
    public SerializedProperty incomingnormalizedTransitionDuration ;
    public SerializedProperty returntodefaultcamafter;
    public SerializedProperty returntime;

    public SerializedProperty maintaincamposition;
    public SerializedProperty maintaincamrotation;


    public SerializedProperty use_defaultcam;
    public SerializedProperty animateoncecam;
    public SerializedProperty smoothtransitioncam;
    public SerializedProperty cameraweightsonly;
    void OnEnable()
    {
        comboname = serializedObject.FindProperty("attackname");
        combosprite = serializedObject.FindProperty("Image");
        Description = serializedObject.FindProperty("Description");
        CustomName = serializedObject.FindProperty("Customname");
        combotimer = serializedObject.FindProperty("combotimer");
        interconnected_combo = serializedObject.FindProperty("interconnected_combo");
        clip = serializedObject.FindProperty("animation");
        type = serializedObject.FindProperty("type");
        key = serializedObject.FindProperty("key");
        layer = serializedObject.FindProperty("layer");
        Prioritytargets = serializedObject.FindProperty("Prioritytargets");
        include_animation_lower_body = serializedObject.FindProperty("include_animation_lower_body");
        leftarm_change_attack = serializedObject.FindProperty("leftarm_change_attack");
        rightarm_change_attack = serializedObject.FindProperty("rightarm_change_attack");
        leftarm_change_def = serializedObject.FindProperty("leftarm_change_def");
        rightarm_change_def = serializedObject.FindProperty("rightarm_change_def");
        TwoHanded_attack = serializedObject.FindProperty("TwoHanded_attack");
        TwoHanded_def = serializedObject.FindProperty("TwoHanded_def");
        leftfeet_change_attack = serializedObject.FindProperty("leftfeet_change_attack");
        rightfeet_change_attack = serializedObject.FindProperty("rightfeet_change_attack");
        leftfeet_change_def = serializedObject.FindProperty("leftfeet_change_def");
        rightfeet_change_def = serializedObject.FindProperty("rightfeet_change_def");
        TwoFooted_attack = serializedObject.FindProperty("TwoFooted_attack");
        TwoFooted_def = serializedObject.FindProperty("TwoFooted_def");
        MovementChange = serializedObject.FindProperty("MovementChange");
         normalizedTimeOffset = serializedObject.FindProperty("normalizedTimeOffset");
        normalizedTransitionDuration = serializedObject.FindProperty("normalizedTransitionDuration");
        ComboAvailable = serializedObject.FindProperty("ComboAvailable");
        nowaitingtransition = serializedObject.FindProperty("nowaitingtransition");
        incomingtransition= serializedObject.FindProperty("incomingtransition");
        incomingnormalizedTimeOffset = serializedObject.FindProperty("incomingnormalizedTimeOffset");
        incomingnormalizedTransitionDuration = serializedObject.FindProperty("incomingnormalizedTransitionDuration");
        use_defaultcamsetting = serializedObject.FindProperty("use_defaultcamsetting");



        returntodefaultcamafter = serializedObject.FindProperty("returntodefaultcamafter");
        returntime = serializedObject.FindProperty("returntime");
        maintaincamposition = serializedObject.FindProperty("maintaincamposition");
        maintaincamrotation = serializedObject.FindProperty("maintaincamrotation");
        use_defaultcam = serializedObject.FindProperty("use_defaultcam");
        animateoncecam = serializedObject.FindProperty("animateoncecam");
        smoothtransitioncam = serializedObject.FindProperty("smoothtransitioncam");
        cameraweightsonly = serializedObject.FindProperty("cameraweightsonly");
        

    //powerup
    powerupspeed = serializedObject.FindProperty("powerupspeed");
        continuous = serializedObject.FindProperty("continuous");
        duration = serializedObject.FindProperty("duration");
        charactertransform = serializedObject.FindProperty("charactertransform");
        needreachmaxtransform = serializedObject.FindProperty("needreachmaxtransform");
        valuedropping = serializedObject.FindProperty("valuedropping");
        drop_speed = serializedObject.FindProperty("drop_speed");
        maxpowermultiplier = serializedObject.FindProperty("maxpowermultiplier");
        minpowermultiplier = serializedObject.FindProperty("minpowermultiplier");


        //attack
        gen_damage = serializedObject.FindProperty("general_damage");
        percent_strength = serializedObject.FindProperty("percent_strength");
        MaxDistance = serializedObject.FindProperty("MaxDistance");

        //Defend
        general_defense = serializedObject.FindProperty("general_defense");
        percent_strength_def = serializedObject.FindProperty("percent_strength_def");

        includeBodyParent = serializedObject.FindProperty("includeBodyParent");
        repeatable = serializedObject.FindProperty("repeatable");
        returnToDedefaultAftermove = serializedObject.FindProperty("returnToDedefaultAftermove");
        returnToDedefault = serializedObject.FindProperty("returnToDedefault");
        returnToDedefaultTime = serializedObject.FindProperty("returnToDedefaultTime");
        Wait_reach_distance = serializedObject.FindProperty("Wait_reach_distance");
        Distance_Offset = serializedObject.FindProperty("Distance_Offset");
        Use_Weights = serializedObject.FindProperty("Use_Weights");
        repeat = serializedObject.FindProperty("repeat");
        Use_Frames = serializedObject.FindProperty("Use_Frames");
        use_both_feet = serializedObject.FindProperty("use_both_feet");
        use_both_hands = serializedObject.FindProperty("use_both_hands");
        includeSpine = serializedObject.FindProperty("includeSpine");

        Do_After = serializedObject.FindProperty("Do_After");
        target_offset = serializedObject.FindProperty("target_offset");
        StartTimeSubMove = serializedObject.FindProperty("StartTimeSubMove");
        MovementonAttack = serializedObject.FindProperty("MovementonAttack");
        Moveattacklayers = serializedObject.FindProperty("Moveattacklayers");
        numrepeat = serializedObject.FindProperty("numrepeat");



        //movement
        /*
        move_right_max = serializedObject.FindProperty("move_right_max");
        move_forward_max = serializedObject.FindProperty("move_forward_max");
        move_left_max = serializedObject.FindProperty("move_left_max");
        move_backward_max = serializedObject.FindProperty("move_backward_max");
        move_up_max = serializedObject.FindProperty("move_up_max");
        move_down_max = serializedObject.FindProperty("move_down_max");
        */

        After_Move = serializedObject.FindProperty("After_Move");
        Dodge = serializedObject.FindProperty("Dodge");
        affectedbygravity = serializedObject.FindProperty("affectedbygravity");
        slowdownwhenlanding = serializedObject.FindProperty("slowdownwhenlanding");
        hold_continuous = serializedObject.FindProperty("hold_continuous");
        slowingdownORland = serializedObject.FindProperty("slowingdownORland");
        contrallablewhencontinuing = serializedObject.FindProperty("contrallablewhencontinuing");
        contrallablewhenslowingdown = serializedObject.FindProperty("contrallablewhenslowingdown");
        controllablewhenmoving = serializedObject.FindProperty("controllablewhenmoving");
        movementlayer = serializedObject.FindProperty("movementlayer");
        needland = serializedObject.FindProperty("needland");
        Use_Frames_Move = serializedObject.FindProperty("Use_Frames_Move");
        HasStartTime = serializedObject.FindProperty("HasStartTime");
        towardstargetXZ = serializedObject.FindProperty("towardstargetXZ");
        towardstargetY = serializedObject.FindProperty("towardstargetY");
        rotTowardsTargetXZ = serializedObject.FindProperty("RotTowardsTargetXZ");
        rotTowardsTargetY = serializedObject.FindProperty("RotTowardsTargetY");
        rotTowardsTargetY = serializedObject.FindProperty("RotTowardsTargetY");
        MaxdistXZ = serializedObject.FindProperty("MaxdistXZ");
        MaxdistY = serializedObject.FindProperty("MaxdistY");
        SlowDownWhenControlled = serializedObject.FindProperty("SlowDownWhenControlled");
        defaultmove = serializedObject.FindProperty("defaultmove");
        use_previous_velocity = serializedObject.FindProperty("use_previous_velocity");
        isKinematic = serializedObject.FindProperty("isKinematic");
        delaystartcontrol = serializedObject.FindProperty("delaystartcontrol");
        arcvertical = serializedObject.FindProperty("arcvertical");
        moveduringtransition = serializedObject.FindProperty("moveduringtransition");
        velocityadd = serializedObject.FindProperty("velocityadd");
        globalDirectPos = serializedObject.FindProperty("globalDirectPos");
        loopend = serializedObject.FindProperty("loopend");
        looptime = serializedObject.FindProperty("looptime");
        keyholds = serializedObject.FindProperty("keyholds");
        
    staticvelocity = serializedObject.FindProperty("staticvelocity");
        MovementAttack = serializedObject.FindProperty("MovementAttack");
        

    attackchangestart = serializedObject.FindProperty("attackchangestart");
        FootIK = serializedObject.FindProperty("FootIK");
        useForce = serializedObject.FindProperty("useForce");
        returnnttostance = serializedObject.FindProperty("returnnttostance");
        loop = serializedObject.FindProperty("loop");
        defaultstopduration = serializedObject.FindProperty("defaultstopduration");
        DistaceFlexible = serializedObject.FindProperty("DistaceFlexible");
        slowingdownair = serializedObject.FindProperty("slowingdownair");
        AttachDistance = serializedObject.FindProperty("AttachDistance");


        LeftFootIK = serializedObject.FindProperty("LeftfootIK");
        RightFootIK = serializedObject.FindProperty("RightfootIK");
        LeftArmIK = serializedObject.FindProperty("LeftArmIK");
        RightArmIK = serializedObject.FindProperty("RightArmIK");

        //Stance
        Body_Defence_Bonus = serializedObject.FindProperty("Body_Defence_Bonus");
        wait_until_hit = serializedObject.FindProperty("wait_until_hit");


        
    layerAffectedPickup = serializedObject.FindProperty("layerAffectedPickup");
    DistanceReach = serializedObject.FindProperty("DistanceReach");
    extendwhennear = serializedObject.FindProperty("extendwhennear");

    differentiateLegGround = serializedObject.FindProperty("differentiateLegGround");
        leftleg = serializedObject.FindProperty("leftleg");
        alternateclip = serializedObject.FindProperty("alternateclip");
        general_Defence_Bonus = serializedObject.FindProperty("general_Defence");

        AllowObjectPickup = serializedObject.FindProperty("AllowObjectPickup");
        minmaxAllow = serializedObject.FindProperty("minmaxAllow");
        only_first_IK = serializedObject.FindProperty("only_first_IK");
        DistanceTouch = serializedObject.FindProperty("DistanceTouch");
        ReachSpeed = serializedObject.FindProperty("ReachSpeed");

       

    Combo_scriptable script = (Combo_scriptable)target;
        Use_Frames_Move.boolValue = script.Use_Frames_Move;
        frameused_move = Use_Frames_Move.boolValue;
        Use_Frames.boolValue = script.Use_Frames;
        frameused = Use_Frames.boolValue;

        leftfootcount = script.LeftfootIK.Count;
        rightarmcount = script.RightArmIK.Count;
        rightfootcount = script.RightfootIK.Count;
        leftarmcount = script.LeftArmIK.Count;
        movementcount = script.move_to_pos.Count;

        effectcount = new List<int>();
        effectcount2 = new List<List<int>>();
        foldoutE = new List<bool>();
        foldoutET2 = new List<bool>();
        foldoutE2 = new List<List<bool>>();
        summoncount = script.summons.Count;
        targettypecommand=script.commands.Count;
        if (script.Entities.Count > 0 && script.Entities != null)
        {
            for (int i = 0; i < script.Entities.Count; i++)
            {
                foldoutE2.Add(new List<bool>());
                foldoutET2.Add(false);
                effectcount2.Add(new List<int>());
                if (script.Entities[i] != null)
                {
                    if (script.Entities[i].maineffectcount > 0)
                    {
                        for (int j = 0; j < script.Entities[i].maineffectcount; j++)
                        {

                            effectcount2[j].Add(0);


                            foldoutE2[i].Add(false);
                        }
                    }
                }
            }
        }
        for (int i = 0; i < script.effects.Count; i++)
        {
            effectcount.Add(script.effects[i].effecttiming.Count);
            foldoutE.Add(false);
        }
        for (int i = 0; i < script.layer.Count; i++)
        {
            if(script.type==Combo_scriptable.ControlType.Humanoid)
            {
                switch (script.layer[i])
                {
                    case 0:
                        if (script.layer.Count > 1)
                        {
                            WholeBody = true;
                        }
                        else 
                            OnlyWholeBody = true;
                            break;
                        case 1:
                        LeftArm = true;

                            break;
                        case 2:
                        RightARm = true;

                            break;
                        case 3:
                        LeftLeg = true;

                            break;
                    case 4:
                        RightLeg = true;

                            break;
                        case 5:
                        Head = true;

                            break;
                        case 6:
                        Spine = true;

                            break;
                        case 7:
                        Waist=true;

                            break;
                        
                }
            }
        }
        maineffectcount = script.effects.Count;
        /*
        effectcount2 = new List<List<List<int>>>();
        foldoutE2 = new List<bool>();
        foldoutET2 = new List<bool>();
        maineffectcount2 = new List<int>();
        for (int i = 0; i < script.Entities.Count; i++)
        {
            maineffectcount2.Add( script.Entities[i].effects.Count);
                foldoutET2.Add(false);
                effectcount2.Add(new List<List<int>>());

            for (int j = 0; j < script.Entities[i].effects.Count; j++)
            {
                effectcount2[j].Add(new List<int>());
                for (int k = 0; k < script.Entities[i].effects[j].effecttiming.Count; k++)
                {
                    effectcount2[j][k].Add(0);
                }
                foldoutE2.Add(false);

            }
        }
        */
        entitycount = script.Entities.Count;
        



        //Effect
        effects = serializedObject.FindProperty("effects");
        //effectmode = serializedObject.FindProperty("effectmode");
        cancel_move=true;
         cancel=true;

    }

    // Update is called once per frame
    public override void OnInspectorGUI()
    {

        //DrawDefaultInspector();
        serializedObject.Update();
        //movetype=(Combo_scriptable.MoveType)EditorGUILayout.EnumPopup("Mode", movetype);

        //var spriterect = new Rect(2, 3, 4, 5);
        //script.Image = EditorGUI.ObjectField(spriterect, combosprite,, typeof(Texture2D));
        //EditorGUILayout.PropertyField(combosprite,new GUIContent("Sprite", "Sprite For UI"),GUILayout.Width(400),GUILayout.Height(50));
        /*
        GUILayout.BeginVertical();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        
        EditorGUILayout.PropertyField(combosprite,new GUIContent("Sprite", "Sprite For UI"));
        GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
GUILayout.EndHorizontal();
        */
        //combosprite = (Sprite)EditorGUILayout.ObjectField(combosprite, typeof(Sprite), allowSceneObjects: true,GUILayout.Width(60),GUILayout.Height(60));
        /*
         var centerStyle = GUI.skin.GetStyle("Label");
         centerStyle.alignment = TextAnchor.UpperCenter;
         Rect newrect = new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50);
         GUI.Label(newrect, "General Info", centerStyle);
         */
        var centerStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
        centerStyle.alignment = TextAnchor.UpperCenter;
        centerStyle.fontStyle = FontStyle.Bold;
        centerStyle.fontSize = 20;
        var centerStyle4 = new GUIStyle(GUI.skin.GetStyle("Label"));
        centerStyle4.alignment = TextAnchor.MiddleRight;
        centerStyle4.fontStyle = FontStyle.Bold;
        centerStyle4.fontSize = 15;
        EditorGUILayout.LabelField("General Info", centerStyle, GUILayout.Height(30));
        //EditorGUILayout.Space();
        Combo_scriptable script = (Combo_scriptable)target;
        script.movetype = (Combo_scriptable.MoveType)EditorGUILayout.EnumPopup("Mode", script.movetype);
        movetype = script.movetype;
        EditorGUILayout.PropertyField(comboname);
        EditorGUILayout.PropertyField(combosprite, new GUIContent("Sprite", "Sprite For UI"));
        EditorGUILayout.PropertyField(Description, new GUIContent("Description", "Description of Move"));
        EditorGUILayout.PropertyField(CustomName, new GUIContent("CustomName", "Custom name for move"));
        EditorGUILayout.PropertyField(key, new GUIContent("KeyCode", "Custom name for move"));
        EditorGUILayout.PropertyField(Prioritytargets, new GUIContent("Priority target", "Targeting To Prioritize"));
        EditorGUILayout.PropertyField(combotimer, new GUIContent("Combo Timers", "Combo Timing"));
        EditorGUILayout.PropertyField(interconnected_combo, new GUIContent("Interconnected Combo", "Connected combos"));
        EditorGUILayout.PropertyField(clip, new GUIContent("Clip", "Animation Clip"));
        EditorGUILayout.PropertyField(isKinematic, new GUIContent("is Kinematic", "is the rigidbody Kinematic?"));
        EditorGUILayout.PropertyField(repeatable, new GUIContent("repeatable", "Is the animation repeatable?"));

        if (script.movetype == Combo_scriptable.MoveType.Attack || script.movetype == Combo_scriptable.MoveType.Defend)
        {
            EditorGUILayout.PropertyField(include_animation_lower_body, new GUIContent("Animate Lower Body", "if lowerbody also animates with upperbody"));
            EditorGUILayout.PropertyField(Do_After, new GUIContent("Do After", "Do move/attack after this"));
            EditorGUILayout.PropertyField(target_offset, new GUIContent("Target Offset", "Vector target offset"));
            EditorGUILayout.PropertyField(repeat, new GUIContent("Repeat Move", "Does the move repeat upon finishing?"));
            EditorGUILayout.PropertyField(includeBodyParent, new GUIContent("Include Body", "Include Whole body (Spins etc)"));
            if (repeat.boolValue == true)
                EditorGUILayout.PropertyField(numrepeat, new GUIContent("Numbers Of Repeat:", "numbers the movement will be repeated"));
        }
        if (script.movetype == Combo_scriptable.MoveType.Attack || script.movetype == Combo_scriptable.MoveType.Defend|| script.movetype == Combo_scriptable.MoveType.Stance)
        {
            EditorGUILayout.PropertyField(returnToDedefaultAftermove, new GUIContent("Default After", "Does the move return to default After Movement?"));
            if (returnToDedefaultAftermove.boolValue == false)
            {
                EditorGUILayout.PropertyField(returnToDedefault, new GUIContent("Return to Default stance", "Does the move return to default?"));
                if (returnToDedefault.boolValue == true & returnToDedefaultAftermove.boolValue == false)
                {
                    if (GUILayout.Button("Reset Return Time"))
                    {
                        float[] time = script.combotimer.ToArray();
                        float holder = 0;
                        for (int i = 0; i < time.Length; i++)
                        {
                            holder = Mathf.Max(holder, time[i]);
                        }
                        script.returnToDedefaultTime = holder;
                    }
                    EditorGUILayout.PropertyField(returnToDedefaultTime, new GUIContent("Time to return to Default", "Time to return to default"));
                }
            }
        }
        
        EditorGUILayout.PropertyField(nowaitingtransition, new GUIContent("Instant Transition", "If the animation transition has to wait until the end of the animation"));
        EditorGUILayout.PropertyField(Use_Frames, new GUIContent("Use Frames", "If the timing uses seconds"));
        script.Use_Frames = Use_Frames.boolValue;

        if (script.animation != null)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Combo Appear", GUILayout.ExpandWidth(false), GUILayout.Width(100));

            EditorGUILayout.FloatField(0, GUILayout.Width(20));

            float timing = 0;
            if (Use_Frames.boolValue == true)
            {

                timing = EditorGUILayout.Slider(script.ComboAvailable, 0, script.animation.length * script.animation.frameRate);

                EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

            }
            else if (Use_Frames.boolValue == false)
            {
                timing = EditorGUILayout.Slider(script.ComboAvailable, 0, script.animation.length);

                EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));



            }
            script.ComboAvailable = timing;

            EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.LabelField("Requires no Animations:",centerStyle, GUILayout.Height(30));
        

        EditorGUILayout.PropertyField(AllowObjectPickup, new GUIContent("Allow Pickup", "Allow Object Pickup"));
        EditorGUILayout.PropertyField(minmaxAllow, new GUIContent("Allow Grab Time", "Min and Max Time between animation for pickingup"));
        EditorGUILayout.PropertyField(layerAffectedPickup, new GUIContent("Affected Reaching Layers", "Reaching affected Layers"));
        EditorGUILayout.PropertyField(DistanceReach, new GUIContent("Distance Reach", "Distance Reach"));
        EditorGUILayout.PropertyField(DistanceTouch, new GUIContent("Distance Touch", "Distance Touch"));
        EditorGUILayout.PropertyField(ReachSpeed, new GUIContent("ReachSpeed", "Reach Speed"));
        EditorGUILayout.PropertyField(extendwhennear, new GUIContent("Extend only near", "Only extednd affected bone when near object distance"));
        EditorGUILayout.PropertyField(only_first_IK, new GUIContent("Only Closest IK", "Only Animate the closest IK"));
        EditorGUILayout.PropertyField(AttachDistance, new GUIContent("Attach Distance", "Object parented to Main"));


    EditorGUILayout.PropertyField(differentiateLegGround, new GUIContent("Differentiate Leg anim", "Differentiate animation based on legs"));
        EditorGUILayout.PropertyField(leftleg, new GUIContent("Is Left Leg", "Is this alternate clip left or right leg"));
        EditorGUILayout.PropertyField(alternateclip, new GUIContent("Alternate Clip", "Alternate animation clip"));

    /*
            if (script.MovementAttack.MovementonAttack != null)
            {

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Start Movement", GUILayout.ExpandWidth(false), GUILayout.Width(100));

                EditorGUILayout.FloatField(0, GUILayout.Width(20));
                float timingmove = 0;
                if (Use_Frames.boolValue == true)
                {

                    timingmove = EditorGUILayout.Slider(script.MovementAttack.StartTimeSubMove, 0, script.animation.length * script.animation.frameRate);

                    EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                }
                else if (Use_Frames.boolValue == false)
                {
                    timingmove = EditorGUILayout.Slider(script.MovementAttack.StartTimeSubMove, 0, script.animation.length);

                    EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));



                }
                script.MovementAttack.StartTimeSubMove = timingmove;

                EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.PropertyField(Moveattacklayers, new GUIContent("Movement Layers", "Layers for animation"));

            }
    */







    EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Animation Setting", centerStyle, GUILayout.ExpandWidth(true), GUILayout.Height(30));
        EditorGUILayout.PropertyField(layer, new GUIContent("Layers: "));

        EditorGUILayout.BeginHorizontal();


        if (GUILayout.Toggle(Humanoid, "Humanoid", "Button"))
        {
            Humanoid = true;
            script.type = Combo_scriptable.ControlType.Humanoid;
            Quad = false;
            Multiple = false;
        }
        if (GUILayout.Toggle(Quad, "Quad", "Button"))
        {
            Quad = true;
            script.type = Combo_scriptable.ControlType.Quad;
            Humanoid = false;
            Multiple = false;

        }
        if (GUILayout.Toggle(Multiple, "Multiple", "Button"))
        {
            Multiple = true;
            script.type = Combo_scriptable.ControlType.Multiple;
            Quad = false;
            Humanoid = false;

        }

        //EditorGUILayout.PropertyField(type, new GUIContent("Type","Character Control Type"));


        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        if (GUILayout.Button("Reset Duration and Offset (Movemnt/Others)"))
        {
            script.normalizedTransitionDuration = 0.5f;
            script.normalizedTimeOffset = -0.5f;
            script.incomingnormalizedTransitionDuration = 0.5f;
            script.incomingnormalizedTimeOffset = -0.5f;

        }
        if (GUILayout.Button("Calculate Offset, Set Duration Manually (Attack/Defend/Stances)"))
        {
            script.normalizedTimeOffset = (0.5f-((script.normalizedTransitionDuration/1)-1)*.10f);
            script.incomingnormalizedTimeOffset = (0.5f-((script.normalizedTransitionDuration/1)-1)*.10f);
        }

        EditorGUILayout.PropertyField(normalizedTransitionDuration, new GUIContent("normalizedTransitionDuration", "Transition Duration"));
        EditorGUILayout.PropertyField(normalizedTimeOffset, new GUIContent("normalizedTimeOffset", "Animation Start Offset"));
        EditorGUILayout.PropertyField(incomingtransition, new GUIContent("Include Incoming Transition", "include exclusive transition time for this?"));
        if (incomingtransition.boolValue == true)
        {
            EditorGUILayout.PropertyField(incomingnormalizedTransitionDuration, new GUIContent("incoming normalizedTransitionDuration", "Transition Duration for this animation"));
            EditorGUILayout.PropertyField(incomingnormalizedTimeOffset, new GUIContent("incoming normalizedTimeOffset", "Animation Start Offset for this animation"));
        }
    EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Layers: ", centerStyle, GUILayout.Height(30));
        //EditorGUILayout.LabelField((Combo_scriptable.LayersHumanoid)script.layer + " " + script.layer.ToString(), centerStyle, GUILayout.Height(30));

        OnTypeChange();



        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        OnModeChange();
        //if (script.movetype != Combo_scriptable.MoveType.Movement)
        {
            
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("Combo Options", centerStyle, GUILayout.ExpandWidth(true), GUILayout.Height(30));
        EditorGUILayout.LabelField("Change combo while action is active");
        if (script.animation != null)
            EditorGUILayout.Slider(attackchangestart, 0, script.animation.length * script.animation.frameRate, GUIContent.none);
        EditorGUILayout.PropertyField(leftarm_change_attack, new GUIContent("Left Arm Attack"));
        EditorGUILayout.PropertyField(rightarm_change_attack, new GUIContent("Right Arm Attack"));
        EditorGUILayout.PropertyField(leftarm_change_def, new GUIContent("Left Arm defense"));
        EditorGUILayout.PropertyField(rightarm_change_def, new GUIContent("Right Arm Defense"));
        EditorGUILayout.PropertyField(TwoHanded_attack, new GUIContent("Two Arm Attack"));
        EditorGUILayout.PropertyField(TwoHanded_def, new GUIContent("Two Arm Defense"));
        EditorGUILayout.PropertyField(leftfeet_change_attack, new GUIContent("Left Feet Attack"));
        EditorGUILayout.PropertyField(rightfeet_change_attack, new GUIContent("Right Feet Attack"));
        EditorGUILayout.PropertyField(leftfeet_change_def, new GUIContent("Left Feet defense"));
        EditorGUILayout.PropertyField(rightfeet_change_def, new GUIContent("Right Feet Defense"));
        EditorGUILayout.PropertyField(TwoFooted_attack, new GUIContent("Two Feet attack"));
        EditorGUILayout.PropertyField(TwoFooted_def, new GUIContent("Two Feet def"));
        EditorGUILayout.PropertyField(MovementChange, new GUIContent("MovementChange"));
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Character Effect", centerStyle, GUILayout.ExpandWidth(true), GUILayout.Height(30));
        Effector(); 
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Entity Effect", centerStyle, GUILayout.ExpandWidth(true), GUILayout.Height(30));
        EntityEffector();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Summoning", centerStyle, GUILayout.ExpandWidth(true), GUILayout.Height(30));
        Summon();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Commands", centerStyle, GUILayout.ExpandWidth(true), GUILayout.Height(30));
        CommandLine();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Camera Movement/Clamping", centerStyle, GUILayout.ExpandWidth(true), GUILayout.Height(30));
        EditorGUILayout.PropertyField(use_defaultcam, new GUIContent("Use Default Cam"));
        EditorGUILayout.PropertyField(use_defaultcamsetting, new GUIContent("Use Default Clamps"));
        EditorGUILayout.PropertyField(returntodefaultcamafter,new GUIContent( "Return To Default"));
        EditorGUILayout.PropertyField(returntime,new GUIContent( "Return Time"));
        EditorGUILayout.PropertyField(maintaincamposition,new GUIContent( "Maintain Position"));
        EditorGUILayout.PropertyField(maintaincamrotation,new GUIContent( "Maintain Rotation"));
        EditorGUILayout.PropertyField(animateoncecam, new GUIContent( "Animate Once"));
        EditorGUILayout.PropertyField(smoothtransitioncam, new GUIContent( "Smooth Loop"));
        EditorGUILayout.PropertyField(cameraweightsonly, new GUIContent("Camera Weight Only"));

        CameraTiming();
        CamClamp();
        EditorUtility.SetDirty(script);
        serializedObject.ApplyModifiedProperties();
    }

    public void OnModeChange()
    {
        #region Styles
        var centerStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
        centerStyle.alignment = TextAnchor.UpperCenter;
        centerStyle.fontStyle = FontStyle.Bold;
        centerStyle.fontSize = 20;
        var centerStyle2 = new GUIStyle(GUI.skin.GetStyle("Label"));
        centerStyle2.alignment = TextAnchor.MiddleCenter;
        centerStyle2.fontSize = 10;
        var centerStyle3 = new GUIStyle(GUI.skin.GetStyle("Label"));
        centerStyle3.alignment = TextAnchor.MiddleLeft;
        centerStyle3.fontStyle = FontStyle.Bold;
        centerStyle3.fontSize = 15;
        var centerStyle4 = new GUIStyle(GUI.skin.GetStyle("Label"));
        centerStyle4.alignment = TextAnchor.MiddleRight;
        centerStyle4.fontStyle = FontStyle.Bold;
        centerStyle4.fontSize = 15;
        var centerStyle5 = new GUIStyle(GUI.skin.GetStyle("Label"));
        centerStyle5.alignment = TextAnchor.MiddleCenter;
        centerStyle5.fontStyle = FontStyle.Bold;
        centerStyle5.fontSize = 15;
        var centerStyle6 = new GUIStyle(GUI.skin.GetStyle("Label"));
        centerStyle6.alignment = TextAnchor.UpperCenter;
        centerStyle6.fontStyle = FontStyle.Bold;
        centerStyle6.fontSize = 20;
        var centerStyle7 = new GUIStyle(GUI.skin.GetStyle("Label"));
        centerStyle7.alignment = TextAnchor.UpperCenter;
        centerStyle7.fontStyle = FontStyle.Bold;
        centerStyle7.fontSize = 15;
        var centerStyle8 = new GUIStyle(GUI.skin.GetStyle("Label"));
        centerStyle8.alignment = TextAnchor.LowerCenter;
        centerStyle8.fontStyle = FontStyle.Bold;
        centerStyle8.fontSize = 15;
        var centerStyle9 = new GUIStyle(GUI.skin.GetStyle("Label"));
        centerStyle9.alignment = TextAnchor.MiddleLeft;
        centerStyle9.fontStyle = FontStyle.Normal;
        centerStyle9.fontSize = 15;
        #endregion
        Combo_scriptable script = (Combo_scriptable)target;
        switch (movetype)
        {
            #region Charge_PowerUp
            case Combo_scriptable.MoveType.Charge_Powerup:
                var screenrect = GUILayoutUtility.GetRect(1, 1);
                var vertrect = EditorGUILayout.BeginVertical();
                EditorGUI.DrawRect(new Rect(screenrect.x - 13, screenrect.y - 1, screenrect.width + 17, vertrect.height + 9), Color.gray);

                EditorGUILayout.LabelField("Charge PowerUP", centerStyle, GUILayout.ExpandWidth(true), GUILayout.Height(30));
                EditorGUILayout.PropertyField(powerupspeed, new GUIContent("Power UP speed", "Power Up speed to max"));
                EditorGUILayout.PropertyField(continuous, new GUIContent("Continuous", "Continuous or must be hold"));
                EditorGUILayout.PropertyField(duration, new GUIContent("Duration", "Duration of PowerUp"));
                EditorGUILayout.PropertyField(charactertransform, new GUIContent("Character Transform", "Transformation effect"));
                EditorGUILayout.PropertyField(effects, new GUIContent("Effects", "Particle effects"));
                EditorGUILayout.PropertyField(needreachmaxtransform, new GUIContent("Need to reach max", "Instant Power Up or with Delay"));
                EditorGUILayout.PropertyField(valuedropping, new GUIContent("Value Dropping", "Will the Value drop when not held?"));
                EditorGUILayout.PropertyField(drop_speed, new GUIContent("Power Down Speed", "Drop speed of power"));
                EditorGUILayout.PropertyField(maxpowermultiplier, new GUIContent("Max Multiplier", "Maximum value of multiplier"));
                EditorGUILayout.PropertyField(minpowermultiplier, new GUIContent("Min Multiplier", "Minimum start of multiplier"));


                EditorGUILayout.EndVertical();

                break;
            #endregion
            #region Attack
            case Combo_scriptable.MoveType.Attack:


                EditorGUILayout.LabelField("Attack", centerStyle, GUILayout.ExpandWidth(true), GUILayout.Height(30));

                EditorGUILayout.PropertyField(gen_damage, new GUIContent("General Damage", "Only for General Gameplay"));
                EditorGUILayout.PropertyField(Wait_reach_distance, new GUIContent("Wait To reach Distance", "Only do action if near distance"));
                EditorGUILayout.Slider(percent_strength, 1, 100, new GUIContent("Base strength percent", "Strength Percentage use(Common)"));
                //EditorGUILayout.PropertyField(MaxDistanceAttack, new GUIContent("Max Distance", "Only Attack when distance is met"));
                EditorGUILayout.Slider(MaxDistance, 0, 1, new GUIContent("Max Distance", "Only Attack when distance is met"));
                EditorGUILayout.PropertyField(Distance_Offset, new GUIContent("Distance Offset", "Distance Offset from Original hit time"));
                EditorGUILayout.PropertyField(Use_Weights, new GUIContent("Use_Weights", "Use fixed targeting"));


                if (script.animation != null)
                {

                    //EditorGUILayout.PropertyField(HasExittime, new GUIContent("Has Exit Time", "If the Attack uses Hit Timing"));
                    if (frameused != Use_Frames.boolValue)
                    {

                        LeftFeetIKTime();

                        RightFeetIKTime();

                        LeftArmIKTime();

                        RightArmIKTime();
                        serializedObject.Update();
                        frameused = Use_Frames.boolValue;
                        serializedObject.ApplyModifiedProperties();
                    }
                    if (Use_Weights.boolValue == true)
                    {
                        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                        EditorGUILayout.LabelField("Hit Time", centerStyle3, GUILayout.ExpandWidth(true), GUILayout.Height(30));

                       // EditorGUILayout.PropertyField(Use_Frames, new GUIContent("Use Frames", "If the timing uses seconds"));
                        EditorGUILayout.Space(10);
                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Toggle(view_timing_IK_only, "View Timing Only", "Button"))
                        {
                            view_timing_IK_only = true;
                            view_value_only = false;
                            cancel = false;
                        }
                        if (GUILayout.Toggle(view_value_only, "View Value Only", "Button"))
                        {
                            view_timing_IK_only = false;
                            view_value_only = true;
                            cancel = false;
                        }
                        if (GUILayout.Toggle(cancel, "View All", "Button"))
                        {
                            view_timing_IK_only = false;
                            view_value_only = false;
                            cancel = true;
                        }
                        EditorGUILayout.EndHorizontal();

                        
                            #region One By One

                            if (script.layer.Contains(1))
                                LeftArmIKCall();
                            if (script.layer.Contains(2))
                                RightArmIKCall();
                            if (script.layer.Contains(3))
                                LeftfeetIKCall();
                            if (script.layer.Contains(4))
                                RightFeetIKCall();
                            //EditorGUILayout.BeginHorizontal();

                            #endregion
                        
                        


                    }
                }
                EditorGUILayout.PropertyField(MovementAttack, new GUIContent("Move Attack", "Movement on attacking"));

                break;
            #endregion
            #region Defend
            case Combo_scriptable.MoveType.Defend:


                EditorGUILayout.LabelField("Defend", centerStyle, GUILayout.ExpandWidth(true), GUILayout.Height(30));

                EditorGUILayout.PropertyField(general_defense, new GUIContent("General Defence", "Only for General Gameplay"));
                EditorGUILayout.PropertyField(Wait_reach_distance, new GUIContent("Wait for Distance", "Wait to reach distance"));
                EditorGUILayout.Slider(percent_strength_def, 1, 100, new GUIContent("Base strength percent", "Strength Percentage use(Common)"));
                //EditorGUILayout.PropertyField(MaxDistanceAttack, new GUIContent("Max Distance", "Only Attack when distance is met"));
                EditorGUILayout.Slider(MaxDistance, 0, 1, new GUIContent("Max Distance", "Only Defend when distance is met"));
                EditorGUILayout.PropertyField(Distance_Offset, new GUIContent("Distance Offset", "Distance Offset from Original hit time"));
                EditorGUILayout.PropertyField(Use_Weights, new GUIContent("Use_Weights", "Use fixed targeting"));

                if (script.animation != null)
                {

                    //  EditorGUILayout.PropertyField(HasExittime_def, new GUIContent("Has Exit Time", "If the Attack uses Hit Timing"));
                    if (frameused_def != Use_Frames.boolValue)
                    {
                        LeftFeetIKTime();

                        RightFeetIKTime();

                        LeftArmIKTime();

                        RightArmIKTime();
                        frameused = Use_Frames.boolValue;
                    }
                    if (Use_Weights.boolValue == true)
                    {
                        EditorGUILayout.LabelField("Hit Time", centerStyle3, GUILayout.ExpandWidth(true), GUILayout.Height(30));

                   //     EditorGUILayout.PropertyField(Use_Frames, new GUIContent("Use Seconds", "If the timing uses seconds"));


                        
                            #region One By One

                            if (script.layer.Contains(1))
                                LeftArmIKCall();
                            if (script.layer.Contains(2))
                                RightArmIKCall();
                            if (script.layer.Contains(3))
                                LeftfeetIKCall();
                            if (script.layer.Contains(4))
                                RightFeetIKCall();
                            #endregion
                        
                       

                    }
                }
                EditorGUILayout.PropertyField(MovementAttack, new GUIContent("Move Attack", "Movement on attacking"));

                break;
            #endregion
            #region Movement/Dodge
            case Combo_scriptable.MoveType.Movement:
                /*
                 delaystartcontrol = serializedObject.FindProperty("delaystartcontrol");
        Lendweight = serializedObject.FindProperty("Lendweight");
        Lstartweight = serializedObject.FindProperty("Lstartweight");
        Rendweight = serializedObject.FindProperty("Rendweight");
        Rstartweight = serializedObject.FindProperty("Rstartweight");
        startHorizontal = serializedObject.FindProperty("startHorizontal");
        startVertical = serializedObject.FindProperty("startVertical");
        
        FootIK = serializedObject.FindProperty("FootIK");
                 */
                EditorGUILayout.LabelField("Main Options", centerStyle6, GUILayout.ExpandWidth(true), GUILayout.Height(30));
                EditorGUILayout.PropertyField(movementlayer, new GUIContent("Movement Layer", "is this movement a dodge?"));
                EditorGUILayout.PropertyField(slowdownwhenlanding, new GUIContent("Slowdown Move when near land", "if player will slowdown when near land"));
                if ((slowdownwhenlanding.boolValue == true || SlowDownWhenControlled.boolValue == true) && script.slowingdownORland == null)
                {
                    EditorGUILayout.HelpBox("Note: No slowdown or landing movement assigned", MessageType.Warning);
                }
                EditorGUILayout.PropertyField(Dodge, new GUIContent("Is Dodge", "is this movement a dodge?"));
                EditorGUILayout.PropertyField(defaultstopduration, new GUIContent("Default Stop Duration", "(default use) movement stopping time"));
                EditorGUILayout.PropertyField(DistaceFlexible, new GUIContent("Distance Flexible", "Can the distance be extended and reduced dynamicaly?"));
                EditorGUILayout.PropertyField(Use_Weights, new GUIContent("Use_Weights", "Use fixed targeting"));

                if (script.animation != null)
                {
                    AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(script.animation);
                    //Debug.Log(settings.loopTime);
                    if (loop.boolValue == true && settings.loopTime == false)
                    {

                        settings.loopTime = true;
                        AnimationUtility.SetAnimationClipSettings(script.animation, settings);
                    }
                    else if (loop.boolValue == false && settings.loopTime == true)
                    {
                        settings.loopTime = false;
                        AnimationUtility.SetAnimationClipSettings(script.animation, settings);

                    }
                }
                EditorGUILayout.PropertyField(returnnttostance, new GUIContent("Return To stance", "Return to original stance after"));
                if (returnnttostance.boolValue == true)
                {
                    EditorGUILayout.HelpBox("Note: Returning to the original stance after the movement immediately stops", MessageType.Info);
                    EditorGUILayout.HelpBox("Note: if on air, stance will be falling/floating or flying", MessageType.Info);

                }

                if (isKinematic.boolValue == false)
                {
                    EditorGUILayout.PropertyField(useForce, new GUIContent("Use Force", "Use Force rather than Vector Position?"));
                    if (useForce.boolValue == true)
                    {
                        EditorGUILayout.HelpBox("Note: Force is the Force/Push applied to the rigidbody to make the object move", MessageType.Info);
                        EditorGUILayout.HelpBox("Note: Speed is the multiplier of the force", MessageType.Info);
                    }
                }
                if (isKinematic.boolValue == true)
                {
                    GUI.enabled = false;
                    EditorGUILayout.PropertyField(affectedbygravity, new GUIContent("Affected by gravity", "is the player affected by gravity"));
                    GUI.enabled = true;
                    EditorGUILayout.HelpBox("Must not be kinematic to be affected by gravity", MessageType.Warning);
                    affectedbygravity.boolValue = false;
                }
                else
                    EditorGUILayout.PropertyField(affectedbygravity, new GUIContent("Affected by gravity", "is the player affected by gravity"));


                if (affectedbygravity.boolValue == true)
                {
                    EditorGUILayout.HelpBox("Vertical is calculated by Force, Vertical speed is the multiplier", MessageType.Warning);
                    EditorGUILayout.PropertyField(arcvertical, new GUIContent("Vertical Arc", "does the vertical movement uses an arc?"));
                }
                EditorGUILayout.PropertyField(needland, new GUIContent("Needs Land", "Does player need land to execute move?"));
                EditorGUILayout.PropertyField(SlowDownWhenControlled, new GUIContent("Slowdown when controlled"));
                EditorGUILayout.PropertyField(use_previous_velocity, new GUIContent("Start With Previous Velocity", "Uses the velocity from previous movement"));

                EditorGUILayout.PropertyField(defaultmove, new GUIContent("Default Stance", "Become a default Stance"));
                if (defaultmove.boolValue == true)
                {
                    EditorGUILayout.HelpBox("Warning a default stance will stay until removed manually, also speed will become zero", MessageType.Warning);
                }
                if (defaultmove.boolValue == false)
                {

                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Move Start", centerStyle6, GUILayout.ExpandWidth(true), GUILayout.Height(30));
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    if (script.animation != null)
                    {

                        EditorGUILayout.PropertyField(HasStartTime, new GUIContent("Has Start Time", "If the Movement uses Move Timing"));
                        if (frameused_move != Use_Frames_Move.boolValue)
                        {
                            #region CheckSeconds



                            LeftFeetIKTime();

                            RightFeetIKTime();

                            LeftArmIKTime();

                            RightArmIKTime();
                            MovementTime();
                            serializedObject.Update();
                            #endregion
                            frameused_move = Use_Frames_Move.boolValue;
                            serializedObject.ApplyModifiedProperties();

                        }
                        if (HasStartTime.boolValue == true)
                        {

                            EditorGUILayout.PropertyField(Use_Frames_Move, new GUIContent("Use Frames", "If the timing uses seconds"));
                            EditorGUILayout.PropertyField(FootIK, new GUIContent("Inverse Kinematics", "Does the animation involves IK?"));
                            EditorGUILayout.PropertyField(moveduringtransition, new GUIContent("moveduringtransition", "Movement also occurs during transition"));
                            EditorGUILayout.PropertyField(MovementAttack, new GUIContent("Move Attack", "Movement on attacking"));

                            if (FootIK.boolValue == true)
                            {
                                /*
                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                                EditorGUILayout.Space();
                                EditorGUILayout.Space();
                                EditorGUILayout.LabelField("IK Feet", centerStyle6, GUILayout.ExpandWidth(true), GUILayout.Height(30));
                                EditorGUILayout.Space();
                                EditorGUILayout.Space();
                                */
                                EditorGUILayout.BeginHorizontal();
                                if (GUILayout.Toggle(view_timing_IK_only, "View Timing Only", "Button"))
                                {
                                    view_timing_IK_only = true;
                                    view_value_only = false;
                                    cancel = false;
                                }
                                if (GUILayout.Toggle(view_value_only, "View Value Only", "Button"))
                                {
                                    view_timing_IK_only = false;
                                    view_value_only = true;
                                    cancel = false;
                                }
                                if (GUILayout.Toggle(cancel, "View All", "Button"))
                                {
                                    view_timing_IK_only = false;
                                    view_value_only = false;
                                    cancel = true;
                                }
                                EditorGUILayout.EndHorizontal();
                                LeftfeetIKCall();
                                //EditorGUILayout.EndFoldoutHeaderGroup();
                                GUILayout.Space(20);

                                RightFeetIKCall();
                                GUILayout.Space(20);

                                LeftArmIKCall();
                                GUILayout.Space(20);

                                RightArmIKCall();

                            }

                            EditorGUILayout.Space();

                            if (towardstargetXZ.boolValue == false || towardstargetY.boolValue == false)
                            {
                                EditorGUILayout.HelpBox("Speed is only used for default movements", MessageType.Info);

                                MoveToPos();

                            }


                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                                EditorGUILayout.Space();
                                EditorGUILayout.Space();
                                EditorGUILayout.LabelField("Locking", centerStyle6, GUILayout.ExpandWidth(true), GUILayout.Height(30));
                                EditorGUILayout.Space();
                                EditorGUILayout.Space();
                            EditorGUILayout.HelpBox("Locking on Targets can cause problems on movement so be careful", MessageType.Info);

                                EditorGUILayout.PropertyField(towardstargetXZ, new GUIContent("TargetXZ", "Does this movement lockontarget with XZ axis?"));
                                EditorGUILayout.PropertyField(towardstargetY, new GUIContent("TargetY", "Does this movement lockontarget with Y axis?"));


                            if (towardstargetXZ.boolValue == true)
                                {
                                EditorGUILayout.PropertyField(MaxdistXZ, new GUIContent("Max Distance XZ", "Max Distance Travel XZ"));
                                    if (rotTowardsTargetY.boolValue == false)
                                    {
                                        EditorGUILayout.PropertyField(rotTowardsTargetXZ, new GUIContent("RotLockXZ", "Target XZ axis to rotate towards target?"));
                                    }

                                }
                                if (towardstargetY.boolValue == true)
                                {
                                    EditorGUILayout.PropertyField(MaxdistY, new GUIContent("Max Distance Y", "Max Distance Travel Y"));
                                    if (rotTowardsTargetXZ.boolValue == false)
                                    {
                                        EditorGUILayout.PropertyField(rotTowardsTargetY, new GUIContent("RotLockY", "Lock Y axis to rotate towards target?"));
                                    }
                                }
                            }
                        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Loop", centerStyle6, GUILayout.ExpandWidth(true), GUILayout.Height(30));
                        EditorGUILayout.Space();
                        EditorGUILayout.Space();
                        if ((script.loop || script.loopend)&&(script.movemode==Combo_scriptable.MoveMode.Direction|| script.movemode == Combo_scriptable.MoveMode.Position))
                        {
                            Rect rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight * 3 + 5);
                            EditorGUI.HelpBox(rect, "Looping with Direction or Position can cause the character to spinning 360 multiple times", MessageType.Warning);
                        }
                        EditorGUILayout.PropertyField(loop, new GUIContent("Loop Whole Animation", "Does the animation Loop after finishing?"));
                        EditorGUILayout.PropertyField(loopend, new GUIContent("Loop End Frame Only", "Only Loop the end frame making the animation static"));
                        EditorGUILayout.PropertyField(looptime, new GUIContent("Loop Time", "Time before exiting animation"));
                        //player only
                        EditorGUILayout.PropertyField(keyholds, new GUIContent("KeyHolds", "Keep Looping until hold")); 





                    }


                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    //      EditorGUILayout.PropertyField(acceleration, new GUIContent("Acceleration", "Acceleration or Deacceleration"));






                    EditorGUILayout.LabelField("Additional Options", centerStyle6, GUILayout.ExpandWidth(true), GUILayout.Height(30));
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();

                    EditorGUILayout.PropertyField(delaystartcontrol, new GUIContent("Delay Control", "Amount of time delay before controlling"));
                    EditorGUILayout.PropertyField(slowingdownORland, new GUIContent("Movement when slowing or landing", "Movement when slowingdown or landing"));
                    EditorGUILayout.PropertyField(slowingdownair, new GUIContent("Movement when slowing or air", "Movement when slowingdown or landing"));
                    EditorGUILayout.PropertyField(After_Move, new GUIContent("After Move", "Movement after reaching end of animation"));

                    EditorGUILayout.PropertyField(contrallablewhencontinuing, new GUIContent("Controllable when continuous movement", "is player controllable when continuing movement?"));
                    EditorGUILayout.PropertyField(contrallablewhenslowingdown, new GUIContent("Controllable when slowing down", "is player controllable when slowingdown?"));
                    EditorGUILayout.PropertyField(controllablewhenmoving, new GUIContent("Controllable when moving", "is player controllable when starting movement?"));
                }
                break;
            #endregion
            #region Pose
            case Combo_scriptable.MoveType.Stance:
                EditorGUILayout.LabelField("Stance", centerStyle, GUILayout.ExpandWidth(true), GUILayout.Height(30));
                EditorGUILayout.PropertyField(general_Defence_Bonus, new GUIContent("General Defence Bonus", "Defence in general mode"));
                EditorGUILayout.PropertyField(hold_continuous, new GUIContent("Is Continuous", "Continuous without changes"));
                EditorGUILayout.PropertyField(wait_until_hit, new GUIContent("Wait Until Hit", "Does the move wait for the attack hit before returning to normal?"));
                EditorGUILayout.PropertyField(Body_Defence_Bonus, new GUIContent("Body Part Defence Bonus", "Layer Animated gets a defensive bonus"));
                EditorGUILayout.PropertyField(Use_Weights, new GUIContent("Use_Weights", "Use fixed targeting"));

                break;
                #endregion

        }
    }

    public void OnTypeChange()
    {
        Combo_scriptable script = (Combo_scriptable)target;
        var oldColor = GUI.backgroundColor;
        var newColor = Color.blue;

        switch (script.type)
        {
            case Combo_scriptable.ControlType.Humanoid:
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (Head)
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button( "Head", GUILayout.Width(80), GUILayout.Height(80)))
                    {
                        WholeBody = false;

                        if (!script.layer.Contains((int)Combo_scriptable.LayersHumanoid.Head))
                        {
                            script.layer.Add((int)Combo_scriptable.LayersHumanoid.Head);
                            Head = true;
                        }
                        else
                        {
                            if (script.layer.Contains((int)Combo_scriptable.LayersHumanoid.Head))
                                script.layer.Remove((int)Combo_scriptable.LayersHumanoid.Head);
                            Head = false;
                        }
                        /*
                        LeftArm = false;
                        RightARm = false;
                        LeftLeg = false;
                        RightLeg = false;
                        Spine = false;
                        WholeBody = false;
                        Waist = false;
                        */
                        OnlyWholeBody = false;
                        if (script.layer.Contains(0))
                            WholeBody = true;

                    }



                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (LeftArm)
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button( "Left Arm", GUILayout.Width(70), GUILayout.Height(170)))
                    {
                        WholeBody = false;
                        
                            if (!script.layer.Contains((int)Combo_scriptable.LayersHumanoid.LeftArm))
                            {
                                script.layer.Add((int)Combo_scriptable.LayersHumanoid.LeftArm);
                                LeftArm = true;
                            }
                            else
                            {
                                if (script.layer.Contains((int)Combo_scriptable.LayersHumanoid.LeftArm))
                                    script.layer.Remove((int)Combo_scriptable.LayersHumanoid.LeftArm);
                                LeftArm = false;
                            }
                        OnlyWholeBody = false;
                        if (script.layer.Contains(0))
                            WholeBody = true;

                    }


                    if (Spine )
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button( "Spine",  GUILayout.Width(140), GUILayout.Height(170)))
                    {
                        WholeBody = false;

                        if (!script.layer.Contains((int)Combo_scriptable.LayersHumanoid.Spine))
                        {
                            script.layer.Add((int)Combo_scriptable.LayersHumanoid.Spine);
                            Spine = true;
                        }
                        else
                        {
                            if (script.layer.Contains((int)Combo_scriptable.LayersHumanoid.Spine))
                                script.layer.Remove((int)Combo_scriptable.LayersHumanoid.Spine);
                            Spine = false;
                        }
                        OnlyWholeBody = false;
                        if (script.layer.Contains(0))
                            WholeBody = true;

                    }


                    if (RightARm )
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button( "Right Arm",  GUILayout.Width(70), GUILayout.Height(170)))
                    {
                        RightARm = !RightARm;
                        WholeBody = false;

                            if (!script.layer.Contains( (int)Combo_scriptable.LayersHumanoid.RightARm))
                                script.layer.Add((int)Combo_scriptable.LayersHumanoid.RightARm);
                            else
                            if (script.layer.Contains((int)Combo_scriptable.LayersHumanoid.RightARm))
                                script.layer.Remove((int)Combo_scriptable.LayersHumanoid.RightARm);
                        OnlyWholeBody = false;
                        if (script.layer.Contains(0))
                            WholeBody = true;

                    }


                    if (Waist )
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button( "Spine", GUILayout.Width(140), GUILayout.Height(170)))
                    {
                        Waist = !Waist;

                        WholeBody = false;

                            if (!script.layer.Contains( (int)Combo_scriptable.LayersHumanoid.Waist))
                                script.layer.Add((int)Combo_scriptable.LayersHumanoid.Waist);
                            else
                            if (script.layer.Contains((int)Combo_scriptable.LayersHumanoid.Waist))
                                script.layer.Remove((int)Combo_scriptable.LayersHumanoid.Waist);
                        OnlyWholeBody = false;
                        if (script.layer.Contains(0))
                            WholeBody = true;

                    }


                    if (LeftLeg )
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Left Leg", GUILayout.Width(70), GUILayout.Height(180)))
                    {
                        LeftLeg = !LeftLeg;
                        WholeBody = false;

                            if (!script.layer.Contains( (int)Combo_scriptable.LayersHumanoid.LeftLeg))
                                script.layer.Add((int)Combo_scriptable.LayersHumanoid.LeftLeg);
                            else
                            if (script.layer.Contains( (int)Combo_scriptable.LayersHumanoid.LeftLeg))
                                script.layer.Remove((int)Combo_scriptable.LayersHumanoid.LeftLeg);
                        OnlyWholeBody = false;
                        if (script.layer.Contains(0))
                            WholeBody = true;


                    }


                    if (RightLeg)
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button("Right Leg", GUILayout.Width(70), GUILayout.Height(180)))
                    {
                        RightLeg = !RightLeg;
                        
                        WholeBody = false;
                            if (!script.layer.Contains( (int)Combo_scriptable.LayersHumanoid.RightLeg))
                                script.layer.Add((int)Combo_scriptable.LayersHumanoid.RightLeg);
                            else
                            if (script.layer.Contains( (int)Combo_scriptable.LayersHumanoid.RightLeg))
                                script.layer.Remove((int)Combo_scriptable.LayersHumanoid.RightLeg);
                        OnlyWholeBody = false;
                        if (script.layer.Contains(0))
                            WholeBody = true;

                    }




                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();


                    if (WholeBody)
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button( "WholeBody", GUILayout.Height(50)))
                    {
                        WholeBody = !WholeBody;
                        if (WholeBody)

                        {
                            for (int i = 1; i < 8; i++)
                            {
                                if (!script.layer.Contains(i))
                                    script.layer.Add(i);

                                

                            }
                        }
                        else
                        {
                            for (int i = 1; i < 8; i++)
                            {
                               

                                 if (script.layer.Contains(i))
                                    script.layer.Remove(i);

                            }
                        }
                        RightLeg = WholeBody;

                        LeftLeg = WholeBody;
                        LeftArm = WholeBody;
                        RightARm = WholeBody;
                        Head = WholeBody;
                        RightLeg = WholeBody;
                        Spine = WholeBody;
                        Waist = WholeBody;
                        OnlyWholeBody = false;
                        
                    }



                    GUI.backgroundColor = oldColor;
                    if (OnlyWholeBody)
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button("Only Wholebody/Does not override other layer", GUILayout.Height(50)))
                    {
                        script.layer.Clear();
                        script.layer.Add(0);
                        //humanoid
                        LeftArm = false; RightARm = false; Head = false; LeftLeg = false; RightLeg = false;
                        Spine = false; WholeBody = false; Waist = false;
                        OnlyWholeBody = !OnlyWholeBody;
                        if (OnlyWholeBody)
                        {
                            script.layer.Clear();
                            script.layer.Add(0);
                        }
                        else
                        {
                            script.layer.Remove(0);
                        }
                    }
                    GUI.backgroundColor = oldColor;
                    break;
                }
            case Combo_scriptable.ControlType.Quad:
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (Head2 )
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button( "Head", GUILayout.Width(80), GUILayout.Height(80)))
                    {
                        
                        Head2 = !Head2;
                        WholeBody2 = false;

                            if (!script.layer.Contains((int)Combo_scriptable.LayersQuadrouped.Head2))
                                script.layer.Add((int)Combo_scriptable.LayersQuadrouped.Head2);
                            else
                            if (script.layer.Contains((int)Combo_scriptable.LayersQuadrouped.Head2))
                                script.layer.Remove((int)Combo_scriptable.LayersQuadrouped.Head2);
                        OnlyWholeBody2 = false;
                        if (script.layer.Contains(0))
                            WholeBody2 = true;

                    }



                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (FrontLeftFeet )
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button("Front Left Feet", GUILayout.Width(100), GUILayout.Height(50)))
                    {
                        
                        FrontLeftFeet = !FrontLeftFeet;
                        WholeBody2 = false;

                            if (!script.layer.Contains((int)Combo_scriptable.LayersQuadrouped.FrontLeftFeet))
                                script.layer.Add((int)Combo_scriptable.LayersQuadrouped.FrontLeftFeet);
                            else
                            if (script.layer.Contains((int)Combo_scriptable.LayersQuadrouped.FrontLeftFeet))
                                script.layer.Remove((int)Combo_scriptable.LayersQuadrouped.FrontLeftFeet);
                        OnlyWholeBody2 = false;
                        if (script.layer.Contains(0))
                            WholeBody2 = true;

                    }


                    if (FrontSpine)
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button("Front Spine",  GUILayout.Width(100), GUILayout.Height(140)))
                    {
                        
                        FrontSpine = !FrontSpine;
                        WholeBody2 = false;

                            if (!script.layer.Contains((int)Combo_scriptable.LayersQuadrouped.FrontSpine))
                                script.layer.Add((int)Combo_scriptable.LayersQuadrouped.FrontSpine);
                            else
                            if (script.layer.Contains((int)Combo_scriptable.LayersQuadrouped.FrontSpine))
                                script.layer.Remove((int)Combo_scriptable.LayersQuadrouped.FrontSpine);
                        OnlyWholeBody2 = false;
                        if (script.layer.Contains(0))
                            WholeBody2 = true;

                    }


                    if (FrontRightFeet )
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button("Front Right Feet",  GUILayout.Width(100), GUILayout.Height(50)))
                    {
                        
                        FrontRightFeet = !FrontRightFeet;
                        WholeBody2 = false;

                            if (!script.layer.Contains((int)Combo_scriptable.LayersQuadrouped.FrontRightFeet))
                                script.layer.Add((int)Combo_scriptable.LayersQuadrouped.FrontRightFeet);
                            else
                            if (script.layer.Contains((int)Combo_scriptable.LayersQuadrouped.FrontRightFeet))
                                script.layer.Remove((int)Combo_scriptable.LayersQuadrouped.FrontRightFeet);
                        OnlyWholeBody2 = false;
                        if (script.layer.Contains(0))
                            WholeBody2 = true;

                    }



                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (BackLeftFeet )
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button( "Back Left Feet", GUILayout.Width(100), GUILayout.Height(50)))
                    {
                        
                        BackLeftFeet = !BackLeftFeet;
                        WholeBody2 = false;

                            if (!script.layer.Contains((int)Combo_scriptable.LayersQuadrouped.BackLeftFeet))
                                script.layer.Add((int)Combo_scriptable.LayersQuadrouped.BackLeftFeet);
                            else
                            if (script.layer.Contains((int)Combo_scriptable.LayersQuadrouped.BackLeftFeet))
                                script.layer.Remove((int)Combo_scriptable.LayersQuadrouped.BackLeftFeet);
                        OnlyWholeBody2 = false;
                        if (script.layer.Contains(0))
                            WholeBody2 = true;

                    }


                    if (BackSpine )
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button( "Back Spine",GUILayout.Width(100), GUILayout.Height(140)))
                    {
                        
                        BackSpine = !BackSpine;
                        WholeBody2 = false;

                            if (!script.layer.Contains((int)Combo_scriptable.LayersQuadrouped.BackSpine))
                                script.layer.Add((int)Combo_scriptable.LayersQuadrouped.BackSpine);
                            else
                            if (script.layer.Contains((int)Combo_scriptable.LayersQuadrouped.BackSpine))
                                script.layer.Remove((int)Combo_scriptable.LayersQuadrouped.BackSpine);
                        OnlyWholeBody2 = false;
                        if (script.layer.Contains(0))
                            WholeBody2 = true;

                    }


                    if (BackRightFeet )
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button( "Back Right Feet", GUILayout.Width(100), GUILayout.Height(50)))
                    {
                       
                        BackRightFeet = !BackRightFeet;
                        WholeBody2 = false;

                            if (!script.layer.Contains((int)Combo_scriptable.LayersQuadrouped.BackRightFeet))
                                script.layer.Add((int)Combo_scriptable.LayersQuadrouped.BackRightFeet);
                            else
                            if (script.layer.Contains((int)Combo_scriptable.LayersQuadrouped.BackRightFeet))
                                script.layer.Remove((int)Combo_scriptable.LayersQuadrouped.BackRightFeet);
                        OnlyWholeBody2 = false;
                        if (script.layer.Contains(0))
                            WholeBody2 = true;

                    }

                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();


                    if ( WholeBody2)
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button("WholeBody",  GUILayout.Height(50)))
                    {
                       

                        WholeBody2 = !WholeBody2;

                        if (WholeBody2)

                            for (int i = 1; i < 8; i++)
                            {
                                if (!script.layer.Contains(i))
                                    script.layer.Add(i);
                                
                             }
                        else
                            for (int i = 1; i < 8; i++)
                            {
                                 if (script.layer.Contains(i))
                                    script.layer.Remove(i);

                            }

                        FrontLeftFeet = WholeBody2;
                        FrontSpine = WholeBody2;
                        FrontRightFeet = WholeBody2;
                        BackLeftFeet = WholeBody2;
                        Head2 = WholeBody2;
                        BackRightFeet = WholeBody2;
                        BackSpine = WholeBody2;
                        OnlyWholeBody2 = false;
                        

                    }
                    GUI.backgroundColor = oldColor;

                    if (OnlyWholeBody2)
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button("Only Wholebody/Does not override other layer", GUILayout.Height(50)))
                    {
                        script.layer.Clear();
                        script.layer.Add(0);
                        //humanoid
                        //quad
                        FrontLeftFeet = false; FrontRightFeet = false; BackLeftFeet = false;
                        BackRightFeet = false; FrontSpine = false; BackSpine = false; WholeBody2 = false; Head2 = false;
                        OnlyWholeBody2 = !OnlyWholeBody2;
                        if (OnlyWholeBody2)
                        {
                            script.layer.Clear();
                            script.layer.Add(0);
                        }
                        else
                        {
                            script.layer.Remove(0);
                        }
                    }
                    GUI.backgroundColor = oldColor;

                    break;
                }
            case Combo_scriptable.ControlType.Multiple:
                {
                    if (Head3 )
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button( "Head",  GUILayout.Width(80), GUILayout.Height(80)))
                    {
                        
                        Head3 = !Head3;
                        WholeBody3 = false;

                            if (!script.layer.Contains((int)Combo_scriptable.LayersMultiple.Head3))
                                script.layer.Add((int)Combo_scriptable.LayersMultiple.Head3);
                            else
                            if (script.layer.Contains((int)Combo_scriptable.LayersMultiple.Head3))
                                script.layer.Remove((int)Combo_scriptable.LayersMultiple.Head3);
                        OnlyWholeBody3 = false;
                        if (script.layer.Contains(0))
                            WholeBody3 = true;

                    }


                    if (Spine3 )
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Spine",  GUILayout.Width(80), GUILayout.Height(80)))
                    {


                        Spine3 = !Spine3;
                        WholeBody3 = false;

                            if (!script.layer.Contains((int)Combo_scriptable.LayersMultiple.Spine3))
                                script.layer.Add((int)Combo_scriptable.LayersMultiple.Spine3);
                            else
                            if (script.layer.Contains((int)Combo_scriptable.LayersMultiple.Spine3))
                                script.layer.Remove((int)Combo_scriptable.LayersMultiple.Spine3);
                        OnlyWholeBody3 = false;
                        if (script.layer.Contains(0))
                            WholeBody3 = true;

                    }


                    if (Layer1 )
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button( "1"))
                    {

                        Layer1 = !Layer1;
                        WholeBody3 = false;

                            if (!script.layer.Contains((int)Combo_scriptable.LayersMultiple.Layer1))
                                script.layer.Add((int)Combo_scriptable.LayersMultiple.Layer1);
                            else
                            if (script.layer.Contains((int)Combo_scriptable.LayersMultiple.Layer1))
                                script.layer.Remove((int)Combo_scriptable.LayersMultiple.Layer1);
                        OnlyWholeBody3 = false;
                        if (script.layer.Contains(0))
                            WholeBody3 = true;

                    }


                    if (Layer2 )
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button( "2"))
                    {

                        Layer2 = !Layer2;
                        WholeBody3 = false;

                            if (!script.layer.Contains((int)Combo_scriptable.LayersMultiple.Layer2))
                                script.layer.Add((int)Combo_scriptable.LayersMultiple.Layer2);
                            else
                            if (script.layer.Contains((int)Combo_scriptable.LayersMultiple.Layer2))
                                script.layer.Remove((int)Combo_scriptable.LayersMultiple.Layer2);
                        OnlyWholeBody3 = false;
                        if (script.layer.Contains(0))
                            WholeBody3 = true;

                    }


                    if (Layer3 )
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button( "3"))
                    {

                        Layer3 = !Layer3;
                        WholeBody3 = false;

                            if (!script.layer.Contains((int)Combo_scriptable.LayersMultiple.Layer3))
                                script.layer.Add((int)Combo_scriptable.LayersMultiple.Layer3);
                            else
                            if (script.layer.Contains((int)Combo_scriptable.LayersMultiple.Layer3))
                                script.layer.Remove((int)Combo_scriptable.LayersMultiple.Layer3);
                        OnlyWholeBody3 = false;
                        if (script.layer.Contains(0))
                            WholeBody3 = true;

                    }


                    if (Layer4 )
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button( "4"))
                    {
                       
                        Layer4 = !Layer4;
                        WholeBody3 = false;

                            if (!script.layer.Contains((int)Combo_scriptable.LayersMultiple.Layer4))
                                script.layer.Add((int)Combo_scriptable.LayersMultiple.Layer4);
                            else
                            if (script.layer.Contains((int)Combo_scriptable.LayersMultiple.Layer4))
                                script.layer.Remove((int)Combo_scriptable.LayersMultiple.Layer4);
                        OnlyWholeBody3 = false;
                        if (script.layer.Contains(0))
                            WholeBody3 = true;

                    }


                    if (Layer5 )
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button("5"))
                    {

                        Layer5 = !Layer5;
                        WholeBody3 = false;

                            if (!script.layer.Contains((int)Combo_scriptable.LayersMultiple.Layer5))
                                script.layer.Add((int)Combo_scriptable.LayersMultiple.Layer5);
                            else
                            if (script.layer.Contains((int)Combo_scriptable.LayersMultiple.Layer5))
                                script.layer.Remove((int)Combo_scriptable.LayersMultiple.Layer5);
                        OnlyWholeBody3 = false;
                        if (script.layer.Contains(0))
                            WholeBody3 = true;

                    }


                    if (Layer6 )
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button( "6"))
                    {

                        Layer6 = !Layer6;
                        WholeBody3 = false;

                            if (!script.layer.Contains((int)Combo_scriptable.LayersMultiple.Layer6))
                                script.layer.Add((int)Combo_scriptable.LayersMultiple.Layer6);
                            else
                            if (script.layer.Contains((int)Combo_scriptable.LayersMultiple.Layer6))
                                script.layer.Remove((int)Combo_scriptable.LayersMultiple.Layer6);
                        OnlyWholeBody3 = false;
                        if (script.layer.Contains(0))
                            WholeBody3 = true;

                    }


                    if (Layer7 )
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button( "7"))
                    {

                        Layer7 = !Layer7;
                        WholeBody3 = false;

                            if (!script.layer.Contains((int)Combo_scriptable.LayersMultiple.Layer7))
                                script.layer.Add((int)Combo_scriptable.LayersMultiple.Layer7);
                            else
                            if (script.layer.Contains((int)Combo_scriptable.LayersMultiple.Layer7))
                                script.layer.Remove((int)Combo_scriptable.LayersMultiple.Layer7);
                        OnlyWholeBody3 = false;
                        if (script.layer.Contains(0))
                            WholeBody3 = true;

                    }


                    if (Layer8 )
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button( "8"))
                    {

                        Layer8 = !Layer8;
                        WholeBody3 = false;

                            if (!script.layer.Contains((int)Combo_scriptable.LayersMultiple.Layer8))
                                script.layer.Add((int)Combo_scriptable.LayersMultiple.Layer8);
                            else
                            if (script.layer.Contains((int)Combo_scriptable.LayersMultiple.Layer8))
                                script.layer.Remove((int)Combo_scriptable.LayersMultiple.Layer8);
                        OnlyWholeBody3 = false;
                        if (script.layer.Contains(0))
                            WholeBody3 = true;


                    }


                    if ( WholeBody3)
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;

                    EditorGUILayout.EndHorizontal();
                    if (GUILayout.Button("Wholebody",  GUILayout.Height(50)))
                    {
                        

                        WholeBody3 = !WholeBody3;
                        if (WholeBody3)

                            for (int i = 1; i < 11; i++)
                            {
                                if (!script.layer.Contains(i))
                                    script.layer.Add(i);


                            }
                        else
                            for (int i = 1; i < 11; i++)
                            { if (script.layer.Contains(i))
                                    script.layer.Remove(i);
                             }
                        Head3 = WholeBody3;
                        Spine3 = WholeBody3;
                        Layer1 = WholeBody3;
                        Layer2 = WholeBody3;
                        Layer3 = WholeBody3;
                        Layer4 = WholeBody3;
                        Layer5 = WholeBody3;
                        Layer6 = WholeBody3;
                        Layer7 = WholeBody3;
                        Layer8 = WholeBody3;
                        OnlyWholeBody3 = false;
                    }
                    GUI.backgroundColor = oldColor;
                    if (OnlyWholeBody3)
                        GUI.backgroundColor = newColor;
                    else
                        GUI.backgroundColor = oldColor;
                    if (GUILayout.Button("Only Wholebody/Does not override other layer", GUILayout.Height(50)))
                    {
                        OnlyWholeBody3 = !OnlyWholeBody3;
                        if (OnlyWholeBody3)
                        {
                            script.layer.Clear();
                            script.layer.Add(0);
                        }
                        else
                        {
                            script.layer.Remove(0);
                        }
        Head3 = false; Spine3 = false; Layer1 = false; Layer2 = false;
        Layer3 = false; Layer4 = false; Layer5 = false; Layer6 = false; Layer7 = false; Layer8 = false;
        WholeBody3 = false;

                    }
                    GUI.backgroundColor = oldColor;
                    break;
                }

        }
    }

    void LeftArmIKCall()
    {
        var centerStyle4 = new GUIStyle(GUI.skin.GetStyle("Label"));
        centerStyle4.alignment = TextAnchor.MiddleRight;
        centerStyle4.fontStyle = FontStyle.Bold;
        centerStyle4.fontSize = 15;
        #region LeftArm
        Combo_scriptable script = (Combo_scriptable)target;
        EditorGUILayout.BeginHorizontal();
        foldoutLA = EditorGUILayout.Foldout(foldoutLA, new GUIContent("Left Arm IK"));

        GUILayout.FlexibleSpace();
        if (script.LeftArmIK.Count < leftarmcount)
        {
            script.LeftArmIK.Add(new InverseKinematicTiming(0, 0));
        }
        else if (script.LeftArmIK.Count > leftarmcount)
        {
            script.LeftArmIK.RemoveAt(script.LeftArmIK.Count - 1);
        }
        else if (leftarmcount == 0)
        {
            script.LeftArmIK.Clear();
        }

        leftarmcount = EditorGUILayout.IntField(leftarmcount, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10);
        if (foldoutLA)
        {

            //EditorGUILayout.PropertyField(LeftFootIK);
            if (script.LeftArmIK.Count == leftarmcount)
            {
                var screenrect2 = GUILayoutUtility.GetRect(1, 1);
                var vertrect2 = EditorGUILayout.BeginVertical();
                EditorGUI.DrawRect(new Rect(screenrect2.x - 13, screenrect2.y - 1, screenrect2.width + 17, vertrect2.height + 9), new Color(0.5f, 0.5f, 0.5f, 0.5f));
                Color color = GUI.backgroundColor;
                for (int i = 0; i < leftarmcount; i++)
                {
                    //GUI.backgroundColor = new Color(0,0,0,0.5f);
                    GUI.backgroundColor = color;
                    GUI.backgroundColor = new Color(0, 0, 0, 0.5f);
                    float timing = script.LeftArmIK[i].timing;
                    float value = script.LeftArmIK[i].value;
                    if (view_value_only == false)
                    {

                        if (script.movetype == Combo_scriptable.MoveType.Movement)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Timing", GUILayout.ExpandWidth(false), GUILayout.Width(55));

                            EditorGUILayout.FloatField(0, GUILayout.Width(20));
                            if (Use_Frames_Move.boolValue == true)
                            {

                                timing = EditorGUILayout.Slider(script.LeftArmIK[i].timing, 0, script.animation.length * script.animation.frameRate);

                                EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                script.LeftArmIK[i] = new InverseKinematicTiming(timing, 0);
                            }
                            else if (Use_Frames.boolValue == false)
                            {
                                timing = EditorGUILayout.Slider(script.LeftArmIK[i].timing, 0, script.animation.length);

                                EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                script.LeftArmIK[i] = new InverseKinematicTiming(timing, 0);


                            }
                            EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                            EditorGUILayout.EndHorizontal();
                        }
                        else if (script.movetype == Combo_scriptable.MoveType.Attack || (script.movetype == Combo_scriptable.MoveType.Defend))
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Timing", GUILayout.ExpandWidth(false), GUILayout.Width(55));

                            EditorGUILayout.FloatField(0, GUILayout.Width(20));
                            if (Use_Frames.boolValue == true)
                            {

                                timing = EditorGUILayout.Slider(script.LeftArmIK[i].timing, 0, script.animation.length * script.animation.frameRate);

                                EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                script.LeftArmIK[i] = new InverseKinematicTiming(timing, 0);
                            }
                            else if (Use_Frames.boolValue == false)
                            {
                                timing = EditorGUILayout.Slider(script.LeftArmIK[i].timing, 0, script.animation.length);

                                EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                script.LeftArmIK[i] = new InverseKinematicTiming(timing, 0);


                            }
                            EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                            EditorGUILayout.EndHorizontal();
                        }

                    }

                    GUI.backgroundColor = new Color(0, 0, 0, 0.5f);

                    if (view_timing_IK_only == false)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Value", GUILayout.ExpandWidth(false), GUILayout.Width(55));
                        EditorGUILayout.FloatField(0, GUILayout.Width(80));
                        value = EditorGUILayout.Slider(value, 0, 1);
                        EditorGUILayout.FloatField(1, GUILayout.Width(80));

                        EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));
                        EditorGUILayout.EndHorizontal();
                    }
                    script.LeftArmIK[i] = new InverseKinematicTiming(timing, value);
                    GUI.backgroundColor = Color.grey;
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                }
                GUI.backgroundColor = Color.gray;
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("+", GUILayout.Width(30)))
                {
                    leftarmcount += 1;
                }
                if (GUILayout.Button("-", GUILayout.Width(30)))
                {
                    leftarmcount -= 1;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
                GUI.backgroundColor = color;
            }

        }
        #endregion
    }
    void RightFeetIKCall()
    {
        var centerStyle4 = new GUIStyle(GUI.skin.GetStyle("Label"));
        centerStyle4.alignment = TextAnchor.MiddleRight;
        centerStyle4.fontStyle = FontStyle.Bold;
        centerStyle4.fontSize = 15;
        Combo_scriptable script = (Combo_scriptable)target;

        #region RightFoot 
        EditorGUILayout.BeginHorizontal();
        foldoutRL = EditorGUILayout.Foldout(foldoutRL, new GUIContent("Right Leg IK"));

        GUILayout.FlexibleSpace();
        if (script.RightfootIK.Count < rightfootcount)
        {
            script.RightfootIK.Add(new InverseKinematicTiming(0, 0));
        }
        else if (script.RightfootIK.Count > rightfootcount)
        {
            script.RightfootIK.RemoveAt(script.RightfootIK.Count - 1);
        }
        else if (rightfootcount == 0)
        {
            script.RightfootIK.Clear();
        }
        rightfootcount = EditorGUILayout.IntField(rightfootcount, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10);
        if (foldoutRL)
        {

            //EditorGUILayout.PropertyField(LeftFootIK);
            if (script.RightfootIK.Count == rightfootcount)
            {
                var screenrect2 = GUILayoutUtility.GetRect(1, 1);
                var vertrect2 = EditorGUILayout.BeginVertical();
                EditorGUI.DrawRect(new Rect(screenrect2.x - 13, screenrect2.y - 1, screenrect2.width + 17, vertrect2.height + 9), new Color(0.5f, 0.5f, 0.5f, 0.5f));
                Color color = GUI.backgroundColor;
                for (int i = 0; i < rightfootcount; i++)
                {
                    //GUI.backgroundColor = new Color(0,0,0,0.5f);
                    GUI.backgroundColor = color;
                    GUI.backgroundColor = new Color(0, 0, 0, 0.5f);
                    float timing = script.RightfootIK[i].timing;
                    float value = script.RightfootIK[i].value;
                    if (view_value_only == false)
                    {
                        if (script.movetype == Combo_scriptable.MoveType.Movement)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Timing", GUILayout.ExpandWidth(false), GUILayout.Width(55));

                            EditorGUILayout.FloatField(0, GUILayout.Width(20));

                            if (Use_Frames_Move.boolValue == true)
                            {

                                timing = EditorGUILayout.Slider(script.RightfootIK[i].timing, 0, script.animation.length * script.animation.frameRate);

                                EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                script.RightfootIK[i] = new InverseKinematicTiming(timing, 0);
                            }
                            else if (Use_Frames_Move.boolValue == false)
                            {
                                timing = EditorGUILayout.Slider(script.RightfootIK[i].timing, 0, script.animation.length);

                                EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                script.RightfootIK[i] = new InverseKinematicTiming(timing, 0);


                            }
                            EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                            EditorGUILayout.EndHorizontal();
                        }
                        else if (script.movetype == Combo_scriptable.MoveType.Attack || (script.movetype == Combo_scriptable.MoveType.Defend))
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Timing", GUILayout.ExpandWidth(false), GUILayout.Width(55));

                            EditorGUILayout.FloatField(0, GUILayout.Width(20));

                            if (Use_Frames.boolValue == true)
                            {

                                timing = EditorGUILayout.Slider(script.RightfootIK[i].timing, 0, script.animation.length * script.animation.frameRate);

                                EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                script.RightfootIK[i] = new InverseKinematicTiming(timing, 0);
                            }
                            else if (Use_Frames.boolValue == false)
                            {
                                timing = EditorGUILayout.Slider(script.RightfootIK[i].timing, 0, script.animation.length);

                                EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                script.RightfootIK[i] = new InverseKinematicTiming(timing, 0);


                            }
                            EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                            EditorGUILayout.EndHorizontal();
                        }

                    }

                    GUI.backgroundColor = new Color(0, 0, 0, 0.5f);

                    if (view_timing_IK_only == false)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Value", GUILayout.ExpandWidth(false), GUILayout.Width(55));
                        EditorGUILayout.FloatField(0, GUILayout.Width(80));
                        value = EditorGUILayout.Slider(value, 0, 1);
                        EditorGUILayout.FloatField(1, GUILayout.Width(80));

                        EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));
                        EditorGUILayout.EndHorizontal();
                    }
                    script.RightfootIK[i] = new InverseKinematicTiming(timing, value);
                    GUI.backgroundColor = Color.grey;
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                }
                GUI.backgroundColor = Color.gray;
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("+", GUILayout.Width(30)))
                {
                    rightfootcount += 1;
                }
                if (GUILayout.Button("-", GUILayout.Width(30)))
                {
                    rightfootcount -= 1;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
                GUI.backgroundColor = color;
            }
        }
        #endregion
    }
    void LeftfeetIKCall()
    {
        var centerStyle4 = new GUIStyle(GUI.skin.GetStyle("Label"));
        centerStyle4.alignment = TextAnchor.MiddleRight;
        centerStyle4.fontStyle = FontStyle.Bold;
        centerStyle4.fontSize = 15; Combo_scriptable script = (Combo_scriptable)target;

        #region LeftFoot
        EditorGUILayout.BeginHorizontal();
        foldoutLL = EditorGUILayout.Foldout(foldoutLL, new GUIContent("Left Leg IK"));
        GUILayout.FlexibleSpace();
        if (script.LeftfootIK.Count < leftfootcount)
        {
            script.LeftfootIK.Add(new InverseKinematicTiming(0, 0));
        }
        else if (script.LeftfootIK.Count > leftfootcount)
        {
            script.LeftfootIK.RemoveAt(script.LeftfootIK.Count - 1);
        }
        else if (leftfootcount == 0)
        {
            script.LeftfootIK.Clear();
        }
        leftfootcount = EditorGUILayout.IntField(leftfootcount, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10);
        if (foldoutLL)
        {

            //EditorGUILayout.PropertyField(LeftFootIK);
            if (script.LeftfootIK.Count == leftfootcount)
            {
                var screenrect2 = GUILayoutUtility.GetRect(1, 1);
                var vertrect2 = EditorGUILayout.BeginVertical();
                EditorGUI.DrawRect(new Rect(screenrect2.x - 13, screenrect2.y - 1, screenrect2.width + 17, vertrect2.height + 9), new Color(0.5f, 0.5f, 0.5f, 0.5f));
                Color color = GUI.backgroundColor;
                for (int i = 0; i < leftfootcount; i++)
                {
                    //GUI.backgroundColor = new Color(0,0,0,0.5f);
                    GUI.backgroundColor = color;
                    GUI.backgroundColor = new Color(0, 0, 0, 0.5f);
                    float timing = script.LeftfootIK[i].timing;
                    float value = script.LeftfootIK[i].value;
                    if (view_value_only == false)
                    {
                        if (script.movetype == Combo_scriptable.MoveType.Movement)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Timing", GUILayout.ExpandWidth(false), GUILayout.Width(55));

                            EditorGUILayout.FloatField(0, GUILayout.Width(20));
                            if (Use_Frames_Move.boolValue == true)
                            {

                                timing = EditorGUILayout.Slider(script.LeftfootIK[i].timing, 0, script.animation.length * script.animation.frameRate);

                                EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                script.LeftfootIK[i] = new InverseKinematicTiming(timing, 0);
                            }
                            else
                            {
                                timing = EditorGUILayout.Slider(script.LeftfootIK[i].timing, 0, script.animation.length);

                                EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                script.LeftfootIK[i] = new InverseKinematicTiming(timing, 0);


                            }
                            EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                            EditorGUILayout.EndHorizontal();
                        }
                        else if (script.movetype == Combo_scriptable.MoveType.Attack || (script.movetype == Combo_scriptable.MoveType.Defend))
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Timing", GUILayout.ExpandWidth(false), GUILayout.Width(55));

                            EditorGUILayout.FloatField(0, GUILayout.Width(20));
                            if (Use_Frames.boolValue == true)
                            {

                                timing = EditorGUILayout.Slider(script.LeftfootIK[i].timing, 0, script.animation.length * script.animation.frameRate);

                                EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                script.LeftfootIK[i] = new InverseKinematicTiming(timing, 0);
                            }
                            else if (Use_Frames.boolValue == false)
                            {
                                timing = EditorGUILayout.Slider(script.LeftfootIK[i].timing, 0, script.animation.length);

                                EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                script.LeftfootIK[i] = new InverseKinematicTiming(timing, 0);


                            }
                            EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                            EditorGUILayout.EndHorizontal();
                        }


                    }
                    GUI.backgroundColor = new Color(0, 0, 0, 0.5f);

                    if (view_timing_IK_only == false)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Value", GUILayout.ExpandWidth(false), GUILayout.Width(55));
                        EditorGUILayout.FloatField(0, GUILayout.Width(80));
                        value = EditorGUILayout.Slider(value, 0, 1);
                        EditorGUILayout.FloatField(1, GUILayout.Width(80));

                        EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));
                        EditorGUILayout.EndHorizontal();
                    }
                    script.LeftfootIK[i] = new InverseKinematicTiming(timing, value);
                    GUI.backgroundColor = Color.grey;
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                }
                GUI.backgroundColor = Color.gray;
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("+", GUILayout.Width(30)))
                {
                    leftfootcount += 1;
                }
                if (GUILayout.Button("-", GUILayout.Width(30)))
                {
                    leftfootcount -= 1;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
                GUI.backgroundColor = color;
            }
        }
        #endregion
    }
    void RightArmIKCall()
    {
        var centerStyle4 = new GUIStyle(GUI.skin.GetStyle("Label"));
        centerStyle4.alignment = TextAnchor.MiddleRight;
        centerStyle4.fontStyle = FontStyle.Bold;
        centerStyle4.fontSize = 15; Combo_scriptable script = (Combo_scriptable)target;

        #region RightArm
        EditorGUILayout.BeginHorizontal();
        foldoutRA = EditorGUILayout.Foldout(foldoutRA, new GUIContent("Right Arm IK"));
        GUILayout.FlexibleSpace();
        if (script.RightArmIK.Count < rightarmcount)
        {
            script.RightArmIK.Add(new InverseKinematicTiming(0, 0));
        }
        else if (script.RightArmIK.Count > rightarmcount)
        {
            script.RightArmIK.RemoveAt(script.RightArmIK.Count - 1);
        }
        else if (rightarmcount == 0)
        {
            script.RightArmIK.Clear();
        }
        rightarmcount = EditorGUILayout.IntField(rightarmcount, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10);
        if (foldoutRA)
        {

            //EditorGUILayout.PropertyField(LeftFootIK);
            if (script.RightArmIK.Count == rightarmcount)
            {
                var screenrect2 = GUILayoutUtility.GetRect(1, 1);
                var vertrect2 = EditorGUILayout.BeginVertical();
                EditorGUI.DrawRect(new Rect(screenrect2.x - 13, screenrect2.y - 1, screenrect2.width + 17, vertrect2.height + 9), new Color(0.5f, 0.5f, 0.5f, 0.5f));
                Color color = GUI.backgroundColor;
                for (int i = 0; i < rightarmcount; i++)
                {
                    //GUI.backgroundColor = new Color(0,0,0,0.5f);
                    GUI.backgroundColor = color;
                    GUI.backgroundColor = new Color(0, 0, 0, 0.5f);
                    float timing = script.RightArmIK[i].timing;
                    float value = script.RightArmIK[i].value;
                    if (view_value_only == false)
                    {
                        if (script.movetype == Combo_scriptable.MoveType.Movement)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Timing", GUILayout.ExpandWidth(false), GUILayout.Width(55));

                            EditorGUILayout.FloatField(0, GUILayout.Width(20));
                            if (Use_Frames_Move.boolValue == true)
                            {

                                timing = EditorGUILayout.Slider(script.RightArmIK[i].timing, 0, script.animation.length * script.animation.frameRate);

                                EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                script.RightArmIK[i] = new InverseKinematicTiming(timing, 0);
                            }
                            else
                            {
                                timing = EditorGUILayout.Slider(script.RightArmIK[i].timing, 0, script.animation.length);

                                EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                script.RightArmIK[i] = new InverseKinematicTiming(timing, 0);


                            }
                            EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                            EditorGUILayout.EndHorizontal();
                        }
                        else if (script.movetype == Combo_scriptable.MoveType.Attack || (script.movetype == Combo_scriptable.MoveType.Defend))
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Timing", GUILayout.ExpandWidth(false), GUILayout.Width(55));

                            EditorGUILayout.FloatField(0, GUILayout.Width(20));
                            if (Use_Frames.boolValue == true)
                            {

                                timing = EditorGUILayout.Slider(script.RightArmIK[i].timing, 0, script.animation.length * script.animation.frameRate);

                                EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                script.RightArmIK[i] = new InverseKinematicTiming(timing, 0);
                            }
                            else if (Use_Frames.boolValue == false)
                            {
                                timing = EditorGUILayout.Slider(script.RightArmIK[i].timing, 0, script.animation.length);

                                EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                script.RightArmIK[i] = new InverseKinematicTiming(timing, 0);


                            }
                            EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                            EditorGUILayout.EndHorizontal();
                        }


                    }
                    GUI.backgroundColor = new Color(0, 0, 0, 0.5f);

                    if (view_timing_IK_only == false)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Value", GUILayout.ExpandWidth(false), GUILayout.Width(55));
                        EditorGUILayout.FloatField(0, GUILayout.Width(80));
                        value = EditorGUILayout.Slider(value, 0, 1);
                        EditorGUILayout.FloatField(1, GUILayout.Width(80));

                        EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));
                        EditorGUILayout.EndHorizontal();
                    }
                    script.RightArmIK[i] = new InverseKinematicTiming(timing, value);
                    GUI.backgroundColor = Color.grey;
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                }
                GUI.backgroundColor = Color.gray;
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("+", GUILayout.Width(30)))
                {
                    rightarmcount += 1;
                }
                if (GUILayout.Button("-", GUILayout.Width(30)))
                {
                    rightarmcount -= 1;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndVertical();
                GUI.backgroundColor = color;
            }
        }
        #endregion
    }

    void RightFeetIKTime()
    {
        Combo_scriptable script = (Combo_scriptable)target;
        #region RightFoot

        for (int i = 0; i < script.RightfootIK.Count; i++)
        {
            if (script.movetype == Combo_scriptable.MoveType.Movement)
            {
                if (Use_Frames_Move.boolValue == true)
                {

                    script.RightfootIK[i] = new InverseKinematicTiming(script.RightfootIK[i].timing * script.animation.frameRate, script.RightfootIK[i].value);
                }
                else if (Use_Frames_Move.boolValue == false)
                {
                    script.RightfootIK[i] = new InverseKinematicTiming(script.RightfootIK[i].timing / script.animation.frameRate, script.RightfootIK[i].value);


                }
            }
            else if (script.movetype == Combo_scriptable.MoveType.Attack || script.movetype == Combo_scriptable.MoveType.Defend)
            {
                if (Use_Frames.boolValue == true)
                {

                    script.RightfootIK[i] = new InverseKinematicTiming(script.RightfootIK[i].timing * script.animation.frameRate, script.RightfootIK[i].value);

                }
                else if (Use_Frames.boolValue == false)
                {

                    script.RightfootIK[i] = new InverseKinematicTiming(script.RightfootIK[i].timing / script.animation.frameRate, script.RightfootIK[i].value);


                }
            }

        }
        #endregion
    }
    void LeftFeetIKTime()
    {
        Combo_scriptable script = (Combo_scriptable)target;
        #region Leftfoot

        for (int i = 0; i < script.LeftfootIK.Count; i++)
        {
            if (script.movetype == Combo_scriptable.MoveType.Movement)
            {
                if (Use_Frames_Move.boolValue == true)
                {

                    script.LeftfootIK[i] = new InverseKinematicTiming(script.LeftfootIK[i].timing * script.animation.frameRate, script.LeftfootIK[i].value);
                }
                else if (Use_Frames_Move.boolValue == false)
                {
                    script.LeftfootIK[i] = new InverseKinematicTiming(script.LeftfootIK[i].timing / script.animation.frameRate, script.LeftfootIK[i].value);


                }
            }
            else if (script.movetype == Combo_scriptable.MoveType.Attack || script.movetype == Combo_scriptable.MoveType.Defend)
            {
                if (Use_Frames.boolValue == true)
                {

                    script.LeftfootIK[i] = new InverseKinematicTiming(script.LeftfootIK[i].timing * script.animation.frameRate, script.LeftfootIK[i].value);

                }
                else if (Use_Frames.boolValue == false)
                {

                    script.LeftfootIK[i] = new InverseKinematicTiming(script.LeftfootIK[i].timing / script.animation.frameRate, script.LeftfootIK[i].value);


                }
            }

        }

        #endregion
    }
    void RightArmIKTime()
    {
        Combo_scriptable script = (Combo_scriptable)target;
        #region RightArm

        for (int i = 0; i < script.RightArmIK.Count; i++)
        {
            if (script.movetype == Combo_scriptable.MoveType.Movement)
            {
                if (Use_Frames_Move.boolValue == true)
                {

                    script.RightArmIK[i] = new InverseKinematicTiming(script.RightArmIK[i].timing * script.animation.frameRate, script.RightArmIK[i].value);
                }
                else if (Use_Frames_Move.boolValue == false)
                {
                    script.RightArmIK[i] = new InverseKinematicTiming(script.RightArmIK[i].timing / script.animation.frameRate, script.RightArmIK[i].value);


                }
            }
            else if (script.movetype == Combo_scriptable.MoveType.Attack || script.movetype == Combo_scriptable.MoveType.Defend)
            {
                if (Use_Frames.boolValue == true)
                {

                    script.RightArmIK[i] = new InverseKinematicTiming(script.RightArmIK[i].timing * script.animation.frameRate, script.RightArmIK[i].value);

                }
                else if (Use_Frames.boolValue == false)
                {

                    script.RightArmIK[i] = new InverseKinematicTiming(script.RightArmIK[i].timing / script.animation.frameRate, script.RightArmIK[i].value);


                }
            }

        }

        #endregion
    }
    void LeftArmIKTime()
    {

        #region LeftArm
        Combo_scriptable script = (Combo_scriptable)target;
        for (int i = 0; i < script.LeftArmIK.Count; i++)
        {
            if (script.movetype == Combo_scriptable.MoveType.Movement)
            {
                if (Use_Frames_Move.boolValue == true)
                {

                    script.LeftArmIK[i] = new InverseKinematicTiming(script.LeftArmIK[i].timing * script.animation.frameRate, script.LeftArmIK[i].value);

                }
                else if (Use_Frames_Move.boolValue == false)
                {

                    script.LeftArmIK[i] = new InverseKinematicTiming(script.LeftArmIK[i].timing / script.animation.frameRate, script.LeftArmIK[i].value);


                }
            }
            else if (script.movetype == Combo_scriptable.MoveType.Attack || script.movetype == Combo_scriptable.MoveType.Defend)
            {
                if (Use_Frames.boolValue == true)
                {

                    script.LeftArmIK[i] = new InverseKinematicTiming(script.LeftArmIK[i].timing * script.animation.frameRate, script.LeftArmIK[i].value);

                }
                else if (Use_Frames.boolValue == false)
                {

                    script.LeftArmIK[i] = new InverseKinematicTiming(script.LeftArmIK[i].timing / script.animation.frameRate, script.LeftArmIK[i].value);


                }
            }
        }

        #endregion
    }


    void MovementTime()
    {
        Combo_scriptable script = (Combo_scriptable)target;

        #region Movement

        for (int i = 0; i < script.move_to_pos.Count; i++)
        {
            if (script.movetype == Combo_scriptable.MoveType.Movement)
            {
                if (Use_Frames_Move.boolValue == true)
                {
                    Debug.Log("test");
                    script.move_to_pos[i] = (new MovementTiming(script.move_to_pos[i].Direction, script.move_to_pos[i].Directiontiming, script.move_to_pos[i].speedtiming * script.animation.frameRate, script.move_to_pos[i].horizontalspeed, script.move_to_pos[i].verticalspeed, script.move_to_pos[i].allowmovement, script.move_to_pos[i].directionchangespeed, script.move_to_pos[i].faceDirection));
                }
                else if (Use_Frames_Move.boolValue == false)
                {
                    script.move_to_pos[i] = (new MovementTiming(script.move_to_pos[i].Direction, script.move_to_pos[i].Directiontiming, script.move_to_pos[i].speedtiming / script.animation.frameRate, script.move_to_pos[i].horizontalspeed, script.move_to_pos[i].verticalspeed, script.move_to_pos[i].allowmovement, script.move_to_pos[i].directionchangespeed, script.move_to_pos[i].faceDirection));


                }
            }
        }

        #endregion
    }

    public void Effector()
    {
        Combo_scriptable script = (Combo_scriptable)target;
        
        Color color = GUI.backgroundColor;
        Color contcolor = GUI.contentColor;
        var centerStyle4 = new GUIStyle(GUI.skin.GetStyle("Label"));
        centerStyle4.alignment = TextAnchor.MiddleRight;
        centerStyle4.fontStyle = FontStyle.Bold;
        centerStyle4.fontSize = 15;
        #region Effect
        var screenrect1 = GUILayoutUtility.GetRect(1, 1);
        var vertrect1 = EditorGUILayout.BeginVertical();
        EditorGUI.DrawRect(new Rect(screenrect1.x - 13, screenrect1.y - 1, screenrect1.width + 17, vertrect1.height + 9), new Color(0, 0, 0.5f, 0.5f));

        EditorGUILayout.BeginHorizontal();
        foldoutET = EditorGUILayout.Foldout(foldoutET, new GUIContent("Effect Hierarchy(Character)"));
        GUILayout.FlexibleSpace();

        maineffectcount = EditorGUILayout.IntField(maineffectcount, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10);
        
        if (script.effects.Count < maineffectcount)
        {
            script.effects.Add(new Particle_Effect());
        }
        else if (script.effects.Count > maineffectcount)
        {
            script.effects.RemoveAt(script.effects.Count - 1);
        }
        else if (maineffectcount == 0)
        {
            script.effects.Clear();
        }
        if (foldoutE.Count < maineffectcount)
        {
            foldoutE.Add(false);
        }
        else if (foldoutE.Count > maineffectcount)
        {
            foldoutE.RemoveAt(foldoutE.Count - 1);
        }
        else if (maineffectcount == 0)
        {
            foldoutE.Clear();
        }
        if (effectcount.Count < maineffectcount)
        {
            effectcount.Add(0);
        }else if (maineffectcount == 0)
        {
            effectcount.Clear();
        }
        else if (effectcount.Count > maineffectcount)
        {
            effectcount.RemoveAt(foldoutE.Count - 1);
        }
        

        
        if (foldoutET)
        {
            if (script.effects.Count == maineffectcount)
            {
                for (int j = 0; j < script.effects.Count; j++)
                {

                    if (script.effects[j].effecttiming.Count < effectcount[j])
                    {
                        script.effects[j].effecttiming.Add(new EffectTiming(0, false, "", 0, false, "", 0, false, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)));
                    }
                    else if (script.effects[j].effecttiming.Count > effectcount[j])
                    {
                        script.effects[j].effecttiming.RemoveAt(script.effects[j].effecttiming.Count - 1);
                    }
                    else if (effectcount[j] == 0)
                    {
                        script.effects[j].effecttiming.Clear();
                    }

                    
                    GUI.backgroundColor = Color.red;
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                    GUI.backgroundColor = color;
                    EditorGUILayout.BeginHorizontal();
                    foldoutE[j] = EditorGUILayout.Foldout(foldoutE[j], new GUIContent("Effect "+j.ToString()));
                    GUILayout.FlexibleSpace();

                    effectcount[j] = EditorGUILayout.IntField(effectcount[j], GUILayout.Width(50));
                    
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space(10);
                    if (foldoutE[j])
                    {
                        /*
                        if (script.effectmode.Count < script.effects.Count)
                        {
                            script.effectmode.Add(new EffectMode());
                        }
                        else if (script.effectmode.Count > script.effects.Count)
                        {
                            script.effectmode.RemoveAt(script.effectmode.Count - 1);
                        }
                        else if (script.effects.Count == 0)
                        {
                            script.effectmode.Clear();
                        }
                        script.effectmode[j]=(EffectMode)EditorGUILayout.EnumPopup("effectmode", script.effectmode[j]);
                        */
                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Toggle(view_effect_position_only, "View Position Only", "Button"))
                        {
                            view_effect_position_only = true;
                            view_effect_rotation_only = false;
                            view_effect_all = false;
                            view_effect_timing_only = false;



                        }
                        if (GUILayout.Toggle(view_effect_rotation_only, "View Rotation Only", "Button"))
                        {
                            view_effect_position_only = false;
                            view_effect_rotation_only = true;
                            view_effect_all = false;
                            view_effect_timing_only = false;
                        }
                        if (GUILayout.Toggle(view_effect_timing_only, "View Timing Only", "Button"))
                        {
                            view_effect_position_only = false;
                            view_effect_rotation_only = false;
                            view_effect_all = false;
                            view_effect_timing_only = true;
                        }
                        if (GUILayout.Toggle(view_effect_all, "View All", "Button"))
                        {
                            view_effect_position_only = false;
                            view_effect_rotation_only = false;
                            view_effect_all = true;
                            view_effect_timing_only = false;
                        }



                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space(10);

                        //EditorGUILayout.PropertyField(LeftFootIK);
                        if (script.effects[j].effecttiming.Count == effectcount[j])
                        {
                            var screenrect2 = GUILayoutUtility.GetRect(1, 1);
                            var vertrect2 = EditorGUILayout.BeginVertical();
                            EditorGUI.DrawRect(new Rect(screenrect2.x - 13, screenrect2.y - 1, screenrect2.width + 17, vertrect2.height + 9), new Color(0.5f, 0.5f, 0.5f, 0.5f));
                            
                            for (int i = 0; i < effectcount[j]; i++)
                            {
                                //GUI.backgroundColor = new Color(0,0,0,0.5f);
                                GUI.contentColor = Color.white;
                                GUI.backgroundColor = color;
                                GUI.backgroundColor = new Color(0, 0, 0, 0.5f);
                                float effect_start_time = script.effects[j].effecttiming[i].effect_start_time;
                                bool start = script.effects[j].effecttiming[i].start;
                                string effect_transform_parent = script.effects[j].effecttiming[i].effect_transform_parent;
                                float effect_start_time_parent_position = script.effects[j].effecttiming[i].effect_start_time_parent_position;
                                bool effect_follow_parent_position = script.effects[j].effecttiming[i].effect_follow_parent_position;
                                string effect_rotation_parent = script.effects[j].effecttiming[i].effect_rotation_parent;
                                float effect_start_time_parent_rotation = script.effects[j].effecttiming[i].effect_start_time_parent_rotation;
                                bool effect_follow_parent_rotation = script.effects[j].effecttiming[i].effect_follow_parent_rotation;
                                Vector3 pos = script.effects[j].effecttiming[i].position;
                                Vector4 rot = new Vector4(script.effects[j].effecttiming[i].rotation.x, script.effects[j].effecttiming[i].rotation.y, script.effects[j].effecttiming[i].rotation.z, script.effects[j].effecttiming[i].rotation.w);
                                Quaternion rotation = new Quaternion(0, 0, 0, 0);
                                if (view_effect_all && view_effect_rotation_only == false && view_effect_timing_only == false && view_effect_position_only == false)
                                {
                                    #region Time
                                    EditorGUILayout.BeginHorizontal();
                                    EditorGUILayout.LabelField("Start Timing", GUILayout.ExpandWidth(false), GUILayout.Width(100));

                                    EditorGUILayout.FloatField(0, GUILayout.Width(20));

                                    if (Use_Frames_Move.boolValue == true)
                                    {

                                        effect_start_time = EditorGUILayout.Slider(script.effects[j].effecttiming[i].effect_start_time, 0, script.animation.length * script.animation.frameRate);

                                        EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                        //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);
                                    }
                                    else
                                    {
                                        effect_start_time = EditorGUILayout.Slider(script.effects[j].effecttiming[i].effect_start_time, 0, script.animation.length);

                                        EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                        //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);


                                    }
                                    //Debug.Log(timing);
                                    EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                                    EditorGUILayout.EndHorizontal();

                                    EditorGUILayout.BeginHorizontal();
                                    EditorGUILayout.LabelField("Parent Position Timing", GUILayout.ExpandWidth(false), GUILayout.Width(150));

                                    EditorGUILayout.FloatField(0, GUILayout.Width(20));
                                    if (Use_Frames_Move.boolValue == true)
                                    {

                                        effect_start_time_parent_position = EditorGUILayout.Slider(script.effects[j].effecttiming[i].effect_start_time_parent_position, 0, script.animation.length * script.animation.frameRate);

                                        EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                        //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);
                                    }
                                    else
                                    {
                                        effect_start_time_parent_position = EditorGUILayout.Slider(script.effects[j].effecttiming[i].effect_start_time_parent_position, 0, script.animation.length);

                                        EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                        //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);


                                    }
                                    //Debug.Log(timing);
                                    EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                                    EditorGUILayout.EndHorizontal();

                                    EditorGUILayout.BeginHorizontal();
                                    EditorGUILayout.LabelField("Parent Rotation Timing", GUILayout.ExpandWidth(false), GUILayout.Width(150));

                                    EditorGUILayout.FloatField(0, GUILayout.Width(20));
                                    if (Use_Frames_Move.boolValue == true)
                                    {

                                        effect_start_time_parent_rotation = EditorGUILayout.Slider(script.effects[j].effecttiming[i].effect_start_time_parent_rotation, 0, script.animation.length * script.animation.frameRate);

                                        EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                        //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);
                                    }
                                    else
                                    {
                                        effect_start_time_parent_rotation = EditorGUILayout.Slider(script.effects[j].effecttiming[i].effect_start_time_parent_rotation, 0, script.animation.length);

                                        EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                        //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);


                                    }
                                    //Debug.Log(timing);
                                    EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                                    EditorGUILayout.EndHorizontal();
                                    #endregion
                                    #region Booleans
                                    EditorGUILayout.BeginHorizontal();
                                    if (start == false)
                                        EditorGUILayout.LabelField("Stopped");
                                    else
                                        EditorGUILayout.LabelField("Playing");
                                    GUI.backgroundColor = color;
                                    start = EditorGUILayout.Toggle(start);
                                    EditorGUILayout.EndHorizontal();
                                    EditorGUILayout.BeginHorizontal();
                                    if (effect_follow_parent_position == false)
                                        EditorGUILayout.LabelField("WorldPos");
                                    else
                                        EditorGUILayout.LabelField("LocalPos");
                                    GUI.backgroundColor = color;
                                    effect_follow_parent_position = EditorGUILayout.Toggle(effect_follow_parent_position);
                                    EditorGUILayout.EndHorizontal();
                                    EditorGUILayout.BeginHorizontal();

                                    if (effect_follow_parent_rotation == false)
                                        EditorGUILayout.LabelField("WorldRotation");
                                    else
                                        EditorGUILayout.LabelField("LocalRotation");
                                    GUI.backgroundColor = color;
                                    effect_follow_parent_rotation = EditorGUILayout.Toggle(effect_follow_parent_rotation);
                                    EditorGUILayout.EndHorizontal();
                                    #endregion


                                    //directionchangespeed = EditorGUILayout.FloatField("Direction change speed", directionchangespeed);

                                    pos = EditorGUILayout.Vector3Field("Position", pos);

                                    rot = EditorGUILayout.Vector4Field("Rotation", rot);
                                    rotation = new Quaternion(rot.x, rot.y, rot.z, rot.w);





                                }
                                else if (view_effect_timing_only == false && view_effect_rotation_only == false && view_effect_all == false)
                                {
                                    EditorGUILayout.BeginHorizontal();
                                    EditorGUILayout.LabelField("Parent Position Timing", GUILayout.ExpandWidth(false), GUILayout.Width(100));

                                    EditorGUILayout.FloatField(0, GUILayout.Width(20));
                                    if (Use_Frames_Move.boolValue == true)
                                    {

                                        effect_start_time_parent_position = EditorGUILayout.Slider(script.effects[j].effecttiming[i].effect_start_time_parent_position, 0, script.animation.length * script.animation.frameRate);

                                        EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                        //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);
                                    }
                                    else
                                    {
                                        effect_start_time_parent_position = EditorGUILayout.Slider(script.effects[j].effecttiming[i].effect_start_time_parent_position, 0, script.animation.length);

                                        EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                        //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);


                                    }
                                    //Debug.Log(timing);
                                    EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                                    EditorGUILayout.EndHorizontal();

                                    if (effect_follow_parent_position == false)
                                        EditorGUILayout.LabelField("Using WorldPos");
                                    else
                                        EditorGUILayout.LabelField("Using LocalPos");
                                    effect_follow_parent_position = EditorGUILayout.Toggle(effect_follow_parent_position);

                                    pos = EditorGUILayout.Vector3Field("Position", pos);

                                }
                                else if (view_effect_position_only == false && view_effect_timing_only == false && view_effect_all == false)

                                {

                                    EditorGUILayout.BeginHorizontal();
                                    EditorGUILayout.LabelField("Parent Rotation Timing", GUILayout.ExpandWidth(false), GUILayout.Width(100));

                                    EditorGUILayout.FloatField(0, GUILayout.Width(20));
                                    if (Use_Frames_Move.boolValue == true)
                                    {

                                        effect_start_time_parent_rotation = EditorGUILayout.Slider(script.effects[j].effecttiming[i].effect_start_time_parent_rotation, 0, script.animation.length * script.animation.frameRate);

                                        EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                        //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);
                                    }
                                    else
                                    {
                                        effect_start_time_parent_rotation = EditorGUILayout.Slider(script.effects[j].effecttiming[i].effect_start_time_parent_rotation, 0, script.animation.length);

                                        EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                        //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);


                                    }
                                    //Debug.Log(timing);
                                    EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                                    EditorGUILayout.EndHorizontal();

                                    EditorGUILayout.BeginHorizontal();
                                    if (effect_follow_parent_rotation == false)
                                        EditorGUILayout.LabelField("WorldRotation");
                                    else
                                        EditorGUILayout.LabelField("LocalRotation");
                                    GUI.backgroundColor = color;
                                    effect_follow_parent_rotation = EditorGUILayout.Toggle(effect_follow_parent_rotation);
                                    EditorGUILayout.EndHorizontal();




                                    rot = EditorGUILayout.Vector4Field("Rotation", rot);
                                    rotation = new Quaternion(rot.x, rot.y, rot.z, rot.w);
                                }
                                else if (view_effect_position_only == false && view_effect_rotation_only == false && view_effect_all == false)
                                {

                                    EditorGUILayout.BeginHorizontal();
                                    EditorGUILayout.LabelField("Start Timing", GUILayout.ExpandWidth(false), GUILayout.Width(100));

                                    EditorGUILayout.FloatField(0, GUILayout.Width(20));

                                    if (Use_Frames_Move.boolValue == true)
                                    {

                                        effect_start_time = EditorGUILayout.Slider(script.effects[j].effecttiming[i].effect_start_time, 0, script.animation.length * script.animation.frameRate);

                                        EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                        //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);
                                    }
                                    else
                                    {
                                        effect_start_time = EditorGUILayout.Slider(script.effects[j].effecttiming[i].effect_start_time, 0, script.animation.length);

                                        EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                        //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);


                                    }
                                    //Debug.Log(timing);
                                    EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                                    EditorGUILayout.EndHorizontal();

                                    EditorGUILayout.BeginHorizontal();
                                    EditorGUILayout.LabelField("Parent Position Timing", GUILayout.ExpandWidth(false), GUILayout.Width(100));

                                    EditorGUILayout.FloatField(0, GUILayout.Width(20));
                                    if (Use_Frames_Move.boolValue == true)
                                    {

                                        effect_start_time_parent_position = EditorGUILayout.Slider(script.effects[j].effecttiming[i].effect_start_time_parent_position, 0, script.animation.length * script.animation.frameRate);

                                        EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                        //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);
                                    }
                                    else
                                    {
                                        effect_start_time_parent_position = EditorGUILayout.Slider(script.effects[j].effecttiming[i].effect_start_time_parent_position, 0, script.animation.length);

                                        EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                        //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);


                                    }
                                    //Debug.Log(timing);
                                    EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                                    EditorGUILayout.EndHorizontal();

                                    EditorGUILayout.BeginHorizontal();
                                    EditorGUILayout.LabelField("Parent Rotation Timing", GUILayout.ExpandWidth(false), GUILayout.Width(100));

                                    EditorGUILayout.FloatField(0, GUILayout.Width(20));
                                    if (Use_Frames_Move.boolValue == true)
                                    {

                                        effect_start_time_parent_rotation = EditorGUILayout.Slider(script.effects[j].effecttiming[i].effect_start_time_parent_rotation, 0, script.animation.length * script.animation.frameRate);

                                        EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                        //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);
                                    }
                                    else
                                    {
                                        effect_start_time_parent_rotation = EditorGUILayout.Slider(script.effects[j].effecttiming[i].effect_start_time_parent_rotation, 0, script.animation.length);

                                        EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                        //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);


                                    }
                                    //Debug.Log(timing);
                                    EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                                    EditorGUILayout.EndHorizontal();
                                }

                                script.effects[j].effecttiming[i] = new EffectTiming(effect_start_time, start, effect_transform_parent, effect_start_time_parent_position, effect_follow_parent_position, effect_rotation_parent, effect_start_time_parent_rotation, effect_follow_parent_rotation, pos, rotation);
                                GUI.backgroundColor = Color.grey;
                                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                            }
                            GUI.backgroundColor = Color.gray;
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.FlexibleSpace();
                            if (GUILayout.Button("+", GUILayout.Width(30)))
                            {
                                effectcount[j] += 1;
                            }
                            if (GUILayout.Button("-", GUILayout.Width(30)))
                            {
                                effectcount[j] -= 1;
                            }
                            EditorGUILayout.EndHorizontal();

                            EditorGUILayout.EndVertical();
                            GUI.backgroundColor = color;
                            GUI.contentColor = contcolor;
                        }
                    }
                }
            }
        }
        EditorGUILayout.EndVertical();
        #endregion
    }

    public void EntityEffector()
    {
        Combo_scriptable script = (Combo_scriptable)target;

        Color color = GUI.backgroundColor;
        Color contcolor = GUI.contentColor;
        var centerStyle4 = new GUIStyle(GUI.skin.GetStyle("Label"));
        centerStyle4.alignment = TextAnchor.MiddleRight;
        centerStyle4.fontStyle = FontStyle.Bold;
        centerStyle4.fontSize = 15;
        #region Effect
        var screenrect1 = GUILayoutUtility.GetRect(1, 1);
        var vertrect1 = EditorGUILayout.BeginVertical();
        EditorGUI.DrawRect(new Rect(screenrect1.x - 13, screenrect1.y - 1, screenrect1.width + 17, vertrect1.height + 9), new Color(0, 0, 0.5f, 0.5f));

        EditorGUILayout.BeginHorizontal();
        foldoutEN = EditorGUILayout.Foldout(foldoutEN, new GUIContent("Effect Hierarchy(Entity)"));
        GUILayout.FlexibleSpace();

        entitycount = EditorGUILayout.IntField(entitycount, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10);
        if (foldoutEN)
        {
            if (script.Entities.Count < entitycount)
            {
                script.Entities.Add(new EntityScriptable());
            }
            else if (script.Entities.Count > entitycount)
            {
                script.Entities.RemoveAt(script.Entities.Count - 1);
            }
            else if (entitycount == 0)
            {
                script.Entities.Clear();
            }
            if (foldoutET2.Count < entitycount)
            {
                foldoutET2.Add(false);
            }
            else if (foldoutET2.Count > entitycount)
            {
                foldoutET2.RemoveAt(foldoutET2.Count - 1);
            }
            else if (entitycount == 0)
            {
                foldoutET2.Clear();
            }
            if (foldoutE2.Count < script.Entities.Count)
            {
                foldoutE2.Add(new List<bool>());
            }
            else if (foldoutE2.Count > script.Entities.Count)
            {
                foldoutE2.RemoveAt(foldoutE2.Count - 1);
            }
            else if (script.Entities.Count == 0)
            {
                foldoutE2.Clear();
            }
            for (int i = 0; i < script.Entities.Count; i++)
            {

                EditorGUILayout.BeginHorizontal();
                foldoutET2[i] = EditorGUILayout.Foldout(foldoutET2[i], new GUIContent("Entity "+i.ToString()));
                GUILayout.FlexibleSpace();

                script.Entities[i].maineffectcount = EditorGUILayout.IntField(script.Entities[i].maineffectcount, GUILayout.Width(50));
                EditorGUILayout.EndHorizontal();
                if (script.Entities[i].effects.Count < script.Entities[i].maineffectcount)
                {
                    script.Entities[i].effects.Add(new Particle_Effect());
                }
                else if (script.Entities[i].effects.Count > script.Entities[i].maineffectcount)
                {
                    script.Entities[i].effects.RemoveAt(script.Entities[i].effects.Count - 1);
                }
                else if (script.Entities[i].maineffectcount == 0)
                {
                    script.Entities[i].effects.Clear();
                }


                if (foldoutE2[i].Count < script.Entities[i].maineffectcount)
                {
                    foldoutE2[i].Add(false);
                }
                else if (foldoutE2[i].Count > script.Entities[i].maineffectcount)
                {
                    foldoutE2[i].RemoveAt(foldoutE2[i].Count - 1);
                }
                else if (script.Entities[i].maineffectcount == 0)
                {
                    foldoutE2[i].Clear();
                }
                if (script.Entities.Count==0)
                {
                    effectcount2.Clear();
                }
                while (effectcount2.Count < script.Entities.Count)
                {
                    effectcount2.Add(new List<int>());
                }
                while (effectcount2.Count > script.Entities.Count)
                {
                    effectcount2.RemoveAt(effectcount2.Count - 1);
                }

                

                if (foldoutET2[i])
                {
                    if (script.Entities[i].effects.Count == script.Entities[i].maineffectcount)
                    {
                        for (int j = 0; j < script.Entities[i].effects.Count; j++)
                        {
                if (script.Entities[i].effects.Count == 0)
                {
                    effectcount2[i].Clear();
                }
                while (effectcount2[i].Count < script.Entities[i].effects.Count)
                {
                    effectcount2[i].Add(0);
                }
                while (effectcount2[i].Count > script.Entities[i].effects.Count)
                {
                    effectcount2[i].RemoveAt(effectcount2[i].Count - 1);
                }
                            if (script.Entities[i].effects[j].effecttiming.Count < effectcount2[i][j])
                            {
                                script.Entities[i].effects[j].effecttiming.Add(new EffectTiming(0, false, "", 0, false, "", 0, false, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)));
                            }
                            else if (script.Entities[i].effects[j].effecttiming.Count > effectcount2[i][j])
                            {
                                script.Entities[i].effects[j].effecttiming.RemoveAt(script.Entities[i].effects[j].effecttiming.Count - 1);
                            }
                            else if (effectcount2[i][j] == 0)
                            {
                                script.Entities[i].effects[j].effecttiming.Clear();
                            }


                            GUI.backgroundColor = Color.red;
                            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                            GUI.backgroundColor = color;
                            EditorGUILayout.BeginHorizontal();
                            foldoutE2[i][j] = EditorGUILayout.Foldout(foldoutE2[i][j], new GUIContent("Effect " + j.ToString()));
                            GUILayout.FlexibleSpace();

                            effectcount2[i][j] = EditorGUILayout.IntField(effectcount2[i][j], GUILayout.Width(50));

                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.Space(10);

                            if (foldoutE2[i][j])
                            {
                                /*
                                if (script.effectmode.Count < script.effects.Count)
                                {
                                    script.effectmode.Add(new EffectMode());
                                }
                                else if (script.effectmode.Count > script.effects.Count)
                                {
                                    script.effectmode.RemoveAt(script.effectmode.Count - 1);
                                }
                                else if (script.effects.Count == 0)
                                {
                                    script.effectmode.Clear();
                                }
                                script.effectmode[j]=(EffectMode)EditorGUILayout.EnumPopup("effectmode", script.effectmode[j]);
                                */
                                EditorGUILayout.BeginHorizontal();
                                if (GUILayout.Toggle(view_effect_position_only, "View Position Only", "Button"))
                                {
                                    view_effect_position_only = true;
                                    view_effect_rotation_only = false;
                                    view_effect_all = false;
                                    view_effect_timing_only = false;



                                }
                                if (GUILayout.Toggle(view_effect_rotation_only, "View Rotation Only", "Button"))
                                {
                                    view_effect_position_only = false;
                                    view_effect_rotation_only = true;
                                    view_effect_all = false;
                                    view_effect_timing_only = false;
                                }
                                if (GUILayout.Toggle(view_effect_timing_only, "View Timing Only", "Button"))
                                {
                                    view_effect_position_only = false;
                                    view_effect_rotation_only = false;
                                    view_effect_all = false;
                                    view_effect_timing_only = true;
                                }
                                if (GUILayout.Toggle(view_effect_all, "View All", "Button"))
                                {
                                    view_effect_position_only = false;
                                    view_effect_rotation_only = false;
                                    view_effect_all = true;
                                    view_effect_timing_only = false;
                                }
                                EditorGUILayout.EndHorizontal();
                                if (script.Entities[i].effects[j].effecttiming.Count == effectcount2[i][j])
                                {
                                    var screenrect2 = GUILayoutUtility.GetRect(1, 1);
                                    var vertrect2 = EditorGUILayout.BeginVertical();
                                    EditorGUI.DrawRect(new Rect(screenrect2.x - 13, screenrect2.y - 1, screenrect2.width + 17, vertrect2.height + 9), new Color(0.5f, 0.5f, 0.5f, 0.5f));

                                    for (int k = 0; k < effectcount2[i][j]; k++)
                                    {
                                        //GUI.backgroundColor = new Color(0,0,0,0.5f);
                                        GUI.contentColor = Color.white;
                                        GUI.backgroundColor = color;
                                        GUI.backgroundColor = new Color(0, 0, 0, 0.5f);
                                        float effect_start_time = script.Entities[i].effects[j].effecttiming[k].effect_start_time;
                                        bool start = script.Entities[i].effects[j].effecttiming[k].start;
                                        string effect_transform_parent = script.Entities[i].effects[j].effecttiming[k].effect_transform_parent;
                                        float effect_start_time_parent_position = script.Entities[i].effects[j].effecttiming[k].effect_start_time_parent_position;
                                        bool effect_follow_parent_position = script.Entities[i].effects[j].effecttiming[k].effect_follow_parent_position;
                                        string effect_rotation_parent = script.Entities[i].effects[j].effecttiming[k].effect_rotation_parent;
                                        float effect_start_time_parent_rotation = script.Entities[i].effects[j].effecttiming[k].effect_start_time_parent_rotation;
                                        bool effect_follow_parent_rotation = script.Entities[i].effects[j].effecttiming[k].effect_follow_parent_rotation;
                                        Vector3 pos = script.Entities[i].effects[j].effecttiming[k].position;
                                        Vector4 rot = new Vector4(script.Entities[i].effects[j].effecttiming[k].rotation.x, script.Entities[i].effects[j].effecttiming[k].rotation.y, script.Entities[i].effects[j].effecttiming[k].rotation.z, script.Entities[i].effects[j].effecttiming[k].rotation.w);
                                        Quaternion rotation = new Quaternion(0, 0, 0, 0);
                                        if (view_effect_all && view_effect_rotation_only == false && view_effect_timing_only == false && view_effect_position_only == false)
                                        {
                                            #region Time
                                            EditorGUILayout.BeginHorizontal();
                                            EditorGUILayout.LabelField("Start Timing", GUILayout.ExpandWidth(false), GUILayout.Width(100));

                                            EditorGUILayout.FloatField(0, GUILayout.Width(20));

                                            if (Use_Frames_Move.boolValue == true)
                                            {

                                                effect_start_time = EditorGUILayout.Slider(script.Entities[i].effects[j].effecttiming[k].effect_start_time, 0, script.animation.length * script.animation.frameRate);

                                                EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                                //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);
                                            }
                                            else
                                            {
                                                effect_start_time = EditorGUILayout.Slider(script.Entities[i].effects[j].effecttiming[k].effect_start_time, 0, script.animation.length);

                                                EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                                //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);


                                            }
                                            //Debug.Log(timing);
                                            EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                                            EditorGUILayout.EndHorizontal();

                                            EditorGUILayout.BeginHorizontal();
                                            EditorGUILayout.LabelField("Parent Position Timing", GUILayout.ExpandWidth(false), GUILayout.Width(100));

                                            EditorGUILayout.FloatField(0, GUILayout.Width(20));
                                            if (Use_Frames_Move.boolValue == true)
                                            {

                                                effect_start_time_parent_position = EditorGUILayout.Slider(script.Entities[i].effects[j].effecttiming[k].effect_start_time_parent_position, 0, script.animation.length * script.animation.frameRate);

                                                EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                                //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);
                                            }
                                            else
                                            {
                                                effect_start_time_parent_position = EditorGUILayout.Slider(script.Entities[i].effects[j].effecttiming[k].effect_start_time_parent_position, 0, script.animation.length);

                                                EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                                //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);


                                            }
                                            //Debug.Log(timing);
                                            EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                                            EditorGUILayout.EndHorizontal();

                                            EditorGUILayout.BeginHorizontal();
                                            EditorGUILayout.LabelField("Parent Rotation Timing", GUILayout.ExpandWidth(false), GUILayout.Width(100));

                                            EditorGUILayout.FloatField(0, GUILayout.Width(20));
                                            if (Use_Frames_Move.boolValue == true)
                                            {

                                                effect_start_time_parent_rotation = EditorGUILayout.Slider(script.Entities[i].effects[j].effecttiming[k].effect_start_time_parent_rotation, 0, script.animation.length * script.animation.frameRate);

                                                EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                                //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);
                                            }
                                            else
                                            {
                                                effect_start_time_parent_rotation = EditorGUILayout.Slider(script.Entities[i].effects[j].effecttiming[k].effect_start_time_parent_rotation, 0, script.animation.length);

                                                EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                                //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);


                                            }
                                            //Debug.Log(timing);
                                            EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                                            EditorGUILayout.EndHorizontal();
                                            #endregion
                                            #region Booleans
                                            EditorGUILayout.BeginHorizontal();
                                            if (start == false)
                                                EditorGUILayout.LabelField("Stopped");
                                            else
                                                EditorGUILayout.LabelField("Playing");
                                            GUI.backgroundColor = color;
                                            start = EditorGUILayout.Toggle(start);
                                            EditorGUILayout.EndHorizontal();
                                            EditorGUILayout.BeginHorizontal();
                                            if (effect_follow_parent_position == false)
                                                EditorGUILayout.LabelField("WorldPos");
                                            else
                                                EditorGUILayout.LabelField("LocalPos");
                                            GUI.backgroundColor = color;
                                            effect_follow_parent_position = EditorGUILayout.Toggle(effect_follow_parent_position);
                                            EditorGUILayout.EndHorizontal();
                                            EditorGUILayout.BeginHorizontal();

                                            if (effect_follow_parent_rotation == false)
                                                EditorGUILayout.LabelField("WorldRotation");
                                            else
                                                EditorGUILayout.LabelField("LocalRotation");
                                            GUI.backgroundColor = color;
                                            effect_follow_parent_rotation = EditorGUILayout.Toggle(effect_follow_parent_rotation);
                                            EditorGUILayout.EndHorizontal();
                                            #endregion


                                            //directionchangespeed = EditorGUILayout.FloatField("Direction change speed", directionchangespeed);

                                            pos = EditorGUILayout.Vector3Field("Position", pos);

                                            rot = EditorGUILayout.Vector4Field("Rotation", rot);
                                            rotation = new Quaternion(rot.x, rot.y, rot.z, rot.w);





                                        }
                                        else if (view_effect_timing_only == false && view_effect_rotation_only == false && view_effect_all == false)
                                        {
                                            EditorGUILayout.BeginHorizontal();
                                            EditorGUILayout.LabelField("Parent Position Timing", GUILayout.ExpandWidth(false), GUILayout.Width(100));

                                            EditorGUILayout.FloatField(0, GUILayout.Width(20));
                                            if (Use_Frames_Move.boolValue == true)
                                            {

                                                effect_start_time_parent_position = EditorGUILayout.Slider(script.Entities[i].effects[j].effecttiming[k].effect_start_time_parent_position, 0, script.animation.length * script.animation.frameRate);

                                                EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                                //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);
                                            }
                                            else
                                            {
                                                effect_start_time_parent_position = EditorGUILayout.Slider(script.Entities[i].effects[j].effecttiming[k].effect_start_time_parent_position, 0, script.animation.length);

                                                EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                                //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);


                                            }
                                            //Debug.Log(timing);
                                            EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                                            EditorGUILayout.EndHorizontal();

                                            if (effect_follow_parent_position == false)
                                                EditorGUILayout.LabelField("Using WorldPos");
                                            else
                                                EditorGUILayout.LabelField("Using LocalPos");
                                            effect_follow_parent_position = EditorGUILayout.Toggle(effect_follow_parent_position);

                                            pos = EditorGUILayout.Vector3Field("Position", pos);

                                        }
                                        else if (view_effect_position_only == false && view_effect_timing_only == false && view_effect_all == false)

                                        {

                                            EditorGUILayout.BeginHorizontal();
                                            EditorGUILayout.LabelField("Parent Rotation Timing", GUILayout.ExpandWidth(false), GUILayout.Width(100));

                                            EditorGUILayout.FloatField(0, GUILayout.Width(20));
                                            if (Use_Frames_Move.boolValue == true)
                                            {

                                                effect_start_time_parent_rotation = EditorGUILayout.Slider(script.Entities[i].effects[j].effecttiming[k].effect_start_time_parent_rotation, 0, script.animation.length * script.animation.frameRate);

                                                EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                                //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);
                                            }
                                            else
                                            {
                                                effect_start_time_parent_rotation = EditorGUILayout.Slider(script.Entities[i].effects[j].effecttiming[k].effect_start_time_parent_rotation, 0, script.animation.length);

                                                EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                                //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);


                                            }
                                            //Debug.Log(timing);
                                            EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                                            EditorGUILayout.EndHorizontal();

                                            EditorGUILayout.BeginHorizontal();
                                            if (effect_follow_parent_rotation == false)
                                                EditorGUILayout.LabelField("WorldRotation");
                                            else
                                                EditorGUILayout.LabelField("LocalRotation");
                                            GUI.backgroundColor = color;
                                            effect_follow_parent_rotation = EditorGUILayout.Toggle(effect_follow_parent_rotation);
                                            EditorGUILayout.EndHorizontal();




                                            rot = EditorGUILayout.Vector4Field("Rotation", rot);
                                            rotation = new Quaternion(rot.x, rot.y, rot.z, rot.w);
                                        }
                                        else if (view_effect_position_only == false && view_effect_rotation_only == false && view_effect_all == false)
                                        {

                                            EditorGUILayout.BeginHorizontal();
                                            EditorGUILayout.LabelField("Start Timing", GUILayout.ExpandWidth(false), GUILayout.Width(100));

                                            EditorGUILayout.FloatField(0, GUILayout.Width(20));

                                            if (Use_Frames_Move.boolValue == true)
                                            {

                                                effect_start_time = EditorGUILayout.Slider(script.Entities[i].effects[j].effecttiming[k].effect_start_time, 0, script.animation.length * script.animation.frameRate);

                                                EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                                //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);
                                            }
                                            else
                                            {
                                                effect_start_time = EditorGUILayout.Slider(script.Entities[i].effects[j].effecttiming[k].effect_start_time, 0, script.animation.length);

                                                EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                                //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);


                                            }
                                            //Debug.Log(timing);
                                            EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                                            EditorGUILayout.EndHorizontal();

                                            EditorGUILayout.BeginHorizontal();
                                            EditorGUILayout.LabelField("Parent Position Timing", GUILayout.ExpandWidth(false), GUILayout.Width(100));

                                            EditorGUILayout.FloatField(0, GUILayout.Width(20));
                                            if (Use_Frames_Move.boolValue == true)
                                            {

                                                effect_start_time_parent_position = EditorGUILayout.Slider(script.Entities[i].effects[j].effecttiming[k].effect_start_time_parent_position, 0, script.animation.length * script.animation.frameRate);

                                                EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                                //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);
                                            }
                                            else
                                            {
                                                effect_start_time_parent_position = EditorGUILayout.Slider(script.Entities[i].effects[j].effecttiming[k].effect_start_time_parent_position, 0, script.animation.length);

                                                EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                                //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);


                                            }
                                            //Debug.Log(timing);
                                            EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                                            EditorGUILayout.EndHorizontal();

                                            EditorGUILayout.BeginHorizontal();
                                            EditorGUILayout.LabelField("Parent Rotation Timing", GUILayout.ExpandWidth(false), GUILayout.Width(100));

                                            EditorGUILayout.FloatField(0, GUILayout.Width(20));
                                            if (Use_Frames_Move.boolValue == true)
                                            {

                                                effect_start_time_parent_rotation = EditorGUILayout.Slider(script.Entities[i].effects[j].effecttiming[k].effect_start_time_parent_rotation, 0, script.animation.length * script.animation.frameRate);

                                                EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                                                //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);
                                            }
                                            else
                                            {
                                                effect_start_time_parent_rotation = EditorGUILayout.Slider(script.Entities[i].effects[j].effecttiming[k].effect_start_time_parent_rotation, 0, script.animation.length);

                                                EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                                                //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);


                                            }
                                            //Debug.Log(timing);
                                            EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                                            EditorGUILayout.EndHorizontal();
                                        }

                                        script.Entities[i].effects[j].effecttiming[k] = new EffectTiming(effect_start_time, start, effect_transform_parent, effect_start_time_parent_position, effect_follow_parent_position, effect_rotation_parent, effect_start_time_parent_rotation, effect_follow_parent_rotation, pos, rotation);
                                        GUI.backgroundColor = Color.grey;
                                        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                                    }
                                    GUI.backgroundColor = Color.gray;
                                    EditorGUILayout.BeginHorizontal();
                                    GUILayout.FlexibleSpace();
                                    if (GUILayout.Button("+", GUILayout.Width(30)))
                                    {
                                        effectcount[j] += 1;
                                    }
                                    if (GUILayout.Button("-", GUILayout.Width(30)))
                                    {
                                        effectcount[j] -= 1;
                                    }
                                    EditorGUILayout.EndHorizontal();

                                    EditorGUILayout.EndVertical();
                                    GUI.backgroundColor = color;
                                    GUI.contentColor = contcolor;
                                }


                                EditorGUILayout.Space(10);

                                //EditorGUILayout.PropertyField(LeftFootIK);
                            }
                        }
                    }
                }

            }
        }
        EditorGUILayout.EndVertical();
        #endregion
    }



    void MoveToPos()
    {
        var centerStyle4 = new GUIStyle(GUI.skin.GetStyle("Label"));
        centerStyle4.alignment = TextAnchor.MiddleRight;
        centerStyle4.fontStyle = FontStyle.Bold;
        centerStyle4.fontSize = 15; Combo_scriptable script = (Combo_scriptable)target;

        #region Move
        EditorGUILayout.BeginHorizontal();
        foldoutM = EditorGUILayout.Foldout(foldoutM, new GUIContent("Movement Pos"));
        GUILayout.FlexibleSpace();

        movementcount = EditorGUILayout.IntField(movementcount, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10);
        if (script.move_to_pos.Count < movementcount)
        {
            script.move_to_pos.Add(new MovementTiming(Vector3.zero, 0, 0, 0, 0, false, 0,false));
        }
        else if (script.move_to_pos.Count > movementcount)
        {
            script.move_to_pos.RemoveAt(script.move_to_pos.Count - 1);
        }
        else if (movementcount == 0)
        {
            script.move_to_pos.Clear();
        }
        if (foldoutM)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Toggle(view_speed_only, "View Speed Only", "Button"))
            {
                view_speed_only = true;
                view_position_only = false;
                view_timing_move_only = false;
                cancel_move = false;

            }
            if (GUILayout.Toggle(view_timing_move_only, "View Timing Only", "Button"))
            {
                view_timing_move_only = true;
                view_position_only = false;
                view_speed_only = false;
                cancel_move = false;
            }
            if (GUILayout.Toggle(view_position_only, "View Position Only", "Button"))
            {
                view_position_only = true;
                view_timing_move_only = false;
                view_speed_only = false;
                cancel_move = false;
            }
            if (GUILayout.Toggle(cancel_move, "View All", "Button"))
            {
                cancel_move = true;
                view_position_only = false;
                view_timing_move_only = false;
                view_speed_only = false;
            }



            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Toggle(script.movemode == Combo_scriptable.MoveMode.Speed, "Use Speed", "Button"))
            {
                script.movemode = Combo_scriptable.MoveMode.Speed;

            }
            if (GUILayout.Toggle(script.movemode == Combo_scriptable.MoveMode.Direction, "Use Direction", "Button"))
            {
                script.movemode = Combo_scriptable.MoveMode.Direction;
                

            }
            if (GUILayout.Toggle(script.movemode == Combo_scriptable.MoveMode.Position, "Use Position", "Button"))
            {
                script.movemode = Combo_scriptable.MoveMode.Position;
                
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);
            if (script.movemode == Combo_scriptable.MoveMode.Speed)
            {
                EditorGUILayout.PropertyField(velocityadd, new GUIContent("velocity Build Up", "builds up velocity/speed"));
                EditorGUILayout.PropertyField(staticvelocity, new GUIContent("Static Velocity", "Keeps the velocity static/instatnt preventing interpolation"));
            }
            else if (script.movemode == Combo_scriptable.MoveMode.Direction)
            {
                EditorGUILayout.PropertyField(globalDirectPos, new GUIContent("Use Global", "use global direction or local"));
                EditorGUILayout.PropertyField(velocityadd, new GUIContent("Direction Build Up", "builds up direction can cause looping 360"));
                EditorGUILayout.PropertyField(staticvelocity, new GUIContent("Static Direction", "Keeps the direction static/instatnt preventing interpolation"));
            }
            else if (script.movemode == Combo_scriptable.MoveMode.Position)
            {
                EditorGUILayout.PropertyField(globalDirectPos, new GUIContent("Use Global", "use global position or local"));
                EditorGUILayout.PropertyField(velocityadd, new GUIContent("Position Extending", "builds up position vector extending the main vector"));
                EditorGUILayout.PropertyField(staticvelocity, new GUIContent("Static Position", "Keeps the position static/instatnt preventing interpolation"));
            }

            EditorGUILayout.Space(10);
            //EditorGUILayout.PropertyField(LeftFootIK);
            if (script.move_to_pos.Count == movementcount)
            {
                var screenrect2 = GUILayoutUtility.GetRect(1, 1);
                var vertrect2 = EditorGUILayout.BeginVertical();
                EditorGUI.DrawRect(new Rect(screenrect2.x - 13, screenrect2.y - 1, screenrect2.width + 17, vertrect2.height + 9), new Color(0.5f, 0.5f, 0.5f, 0.5f));
                Color color = GUI.backgroundColor;
                Color contcolor = GUI.contentColor;
                for (int i = 0; i < movementcount; i++)
                {
                    //GUI.backgroundColor = new Color(0,0,0,0.5f);
                    GUI.contentColor = Color.white;
                    GUI.backgroundColor = color;
                    GUI.backgroundColor = new Color(0, 0, 0, 0.5f);
                    float speedtiming = script.move_to_pos[i].speedtiming;
                    float horizontalspeed = script.move_to_pos[i].horizontalspeed;
                    float verticalspeed = script.move_to_pos[i].verticalspeed;
                    float directiontiming = script.move_to_pos[i].Directiontiming;
                    bool animonly = script.move_to_pos[i].allowmovement;
                    float directionchangespeed = script.move_to_pos[i].directionchangespeed;
                    bool faceDirection= script.move_to_pos[i].faceDirection;
                   
                    Vector3 pos = script.move_to_pos[i].Direction;

                    if (cancel_move)
                    {

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Speed Timing", GUILayout.ExpandWidth(false), GUILayout.Width(100));

                        EditorGUILayout.FloatField(0, GUILayout.Width(20));
                        if (Use_Frames_Move.boolValue == true)
                        {

                            speedtiming = EditorGUILayout.Slider(script.move_to_pos[i].speedtiming, 0, script.animation.length * script.animation.frameRate);

                            EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                            //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);
                        }
                        else
                        {
                            speedtiming = EditorGUILayout.Slider(script.move_to_pos[i].speedtiming, 0, script.animation.length);

                            EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                            //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);


                        }
                        //Debug.Log(timing);
                        EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Direction Timing", GUILayout.ExpandWidth(false), GUILayout.Width(100));

                        EditorGUILayout.FloatField(0, GUILayout.Width(20));
                        if (Use_Frames_Move.boolValue == true)
                        {

                            directiontiming = EditorGUILayout.Slider(script.move_to_pos[i].Directiontiming, 0, script.animation.length * script.animation.frameRate);

                            EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                            //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);
                        }
                        else
                        {
                            directiontiming = EditorGUILayout.Slider(script.move_to_pos[i].Directiontiming, 0, script.animation.length);

                            EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                            //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);


                        }
                        //Debug.Log(timing);
                        EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        if (animonly == false)
                            EditorGUILayout.LabelField("Moving Object");
                        else
                            EditorGUILayout.LabelField("Animation Only");
                        GUI.backgroundColor = color;
                        animonly = EditorGUILayout.Toggle(animonly);
                        EditorGUILayout.EndHorizontal();
                        




                        if (script.movemode == Combo_scriptable.MoveMode.Speed)
                        {
                            horizontalspeed = EditorGUILayout.FloatField("Horizontal Speed", script.move_to_pos[i].horizontalspeed);
                            verticalspeed = EditorGUILayout.FloatField("Vertical Speed", script.move_to_pos[i].verticalspeed);
                        }
                        else if(script.movemode == Combo_scriptable.MoveMode.Direction)
                        {
                            horizontalspeed = EditorGUILayout.FloatField("Speed", script.move_to_pos[i].horizontalspeed);
                            directionchangespeed = EditorGUILayout.FloatField("Direction change speed multiplier", directionchangespeed);                            
                                pos = EditorGUILayout.Vector3Field("Direction", pos);
                            
                        }
                        else if(script.movemode == Combo_scriptable.MoveMode.Position)
                        {
                            horizontalspeed = EditorGUILayout.FloatField("Speed Multiplier", script.move_to_pos[i].horizontalspeed);
                            pos = EditorGUILayout.Vector3Field("Position", pos);
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("Face Direction");

                            GUI.backgroundColor = color;
                            faceDirection = EditorGUILayout.Toggle(faceDirection);
                            EditorGUILayout.EndHorizontal();
                            
                        }
                        /*
                        EditorGUILayout.BeginHorizontal();
                        if (isdirection == combo)
                            EditorGUILayout.LabelField("Using Speed");
                        else
                            EditorGUILayout.LabelField("Using Direction");
                        GUI.backgroundColor = color;
                        isdirection = EditorGUILayout.Toggle(isdirection);
                        EditorGUILayout.EndHorizontal();
                        */


                            /*
                            if (isdirection == false)
                                pos = EditorGUILayout.Vector3Field("Direction", pos);
                            else
                                pos = EditorGUILayout.Vector3Field("Position", pos);
                            */





                    }
                    else if (view_position_only == false && view_speed_only == false)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Speed Timing", GUILayout.ExpandWidth(false), GUILayout.Width(100));

                        EditorGUILayout.FloatField(0, GUILayout.Width(20));
                        if (Use_Frames_Move.boolValue == true)
                        {

                            speedtiming = EditorGUILayout.Slider(script.move_to_pos[i].speedtiming, 0, script.animation.length * script.animation.frameRate);

                            EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                            //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);
                        }
                        else
                        {
                            speedtiming = EditorGUILayout.Slider(script.move_to_pos[i].speedtiming, 0, script.animation.length);

                            EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                            //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);


                        }
                        //Debug.Log(timing);
                        EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Direction Timing", GUILayout.ExpandWidth(false), GUILayout.Width(100));

                        EditorGUILayout.FloatField(0, GUILayout.Width(20));
                        if (Use_Frames_Move.boolValue == true)
                        {

                            directiontiming = EditorGUILayout.Slider(script.move_to_pos[i].Directiontiming, 0, script.animation.length * script.animation.frameRate);

                            EditorGUILayout.FloatField(script.animation.length * script.animation.frameRate, GUILayout.Width(80));

                            //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);
                        }
                        else
                        {
                            directiontiming = EditorGUILayout.Slider(script.move_to_pos[i].Directiontiming, 0, script.animation.length);

                            EditorGUILayout.FloatField(script.animation.length, GUILayout.Width(80));

                            //script.move_to_pos[i] = new MovementTiming(Vector3.zero, 0, false);


                        }
                        //Debug.Log(timing);
                        EditorGUILayout.LabelField("E", centerStyle4, GUILayout.ExpandWidth(false), GUILayout.Width(15));

                        EditorGUILayout.EndHorizontal();
                    }
                    else if (view_speed_only == false && view_timing_move_only == false)

                    {

                        GUI.backgroundColor = new Color(0, 0, 0, 0.5f);
                        EditorGUILayout.BeginHorizontal();
                        if (animonly == false)
                            EditorGUILayout.LabelField("Move Object");
                        else
                            EditorGUILayout.LabelField("Animation Only");
                        GUI.backgroundColor = color;
                        animonly = EditorGUILayout.Toggle(animonly);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Face Direction");

                        GUI.backgroundColor = color;
                        faceDirection = EditorGUILayout.Toggle(faceDirection);
                        EditorGUILayout.EndHorizontal();
                        /*
                                                EditorGUILayout.BeginHorizontal();

                                                if (isdirection == false)
                                                    EditorGUILayout.LabelField("Using Speed");
                                                else
                                                    EditorGUILayout.LabelField("Using Direction");
                                                GUI.backgroundColor = color;
                                                isdirection = EditorGUILayout.Toggle(isdirection);
                                                EditorGUILayout.EndHorizontal();
                                                */


                        if (script.movemode == Combo_scriptable.MoveMode.Speed)
                        {
                            horizontalspeed = EditorGUILayout.FloatField("Horizontal Speed", script.move_to_pos[i].horizontalspeed);
                            verticalspeed = EditorGUILayout.FloatField("Vertical Speed", script.move_to_pos[i].verticalspeed);
                        }
                        else if (script.movemode == Combo_scriptable.MoveMode.Direction)
                        {                          
                                pos = EditorGUILayout.Vector3Field("Direction", pos);
                           

                        }
                        else if (script.movemode == Combo_scriptable.MoveMode.Position)
                        {
                            pos = EditorGUILayout.Vector3Field("Position", pos);
                            
                        }

                        /*
                        if (isdirection == false)
                            pos = EditorGUILayout.Vector3Field("Direction", pos);
                        else
                            pos = EditorGUILayout.Vector3Field("Position", pos);
                        */


                    }
                    else if (view_timing_move_only == false && view_position_only == false)
                    {
                        if (script.movemode == Combo_scriptable.MoveMode.Speed)
                        {
                            horizontalspeed = EditorGUILayout.FloatField("Horizontal Speed", script.move_to_pos[i].horizontalspeed);
                            verticalspeed = EditorGUILayout.FloatField("Vertical Speed", script.move_to_pos[i].verticalspeed);
                        }
                        else if (script.movemode == Combo_scriptable.MoveMode.Direction)
                        {
                            horizontalspeed = EditorGUILayout.FloatField("Speed", script.move_to_pos[i].horizontalspeed);
                            directionchangespeed = EditorGUILayout.FloatField("Direction change speed multiplier", directionchangespeed);

                            pos = EditorGUILayout.Vector3Field("Direction", pos);

                        }
                        else if (script.movemode == Combo_scriptable.MoveMode.Position)
                        {
                            horizontalspeed = EditorGUILayout.FloatField("Horizontal Speed Multiplier", script.move_to_pos[i].horizontalspeed);
                            pos = EditorGUILayout.Vector3Field("Position", pos);
                        }
                        
                    }

                    script.move_to_pos[i] = new MovementTiming(pos, directiontiming, speedtiming, horizontalspeed, verticalspeed, animonly, directionchangespeed,faceDirection);
                    GUI.backgroundColor = Color.grey;
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                }
                GUI.backgroundColor = Color.gray;
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("+", GUILayout.Width(30)))
                {
                    movementcount += 1;
                }
                if (GUILayout.Button("-", GUILayout.Width(30)))
                {
                    movementcount -= 1;
                }
                EditorGUILayout.EndHorizontal();
                if (view_speed_only)
                {
                    EditorGUILayout.LabelField("Horizontal Preview");
                    AnimationCurve curve = new AnimationCurve();
                    /*
                    if (acceleration.boolValue == true)
                    {
                        curve.AddKey(0, script.move_start_speed);
                        curve.AddKey(1, script.acceleration_speed);
                        EditorGUILayout.PropertyField(move_start_speed, new GUIContent("Start Speed", "Instant Power Up or with Delay"));
                        EditorGUILayout.PropertyField(acceleration_speed, new GUIContent("Acceleration", "Will the Value drop when not held?"));
                        EditorGUILayout.PropertyField(maxspeed, new GUIContent("maxspeed", "max speed to reach"));
                                           }
                    else
                    {
                        curve.AddKey(2, script.deacceleration_speed);
                        curve.AddKey(3, script.move_end_speed);
                        EditorGUILayout.PropertyField(deacceleration_speed, new GUIContent("Deacceleration", "Will the Value drop when not held?"));
                        EditorGUILayout.PropertyField(move_end_speed, new GUIContent("End Speed", "Drop speed of power"));

                    }
                    */

                    for (int i = 0; i < script.move_to_pos.Count; i++)
                    {
                        curve.AddKey(i, script.move_to_pos[i].horizontalspeed);
                    }
                    EditorGUILayout.CurveField(curve, GUILayout.Height(200));
                    EditorGUILayout.LabelField("Vertical Preview");
                    AnimationCurve curve2 = new AnimationCurve();
                    /*
                    if (acceleration.boolValue == true)
                    {
                        curve.AddKey(0, script.move_start_speed);
                        curve.AddKey(1, script.acceleration_speed);
                        EditorGUILayout.PropertyField(move_start_speed, new GUIContent("Start Speed", "Instant Power Up or with Delay"));
                        EditorGUILayout.PropertyField(acceleration_speed, new GUIContent("Acceleration", "Will the Value drop when not held?"));
                        EditorGUILayout.PropertyField(maxspeed, new GUIContent("maxspeed", "max speed to reach"));
                                           }
                    else
                    {
                        curve.AddKey(2, script.deacceleration_speed);
                        curve.AddKey(3, script.move_end_speed);
                        EditorGUILayout.PropertyField(deacceleration_speed, new GUIContent("Deacceleration", "Will the Value drop when not held?"));
                        EditorGUILayout.PropertyField(move_end_speed, new GUIContent("End Speed", "Drop speed of power"));

                    }
                    */

                    for (int i = 0; i < script.move_to_pos.Count; i++)
                    {
                        curve2.AddKey(i, script.move_to_pos[i].verticalspeed);
                    }
                    EditorGUILayout.CurveField(curve2, GUILayout.Height(200));

                }

                EditorGUILayout.EndVertical();
                GUI.backgroundColor = color;
                GUI.contentColor = contcolor;
            }
        }
        #endregion
    }

    public void Summon()
    {
        Combo_scriptable script = (Combo_scriptable)target;
        EditorGUILayout.BeginHorizontal();
        dropdownsummon = EditorGUILayout.Foldout(dropdownsummon, new GUIContent("Object Instancing"));
        GUILayout.FlexibleSpace();

        summoncount = EditorGUILayout.IntField(summoncount, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();
        if (dropdownsummon)
        {
            if (script.summons.Count < summoncount)
            {
                script.summons.Add(new SummonTiming());

            }
            else if (script.summons.Count > summoncount)
            {
                //var last = script.floatvalues.Last();
                script.summons.RemoveAt(script.summons.Count - 1);
            }
            else if (0 == summoncount)
            {
                script.summons.Clear();
            }
            

            for (int i = 0; i < script.summons.Count; i++)
            {
                 float time=script.summons[i].time;
     ObjectKnowledge summon = script.summons[i].summon;
     string RelativePath = script.summons[i].RelativePath;
     string parent = script.summons[i].parent;
                float maxfps = script.animation.length;
                if((script.Use_Frames&&!(script.movetype==Combo_scriptable.MoveType.Movement))||(script.Use_Frames_Move&& script.movetype == Combo_scriptable.MoveType.Movement))
                {
                    maxfps *= script.animation.length;
                }
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("S", GUILayout.Width(20));
                EditorGUILayout.FloatField(0, GUILayout.Width(50));
                time = EditorGUILayout.Slider(time, 0, maxfps);
                EditorGUILayout.LabelField("E", GUILayout.Width(20));
                EditorGUILayout.FloatField(maxfps, GUILayout.Width(80));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Object Summoning", GUILayout.Width(150));
                summon = (ObjectKnowledge)EditorGUILayout.ObjectField(summon, typeof(ObjectKnowledge), true);
                EditorGUILayout.EndHorizontal();
                RelativePath=EditorGUILayout.TextField("Path Spawner", RelativePath);
                parent=EditorGUILayout.TextField("Parent Path", parent);
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                script.summons[i] = new SummonTiming(time, summon, RelativePath,parent);
            }
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Spawn", GUILayout.Width(110)))
            {

                summoncount += 1;
            }
            if (GUILayout.Button("Remove Spawn", GUILayout.Width(150)) && summoncount > 0)
            {
                summoncount -= 1;
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    public void CommandLine()
    {

        Combo_scriptable script = (Combo_scriptable)target;
        EditorGUILayout.BeginHorizontal();
        dropdowncommand = EditorGUILayout.Foldout(dropdowncommand, new GUIContent("Commands"));
        GUILayout.FlexibleSpace();

        targettypecommand = EditorGUILayout.IntField(targettypecommand, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();
        if (dropdowncommand)
        {
            if (script.commands.Count < targettypecommand)
            {
                script.commands.Add(new CommandValue());

            }
            else if (script.commands.Count > targettypecommand)
            {
                //var last = script.floatvalues.Last();
                script.commands.RemoveAt(script.commands.Count - 1);
            }
            else if (0 == targettypecommand)
            {
                script.commands.Clear();
            }
            if (script.commandtime.Count < targettypecommand)
            {
                script.commandtime.Add(0);

            }
            else if (script.commandtime.Count > targettypecommand)
            {
                //var last = script.floatvalues.Last();
                script.commandtime.RemoveAt(script.commandtime.Count - 1);
            }
            else if (0 == targettypecommand)
            {
                script.commandtime.Clear();
            }

            if (script.commands.Count < targettypecommand)
            {
                script.commands.Add(new CommandValue());

            }
            else if (script.commands.Count > targettypecommand)
            {
                //var last = script.floatvalues.Last();
                script.commands.RemoveAt(script.commands.Count - 1);
            }
            else if (0 == targettypecommand)
            {
                script.commands.Clear();
            }

            if (script.commands.Count > dropdowncommandvalues.Count)
            {
                dropdowncommandvalues.Add(false);

            }
            else if (script.commands.Count < dropdowncommandvalues.Count)
            {
                //var last = script.floatvalues.Last();
                dropdowncommandvalues.RemoveAt(dropdowncommandvalues.Count - 1);
            }
            else if (0 == script.commands.Count)
            {
                dropdowncommandvalues.Clear();
            }
            


            for (int i = 0; i < script.commands.Count; i++)
            {
                float maxfps = script.animation.length;
                if ((script.Use_Frames && !(script.movetype == Combo_scriptable.MoveType.Movement)) || (script.Use_Frames_Move && script.movetype == Combo_scriptable.MoveType.Movement))
                {
                    maxfps *= script.animation.length;
                }
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("S", GUILayout.Width(20));
                EditorGUILayout.FloatField(0, GUILayout.Width(50));
                script.commandtime[i] = EditorGUILayout.Slider(script.commandtime[i], 0, maxfps);
                EditorGUILayout.LabelField("E", GUILayout.Width(20));
                EditorGUILayout.FloatField(maxfps, GUILayout.Width(80));
                EditorGUILayout.EndHorizontal();

                List<string> TargetClasses = script.commands[i].TargetClasses;
                ObjectKnowledge.ObjectType TargetTypes = script.commands[i].TargetTypes;
                float radiustarget = script.commands[i].radiustarget;
                ObjectKnowledge.InteractionMode Interaction = script.commands[i].Interaction;

                string Activator = script.commands[i].Activator;
                string Deactivator = script.commands[i].Deactivator;
                bool Continuous = script.commands[i].Continuous;
                string Types2 = script.commands[i].Type;
                string Method = script.commands[i].Method;
                string Value = script.commands[i].Value;
                List<string> stringparameter = script.commands[i].stringparameter;
                List<float> floatparameter = script.commands[i].floatparameter;
                List<bool> boolparameter = script.commands[i].boolparameter;
                List<Vector3> vectorparameter = script.commands[i].vectorparameter;
                List<Quaternion> quaternionparameter = script.commands[i].quaternionparameter;

                List<string> methodparameters = script.commands[i].methodparameters;
                ObjectKnowledge.Functionfloat methodparameterfunctions = script.commands[i].methodparameterfunctions;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Type", GUILayout.Width(40));

                TargetTypes = (ObjectKnowledge.ObjectType)EditorGUILayout.EnumPopup(TargetTypes);
                EditorGUILayout.LabelField("Interaction", GUILayout.Width(80));

                Interaction = (ObjectKnowledge.InteractionMode)EditorGUILayout.EnumPopup(Interaction);

                EditorGUILayout.EndHorizontal();
                if (TargetClasses == null)
                {
                    TargetClasses = new List<string>();
                }
                if (stringparameter == null)
                {
                    stringparameter = new List<string>();
                }
                if (floatparameter == null)
                {
                    floatparameter = new List<float>();
                }
                if (boolparameter == null)
                {
                    boolparameter = new List<bool>();
                }
                if (vectorparameter == null)
                {
                    vectorparameter = new List<Vector3>();
                }
                if (quaternionparameter == null)
                {
                    quaternionparameter = new List<Quaternion>();
                }

                if (methodparameters == null)
                {
                    methodparameters = new List<string>();
                }

                for (int j = 0; j < TargetClasses.Count; j++)
                {
                    TargetClasses[j] = EditorGUILayout.TextField("Class Target", TargetClasses[j]);

                }
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Add Class", GUILayout.Width(80)))
                {
                    TargetClasses.Add("");

                }
                if (GUILayout.Button("Remove Class", GUILayout.Width(100)) && script.commands[i].TargetClasses.Count > 0)
                {
                    TargetClasses.RemoveAt(TargetClasses.Count - 1);

                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Activator", GUILayout.Width(70));

                Activator = EditorGUILayout.TextField(Activator);
                EditorGUILayout.LabelField("Deactivator", GUILayout.Width(70));

                Deactivator = EditorGUILayout.TextField(Deactivator);
                EditorGUILayout.LabelField("Continuous", GUILayout.Width(80));

                Continuous = EditorGUILayout.Toggle(Continuous);

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("Type", GUILayout.Width(50));

                Types2 = EditorGUILayout.TextField(Types2);
                EditorGUILayout.LabelField("Method", GUILayout.Width(70));

                Method = EditorGUILayout.TextField(Method);
                EditorGUILayout.LabelField("Value", GUILayout.Width(60));

                Value = EditorGUILayout.TextField(Value);

                EditorGUILayout.EndHorizontal();




                EditorGUILayout.Space(20);
                dropdowncommandvalues[i] = EditorGUILayout.Foldout(dropdowncommandvalues[i], new GUIContent("Values"));
                if (dropdowncommandvalues[i])
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Origin Path", GUILayout.Width(70));

                    script.transformpath[i] = EditorGUILayout.TextField(script.transformpath[i]);
                   
                    EditorGUILayout.EndHorizontal();


                    EditorGUILayout.LabelField("Strings:");
                    for (int o = 0; o < stringparameter.Count; o++)
                    {
                        stringparameter[o] = EditorGUILayout.TextField(stringparameter[o]);
                    }
                    EditorGUILayout.LabelField("Floats:");
                    for (int o = 0; o < floatparameter.Count; o++)
                    {
                        floatparameter[o] = EditorGUILayout.FloatField(floatparameter[o]);
                    }
                    EditorGUILayout.LabelField("Booleans:");
                    for (int o = 0; o < boolparameter.Count; o++)
                    {
                        boolparameter[o] = EditorGUILayout.Toggle(boolparameter[o]);
                    }
                    EditorGUILayout.LabelField("Vectors:");
                    for (int o = 0; o < vectorparameter.Count; o++)
                    {
                        vectorparameter[o] = EditorGUILayout.Vector3Field(GUIContent.none, vectorparameter[o]);
                    }
                    EditorGUILayout.LabelField("Quaternions:");
                    for (int o = 0; o < quaternionparameter.Count; o++)
                    {
                        Vector4 vect = new Vector4(quaternionparameter[o].x, quaternionparameter[o].y, quaternionparameter[o].z, quaternionparameter[o].w);
                        vect = EditorGUILayout.Vector4Field(GUIContent.none, vect);
                        quaternionparameter[o] = new Quaternion(vect.x, vect.y, vect.z, vect.w);
                    }
                    EditorGUILayout.LabelField("Methods:");
                    for (int o = 0; o < methodparameters.Count; o++)
                    {
                        methodparameters[o] = EditorGUILayout.TextField(methodparameters[o]);
                    }



                    #region Buttons
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Add String", GUILayout.Width(130)))
                    {

                        stringparameter.Add("");
                    }
                    if (GUILayout.Button("Remove String", GUILayout.Width(130)) && stringparameter.Count > 0)
                    {
                        stringparameter.RemoveAt(stringparameter.Count - 1);
                    }

                    if (GUILayout.Button("Add Float", GUILayout.Width(130)))
                    {

                        floatparameter.Add(0);
                    }
                    if (GUILayout.Button("Remove Float", GUILayout.Width(130)) && floatparameter.Count > 0)
                    {
                        floatparameter.RemoveAt(floatparameter.Count - 1);
                    }

                    if (GUILayout.Button("Add Bool", GUILayout.Width(130)))
                    {

                        boolparameter.Add(false);
                    }
                    if (GUILayout.Button("Remove Bool", GUILayout.Width(130)) && boolparameter.Count > 0)
                    {
                        boolparameter.RemoveAt(boolparameter.Count - 1);
                    }

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Add Vector", GUILayout.Width(130)))
                    {

                        vectorparameter.Add(new Vector3(0, 0, 0));
                    }
                    if (GUILayout.Button("Remove Vector", GUILayout.Width(130)) && vectorparameter.Count > 0)
                    {
                        vectorparameter.RemoveAt(vectorparameter.Count - 1);
                    }

                    if (GUILayout.Button("Add Quaternion", GUILayout.Width(130)))
                    {

                        quaternionparameter.Add(new Quaternion());
                    }
                    if (GUILayout.Button("Remove Quaternion", GUILayout.Width(130)) && quaternionparameter.Count > 0)
                    {
                        quaternionparameter.RemoveAt(quaternionparameter.Count - 1);
                    }

                    if (GUILayout.Button("Add Method", GUILayout.Width(130)))
                    {

                        methodparameters.Add("");
                    }
                    if (GUILayout.Button("Remove Method", GUILayout.Width(130)) && methodparameters.Count > 0)
                    {
                        methodparameters.RemoveAt(methodparameters.Count - 1);
                    }

                    EditorGUILayout.EndHorizontal();
                    #endregion


                }
                script.commands[i] = new CommandValue(Activator, Deactivator, Continuous, TargetClasses, TargetTypes, radiustarget, Interaction, Types2, Method, Value, stringparameter, floatparameter, boolparameter, vectorparameter, quaternionparameter, methodparameters, methodparameterfunctions);


            }
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Command", GUILayout.Width(110)))
            {
               
                    script.transformpath.Add("");

                
                targettypecommand += 1;
            }
            if (GUILayout.Button("Remove Command", GUILayout.Width(150)) && targettypecommand > 0)
            {
                targettypecommand -= 1;
                script.transformpath.RemoveAt(script.transformpath.Count - 1);

            }
            EditorGUILayout.EndHorizontal();

        }
    } 
    public void CameraTiming()
    {
        Combo_scriptable script = (Combo_scriptable)target;
        EditorGUILayout.BeginHorizontal();
        dropdowncam = EditorGUILayout.Foldout(dropdowncam, new GUIContent("Vectors"));
        GUILayout.FlexibleSpace();

        EditorGUILayout.IntField(script.camtime.Count, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();
        if(dropdowncam)
        {
            for (int i = 0; i < script.camtime.Count; i++)
            {
                Vector3 CamPos=script.camtime[i].CamPos;
     Quaternion CamRot = script.camtime[i].CamRot;
     float Time = script.camtime[i].Time;
     float weight = script.camtime[i].weight;

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Position", GUILayout.Width(80));
                CamPos = EditorGUILayout.Vector3Field(GUIContent.none,CamPos);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Rotation", GUILayout.Width(80));

                Vector4 val = new Vector4(CamRot.x, CamRot.y, CamRot.z, CamRot.w);
                    val= EditorGUILayout.Vector4Field(GUIContent.none, val);
                CamRot = new Quaternion(val.x, val.y, val.z, val.w);
                EditorGUILayout.EndHorizontal();

                

                float maxfps = script.animation.length;
                if ((script.Use_Frames && !(script.movetype == Combo_scriptable.MoveType.Movement)) || (script.Use_Frames_Move && script.movetype == Combo_scriptable.MoveType.Movement))
                {
                    maxfps *= script.animation.length;
                }
                EditorGUILayout.LabelField("Timing:");

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("S", GUILayout.Width(20));
                EditorGUILayout.FloatField(0, GUILayout.Width(50));
                Time = EditorGUILayout.Slider(Time, 0, maxfps);
                EditorGUILayout.LabelField("E", GUILayout.Width(20));
                EditorGUILayout.FloatField(maxfps, GUILayout.Width(80));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Weight:");
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("S", GUILayout.Width(20));
                EditorGUILayout.FloatField(0, GUILayout.Width(50));
                weight = EditorGUILayout.Slider(weight, 0, 1);
                EditorGUILayout.LabelField("E", GUILayout.Width(20));
                EditorGUILayout.FloatField(maxfps, GUILayout.Width(80));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                script.camtime[i] = new CameraTiming(CamPos, CamRot, Time,weight);
            }
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Smoothing", GUILayout.Width(90));

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Start", GUILayout.Width(50));
            startsmooth=EditorGUILayout.IntField(GUIContent.none,startsmooth,GUILayout.Width(50));

            EditorGUILayout.LabelField("End", GUILayout.Width(50));
            endsmooth = EditorGUILayout.IntField(GUIContent.none, endsmooth, GUILayout.Width(50));
            EditorGUILayout.LabelField("Smoothness", GUILayout.Width(80));
            smoothnesscam = EditorGUILayout.FloatField(GUIContent.none, smoothnesscam, GUILayout.Width(100));
            if(startsmooth<0)
            {
                startsmooth = 0;
            }
            if(endsmooth>script.camtime.Count-1&&script.camtime.Count>0)
            {
                endsmooth = script.camtime.Count-1;
            }
             if(script.camtime.Count==0)
            {
                endsmooth = 0;
                startsmooth = 0;

            }
            if (GUILayout.Button("Smoothen"))
            {
                Debug.Log("processing");

                if (startsmooth!=endsmooth&&startsmooth<endsmooth)
                {
                    Vector3[] smoothened = new Vector3[(endsmooth - startsmooth)+1];
                    Vector4[] smoothenedrot = new Vector4[(endsmooth - startsmooth)+1];
                    float[] smoothenedfloat = new float[(endsmooth - startsmooth)+1];
                    float[] smoothenedweight = new float[(endsmooth - startsmooth)+1];
                    int indexer = 0;
                    Debug.Log("Inserting Initial");

                    for (int i = startsmooth; i < endsmooth+1; i++)
                    {
                        smoothened[indexer] = script.camtime[i].CamPos;
                        smoothenedrot[indexer] = new Vector4(script.camtime[i].CamRot.x,script.camtime[i].CamRot.y,script.camtime[i].CamRot.z,script.camtime[i].CamRot.w);
                        smoothenedfloat[indexer] = script.camtime[i].Time;
                        smoothenedweight[indexer] = script.camtime[i].weight;
                        indexer++;
                        Debug.Log(script.camtime[i].CamPos+" / "+ script.camtime[i].Time);
                         
                    }
                    Debug.Log("Finalizing");

                    smoothened = MakeSmoothCurve(smoothened, smoothnesscam);
                    smoothenedrot = MakeSmoothCurve(smoothenedrot, smoothnesscam);
                    smoothenedfloat = MakeSmoothCurve(smoothenedfloat, smoothnesscam);
                    smoothenedweight = MakeSmoothCurve(smoothenedweight, smoothnesscam);
                    int index2 = 0;
                    List<CameraTiming> camtime2 = new List<CameraTiming>();
                    for (int i = 0; i < script.camtime.Count; i++)
                    {
                        if(i==startsmooth)
                        {
                            for (int j = 0; j < smoothened.Length; j++)
                            {
                                camtime2.Add(new global::CameraTiming(smoothened[j], new Quaternion(smoothenedrot[j].x, smoothenedrot[j].y, smoothenedrot[j].z, smoothenedrot[j].w), smoothenedfloat[j], smoothenedweight[j]));
                            }
                            continue;
                        }
                        else if(i>=startsmooth&&i <= endsmooth)
                        {
                            continue;
                        }
                        camtime2.Add(script.camtime[i]);
                    }
                    Debug.Log("Output:");
                    {
                        for (int i = 0; i < camtime2.Count; i++)
                        {
                            Debug.Log(camtime2[i].CamPos);
                        }
                    }
                    script.camtime = camtime2; 
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Add Position", GUILayout.Width(110)))
            {

                script.camtime.Add(new global::CameraTiming());


            }
            if (GUILayout.Button("Remove Position", GUILayout.Width(150)) && script.camtime.Count > 0)
            {
                script.camtime.RemoveAt(script.camtime.Count - 1);

            }
            EditorGUILayout.EndHorizontal();

        }
    }
    public static Vector3[] MakeSmoothCurve(Vector3[] arrayToCurve, float smoothness)
    {
        List<Vector3> points;
        List<Vector3> curvedPoints;
        int pointsLength = 0;
        int curvedLength = 0;

        if (smoothness < 1.0f) smoothness = 1.0f;

        pointsLength = arrayToCurve.Length;

        curvedLength = (pointsLength * Mathf.RoundToInt(smoothness)) - 1;
        curvedPoints = new List<Vector3>(curvedLength);

        float t = 0.0f;
        for (int pointInTimeOnCurve = 0; pointInTimeOnCurve < curvedLength + 1; pointInTimeOnCurve++)
        {
            t = Mathf.InverseLerp(0, curvedLength, pointInTimeOnCurve);

            points = new List<Vector3>(arrayToCurve);

            for (int j = pointsLength - 1; j > 0; j--)
            {
                for (int i = 0; i < j; i++)
                {
                    points[i] = (1 - t) * points[i] + t * points[i + 1];
                }
            }

            curvedPoints.Add(points[0]);
        }

        return (curvedPoints.ToArray());
    }
    public static float[] MakeSmoothCurve(float[] arrayToCurve, float smoothness)
    {
        List<float> points;
        List<float> curvedPoints;
        int pointsLength = 0;
        int curvedLength = 0;

        if (smoothness < 1.0f) smoothness = 1.0f;

        pointsLength = arrayToCurve.Length;

        curvedLength = (pointsLength * Mathf.RoundToInt(smoothness)) - 1;
        curvedPoints = new List<float>(curvedLength);

        float t = 0.0f;
        for (int pointInTimeOnCurve = 0; pointInTimeOnCurve < curvedLength + 1; pointInTimeOnCurve++)
        {
            t = Mathf.InverseLerp(0, curvedLength, pointInTimeOnCurve);

            points = new List<float>(arrayToCurve);

            for (int j = pointsLength - 1; j > 0; j--)
            {
                for (int i = 0; i < j; i++)
                {
                    points[i] = (1 - t) * points[i] + t * points[i + 1];
                }
            }

            curvedPoints.Add(points[0]);
        }

        return (curvedPoints.ToArray());
    }
    public static Vector4[] MakeSmoothCurve(Vector4[] arrayToCurve, float smoothness)
    {
        List<Vector4> points;
        List<Vector4> curvedPoints;
        int pointsLength = 0;
        int curvedLength = 0;

        if (smoothness < 1.0f) smoothness = 1.0f;

        pointsLength = arrayToCurve.Length;

        curvedLength = (pointsLength * Mathf.RoundToInt(smoothness)) - 1;
        curvedPoints = new List<Vector4>(curvedLength);

        float t = 0.0f;
        for (int pointInTimeOnCurve = 0; pointInTimeOnCurve < curvedLength + 1; pointInTimeOnCurve++)
        {
            t = Mathf.InverseLerp(0, curvedLength, pointInTimeOnCurve);

            points = new List<Vector4>(arrayToCurve);

            for (int j = pointsLength - 1; j > 0; j--)
            {
                for (int i = 0; i < j; i++)
                {
                    points[i] = (1 - t) * points[i] + t * points[i + 1];
                }
            }

            curvedPoints.Add(points[0]);
        }

        return (curvedPoints.ToArray());
    }

    public void CamClamp()
    {
        Combo_scriptable script = (Combo_scriptable)target;
        EditorGUILayout.BeginHorizontal();
        dropdowncamclamp = EditorGUILayout.Foldout(dropdowncamclamp, new GUIContent("Clamps"));
        GUILayout.FlexibleSpace();

        EditorGUILayout.IntField(script.cameraClamps.Count, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();
        if (dropdowncamclamp)
        {
            for (int i = 0; i < script.cameraClamps.Count; i++)
            {
                float Time = script.cameraClamps[i].time;
                Vector2 Xminmax = script.cameraClamps[i].Xminmax;
                Vector2 Yminmax = script.cameraClamps[i].Yminmax;
                Vector2 threshold = script.cameraClamps[i].threshold;
            float maxfps = script.animation.length;
                if ((script.Use_Frames && !(script.movetype == Combo_scriptable.MoveType.Movement)) || (script.Use_Frames_Move && script.movetype == Combo_scriptable.MoveType.Movement))
                {
                    maxfps *= script.animation.length;
                }
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("S", GUILayout.Width(20));
                EditorGUILayout.FloatField(0, GUILayout.Width(50));
                Time = EditorGUILayout.Slider(Time, 0, maxfps);
                EditorGUILayout.LabelField("E", GUILayout.Width(20));
                EditorGUILayout.FloatField(maxfps, GUILayout.Width(80));
                EditorGUILayout.EndHorizontal();
                Xminmax = EditorGUILayout.Vector2Field("X Min/Max", Xminmax);
                Yminmax = EditorGUILayout.Vector2Field("Y Min/Max", Yminmax);
                threshold = EditorGUILayout.Vector2Field("Rotate threshold", threshold);

            }
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Add Clamp", GUILayout.Width(110)))
            {

                script.cameraClamps.Add(new CameraClamp());


            }
            if (GUILayout.Button("Remove Clamp", GUILayout.Width(150)) && script.cameraClamps.Count > 0)
            {
                script.cameraClamps.RemoveAt(script.cameraClamps.Count - 1);

            }
            EditorGUILayout.EndHorizontal();
        }
        
    }
}
