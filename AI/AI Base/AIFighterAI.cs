using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AIFighterAI : MonoBehaviour
{
    //public TargetInfo targets;
    [Header("Main Settings")]
    public List<GameObject> possibletargets;
    public bool leftoccupied;
    public bool rightoccupied;
    public Transform LShoulder;
    public Transform RShoulder;
    public Transform LLeg;
    public Transform RLeg;
    public Transform targetL;
    public Transform targetR;
    public Animator anim;

    [Header("Attack Info")]
    public float Lattack_distance;
    public float Rattack_distance;
    public float targeting_distance;
    public float left_timing;
    public float right_timing;
    public float Direction;
    public AnimatorEvent animator;

    [Header("Predict")]
    private Vector3 targetLlastpos;
    private Vector3 targetRlastpos;
    [Range(3f, 4f)]
    public float efficiencyL;
    [Range(3f, 4f)]
    public float efficiencyR;
    public Vector3 predictedLpos;
    public Vector3 predictedRpos;

    [Header("Combos")]
    public Combo_scriptable currentStanceL;
    public Combo_scriptable currentComboL;
    public Combo_scriptable currentStanceR;
    public Combo_scriptable currentComboR;

    public List<float> allComboLDist;
    public List<float> allComboRDist;
    //public bool changedComboL;
    public float returnToStance;
    float time;
    public float returnToStanceSpeed;
    public Player_Model_Scriptable model;
    public bool filterLnR;

    //improve
    public bool Priorityattacking;
    public bool PriorityDefend;
    public bool PriorityEvade;
    public bool PriorityMove;
    public bool PriorityStop;
    //implement
    public bool hesitate;
    public bool wait_for_opening;

    public float lrecharge;
    public float rrecharge;
    public float leftRecharge;
    public float rightRecharge;
    public float leftRechargeSpeed;
    public float rightRechargeSpeed;

    public Transform[] currentTarget = new Transform[4];

    //[Header("Test")]
    //public float lerptest;
    //public float inverselerptest;
    //public GameObject LtargetPoint;
    public bool leftHanded;
    public bool Lhit;
    public bool Rhit;

    public bool AiControlled;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (AiControlled)
        {
            Direction = AngleDir(transform.forward, transform.position - possibletargets[0].transform.position, transform.up);

            #region Occupy LR
            if (anim.GetCurrentAnimatorStateInfo(1).normalizedTime > 1f && !anim.IsInTransition(1))
            {
                if (lrecharge < leftRecharge)
                    lrecharge += leftRechargeSpeed * Time.deltaTime;
                if (currentComboL != currentStanceL && leftoccupied == true)
                {
                    animator.Add_Animation_Toanim(currentStanceL);
                    currentComboL = currentStanceL;

                }
                leftoccupied = false;
            }
            if (anim.GetCurrentAnimatorStateInfo(2).normalizedTime > 1f && !anim.IsInTransition(2))
            {
                if (rrecharge < rightRecharge)
                    rrecharge += rightRechargeSpeed * Time.deltaTime;
                if (currentComboR != currentStanceR && rightoccupied == true)
                {
                    animator.Add_Animation_Toanim(currentStanceR);
                    currentComboR = currentStanceR;

                }
                rightoccupied = false;
            }
            #endregion

            #region Targeting
            for (int i = 0; i < possibletargets.Count; i++)
            {

                float dist = Vector3.Distance(possibletargets[i].transform.position, transform.position);
                if (dist < targeting_distance)
                {
                    //if(possibletargets[i].attacking==true&&dist <= defense_distance)
                    //{

                    Direction = AngleDir(transform.forward, transform.position - possibletargets[i].transform.position, transform.up);

                    ChangeCombo(Direction);
                    #region Left
                    if ((Direction == -1 && leftoccupied == false && lrecharge >= leftRecharge) || (leftoccupied == false && (rrecharge < rightRecharge || rightoccupied) && Direction == 1) && Rhit)
                    {
                        targetL.position = possibletargets[i].transform.position;
                        currentTarget[1] = possibletargets[i].transform;
                        LeadTarget(-1);
                        //targetL = LtargetPoint.transform;

                        //Debug.Log(Vector3.Distance(predictedLpos, transform.position));
                        if (Vector3.Distance(predictedLpos, LShoulder.transform.position) < Lattack_distance)
                        {

                            //Debug.Log("Left");
                            leftoccupied = true;
                            animator.containsLeft = true;
                            animator.Add_Animation_Toanim(currentComboL);
                            lrecharge = 0;
                            Lhit = true;
                            Rhit = false;
                        }
                        else
                        {
                            ResetCombo();


                        }

                    }
                    #endregion
                    else if ((Direction == 1 && rightoccupied == false && rrecharge >= rightRecharge) || (rightoccupied == false && (lrecharge < leftRecharge || leftoccupied) && Direction == -1) && Lhit)
                    {
                        LeadTarget(1);
                        //targetL = LtargetPoint.transform;
                        targetR.position = possibletargets[i].transform.position;
                        currentTarget[2] = possibletargets[i].transform;
                        //Debug.Log(Vector3.Distance(predictedLpos, transform.position));
                        if (Vector3.Distance(predictedRpos, RShoulder.transform.position) < Rattack_distance)
                        {


                            rightoccupied = true;
                            animator.containsRight = true;
                            animator.Add_Animation_Toanim(currentComboR);
                            rrecharge = 0;
                            Rhit = true;
                            Lhit = false;
                        }
                        else
                        {
                            ResetCombo();

                        }
                    }
                    //}
                }

            }
            #endregion
        }
    }

    public void ResetCombo()
    {
        /*
        if (currentStanceL != null)
        {
            if (currentComboL != currentStanceL )
            {
                
                    if (time < returnToStance)
                    {
                        time += returnToStanceSpeed * Time.deltaTime;
                    }
                    else
                    {
                        currentComboL = currentStanceL;
                    time = 0;
                    }
                

            }
            else
            {
                time = 0;
            }
        }
        if (currentStanceR != null)
        {
            if (currentComboR != currentStanceR)
            {

                if (time < returnToStance)
                {
                    time += returnToStanceSpeed * Time.deltaTime;
                }
                else
                {
                    currentComboR = currentStanceR;
                    time = 0;
                }


            }
            else
            {
                time = 0;
            }
        }
        */
    }
    public void ChangeCombo(float direction)
    {
        List<int> allComboLindex = new List<int>();
        List<int> allComboRindex = new List<int>();
        /*
        if (filterLnR)
        {
            if ((direction == -1 && leftoccupied == false && lrecharge >= leftRecharge) || (leftoccupied == false && (rrecharge < rightRecharge || rightoccupied) && direction == 1&& Rhit))
            {
                for (int i = 0; i < currentComboL.interconnected_combo.Count; i++)
                {
                    //Debug.Log("Test");
                    float distL = Vector3.Distance(possibletargets[0].transform.position, LShoulder.transform.position);
                    float distanceL = Mathf.Lerp(0, model.LeftArmReach, currentComboL.interconnected_combo[i].MaxDistance);// + currentComboL.interconnected_combo[i].Distance_Offset; //lerp or inverselerp
                                                                                                                                  ////Debug.Log(distance);

                    if (Priorityattacking || PriorityDefend || PriorityEvade)
                    {
                        if (Priorityattacking)
                        {
                            if (currentComboL.interconnected_combo[i].layer == 1 && currentComboL.interconnected_combo[i].movetype == Combo_scriptable.MoveType.Attack)
                            {
                                if (distanceL < distL)
                                {
                                    allComboLDist.Add(distanceL); //maxdistance is 0-1 lerp of reach
                                    allComboLindex.Add(i);
                                }
                            }
                        }
                        else if (PriorityDefend)
                        {
                            if (currentComboL.interconnected_combo[i].layer == 1 && currentComboL.interconnected_combo[i].movetype == Combo_scriptable.MoveType.Defend)
                            {
                                if (distanceL < distL)
                                {
                                    allComboLDist.Add(distanceL); //maxdistance is 0-1 lerp of reach
                                    allComboLindex.Add(i);
                                }
                            }
                        }
                        else if (PriorityEvade)
                        {
                            if (currentComboL.interconnected_combo[i].layer == 1 && currentComboL.interconnected_combo[i].movetype == Combo_scriptable.MoveType.Movement)
                            {
                                if (distanceL < distL)
                                {
                                    allComboLDist.Add(distanceL); //maxdistance is 0-1 lerp of reach
                                    allComboLindex.Add(i);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (currentComboL.interconnected_combo[i].layer == 1)
                        {
                            
                            if (distanceL > distL)
                            {
                                allComboLDist.Add(distanceL); //maxdistance is 0-1 lerp of reach
                                allComboLindex.Add(i);
                            }
                        }
                    }

                }
            }
            else if ((direction == 1 && rightoccupied == false && rrecharge >= rightRecharge) || (rightoccupied == false && (lrecharge < leftRecharge || leftoccupied) && direction == -1&& Lhit))
            {
               
                for (int i = 0; i < currentComboR.interconnected_combo.Count; i++)
                {
                    float distR = Vector3.Distance(possibletargets[0].transform.position, RShoulder.transform.position);

                    float distanceR = Mathf.Lerp(0, model.RightArmReach, currentComboR.interconnected_combo[i].MaxDistance);// + currentComboL.interconnected_combo[i].Distance_Offset; //lerp or inverselerp

                    
                    if (Priorityattacking || PriorityDefend || PriorityEvade)
                    {
                        if (Priorityattacking)
                        {
                            if (currentComboR.interconnected_combo[i].layer == 2 && currentComboR.interconnected_combo[i].movetype == Combo_scriptable.MoveType.Attack)
                            {
                                if (distanceR < distR)
                                {
                                    allComboRDist.Add(distanceR); //maxdistance is 0-1 lerp of reach
                                    allComboRindex.Add(i);
                                }
                            }
                        }
                        else if (PriorityDefend)
                        {
                            if (currentComboR.interconnected_combo[i].layer == 2 && currentComboR.interconnected_combo[i].movetype == Combo_scriptable.MoveType.Defend)
                            {
                                if (distanceR < distR)
                                {
                                    allComboRDist.Add(distanceR); //maxdistance is 0-1 lerp of reach
                                    allComboRindex.Add(i);
                                }
                            }
                        }
                        else if (PriorityEvade)
                        {
                            if (currentComboR.interconnected_combo[i].layer == 2 && currentComboR.interconnected_combo[i].movetype == Combo_scriptable.MoveType.Movement)
                            {
                                if (distanceR < distR)
                                {
                                    allComboRDist.Add(distanceR); //maxdistance is 0-1 lerp of reach
                                    allComboRindex.Add(i);
                                }
                            }
                        }
                    }
                    else
                    {
                        
                        if (currentComboR.interconnected_combo[i].layer == 2)
                        {
                            
                            if (distR<distanceR)
                            {
                               // Debug.Log("Test");
                                
                                allComboRDist.Add(distanceR); //maxdistance is 0-1 lerp of reach
                                allComboRindex.Add(i);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < currentComboR.interconnected_combo.Count; i++)
            {
                float distL = Vector3.Distance(possibletargets[0].transform.position, LShoulder.transform.position);
                float distanceL = Mathf.Lerp(0, model.LeftArmReach, currentComboL.interconnected_combo[i].MaxDistance);// + currentComboL.interconnected_combo[i].Distance_Offset; //lerp or inverselerp
                if (distanceL <= distL)
                {
                    allComboLDist.Add(distanceL); //maxdistance is 0-1 lerp of reach
                    allComboLindex.Add(i);

                }
            }
            for (int i = 0; i < currentComboR.interconnected_combo.Count; i++)
            {
                float distR = Vector3.Distance(possibletargets[0].transform.position, RShoulder.transform.position);
                float distanceR = Mathf.Lerp(0, model.RightArmReach, currentComboR.interconnected_combo[i].MaxDistance);// + currentComboL.interconnected_combo[i].Distance_Offset; //lerp or inverselerp
                if (distanceR <= distR)
                {
                    allComboRDist.Add(distanceR); //maxdistance is 0-1 lerp of reach
                    allComboRindex.Add(i);

                }
            }
            
            



        }
            */

        
        if (allComboLDist.Count > 0)
        {
            
                int index = Random.Range(0, allComboLindex.Count);
                Lattack_distance = allComboLDist[index];
        //        left_timing = currentComboL.interconnected_combo[index].Hit_time;
                currentComboL = currentComboL.interconnected_combo[index].combo;
                //Debug.Log(currentComboL.interconnected_combo[index].name);
                allComboLDist.Clear();
                allComboRDist.Clear();
            
            

        }
        else if(allComboRDist.Count > 0)
        {
             
                int index = Random.Range(0, allComboRindex.Count);
                Rattack_distance = allComboRDist[index];
         //       right_timing = currentComboR.interconnected_combo[index].Hit_time;
                currentComboR = currentComboR.interconnected_combo[index].combo;
                //Debug.Log(currentComboL.interconnected_combo[index].name);
                allComboLDist.Clear();
                //allComboRDist.Clear();
            
        }
    }
    private void OnDrawGizmos()
    {
        //inverselerptest = Mathf.InverseLerp(0, 90, 9);      //0.1, value to percentage
        // lerptest = Mathf.Lerp(0, 90, 1);                    //90, percentage to value
        
       // UpdateRadials();
        
    }
    //-1= left 1= right 0=forward
    float AngleDir(Vector3 fwd, Vector3 targetDir,Vector3 up)
    {
        Vector3 right = Vector3.Cross(up, fwd);
        float dir = Vector3.Dot(right, targetDir);

        //test4 = Vector3.Cross(transform.right,fwd); //top =1, bottom=-1
        //float dir2 = Vector3.Dot(targetDir,test4);

        //test4 = Vector3.Cross(up, transform.right); // forward=1,back=-1
        //float dir3 = Vector3.Dot(test4, targetDir);

        //test3 = dir2;

        if (dir > 0)
            return 1f;
        else if (dir < 0)
            return -1f;
        else
            return 0;
    }

    private void LeadTarget(float Direction)
    {
        if (targetL == null) return;
        if (Direction == -1)
        {
            // Get target position in one second ahead 
            Vector3 targetSpeed = (targetL.position - targetLlastpos);
            targetSpeed /= Time.deltaTime; // Target distance in one second. Since "Time.deltaTime" = 1/FPS

            // ---------------------------------------------------------------------------------------------
            // Calculate the the lead target position based on target speed and projectileTravelTime to reach the target

            float distance = Vector3.Distance(LShoulder.transform.position, targetL.position);
            float projectileTravelTime = distance / Mathf.Max(left_timing, 2f);
            Vector3 aimPoint = targetL.position + targetSpeed * efficiencyL / 4 * projectileTravelTime;

            float distance2 = Vector3.Distance(LShoulder.transform.position, aimPoint);
            float projectileTravelTime2 = distance2 / Mathf.Max(left_timing, 2f);
            predictedLpos = targetL.position + targetSpeed * efficiencyL / 4 * projectileTravelTime2;

            Debug.DrawLine(LShoulder.transform.position, predictedLpos, Color.blue);

            targetLlastpos = targetL.position;
            //Debug.Log("Finished3");
        }
        else if (Direction==1)
        {
            Vector3 targetSpeed = (targetR.position - targetRlastpos);
            targetSpeed /= Time.deltaTime; // Target distance in one second. Since "Time.deltaTime" = 1/FPS

            // ---------------------------------------------------------------------------------------------
            // Calculate the the lead target position based on target speed and projectileTravelTime to reach the target

            float distance = Vector3.Distance(RShoulder.transform.position, targetR.position);
            float projectileTravelTime = distance / Mathf.Max(right_timing, 2f);
            Vector3 aimPoint = targetR.position + targetSpeed * efficiencyR / 4 * projectileTravelTime;

            float distance2 = Vector3.Distance(RShoulder.transform.position, aimPoint);
            float projectileTravelTime2 = distance2 / Mathf.Max(right_timing, 2f);
            predictedRpos = targetR.position + targetSpeed * efficiencyR / 4 * projectileTravelTime2;

            Debug.DrawLine(RShoulder.transform.position, predictedRpos, Color.yellow);

            targetRlastpos = targetR.position;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == currentTarget[1])
            currentTarget[1] = null;
        else if (collision.transform == currentTarget[2])
            currentTarget[2] = null;
    }

}
