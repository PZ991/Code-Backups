using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default_Animations", menuName = "ScriptableObjects/Default_Anims")]
[System.Serializable]
public class DefaultAnim_Scriptable : ScriptableObject
{
    [Header("Defaults")]
    public Combo_scriptable defaultstand;
    public Combo_scriptable defaultcrouch;
    public Combo_scriptable defaultprone;
    [Header("Rotating")]
    public Combo_scriptable rotate_right;
    public Combo_scriptable rotate_left;
    public float stand_rotate_speed;
    public Combo_scriptable crouch_rotate_right;
    public Combo_scriptable crouch_rotate_left;
    public float crouch_rotate_speed;

    public Combo_scriptable prone_rotate_right;
    public Combo_scriptable prone_rotate_left;
    public float prone_rotate_speed;
    [Header("Transitioning")]
    public Combo_scriptable Stand_To_Crouch;
    public Combo_scriptable Stand_To_Prone;
    public Combo_scriptable Crouch_To_Stand;
    public Combo_scriptable Crouch_To_Prone;
    public Combo_scriptable Prone_To_Stand;
    public Combo_scriptable Prone_To_Crouch;
    public Combo_scriptable Walk_To_Stand;
    public Combo_scriptable Walk_To_Crouch;
    public Combo_scriptable Walk_To_Prone;
    public Combo_scriptable Run_To_Stand;
    public Combo_scriptable Run_To_Crouch;
    public Combo_scriptable Run_To_Prone;

    [Header("Walk")]
    public Combo_scriptable walkforward;
    public Combo_scriptable walkbackward;
    public Combo_scriptable walkright;
    public Combo_scriptable walkleft;
    public bool sideway_is_sides_walk;
    [Header("Run")]
    public Combo_scriptable runforward;
    public Combo_scriptable runbackward;
    public Combo_scriptable runright;
    public Combo_scriptable runleft;
    public bool sideway_is_sides_run;
    [Header("Crouch Move")]
    public Combo_scriptable walkforwardcrouch;
    public Combo_scriptable walkbackwardcrouch;
    public Combo_scriptable walkrightcrouch;
    public Combo_scriptable walkleftcrouch;
    public bool sideway_is_sides_crouchwalk;
    
    [Header("Prone Move")]
    public Combo_scriptable proneforward;
    public Combo_scriptable pronebackward;
    public Combo_scriptable proneright;
    public Combo_scriptable proneleft;
    public bool sideway_is_sides_prone;
    [Header("Jump")]
    public Combo_scriptable staticjump;
    public Combo_scriptable walkjumpforward;
    public Combo_scriptable runjump;
    public Combo_scriptable topwaterjump;
    public Combo_scriptable underwaterboost;
    [Header("Slide")]
    public Combo_scriptable slideforwardLeftLeg;
    public Combo_scriptable slideforwardRightLeg;
    
    [Header("Falling")]
    public Combo_scriptable staticFall;
    public Combo_scriptable forwardfall;
    public Combo_scriptable rightfall;
    public Combo_scriptable leftfall;
    public Combo_scriptable backwardfall;
    [Header("Gliding")]
    public Combo_scriptable staticgliding;
    public Combo_scriptable forwardgliding;
    public Combo_scriptable rightlgliding;
    public Combo_scriptable leftfgliding;
    public Combo_scriptable backwardgliding;
    [Header("Landing")]
    public Combo_scriptable staticLanding;
    public Combo_scriptable forwardLanding;
    public Combo_scriptable rightLanding;
    public Combo_scriptable leftLanding;
    [Header("Dodge Walk/Run")]
    public Combo_scriptable Dodge_Left;
    public Combo_scriptable Dodge_Right;
    public Combo_scriptable Dodge_Back;
    public Combo_scriptable Dodge_Forward;
    [Header("Dodge Prone Forward")]
    public Combo_scriptable Dodge_Left_prone;
    public Combo_scriptable Dodge_Right_prone;
    public Combo_scriptable Dodge_Back_prone;
    public Combo_scriptable Dodge_Forward_prone;
    [Header("Dodge Prone Backward")]
    public Combo_scriptable Dodge_Left_prone_back;
    public Combo_scriptable Dodge_Right_prone_back;
    public Combo_scriptable Dodge_Back_prone_back;
    public Combo_scriptable Dodge_Forward_prone_back;

