using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;


public class AIMovementAI : MonoBehaviour
{


    public GameObject obj;
    Vector3 oldpos;
    Vector3 newpos;
    Vector3 vel;
    public float dist;
    public float Timebetween;

    public Animator animator;
    public Animator animatorshadow;
    public AnimatorEvent anim;
    public AnimatorEvent animshadow;
    public bool gonnaHit;
    public bool stillhitting;
    public Combo_scriptable movementcombo;
    public Combo_scriptable previousMovecombo;
    public Combo_scriptable defaultstance;
    public Transform parentMov;
    public Transform shadowMov;
    public Vector3 target;
    public bool move;
    public float LR;
    public float FB;
    public float UD;
    public float startspeed;
    public float currentspeed;
    public float speedmultiplier;
    public bool started;
    public Forward forward;
    public bool jumped;
    public bool grounded;
    public bool controlled;
    Vector3 previousdirection;
    public bool once;
    public List<Combo_scriptable> dodgelist;
    public Vector3 directionchange;

    void Start()
    {
        oldpos = obj.transform.position;
        foreach (Combo_scriptable combo in dodgelist)
        {
            if (combo.Dodge)
            {
                dodgelist.Add(combo);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        newpos = obj.transform.position;
        var media = (newpos - oldpos);
        vel = media / Time.deltaTime;
        dist = Vector3.Distance(newpos, oldpos);
        float newdist = Vector3.Distance(obj.transform.position, transform.position);
        Timebetween = newdist / vel.magnitude;
        oldpos = newpos;
        newpos = obj.transform.position;

        if (movementcombo.Dodge == true)
        {
            Dodge();
        }
        else
        {
            Movement();
            //Debug.Log("move");
        }
    }
    private void Dodge()
    {
        Rigidbody rb = obj.transform.GetComponent<Rigidbody>();
        if (rb != null)
        {
            RaycastHit hit;
            /// dist = Vector3.Magnitude(vel);
            if (rb.SweepTest(vel, out hit, 10))
            {
                gonnaHit = true;
                //Debug.Log(vel);
                if (hit.collider.tag == "Shadow")
                {
                    // Gizmos.DrawWireCube(transform.position, transform.localScale);

                    stillhitting = true;



                }

            }
        }
        if (gonnaHit)
        {
            if (stillhitting)
            {
                animatorshadow.Play("state", 0, 100);//play anim at end(for shadow simulation)
                RaycastHit hit;
                if (rb.SweepTest(vel, out hit, Timebetween))
                {
                    if (hit.collider.tag != "Shadow")//&& (hit.transform.root.GetChild(0) == this || hit.transform.root == this))
                    {

                        Debug.Log("hit2");
                        animator.CrossFade("state", 0.2f);
                        //Gizmos.DrawWireCube(transform.position, Vector3.one);
                        gonnaHit = false;
                        stillhitting = false;

                    }
                    else
                    {
                        //change currentcombo
                    }
                }
            }





        }

        #region V1
        /*
        RaycastHit hit;
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.forward));
        /*
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 100))
        {
          

            Vector3 radius = transform.localScale;
            
        }
        //////
        Vector3 leftSpos;
        Vector3 rightSpos;
        float size;
        if (front)
        {
             leftSpos = Lshoulder.transform.right * (Lshoulder.transform.localScale.x / 2) * -1;
             rightSpos = Rshoulder.transform.right * (Rshoulder.transform.localScale.x / 2) * 1;
             size = Vector3.Distance(Lshoulder.transform.position + leftSpos, Rshoulder.transform.position + rightSpos);

        }
        else
        {
             leftSpos = Lshoulder.transform.forward * (Lshoulder.transform.localScale.z / 2) * 1;
             rightSpos = Rshoulder.transform.forward * (Rshoulder.transform.localScale.z / 2) * -1;
            size = Vector3.Distance(Lshoulder.transform.position+leftSpos , Rshoulder.transform.position-rightSpos );

        }
        Gizmos.DrawSphere(leftSpos, 0.2f);
        Gizmos.DrawSphere(rightSpos, 0.2f);
         Vector3 spine = (Lshoulder.transform.position + Rshoulder.transform.position + Lknee.transform.position + Rknee.transform.position)/4;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(spine, new Vector3(size,size,size));
        if(Physics.BoxCast(transform.position,transform.localScale/2,transform.forward,out hit))
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(obj.transform.position, obj.transform.localScale);
        }
        */
        #endregion
        #region V2
        /*
        var cap = transform.GetComponent<CapsuleCollider>();
        direction = AngleDir(transform.forward, transform.position - obj.transform.position, transform.up);
        LeadTarget();
        Vector3 p1 = transform.position+cap.center+Vector3.up*-cap.height*.5f;
        Vector3 p2 = p1+Vector3.up*cap.height;

       
        if (Physics.CapsuleCast(p1, p2, cap.radius, transform.forward))
        {
            Gizmos.DrawWireCube(obj.transform.position, obj.transform.localScale);
        }
        */
        #endregion

    }

    //fix animation, not synchronized with animatorevent

    public void Movement()
    {
        float layernormalizedtime;
        layernormalizedtime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        // if ((target == Vector3.zero || move == false))// &&(layernormalizedtime<1&& acceleration == true))
        //     {
        if (once == false)
        {
            anim.Add_Animation_Toanim(movementcombo);
            once = true;
        }
        else if(anim.previousclips[0]!=null)
        {
            float normalizedTime = 0;
            if (anim.previousclips[0].animation != null)
            {
                normalizedTime = (animator.GetCurrentAnimatorStateInfo(0).normalizedTime - Mathf.Floor(animator.GetCurrentAnimatorStateInfo(0).normalizedTime));
                // Debug.Log(normalizedTime);
            }
            //     if (layernormalizedtime >= normalizedTime && anim.previousclips[0]==movementcombo)
            //      {
            //if (anim.IsInTransition(0) == false)
            //{
            //}
            //Debug.Log(normalizedTime);
            DirectionChange(anim.previousclips[0].animation, anim.previousclips[0].move_to_pos, 0, anim.previousclips[0].Use_Frames_Move);
            if (movementcombo.HasStartTime == true)
            {
                LR = directionchange.x;
                FB = directionchange.z;
                UD = directionchange.y;
                SetDirectionTarget(LR, UD, FB);
                //SetDirectionTarget(LR, UD, FB);

                move = true;
            }
            else if (movementcombo.HasStartTime == false)
            {
                LR = directionchange.x;
                FB = directionchange.z;
                UD = directionchange.y;
                SetDirectionTarget(LR, UD, FB);

                move = true;


            }

            //    }


            //    }
            //   else
            //   {
            #region speed

            HorizontalSpeed(anim.previousclips[0].animation, anim.previousclips[0].move_to_pos, 0, anim.previousclips[0].Use_Frames_Move);


            //Debug.Log(currentspeed);
            #endregion
            Rigidbody rb = transform.GetComponent<Rigidbody>();
            #region Gravity
            if (movementcombo.affectedbygravity == true)
            {
                rb.useGravity = true;
                rb.isKinematic = false;
                if (jumped == false)
                {
                    if (movementcombo.arcvertical)
                    {
                        target.y = transform.position.y;
                        Debug.Log("jumped");
                        rb.AddForce(Vector3.up * UD * movementcombo.Verticalspeed);
                        jumped = true;
                    }

                }
            }
            else
            {
                rb.useGravity = false;
            }
            #endregion
            #region Kinematic
            if (movementcombo.isKinematic)
            {
                rb.isKinematic = true;
            }
            else
            {
                rb.useGravity = false;
                rb.isKinematic = false;
            }
            #endregion


            if (Vector3.Distance(transform.position, target) < 0.2f && (movementcombo.After_Move != null || movementcombo.slowingdownORland != null))
            {
                jumped = false;
                move = false;
                once = false;
                started = false;
                previousdirection = (transform.position + (target - transform.position)) * 2; // slowdown direction
                previousMovecombo = movementcombo;
                if ((movementcombo.slowdownwhenlanding && grounded) || (controlled && movementcombo.SlowDownWhenControlled))
                {

                    movementcombo = movementcombo.slowingdownORland;
                }
                else
                {
                    Debug.Log("Next");
                    Debug.Log(previousdirection);
                    Debug.Log(transform.position);

                    movementcombo = movementcombo.After_Move;
                    //Do after move
                }
            }
            /*
            else if (layernormalizedtime >= 1)
            {
                jumped = false;
                move = false;
                currentspeed = 0;
                movementcombo = defaultstance;
                anim.Add_Animation_Toanim(movementcombo);
            }
            */
            else
            {
                
                //rb.MovePosition(transform.position + (target - transform.position) * currentspeed * Time.deltaTime);
                //       rb.position = transform.position + (target - transform.position).normalized * currentspeed * Time.deltaTime;
                rb.MovePosition(target);
                //   Debug.Log(currentval);
                //       rb.position = transform.position + (target - transform.position).normalized * currentspeed * Time.deltaTime;
                //rb.position = target;



            }
            /*
            else if (acceleration == false && movementcombo.use_previous_velocity == true)
            {
                Debug.Log("Slowdown");
                rb.position = transform.position + (previousdirection - transform.position).normalized * currentspeed * Time.deltaTime;

                //rb.MovePosition(transform.position + (target - transform.position) * currentspeed * Time.deltaTime);
            }
            */
        }
        ///transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * currentspeed);
        //   }

    }
    /*
    public void LeadTarget()
    {
        Vector3 targetSpeed = (obj.transform.position - targetLastpos);
        targetSpeed /= Time.deltaTime; // Target distance in one second. Since "Time.deltaTime" = 1/FPS

        // ---------------------------------------------------------------------------------------------
        // Calculate the the lead target position based on target speed and projectileTravelTime to reach the target

        float distance = Vector3.Distance(transform.position, obj.transform.position);
        float projectileTravelTime = distance / Mathf.Max(1, 2f);// 1= projectilespeed
        Vector3 aimPoint = obj.transform.position + targetSpeed * efficiency / 4 * projectileTravelTime;

        float distance2 = Vector3.Distance(transform.position, aimPoint);
        float projectileTravelTime2 = distance2 / Mathf.Max(1, 2f);
        predictedpos = obj.transform.position + targetSpeed * efficiency / 4 * projectileTravelTime2;

        Debug.DrawLine(transform.position, predictedpos, Color.blue);

        targetLastpos = obj.transform.position;

        

    }
    */
    #region LeadTarget
    /*
    private void LeadTarget()
    {
        if (target == null) return;
        // Get target position in one second ahead 
        Vector3 targetSpeed = (target.position - targetlastPosition);
        targetSpeed /= Time.deltaTime; // Target distance in one second. Since "Time.deltaTime" = 1/FPS

        // ---------------------------------------------------------------------------------------------
        // Calculate the the lead target position based on target speed and projectileTravelTime to reach the target

        float distance = Vector3.Distance(transform.position, target.position);
        float projectileTravelTime = distance / Mathf.Max(ProjectileSpeed, 2f);
        Vector3 aimPoint = target.position + targetSpeed * Efficiency / 4 * projectileTravelTime;

        float distance2 = Vector3.Distance(transform.position, aimPoint);
        float projectileTravelTime2 = distance2 / Mathf.Max(ProjectileSpeed, 2f);
        predictedTargetPosition = target.position + targetSpeed * Efficiency / 4 * projectileTravelTime2;

        Debug.DrawLine(transform.position, predictedTargetPosition, Color.blue);

        targetlastPosition = target.position;
    }
    */
    #endregion
    public void SetDirectionTarget(float X, float Y, float Z)
    {
        Vector3 movepos = Vector3.zero;
        switch (forward)
        {
            case Forward.Z:
                {
                    movepos = new Vector3(LR, UD, FB);
                    break;
                }
            case Forward.nZ:
                {
                    movepos = new Vector3(-LR, UD, -FB);
                    break;
                }
            case Forward.X:
                {
                    movepos = new Vector3(FB, UD, -LR);
                    break;
                }
            case Forward.nX:
                {
                    movepos = new Vector3(-FB, UD, LR);
                    break;
                }
            case Forward.Y:
                {
                    movepos = new Vector3(FB, -LR, -UD);
                    break;
                }
            case Forward.nY:
                {
                    movepos = new Vector3(FB, LR, UD);
                    break;
                }

        }
        // Debug.Log(movepos);
        //Debug.Log(transform.position); 
        target = transform.position + movepos;
    }

    private void HorizontalSpeed(AnimationClip clip, List<MovementTiming> IK, int layer, bool useSeconds)
    {
        // Debug.Log(layer);
        int currentval = 0;
        for (int i = 0; i < IK.Count; i++)
        {
            float currenttime2 = Mathf.Lerp(0, clip.length, (animator.GetCurrentAnimatorStateInfo(layer).normalizedTime - Mathf.Floor(animator.GetCurrentAnimatorStateInfo(layer).normalizedTime)));
            float currenttime = currenttime2;


            if (currenttime >= IK[i].speedtiming)
            {

                if (i < IK.Count - 1)
                {
                    currentval += 1;
                }

            }
        }
        float endweight = 0;
        float startweight = 0;


        float calculatedfps;
        float calculatedrestart;
        if (useSeconds == true)
        {
            calculatedfps = endweight / clip.frameRate;           //no division, 0:06/24fps, 6/24
            calculatedrestart = startweight / clip.frameRate;
        }
        else
        {
            calculatedfps = endweight;                           //using clip length
            calculatedrestart = startweight;

        }

        float timed;
        float normalizedTime = Mathf.Lerp(0, clip.length, (animator.GetCurrentAnimatorStateInfo(layer).normalizedTime - Mathf.Floor(animator.GetCurrentAnimatorStateInfo(layer).normalizedTime)));


        timed = Mathf.InverseLerp(IK[Mathf.Clamp(currentval - 1, 0, IK.Count)].speedtiming, IK[currentval].speedtiming, normalizedTime);
        currentspeed = Mathf.Lerp(IK[Mathf.Clamp(currentval - 1, 0, IK.Count)].horizontalspeed, IK[currentval].horizontalspeed, timed);
        //  Debug.Log(currentspeed);

    }
    private void DirectionChange(AnimationClip clip, List<MovementTiming> IK, int layer, bool useSeconds)
    {
        // Debug.Log(layer);
        int currentval = 0;
        for (int i = 0; i < IK.Count; i++)
        {
            float currenttime2 = animator.GetCurrentAnimatorStateInfo(0).normalizedTime - Mathf.Floor(animator.GetCurrentAnimatorStateInfo(layer).normalizedTime);
            float currenttime = currenttime2;


            if (currenttime >= IK[i].Directiontiming)
            {
                if (i < IK.Count - 1)
                {
                    currentval += 1;

                }
            }
        }
        float endweight = 0;
        float startweight = 0;


        float calculatedfps;
        float calculatedrestart;
        if (useSeconds == true)
        {
            calculatedfps = endweight / clip.frameRate;           //no division, 0:06/24fps, 6/24
            calculatedrestart = startweight / clip.frameRate;
        }
        else
        {
            calculatedfps = endweight;                           //using clip length
            calculatedrestart = startweight;

        }

       


        // timed = Mathf.InverseLerp(IK[Mathf.Clamp(currentval - 1, 0, IK.Count)].Direction.x, IK[currentval].Direction.x, normalizedTime);
        float currentdirectionx = Mathf.InverseLerp(directionchange.x, IK[Mathf.Clamp(currentval, 0, IK.Count)].Direction.x, currentspeed );
        float currentdirectiony = Mathf.InverseLerp(directionchange.y, IK[Mathf.Clamp(currentval, 0, IK.Count)].Direction.y, currentspeed );
        float currentdirectionz = Mathf.InverseLerp(directionchange.z, IK[Mathf.Clamp(currentval, 0, IK.Count)].Direction.z, currentspeed );
        Vector3 currentdirection = new Vector3(currentdirectionx,currentdirectiony,currentdirectionz);
        directionchange = currentdirection;
        //  Debug.Log(currentspeed);

    }

    private bool CheckAcceleration(AnimationClip clip, List<MovementTiming> IK, int layer, bool useSeconds)
    {
        int currentval = 0;
        for (int i = 0; i < IK.Count; i++)
        {
            float currenttime2 = Mathf.Lerp(0, clip.length, (animator.GetCurrentAnimatorStateInfo(layer).normalizedTime - Mathf.Floor(animator.GetCurrentAnimatorStateInfo(layer).normalizedTime)));
            float currenttime = currenttime2;


            if (currenttime >= IK[i].speedtiming)
            {

                if (i < IK.Count - 1)
                {
                    currentval += 1;
                }

            }
        }
        float endweight = 0;
        float startweight = 0;


        float calculatedfps;
        float calculatedrestart;
        if (useSeconds == true)
        {
            calculatedfps = endweight / clip.frameRate;           //no division, 0:06/24fps, 6/24
            calculatedrestart = startweight / clip.frameRate;
        }
        else
        {
            calculatedfps = endweight;                           //using clip length
            calculatedrestart = startweight;

        }



        return (IK[Mathf.Clamp(currentval - 1, 0, IK.Count)].horizontalspeed > IK[Mathf.Clamp(currentval, 0, IK.Count)].horizontalspeed);



    }

    public enum Forward { Z, nZ, X, nX, Y, nY }
}
