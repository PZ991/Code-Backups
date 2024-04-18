using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallcover : MonoBehaviour
{
    public Collider wall;
    public MeshFilter mesh;
    public Collider target_object;
    public Transform head;
    public Collider player;
    public List<Transform> enemy;
    public Animator anim;

    public float coverdistance;
    public int maxdivision;
    public float divisiondistance;
    public float separation;
    public float realpossibledivision;
    public float line1size;
    public bool check_standing;
    public bool check_crouch;
    public bool check_prone;
    public Orientation orientation=Orientation.nY;
    public Side side;
    public Vector3[] front;
    public Vector3[] back;
    public Vector3[] left;
    public Vector3[] right;
    public CoverInfo[] frontc;
    public CoverInfo[] backc;
    public CoverInfo[] leftc;
    public CoverInfo[] rightc;
    public GameObject prefab;
    public bool check;
    public LayerMask ground;
    void Start()
    {
        if (check == true)
        {
            StartCoroutine(GoToPositions(player.transform, side));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        //Check();
        //check = true;


    }
    private void OnDrawGizmos()
    {
        #region DrawCorners
        //Gizmos.DrawSphere(RotateAroundPivot2(wall.bounds.min,wall.transform.position,Quaternion.Euler(wall.transform.eulerAngles)), 0.5f);
        //Gizmos.DrawSphere(RotateAroundPivot(wall.bounds.min,wall.transform.position,wall.transform.eulerAngles), 0.5f);
        //Gizmos.DrawSphere(wall.bounds.min, 0.5f);
        //Gizmos.DrawSphere((wall.bounds.max+wall.bounds.min)/2, 0.5f);
        //Gizmos.DrawSphere(wall.bounds.max, 0.5f);
        // 1 xyz
        #region Instance
        Mesh wallmesh = mesh.sharedMesh;
        Vector3 test=Vector3.zero;
        Vector3 red=Vector3.zero;
        Vector3 yellow = Vector3.zero;
        Vector3 green = Vector3.zero;
        Vector3 blue = Vector3.zero;
        Vector3 magenta = Vector3.zero;
        Vector3 cyan = Vector3.zero;
        Vector3 clear = Vector3.zero;
        Vector3 grey = Vector3.zero;
        Vector3[] line1 = new Vector3[4];
        #endregion

        #region Corner Vectors
        Vector3 redi = wall.transform.position + Vector3.Scale(Vector3.Scale(wallmesh.bounds.size, wall.transform.localScale) * .5f, new Vector3(1, 1, 1));
        //test = (wall.transform.position + Vector3.Scale(Vector3.Scale(wallmesh.bounds.size, wall.transform.localScale) * .5f, new Vector3(1, 1, 1)));
        Vector3 yellowi = wall.transform.position + Vector3.Scale(Vector3.Scale(wallmesh.bounds.size, wall.transform.localScale) * .5f, new Vector3(1, -1, 1));
        Vector3 greeni = wall.transform.position + Vector3.Scale(Vector3.Scale(wallmesh.bounds.size, wall.transform.localScale) * .5f, new Vector3(1, -1, -1));
        Vector3 bluei = wall.transform.position + Vector3.Scale(Vector3.Scale(wallmesh.bounds.size, wall.transform.localScale) * .5f, new Vector3(-1, -1, -1));
        Vector3 magentai = wall.transform.position + Vector3.Scale(Vector3.Scale(wallmesh.bounds.size, wall.transform.localScale) * .5f, new Vector3(-1, 1, -1));
        Vector3 cyani = wall.transform.position + Vector3.Scale(Vector3.Scale(wallmesh.bounds.size, wall.transform.localScale) * .5f, new Vector3(-1, 1, 1));
        Vector3 cleari = wall.transform.position + Vector3.Scale(Vector3.Scale(wallmesh.bounds.size, wall.transform.localScale) * .5f, new Vector3(1, 1, -1));
        Vector3 greyi = wall.transform.position + Vector3.Scale(Vector3.Scale(wallmesh.bounds.size, wall.transform.localScale) * .5f, new Vector3(-1, -1, 1));

        //real
        Vector3 targetposred = wall.transform.position - (wall.transform.position - redi);
         red = RotateAroundPivot2(targetposred, wall.transform.position, wall.transform.localRotation);
        
        Vector3 targetposyellow = wall.transform.position - (wall.transform.position - yellowi);
         yellow = RotateAroundPivot2(targetposyellow, wall.transform.position, wall.transform.localRotation);
        
        Vector3 targetposgreen = wall.transform.position - (wall.transform.position - greeni);
         green = RotateAroundPivot2(targetposgreen, wall.transform.position, wall.transform.localRotation);
        
        Vector3 targetposblue = wall.transform.position - (wall.transform.position - bluei);
         blue = RotateAroundPivot2(targetposblue, wall.transform.position, wall.transform.localRotation);
        
        Vector3 targetposmagenta = wall.transform.position - (wall.transform.position - magentai);
         magenta = RotateAroundPivot2(targetposmagenta, wall.transform.position, wall.transform.localRotation);
        
        Vector3 targetposcyan = wall.transform.position - (wall.transform.position - cyani);
         cyan = RotateAroundPivot2(targetposcyan, wall.transform.position, wall.transform.localRotation);
        
        Vector3 targetposclear = wall.transform.position - (wall.transform.position - cleari);
         clear = RotateAroundPivot2(targetposclear, wall.transform.position, wall.transform.localRotation);
        
        Vector3 targetposgrey = wall.transform.position - (wall.transform.position - greyi);
         grey = RotateAroundPivot2(targetposgrey, wall.transform.position, wall.transform.localRotation);
        
        
        #endregion

        //Gizmos.DrawSphere(realpos, 1); 
        
        #region Corners
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(red, 0.5f);
        // 2 x-yz
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(yellow, 0.5f);
        // 3 x-y-z
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(green,  0.5f);
        // 4 -x-y-z
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(blue, 0.5f);
        // 5 -xy-z
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(magenta, 0.5f);
        // 6 -x,yz
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(cyan, 0.5f);
        // 7 xy-z
        Gizmos.color = Color.clear;
        Gizmos.DrawSphere(clear, 0.5f);
        // 8 -x-yz
        Gizmos.color = Color.grey;
        Gizmos.DrawSphere(grey, 0.5f);
        #endregion



        #region Orientation
        switch (orientation)
        {
            case Orientation.Y:
                {
                    
                    // Top
                    line1[0] = clear;
                    line1[1] = magenta;
                    line1[2] = cyan;
                    line1[3] = red;

                    Gizmos.color = Color.green;

                    break;
                }
            case Orientation.nY:
                {
                    // Bottom
                    line1[0] = grey;
                    line1[1] = blue;
                    line1[2] = green;
                    line1[3] = yellow;
                    Gizmos.color = Color.green;


                    
                    break;
                }
                case Orientation.Z:
                {
                    // Forward
                    line1[0] = red;
                    line1[1] = cyan;
                    line1[2] = grey;
                    line1[3] = yellow;

                    Gizmos.color = Color.blue;

                    break;
                }
                case Orientation.nZ:
                {
                    // Back
                    line1[0] =blue;
                    line1[1] = magenta;
                    line1[2] = clear;
                    line1[3] = green;

                    Gizmos.color = Color.blue;

                    break;
                }
                case Orientation.nX:
                {
                    // Right
                    line1[0] = magenta;
                    line1[1] = cyan;
                    line1[2] = grey;
                    line1[3] = blue;

                    Gizmos.color = Color.red;

                    break;
                }
                case Orientation.X:
                {
                    // Left
                    line1[0] = clear;
                    line1[1] = red;
                    line1[2] = yellow;
                    line1[3] = green;

                    Gizmos.color = Color.red;

                    break;
                }

        }
        Gizmos.DrawLine(line1[0], line1[1]);
        Gizmos.DrawLine(line1[1], line1[2]);
        Gizmos.DrawLine(line1[2], line1[3]);
        Gizmos.DrawLine(line1[3], line1[0]);

        #endregion


        #endregion

        #region Side Vectors
        if (side == Side.Back)
        {
            if (target_object != null)
            {
                line1size = Vector3.Distance(line1[1], line1[2]);
                divisiondistance = Vector3.Distance(RotateAroundPivot((target_object.bounds.center + new Vector3(-target_object.transform.localScale.x, -target_object.transform.localScale.y, target_object.transform.localScale.z) * 0.5f), target_object.transform.position, target_object.transform.eulerAngles), RotateAroundPivot((target_object.bounds.center + new Vector3(target_object.transform.localScale.x, -target_object.transform.localScale.y, target_object.transform.localScale.z) * 0.5f), target_object.transform.position, target_object.transform.eulerAngles));
                divisiondistance = Mathf.Round((divisiondistance + separation) / 0.625f) * 0.625f;
            }

            //if (divisiondistance % 0.625f == 0)
               // Gizmos.DrawSphere(Vector3.Lerp(line1[1], line1[2], divisiondistance / Vector3.Distance(line1[1], line1[2])), 0.5f);
            float realpossible = line1size / divisiondistance;
            realpossibledivision = Mathf.FloorToInt(realpossible);

            float leftover = Vector3.Distance(Vector3.Lerp(line1[1], line1[2], (divisiondistance / Vector3.Distance(line1[1], line1[2]) * realpossibledivision)), line1[2]);
            float excess = leftover / realpossibledivision;

            Vector3[] initial = new Vector3[((int)realpossibledivision) + 1];
            for (int i = 0; i <= realpossibledivision; i++)
            {
                Gizmos.color = Color.black;
                initial[i] = Vector3.Lerp(line1[1], line1[2], ((divisiondistance+excess) / Vector3.Distance(line1[1], line1[2]) * i));
                Gizmos.DrawSphere(initial[i], 0.5f);

            }

            back = new Vector3[initial.Length - 1];

            for (int i = 0; i < initial.Length - 1; i++)
            {
                Gizmos.color = Color.white;
                Vector3 final = Vector3.Lerp(initial[i], initial[i + 1], 0.5f);
                Gizmos.DrawSphere(final, 0.5f);
                back[i] = final;
            }
        }

        else if(side==Side.Front)
        {
            if (target_object != null)
            {
                line1size = Vector3.Distance(line1[3], line1[0]);
                divisiondistance = Vector3.Distance(RotateAroundPivot((target_object.bounds.center + new Vector3(-target_object.transform.localScale.x, -target_object.transform.localScale.y, target_object.transform.localScale.z) * 0.5f), target_object.transform.position, target_object.transform.eulerAngles), RotateAroundPivot((target_object.bounds.center + new Vector3(target_object.transform.localScale.x, -target_object.transform.localScale.y, target_object.transform.localScale.z) * 0.5f), target_object.transform.position, target_object.transform.eulerAngles));
                divisiondistance = Mathf.Round((divisiondistance + separation) / 0.625f) * 0.625f;
            }

            //if (divisiondistance % 0.625f == 0)
               // Gizmos.DrawSphere(Vector3.Lerp(line1[3], line1[0], divisiondistance / Vector3.Distance(line1[3], line1[0])), 0.5f);
            float realpossible = line1size / divisiondistance;
            realpossibledivision = Mathf.FloorToInt(realpossible);

            float leftover = Vector3.Distance(Vector3.Lerp(line1[3], line1[0], (divisiondistance / Vector3.Distance(line1[3], line1[0]) * realpossibledivision)), line1[0]);
            float excess = leftover / realpossibledivision;
            Debug.Log(leftover);

            Vector3[] initial = new Vector3[((int)realpossibledivision) + 1];
            for (int i = 0; i <= realpossibledivision; i++)
            {
                Gizmos.color = Color.black;
                initial[i] = Vector3.Lerp(line1[3], line1[0], ((divisiondistance+excess) / Vector3.Distance(line1[3], line1[0]) * i));
                Gizmos.DrawSphere(initial[i], 0.5f);

            }

            front = new Vector3[initial.Length - 1];

            for (int i = 0; i < initial.Length - 1; i++)
            {
                Gizmos.color = Color.white;
                Vector3 final = Vector3.Lerp(initial[i], initial[i + 1], 0.5f);
                Gizmos.DrawSphere(final, 0.5f);
                front[i] = final;
            }
        }

        else if(side==Side.Left)
        {
            #region LeftSide
            if (target_object != null)
            {
                line1size = Vector3.Distance(line1[2], line1[3]);
                divisiondistance = Vector3.Distance(RotateAroundPivot((target_object.bounds.center + new Vector3(-target_object.transform.localScale.x, -target_object.transform.localScale.y, target_object.transform.localScale.z) * 0.5f), target_object.transform.position, target_object.transform.eulerAngles), RotateAroundPivot((target_object.bounds.center + new Vector3(target_object.transform.localScale.x, -target_object.transform.localScale.y, target_object.transform.localScale.z) * 0.5f), target_object.transform.position, target_object.transform.eulerAngles));
                divisiondistance = Mathf.Round((divisiondistance + separation) / 0.625f) * 0.625f;
            }

            //if (divisiondistance % 0.625f == 0)
                //Gizmos.DrawSphere(Vector3.Lerp(line1[2], line1[3], divisiondistance / Vector3.Distance(line1[2], line1[3])), 0.5f);
            float realpossible = (line1size / divisiondistance);
            
                
            
            realpossibledivision = Mathf.FloorToInt(realpossible);

            float leftover = Vector3.Distance(Vector3.Lerp(line1[2], line1[3], ( divisiondistance / Vector3.Distance(line1[2], line1[3]) * realpossibledivision)),line1[3]);
            float excess=leftover / realpossibledivision;
           

            Vector3[] initial = new Vector3[((int)realpossibledivision) + 1];
            for (int i = 0; i <= realpossibledivision; i++)
            {
                
                    Gizmos.color = Color.black;
                    initial[i] = Vector3.Lerp(line1[2], line1[3], ((divisiondistance+excess) / Vector3.Distance(line1[2], line1[3]) * i));
                    Gizmos.DrawSphere(initial[i], 0.5f);
                
                
            }

            left = new Vector3[initial.Length - 1];

            for (int i = 0; i < initial.Length - 1; i++)
            {
                Gizmos.color = Color.white;
                Vector3 final = Vector3.Lerp(initial[i], initial[i + 1], 0.5f);
                Gizmos.DrawSphere(final, 0.5f);
                left[i] = final;
            }
            #endregion
        }

        else if(side==Side.Right)
        {
            if (target_object != null)
            {
                line1size = Vector3.Distance(line1[0], line1[1]);
                divisiondistance = Vector3.Distance(RotateAroundPivot((target_object.bounds.center + new Vector3(-target_object.transform.localScale.x, -target_object.transform.localScale.y, target_object.transform.localScale.z) * 0.5f), target_object.transform.position, target_object.transform.eulerAngles), RotateAroundPivot((target_object.bounds.center + new Vector3(target_object.transform.localScale.x, -target_object.transform.localScale.y, target_object.transform.localScale.z) * 0.5f), target_object.transform.position, target_object.transform.eulerAngles));
                divisiondistance = Mathf.Round((divisiondistance + separation) / 0.625f) * 0.625f;
            }

            //if (divisiondistance % 0.625f == 0)
               // Gizmos.DrawSphere(Vector3.Lerp(line1[0], line1[1], divisiondistance / Vector3.Distance(line1[0], line1[3])), 0.5f);
            float realpossible = line1size / divisiondistance;
            realpossibledivision = Mathf.FloorToInt(realpossible);

            float leftover = Vector3.Distance(Vector3.Lerp(line1[0], line1[1], (divisiondistance / Vector3.Distance(line1[0], line1[1]) * realpossibledivision)), line1[1]);
            float excess = leftover / realpossibledivision;

            Vector3[] initial = new Vector3[((int)realpossibledivision) + 1];
            for (int i = 0; i <= realpossibledivision; i++)
            {
                Gizmos.color = Color.black;
                initial[i] = Vector3.Lerp(line1[0], line1[1], ((divisiondistance+excess) / Vector3.Distance(line1[0], line1[1]) * i));
                Gizmos.DrawSphere(initial[i], 0.5f);

            }

            right = new Vector3[initial.Length - 1];

            for (int i = 0; i < initial.Length - 1; i++)
            {
                Gizmos.color = Color.white;
                Vector3 final = Vector3.Lerp(initial[i], initial[i + 1], 0.5f);
                Gizmos.DrawSphere(final, 0.5f);
                right[i] = final;
            }
        }

        #endregion

        #region Checkside
        //var relativepoint = transform.InverseTransformPoint(player.transform.position);
        #region V1
        
        Vector3 direction1 = (Quaternion.Euler(0, -135, 0) * (line1[0] - line1[1]));
        Vector3 direction2 = (Quaternion.Euler(0, -135, 0) * (line1[1] - line1[2]));
        Vector3 direction3 = (Quaternion.Euler(0, -135, 0) * (line1[2] - line1[3]));
        Vector3 direction4 = (Quaternion.Euler(0, -135, 0) * (line1[3] - line1[0]));
        Gizmos.color = Color.black;
        //Gizmos.DrawRay(line1[1], direction1);   //>60
        Gizmos.DrawRay(line1[1], direction1.normalized);
        Gizmos.DrawRay(line1[0], line1[1]-line1[0]);
        Gizmos.DrawRay(line1[0], direction4.normalized);
        Gizmos.DrawRay(line1[0], line1[0]-line1[3]);
        Gizmos.DrawRay(line1[3], direction3.normalized);
        Gizmos.DrawRay(line1[2], direction2.normalized);


        Gizmos.color = Color.red;
        Gizmos.DrawRay(line1[0], enemy[0].position-line1[0]);
        Gizmos.DrawRay(line1[1], enemy[0].position-line1[1]);
        Gizmos.DrawRay(line1[3], enemy[0].position-line1[3]);
        Gizmos.DrawRay(line1[2], enemy[0].position-line1[2]);

        Vector3 averagepos = Vector3.zero;
        for (int i = 0; i < enemy.Count; i++)
        {
            averagepos += enemy[i].transform.position;

        }
        averagepos = averagepos / enemy.Count;
        Gizmos.color = Color.red;
        Gizmos.DrawCube(averagepos, Vector3.one);


        //if(Vector3.Angle(direction2, Quaternion.Euler(0, -135, 0) * enemy[0].position-line1[2])<Vector3.Angle(direction1,direction2)&&Vector3.Angle(direction1, Quaternion.Euler(0, -135, 0) * enemy[0].position-line1[1])<Vector3.Angle(direction1,direction2))
        float angledblue = Vector3.SignedAngle(direction1.normalized, line1[1] - line1[0], Vector3.up);
        float angledgrey = Vector3.SignedAngle(direction4.normalized,line1[0] - line1[1], Vector3.up);
        float angledyellow = Vector3.SignedAngle(direction3.normalized,line1[3] - line1[0], Vector3.up);
        float angledgreen = Vector3.SignedAngle(direction2.normalized,line1[2] - line1[1], Vector3.up);
        
        
        float angledb = Vector3.SignedAngle((line1[1] - averagepos).normalized, line1[0]-line1[1], Vector3.up);
        float angledgy = Vector3.SignedAngle((line1[0] - averagepos).normalized,line1[1]-line1[0], Vector3.up);
        float angledy = Vector3.SignedAngle((line1[3] - averagepos).normalized,line1[0]-line1[3], Vector3.up);
        float angledg = Vector3.SignedAngle((line1[2] - averagepos).normalized,line1[1]-line1[2], Vector3.up);

        if (angledb < angledblue && angledgy > angledgrey)
            side = Side.Right;
        else if (angledgy < angledgrey&& angledy > angledyellow)

            side = Side.Front;
        else if (angledy < angledyellow && angledg > angledgreen)
            side = Side.Left;
        else if (angledg < angledgreen && angledb > angledblue)
            side = Side.Back;

        #endregion
        #region V2
        /*
        Vector3 averagepos =Vector3.zero;
        for (int i = 0; i < enemy.Count; i++)
        {
            averagepos += enemy[i].transform.position;

        }
        averagepos =averagepos/ enemy.Count;
        Gizmos.color = Color.red;
        Gizmos.DrawCube(averagepos, Vector3.one);
        float[] cornerdist = new float[4];
         cornerdist[0] = Vector3.Distance(averagepos, line1[0]);
         cornerdist[1] = Vector3.Distance(averagepos, line1[1]);
         cornerdist[2] = Vector3.Distance(averagepos, line1[2]);
         cornerdist[3] = Vector3.Distance(averagepos, line1[3]);

        float min= Mathf.Min(cornerdist);
        float min2 = Mathf.Max(cornerdist);
        int smallest = 0;
        int secondsmallest = 0;
        for (int i = 0; i < cornerdist.Length; i++)
        {
            if(cornerdist[i]==min)
            {
                smallest = i;
                break;
            }
        }

        
        
        for (int i = 0; i < cornerdist.Length; i++)
        {
            if(cornerdist[i]!=min&&cornerdist[i]<min2)
            {
                min2 = cornerdist[i];
                secondsmallest = i;
            }
        }

        if((smallest == 0&& secondsmallest == 1)||(smallest == 1&& secondsmallest == 0))
        {
            side = Side.Left;
            //side = Side.Right;
        }
        else if ((smallest == 1 && secondsmallest == 2) || (smallest == 2 && secondsmallest == 1))
        {
            //side = Side.Back;
            side = Side.Front;
        }
        else if ((smallest == 2 && secondsmallest == 3) || (smallest == 3 && secondsmallest == 2))
        {
            //side = Side.Left;
            side = Side.Right;
        }
        else if ((smallest == 3 && secondsmallest == 0) || (smallest == 0 && secondsmallest == 3))
        {
            //side = Side.Front;
            side = Side.Back;
        }
        */
        #endregion
        //grey blue green yellow
        #region V3
        /*
        if (orientation == Orientation.nY)
        {
            if (Mathf.Abs(relativepoint.x) > Mathf.Abs(relativepoint.z))
            {
                if (relativepoint.x < 0.0f)
                    side = Side.Right;
                else if (relativepoint.x > 0.0f)
                    side = Side.Left;
            }
            else if (Mathf.Abs(relativepoint.z) > Mathf.Abs(relativepoint.x))
            {
                if (relativepoint.z > 0.0f)
                    side = Side.Front;
                else if (relativepoint.z < 0.0f)
                    side = Side.Back;
            }
        }
        else if(orientation==Orientation.nZ)
        {
            if (Mathf.Abs(relativepoint.x) > Mathf.Abs(relativepoint.y))
            {
                if (relativepoint.x < 0.0f)
                    side = Side.Right;
                else if (relativepoint.x > 0.0f)
                    side = Side.Left;
            }
            else if (Mathf.Abs(relativepoint.y) > Mathf.Abs(relativepoint.x))
            {
                if (relativepoint.y > 0.0f)
                    side = Side.Back;
                else if (relativepoint.y < 0.0f)
                    side = Side.Front;
            }
        }
        */
        #endregion
        //Debug.DrawLine(left[0], left[0] + Vector3.left);

        //player.transform.position = left[0]-(Vector3.left*coverdistance);
        //player.transform.rotation = Quaternion.LookRotation(left[0]-(left[0] - -transform.right));

        #endregion

        #region GroundCover
        /*
        for (int i = 0; i < left.Length; i++)
        {
            RaycastHit hit;
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(left[i], Vector3.down*10);
            if(Physics.Raycast(left[i],Vector3.down,out hit,100f))
            {
                Gizmos.DrawSphere(hit.point, 0.5f);
            }
        }
        */
        #endregion

        //frontc = new CoverInfo[1];
        //frontc[0] = new CoverInfo();
        //Debug.Log(frontc[0].crouch_covered);


        //Gizmos.DrawRay(head.transform.position, ( new Vector3(wall.transform.position.x, head.transform.position.y, wall.transform.position.z)- new Vector3(head.transform.position.x, head.transform.position.y, head.transform.position.z)));
    }
    public IEnumerator GoToPositions(Transform target,Side side)
    {
        Vector3 realdirect = Vector3.zero;
        Vector3[] positions = new Vector3[0];
        //Debug.Log("start");
        switch (side)
            {
                case Side.Front:
                    realdirect = Vector3.forward;
                    frontc = new CoverInfo[front.Length];
                for (int i = 0; i < frontc.Length; i++)
                {
                    frontc[i] = new CoverInfo();
                }
                 positions = front;
                    break;
                case Side.Back:
                    realdirect = Vector3.back;
                    backc = new CoverInfo[back.Length];
                 positions = back;
                    break;
                case Side.Left:
                    realdirect = Vector3.left;
                    leftc = new CoverInfo[left.Length];
                 positions = left;
                    break;
                case Side.Right:
                    realdirect = Vector3.right;
                    rightc = new CoverInfo[right.Length];
               positions = right;
                    break;
                
            }
        for (int i = 0; i < positions.Length; i++)
        {
            //Debug.Log("positions");
            RaycastHit hit;
            if (Physics.Raycast(positions[i], Vector3.down, out hit, 100f,ground))
            {
                //Debug.Log(hit.point);
                //yield return new WaitForSeconds(0.5f);

                target.transform.position = hit.point+(Vector3.forward*coverdistance);
                target.transform.rotation = Quaternion.LookRotation(hit.point - (hit.point - realdirect));
                switch (side)
                {
                    case Side.Front:

                        
                        StartCoroutine(Check(i, frontc));
                        
                        break;
                    case Side.Back:
                        StartCoroutine(Check(i, backc));
                        break;
                    case Side.Left:
                        StartCoroutine(Check(i, leftc));
                        break;
                    case Side.Right:
                        StartCoroutine(Check(i, rightc));
                        break;

                }
                //Gizmos.DrawSphere(hit.point, 0.5f);
            }
            
            
            
            
            yield return new WaitForSeconds(1);
        }
    }

    public IEnumerator Check(int i, CoverInfo[] cover)
    {
        Debug.Log("Check");
        RaycastHit hit;
        check_standing = true;

        if (check_standing)
        {
            anim.CrossFade("wall_cover_stand", 0, 0, 1);
            yield return new WaitForSeconds(0.01f);

            Debug.Log("stand");

            Physics.Raycast(head.transform.position, (new Vector3(wall.transform.position.x, head.transform.position.y, wall.transform.position.z) - new Vector3(head.transform.position.x, head.transform.position.y, head.transform.position.z)), out hit, 100, wall.gameObject.layer);
            
                Debug.DrawRay(head.transform.position, (new Vector3(wall.transform.position.x, head.transform.position.y, wall.transform.position.z) - new Vector3(head.transform.position.x, head.transform.position.y, head.transform.position.z)), Color.cyan, 10);

                if (hit.collider!= null)
                {
                    cover[i].standing_covered = true;
                }
                else
                {
                    cover[i].standing_covered = false;
                }
            
            check_crouch = true;
        }
        yield return new WaitForSeconds(0.01f);
        if (check_crouch)
        {
            anim.CrossFade("wall_cover_crouch", 0, 0, 1);
            yield return new WaitForSeconds(0.01f);

            Debug.Log("crouch");
            Physics.Raycast(head.transform.position, (new Vector3(wall.transform.position.x, head.transform.position.y, wall.transform.position.z) - new Vector3(head.transform.position.x, head.transform.position.y, head.transform.position.z)), out hit, 100);

            Debug.DrawRay(head.transform.position, (new Vector3(wall.transform.position.x, head.transform.position.y, wall.transform.position.z) - new Vector3(head.transform.position.x, head.transform.position.y, head.transform.position.z)), Color.magenta, 10);

            if (hit.collider != null)
                {
                    cover[i].crouch_covered = true;
                }
                else
                {
                    cover[i].crouch_covered = false;
                }

            
            check_standing = false;
            check_prone = true;
            Debug.Log(frontc[i].crouch_covered + " " + i);
        }
        yield return new WaitForSeconds(0.01f);
        if (check_prone)
        {
            anim.CrossFade("wall_cover_prone", 0, 0, 1);
            yield return new WaitForSeconds(0.01f);

            Debug.Log("prone");
            Physics.Raycast(head.transform.position, (new Vector3(wall.transform.position.x, head.transform.position.y, wall.transform.position.z) - new Vector3(head.transform.position.x, head.transform.position.y, head.transform.position.z)), out hit, 100);
            if (hit.collider != null)
            {
                Debug.DrawRay(head.transform.position, (new Vector3(wall.transform.position.x, head.transform.position.y, wall.transform.position.z) - new Vector3(head.transform.position.x, head.transform.position.y, head.transform.position.z)), Color.red, 10);

                if (hit.collider != null)//hit.collider.name == wall.name)
                {
                    cover[i].prone_covered = true;
                }
                else
                {
                    cover[i].prone_covered = false;
                }

            }
            check_crouch = false;
            check_prone = false;
        }
        check = false;
        yield return null;
    }
    void GetCorners(Collider wall,Transform target, Transform position)
    {
    }

    

    Vector3 RotateAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot;
        dir = Quaternion.Euler(angles) * dir;
        point = dir + pivot;
        return point;
        //return Quaternion.Euler(angles) * (  pivot-point) + pivot;
    }

    Vector3 RotateAroundPivot2(Vector3 point, Vector3 pivot, Quaternion angles)
    {
        var finalpos=point-pivot;
        finalpos = angles * finalpos;
        finalpos += pivot;
        return finalpos;
        /*
        var finalpos=point-pivot;
        finalpos = angles * finalpos;
        point = finalpos+pivot;
        return point;
        
        */
    }

    
}
public enum Orientation { Z,nZ,Y,nY,X,nX };
public enum Side { Front,Back,Left,Right};