    [Header("Fighting Stance")]
    public Combo_scriptable Fighting_stance_WB; //wholebody
    public Combo_scriptable Fighting_stance_UB; //upperbody
    public Combo_scriptable Fighting_stance_LB; //lowerbody
    public Combo_scriptable Fighting_stance_LA; //left/right arm and feets
    public Combo_scriptable Fighting_stance_RA;
    public Combo_scriptable Fighting_stance_LF;
    public Combo_scriptable Fighting_stance_RF;

    [Header("Swim Topwater")]
    public Combo_scriptable swim_top_default;
    public Combo_scriptable swim_top_forward;
    public Combo_scriptable swim_top_backward;
    public Combo_scriptable swim_top_right;
    public Combo_scriptable swim_top_left;
    [Header("Swim Underwater")]
    public Combo_scriptable swim_under_default;
    public Combo_scriptable swim_under_forward;
    public Combo_scriptable swim_under_backward;
    public Combo_scriptable swim_under_right;
    public Combo_scriptable swim_under_left;
    [Header("Ladder")]
    public Combo_scriptable stay_default;
    public Combo_scriptable going_up;
    public Combo_scriptable going_down;
    [Header("Rope||Pole")]
    public Combo_scriptable stay_default_pole;
    public Combo_scriptable going_up_pole;
    public Combo_scriptable going_down_pole;
    [Header("Wall Run||walk")]
    public Combo_scriptable wallrun_from_left;
    public Combo_scriptable wallrun_from_right;
    [Header("Wall Cling")]
    public Combo_scriptable wall_cling_left;
    public Combo_scriptable wall_cling_down;
    public Combo_scriptable wall_cling_right;
    public Combo_scriptable wall_cling_backward;
    public Combo_scriptable wall_cling_front;
    [Header("Wall Cover Hide")]
    public Combo_scriptable wall_stand_hide;
    public Combo_scriptable wall_crouch_hide;
    public Combo_scriptable wall_prone_hide;
    [Header("Wall Cover Peek")]
    public Combo_scriptable wall_stand_peek_left;
    public Combo_scriptable wall_crouch_peek_left;
    public Combo_scriptable wall_prone_peek_left;
    public Combo_scriptable wall_stand_peek_right;
    public Combo_scriptable wall_crouch_peek_right;
    public Combo_scriptable wall_prone_peek_right;
    [Header("Aim")]
    public Combo_scriptable AimLeftHand;
    public Combo_scriptable AimRightHand;
    public Combo_scriptable AimBothHands;

    [Header("Others")]
    public Combo_scriptable Pour;
    public Combo_scriptable Throw;
    public Combo_scriptable Craft;
    public Combo_scriptable Use;
    [Header("Camera")]
    public Vector3 Camposdefault;
    public Quaternion Camrotdefault;
    public Vector2 Xminmax;
    public Vector2 Yminmax;
    public Vector2 threshold;

    public float camweight;
    
    public  DefaultAnim_Scriptable (DefaultAnim_Scriptable clone)
    {
        DefaultAnim_Scriptable copy=this;
        copy.defaultstand = clone.defaultstand;
        copy.defaultcrouch = clone.defaultcrouch;
        copy.defaultprone = clone.defaultprone;
        copy.rotate_right = clone.rotate_right;
        copy.rotate_left = clone.rotate_left;
        copy.stand_rotate_speed = clone.stand_rotate_speed;
        copy.crouch_rotate_right = clone.crouch_rotate_right;
        copy.crouch_rotate_left = clone.crouch_rotate_left;
        copy.crouch_rotate_speed = clone.crouch_rotate_speed;

        copy.prone_rotate_right = clone.prone_rotate_right;
        copy.prone_rotate_left = clone.prone_rotate_left;
    copy.prone_rotate_speed = clone.prone_rotate_speed;
        copy.Stand_To_Crouch = clone.Stand_To_Crouch;
        copy.Stand_To_Prone = clone.Stand_To_Prone;
        copy.Crouch_To_Stand = clone.Crouch_To_Stand;
    copy.Crouch_To_Prone = clone.Crouch_To_Prone;
        copy.Prone_To_Stand = clone.Prone_To_Stand;
        copy.Prone_To_Crouch = clone.Prone_To_Crouch;
        copy.Walk_To_Stand = clone.Walk_To_Stand;
        copy.Walk_To_Crouch = clone.Walk_To_Crouch;
        copy.Walk_To_Prone = clone.Walk_To_Prone;
        copy.Run_To_Stand = clone.Run_To_Stand;
        copy.Run_To_Crouch = clone.Run_To_Crouch;
        copy.Run_To_Prone = clone.Run_To_Prone;

        copy.walkforward = clone.walkforward;
        copy.walkbackward = clone.walkbackward;
        copy.walkright = clone.walkright;
        copy.walkleft = clone.walkleft;
    copy.sideway_is_sides_walk = clone.sideway_is_sides_walk;
    copy.runforward=clone.runforward;
    copy.runbackward=clone.runbackward;
    copy.runright=clone.runright;
    copy.runleft=clone.runleft;
    copy.sideway_is_sides_run=clone.sideway_is_sides_run;
    copy.walkforwardcrouch=clone.walkforwardcrouch;
    copy.walkbackwardcrouch=clone.walkbackwardcrouch;
    copy.walkrightcrouch=clone.walkrightcrouch;
    copy.walkleftcrouch=clone.walkleftcrouch;
    copy.sideway_is_sides_crouchwalk=clone.sideway_is_sides_crouchwalk;
    
    copy.proneforward=clone.proneforward;
    copy.pronebackward=clone.pronebackward;
    copy.proneright=clone.proneright;
    copy.proneleft=clone.proneleft;
    copy.sideway_is_sides_prone=clone.sideway_is_sides_prone;
    copy.staticjump=clone.staticjump;
    copy.walkjumpforward=clone.walkjumpforward;
    copy.runjump=clone.runjump;
    copy.topwaterjump=clone.topwaterjump;
    copy.underwaterboost=clone.underwaterboost;
    copy.staticFall=clone.staticFall;
    copy.forwardfall=clone.forwardfall;
    copy.rightfall=clone.rightfall;
    copy.leftfall=clone.leftfall;
    copy.backwardfall=clone.backwardfall;
    copy.staticgliding=clone.staticgliding;
    copy.forwardgliding=clone.forwardgliding;
    copy.rightlgliding=clone.rightlgliding;
    copy.leftfgliding=clone.leftfgliding;
    copy.backwardgliding=clone.backwardgliding;
    copy.staticLanding=clone.staticLanding;
    copy.forwardLanding=clone.forwardLanding;
    copy.rightLanding=clone.rightLanding;
    copy.leftLanding=clone.leftLanding;
    copy.Dodge_Left=clone.Dodge_Left;
    copy.Dodge_Right=clone.Dodge_Right;
    copy.Dodge_Back=clone.Dodge_Back;
    copy.Dodge_Forward=clone.Dodge_Forward;
    copy.Dodge_Left_prone=clone.Dodge_Left_prone;
    copy.Dodge_Right_prone=clone.Dodge_Right_prone;
    copy.Dodge_Back_prone=clone.Dodge_Back_prone;
    copy.Dodge_Forward_prone=clone.Dodge_Forward_prone;
    copy.Dodge_Left_prone_back=clone.Dodge_Left_prone_back;
    copy.Dodge_Right_prone_back=clone.Dodge_Right_prone_back;
    copy.Dodge_Back_prone_back=clone.Dodge_Back_prone_back;
    copy.Dodge_Forward_prone_back=clone.Dodge_Forward_prone_back;

    copy.Fighting_stance_WB=clone.Fighting_stance_WB; //wholebody
    copy.Fighting_stance_UB=clone.Fighting_stance_UB; //upperbody
    copy.Fighting_stance_LB=clone.Fighting_stance_LB; //lowerbody
    copy.Fighting_stance_LA=clone.Fighting_stance_LA; //left/right arm and feets
    copy.Fighting_stance_RA=clone.Fighting_stance_RA;
    copy.Fighting_stance_LF=clone.Fighting_stance_LF;
    copy.Fighting_stance_RF=clone.Fighting_stance_RF;

    copy.swim_top_default=clone.swim_top_default;
    copy.swim_top_forward=clone.swim_top_forward;
    copy.swim_top_backward=clone.swim_top_backward;
    copy.swim_top_right=clone.swim_top_right;
    copy.swim_top_left=clone.swim_top_left;
    copy.swim_under_default=clone.swim_under_default;
    copy.swim_under_forward=clone.swim_under_forward;
    copy.swim_under_backward=clone.swim_under_backward;
    copy.swim_under_right=clone.swim_under_right;
    copy.swim_under_left=clone.swim_under_left;
    copy.stay_default=clone.stay_default;
    copy.going_up=clone.going_up;
    copy.going_down=clone.going_down;
    copy.stay_default_pole=clone.stay_default_pole;
    copy.going_up_pole=clone.going_up_pole;
    copy.going_down_pole=clone.going_down_pole;
    copy.wallrun_from_left=clone.wallrun_from_left;
    copy.wallrun_from_right=clone.wallrun_from_right;
    copy.wall_cling_left=clone.wall_cling_left;
    copy.wall_cling_down=clone.wall_cling_down;
    copy.wall_cling_right=clone.wall_cling_right;
    copy.wall_cling_backward=clone.wall_cling_backward;
    copy.wall_cling_front=clone.wall_cling_front;
        copy.wall_stand_hide = clone.wall_stand_hide;
        copy.wall_crouch_hide = clone.wall_crouch_hide;
        copy.wall_prone_hide = clone.wall_prone_hide;
    copy.wall_stand_peek_left=clone.wall_stand_peek_left;
    copy.wall_crouch_peek_left=clone.wall_crouch_peek_left;
    copy.wall_prone_peek_left=clone.wall_prone_peek_left;
    copy.wall_stand_peek_right=clone.wall_stand_peek_right;
    copy.wall_crouch_peek_right=clone.wall_crouch_peek_right;
    copy.wall_prone_peek_right=clone.wall_prone_peek_right;
    copy.Pour=clone.Pour;
    copy.Throw=clone.Throw;
    copy.Craft=clone.Craft;
    copy.Use=clone.Use;

    copy.AimBothHands=clone.AimBothHands;
    copy.AimLeftHand=clone.AimLeftHand;
    copy.AimRightHand=clone.AimRightHand;
    copy.Camposdefault = clone.Camposdefault;
    copy.Camrotdefault = clone.Camrotdefault;
        copy.slideforwardLeftLeg = clone.slideforwardLeftLeg;
    copy.slideforwardRightLeg = clone.slideforwardRightLeg;


        copy.Xminmax = clone.Xminmax;
        copy.Yminmax = clone.Yminmax;
        copy.threshold = clone.threshold;
        copy.camweight = clone.camweight;

    }
    public  DefaultAnim_Scriptable ()
    {
        DefaultAnim_Scriptable copy= this;
        copy.defaultstand = null;
        copy.defaultcrouch = null;
        copy.defaultprone = null;
        copy.rotate_right = null;
        copy.rotate_left = null;
        copy.stand_rotate_speed = 0;
        copy.crouch_rotate_right = null;
        copy.crouch_rotate_left = null;
        copy.crouch_rotate_speed = 0;

        copy.prone_rotate_right = null;
        copy.prone_rotate_left = null;
    copy.prone_rotate_speed = 0;
        copy.Stand_To_Crouch = null;
        copy.Stand_To_Prone = null;
        copy.Crouch_To_Stand = null;
    copy.Crouch_To_Prone = null;
        copy.Prone_To_Stand = null;
        copy.Prone_To_Crouch = null;
        copy.Walk_To_Stand = null;
        copy.Walk_To_Crouch = null;
        copy.Walk_To_Prone = null;
        copy.Run_To_Stand = null;
        copy.Run_To_Crouch = null;
        copy.Run_To_Prone = null;

        copy.walkforward = null;
        copy.walkbackward = null;
        copy.walkright = null;
        copy.walkleft = null;
    copy.sideway_is_sides_walk = false;
    copy.runforward= null;
    copy.runbackward= null;
    copy.runright= null;
    copy.runleft= null;
    copy.sideway_is_sides_run= false;
    copy.walkforwardcrouch= null;
    copy.walkbackwardcrouch= null;
    copy.walkrightcrouch= null;
    copy.walkleftcrouch= null;
    copy.sideway_is_sides_crouchwalk= false;
    
    copy.proneforward= null;
    copy.pronebackward= null;
    copy.proneright= null;
    copy.proneleft= null;
    copy.sideway_is_sides_prone= false;
    copy.staticjump= null;
    copy.walkjumpforward= null;
    copy.runjump= null;
    copy.topwaterjump= null;
    copy.underwaterboost= null;
    copy.staticFall= null;
    copy.forwardfall= null;
    copy.rightfall= null;
    copy.leftfall= null;
    copy.backwardfall= null;
    copy.staticgliding= null;
    copy.forwardgliding= null;
    copy.rightlgliding= null;
    copy.leftfgliding= null;
    copy.backwardgliding= null;
    copy.staticLanding= null;
    copy.forwardLanding= null;
    copy.rightLanding= null;
    copy.leftLanding= null;
    copy.Dodge_Left= null;
    copy.Dodge_Right= null;
    copy.Dodge_Back= null;
    copy.Dodge_Forward= null;
    copy.Dodge_Left_prone= null;
    copy.Dodge_Right_prone= null;
    copy.Dodge_Back_prone= null;
    copy.Dodge_Forward_prone= null;
    copy.Dodge_Left_prone_back= null;
    copy.Dodge_Right_prone_back= null;
    copy.Dodge_Back_prone_back= null;
    copy.Dodge_Forward_prone_back= null;

    copy.Fighting_stance_WB= null; //wholebody
    copy.Fighting_stance_UB= null; //upperbody
    copy.Fighting_stance_LB= null; //lowerbody
    copy.Fighting_stance_LA= null; //left/right arm and feets
    copy.Fighting_stance_RA= null;
    copy.Fighting_stance_LF= null;
    copy.Fighting_stance_RF= null;

    copy.swim_top_default= null;
    copy.swim_top_forward= null;
    copy.swim_top_backward= null;
    copy.swim_top_right= null;
    copy.swim_top_left= null;
    copy.swim_under_default= null;
    copy.swim_under_forward= null;
    copy.swim_under_backward= null;
    copy.swim_under_right= null;
    copy.swim_under_left= null;
    copy.stay_default= null;
    copy.going_up= null;
    copy.going_down= null;
    copy.stay_default_pole= null;
    copy.going_up_pole= null;
    copy.going_down_pole= null;
    copy.wallrun_from_left= null;
    copy.wallrun_from_right= null;
    copy.wall_cling_left= null;
    copy.wall_cling_down= null;
    copy.wall_cling_right= null;
    copy.wall_cling_backward= null;
    copy.wall_cling_front= null;
        copy.wall_stand_hide = null;
        copy.wall_crouch_hide = null;
        copy.wall_prone_hide = null;
    copy.wall_stand_peek_left= null;
    copy.wall_crouch_peek_left= null;
    copy.wall_prone_peek_left= null;
    copy.wall_stand_peek_right= null;
    copy.wall_crouch_peek_right= null;
    copy.wall_prone_peek_right= null;
    copy.Pour= null;
    copy.Throw= null;
    copy.Craft= null;
    copy.Use= null;

    copy.AimBothHands= null;
    copy.AimLeftHand= null;
    copy.AimRightHand= null;
    copy.Camposdefault = Vector3.zero;
    copy.Camrotdefault = new Quaternion();
        copy.slideforwardLeftLeg = null;
    copy.slideforwardRightLeg = null;


        copy.Xminmax = new Vector2();
        copy.Yminmax = new Vector2();
        copy.threshold = new Vector2();
        copy.camweight = 0;

    }
}
