using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublesideCover : MonoBehaviour
{
    // Start is called before the first frame update
    //public Vector3 newpos;
    //public Vector3 size;

    public Collider wall;
    public MeshFilter mesh;
    public DoubleSideOrientation orientation = DoubleSideOrientation.Y_nY;

    public float gridwidth;
    public float gridheight;
    //public float line1size;
    public LayerMask ground;
    public Vector3[,] grid;
    public Vector3[,] oppositegrid;
    public Vector3[,] midgrid;
    public Vector3[,] oppositemidgrid;
    private Vector3 rot;
    private Vector3 direction;
    private Vector3 othersidepos;
    public Transform testraycast;
    public List<Vector3> wallhit;
    public List<Vector3> wallhitoffset;
    public List<Vector3> oppositewallhit;
    public List<Vector3> oppositewallhitoffset;
    public bool selfonly;
    public List<Vector3> positions;
    public List<Vector3> positions2;
    public List<Vector3> positionsoffset;
    public List<Vector3> positions2offset;

    public List<Vector3> closestpositions2;
    public bool add;
    public bool refresh;
    public bool show;
    public Vector3 realpos;
    public bool once;

    public bool check_standing;
    public bool check_crouch;
    public bool check_prone;
    public Transform head;
    public Animator anim;
    public CoverInfo[] covers=new CoverInfo[1];
    public bool check;
    public float coverdistance;
    public Transform target;
    public int checker;
    void Start()
    {
        if (check)
        {


            StartCoroutine(GoToPositions(target));
        }
    }

    // Update is called once per frame
    

    private void OnDrawGizmos()
    {
        /*
        RaycastHit[] hit=Physics.RaycastAll(transform.position, transform.forward, 100);
        RaycastHit[] hit2=Physics.RaycastAll(transform.position+newpos, -transform.forward, 100);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward);
        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position, -transform.forward);
        Gizmos.color = Color.green;

        for (int i = 0; i < hit.Length; i++)
        {
            
                Gizmos.DrawSphere(hit[i].point+size, 0.5f);
            
        }
        Gizmos.color = Color.red;

        for (int i = 0; i < hit2.Length; i++)
        {
            
                Gizmos.DrawSphere(hit2[i].point-size, 0.5f);
            
        }

        */

        if (show)
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
            Vector3 test = Vector3.zero;
            Vector3 red = Vector3.zero;
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
            Gizmos.DrawSphere(green, 0.5f);
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
                case DoubleSideOrientation.Y_nY:
                    {

                        // Top
                        line1[0] = clear;
                        line1[1] = magenta;
                        line1[2] = cyan;
                        line1[3] = red;

                        Gizmos.color = Color.green;
                        rot = new Vector3(0, 0, 0);
                        direction = Vector3.down;
                        othersidepos = new Vector3(0, -(Vector3.Distance(red, yellow) + 20), 0);
                        break;
                    }
                case DoubleSideOrientation.Z_nZ:
                    {
                        // Forward
                        line1[0] = red;
                        line1[1] = cyan;
                        line1[2] = grey;
                        line1[3] = yellow;

                        Gizmos.color = Color.blue;
                        othersidepos = new Vector3(0, 0, -(Vector3.Distance(clear, red) + 20));
                        rot = new Vector3(90, 0, 0);
                        direction = Vector3.back;
                        break;
                    }
                case DoubleSideOrientation.X_nX:
                    {
                        // Right
                        line1[0] = cyan;
                        line1[1] = magenta;
                        line1[2] = blue;
                        line1[3] = grey;

                        Gizmos.color = Color.red;
                        othersidepos = new Vector3((Vector3.Distance(cyan, red) + 20), 0, 0);
                        rot = new Vector3(90, 0, 90);
                        direction = Vector3.right;
                        break;
                    }

            }
            Gizmos.DrawLine(line1[0], line1[1]);
            Gizmos.DrawLine(line1[1], line1[2]);
            Gizmos.DrawLine(line1[2], line1[3]);
            Gizmos.DrawLine(line1[3], line1[0]);

            #endregion


            #endregion



            #region Diagonal
            //else if (side == Side.Right)
            {



                //Debug.Log(Vector3.Distance(back[0], front[0]));

                Vector3 a = line1[2];
                Vector3 b = line1[3];
                Vector3 c = line1[1];

                float width = 0;

                width = Vector3.Distance(a, b) / gridwidth;
                float height = 0;
                height = Vector3.Distance(a, c) / gridheight;
                //Debug.Log(height);
                grid = new Vector3[(int)gridwidth + 1, (int)gridheight + 1];
                oppositegrid = new Vector3[(int)gridwidth + 1, (int)gridheight + 1];
                for (int x = 0; x <= gridwidth; x++)
                {
                    for (int y = 0; y <= gridheight; y++)
                    {
                        Vector3 posxy = new Vector3(x * width, 10, (y * height)) + line1[1];
                        Vector3 targetposxy = line1[1] - (line1[1] - posxy);
                        Vector3 finalxy = Vector3.zero;

                        finalxy = RotateAroundPivot2(targetposxy, line1[1], Quaternion.Euler(rot));
                        //finalxy = RotateAroundPivot2(RotateAroundPivot2(targetposxy, line1[1], Quaternion.Euler(rot)),transform.position,Quaternion.Euler(transform.eulerAngles));
                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere(finalxy, 0.5f);

                        //Ray ray3 = new Ray(finalxy, direction * 100);
                        //Gizmos.color = Color.blue;
                        //Gizmos.DrawRay(ray3);
                        //RaycastHit hit3 = new RaycastHit();
                        //Physics.Raycast(ray3, out hit3, 100, ground);
                        grid[x, y] = finalxy;
                        oppositegrid[x, y] = othersidepos + grid[x, y];
                        Gizmos.color = Color.blue;
                        Gizmos.DrawSphere(oppositegrid[x, y], 0.5f);
                    }
                }


                midgrid = new Vector3[grid.GetLength(0) - 1, grid.GetLength(1) - 1];
                oppositemidgrid = new Vector3[grid.GetLength(0) - 1, grid.GetLength(1) - 1];
                for (int x = 0; x < grid.GetLength(0) - 1; x++)
                {
                    for (int y = 0; y < grid.GetLength(1) - 1; y++)
                    {
                        Vector3 pos = (grid[x, y] + grid[x + 1, y] + grid[x, y + 1] + grid[x + 1, y + 1]) / 4;

                        midgrid[x, y] = pos;
                        oppositemidgrid[x, y] = othersidepos + midgrid[x, y];
                    }
                }

                wallhit.Clear();
                wallhitoffset.Clear();
                oppositewallhit.Clear();
                oppositewallhitoffset.Clear();
                for (int x = 0; x < midgrid.GetLength(0); x++)
                {
                    for (int y = 0; y < midgrid.GetLength(1); y++)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawCube(midgrid[x, y], Vector3.one);
                        Gizmos.color = Color.black;

                        Gizmos.DrawCube(oppositemidgrid[x, y], Vector3.one);
                        Gizmos.DrawRay(midgrid[x, y], direction);
                        RaycastHit hitting = new RaycastHit();
                        RaycastHit hitting2 = new RaycastHit();
                        /*
                        hitting =Physics.RaycastAll(midgrid[x, y], direction, 100);
                        hitting2 =Physics.RaycastAll(oppositemidgrid[x, y], -direction, 100);
                        //Debug.Log(hitting.Length);
                        for (int i = 0; i < hitting.Length; i++)
                        {
                            Gizmos.color = Color.magenta; 
                            Gizmos.DrawWireCube(hitting[i].point, Vector3.one);

                        }
                        for (int i = 0; i < hitting2.Length; i++)
                        {
                            Gizmos.color = Color.yellow; 
                            Gizmos.DrawWireCube(hitting2[i].point, Vector3.one);

                        }
                        */
                        if (Physics.Raycast(midgrid[x, y], direction, out hitting, 100))
                        {

                            if (selfonly && hitting.collider.name == transform.name)
                            {


                                if (hitting.collider != null)
                                {
                                    Gizmos.color = Color.black;

                                    Gizmos.DrawWireSphere(hitting.point, 0.5f);
                                    wallhit.Add(hitting.point);
                                    wallhitoffset.Add(Vector3.Lerp(hitting.point, midgrid[x, y], 0.1f));
                                        //Gizmos.color = Color.blue;
                                        //Gizmos.DrawWireSphere(Vector3.Lerp(hitting.point, midgrid[x, y], 0.1f), 0.5f);
                                    
                                    RepeatRaycast(hitting.point, direction, false);
                                }
                                Physics.Raycast(oppositemidgrid[x, y], -direction, out hitting2, 100);
                                if (hitting.collider != null)
                                {
                                    Gizmos.color = Color.cyan;
                                    Gizmos.DrawWireSphere(hitting2.point, 0.5f);
                                    oppositewallhit.Add(hitting2.point);
                                    oppositewallhitoffset.Add(Vector3.Lerp(hitting2.point, oppositemidgrid[x, y], 0.1f));
                                    //Gizmos.color = Color.cyan;
                                    //Gizmos.DrawWireSphere(Vector3.Lerp(hitting2.point, oppositemidgrid[x, y], 0.1f), 0.5f);

                                    RepeatRaycast(hitting2.point, -direction, true);
                                }
                            }
                            else if (selfonly == false)
                            {
                                if (hitting.collider != null)
                                {
                                    Gizmos.color = Color.black;
                                    Gizmos.DrawWireSphere(hitting.point, 0.5f);
                                    wallhit.Add(hitting.point);
                                    RepeatRaycast(hitting.point, direction, false);
                                }
                                Physics.Raycast(oppositemidgrid[x, y], -direction, out hitting2, 100);
                                if (hitting.collider != null)
                                {
                                    Gizmos.color = Color.cyan;
                                    Gizmos.DrawWireSphere(hitting2.point, 0.5f);
                                    oppositewallhit.Add(hitting2.point);
                                    RepeatRaycast(hitting2.point, -direction, true);
                                }
                            }
                        }
                        //Gizmos.DrawSphere(new Vector3(0, -Vector3.Distance(red, yellow)-10, 0) + midgrid[x, y], 0.5f);
                    }
                }
                //right = initial;
                /*
                right = new Vector3[initial.Length - 1];

                for (int i = 0; i < initial.Length - 1; i++)
                {
                    Gizmos.color = Color.white;
                    Vector3 final = Vector3.Lerp(initial[i], initial[i + 1], 0.5f);
                    Gizmos.DrawSphere(final, 0.5f);
                    right[i] = final;
                }
                */

            }
            #endregion


            if (refresh)
            {
                positions.Clear();
                foreach (Vector3 pos1 in wallhit)
                {
                    positions.Add(pos1);
                }
                foreach (Vector3 pos2 in oppositewallhit)
                {
                    positions.Add(pos2);
                }
                foreach (Vector3 pos3 in wallhitoffset)
                {
                    positionsoffset.Add(pos3);
                }
                foreach (Vector3 pos4 in oppositewallhitoffset)
                {
                    positionsoffset.Add(pos4);
                }

                refresh = false;
            }
            if (add)
            {
                foreach (Vector3 pos1 in wallhit)
                {
                    positions.Add(pos1);
                }
                foreach (Vector3 pos2 in oppositewallhit)
                {
                    positions.Add(pos2);
                }
                foreach (Vector3 pos3 in wallhitoffset)
                {
                    positionsoffset.Add(pos3);
                }
                foreach (Vector3 pos2 in oppositewallhitoffset)
                {
                    positionsoffset.Add(pos2);
                }
                add = false;
            }
        }
        if (once == false)
        {
            positions2.Clear();
            

            foreach (Vector3 finalpos in positions)
            {
               // Gizmos.color = Color.blue;

                //Vector3 targetposgrey2 =  transform.InverseTransformPoint(finalpos);

                //Vector3 targetpos = RotateAroundPivot2(targetposgrey2, wall.transform.position, wall.transform.localRotation);
                //Vector3 targetposgrey = wall.transform.position - (wall.transform.position - (finalpos));
                //Vector3 targetposgrey3= RotateAroundPivot2(targetposgrey, wall.transform.position, wall.transform.localRotation);

                //Gizmos.DrawWireSphere(targetposgrey,0.5f);
                //Vector3 closestpoint = transform.GetComponent<Collider>().ClosestPoint(finalpos);
                //Gizmos.color = Color.black;
                Vector3 offset = transform.InverseTransformPoint(finalpos);
                

                positions2.Add(offset);
            }
            foreach (Vector3 finalpos in positionsoffset)
            {
                //Gizmos.color = Color.blue;

                //Vector3 targetposgrey2 =  transform.InverseTransformPoint(finalpos);

                //Vector3 targetpos = RotateAroundPivot2(targetposgrey2, wall.transform.position, wall.transform.localRotation);
                //Vector3 targetposgrey = wall.transform.position - (wall.transform.position - (finalpos));
                //Vector3 targetposgrey3= RotateAroundPivot2(targetposgrey, wall.transform.position, wall.transform.localRotation);

                //Gizmos.DrawWireSphere(targetposgrey,0.5f);
                //Vector3 closestpoint = transform.GetComponent<Collider>().ClosestPoint(finalpos);
                //Gizmos.color = Color.black;
                Vector3 offset = transform.InverseTransformPoint(finalpos);
                

                positions2offset.Add(offset);
            }

            once = true;
        }
        if (!check)
        {
            //closestpositions2.Clear();
            for (int i = 0; i < positions2.Count; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(RotateAroundPivot(transform.TransformPoint(positions2[i]), wall.transform.position, wall.transform.eulerAngles), 0.5f);
                Gizmos.color = Color.black;
                Gizmos.DrawWireSphere(RotateAroundPivot(transform.TransformPoint(positions2offset[i]), wall.transform.position, wall.transform.eulerAngles), 0.5f);
                //Vector3 closestpoint = Physics.ClosestPoint(transform.TransformPoint(positions2[i]),transform.GetComponent<Collider>(),transform.position,transform.rotation);
                //Gizmos.color = Color.black;
                //Gizmos.DrawWireCube(closestpoint, Vector3.one);
                //closestpositions2.Add(closestpoint);
                 }
            /*
            foreach (Vector3 hit in wallhitoffset)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(hit, Vector3.one);
            }
            foreach (Vector3 hit in oppositewallhitoffset)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(hit, Vector3.one);
            }
            */
        }
        Gizmos.color = Color.red;
        Vector3 realpos = RotateAroundPivot(transform.TransformPoint(positions2[checker]), wall.transform.position, wall.transform.eulerAngles);
        Vector3 realpos2 = RotateAroundPivot(transform.TransformPoint(positions2offset[checker]), wall.transform.position, wall.transform.eulerAngles);
        Gizmos.DrawCube(RotateAroundPivot(transform.TransformPoint(positions2[checker]), wall.transform.position, wall.transform.eulerAngles), Vector3.one);
        //Gizmos.DrawCube(RotateAroundPivot(transform.TransformPoint(positions2offset[checker]), wall.transform.position, wall.transform.eulerAngles), Vector3.one);
        //Debug.Log(realpos + "/" + realpos2);
    }
    public void Update()
    {
        

    }
    Vector3 RotateAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot;
        dir = Quaternion.Euler(angles*0) * dir;
        point = dir + pivot;
        return point;
        //return Quaternion.Euler(angles) * (  pivot-point) + pivot;
    }

    Vector3 RotateAroundPivot2(Vector3 point, Vector3 pivot, Quaternion angles)
    {
        var finalpos = point - pivot;
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
    public IEnumerator GoToPositions(Transform target)
    {
        //Vector3 realdirect = Vector3.zero;
        //Vector3[] positions = new Vector3[0];
        //Debug.Log("start");
       
            //realdirect = Vector3.forward;
                covers = new CoverInfo[positions.Count];
                for (int i = 0; i < positions.Count; i++)
                {
                    covers[i] = new CoverInfo();
                }
            

        
        for (int i = 0; i < positions2.Count; i++)
        {
            //Debug.Log("positions");
            RaycastHit hit;
            Vector3 realpos = RotateAroundPivot(transform.TransformPoint(positions2[i]), wall.transform.position, wall.transform.eulerAngles);
            Vector3 realpos2 = RotateAroundPivot(transform.TransformPoint(positions2offset[i]), wall.transform.position, wall.transform.eulerAngles);
            if (Physics.Raycast(realpos, Vector3.down, out hit, 100f, ground))
            {
                //Debug.Log(hit.point);
                //yield return new WaitForSeconds(0.5f);
                //  = Vector3.zero;
                //Debug.Log(closestpositions2[i] + "/" + realpos);
                //Gizmos.color = Color.red;
                //Gizmos.DrawCube(RotateAroundPivot(transform.TransformPoint(positions2[i]), wall.transform.position, wall.transform.eulerAngles), Vector3.one);
                //Gizmos.DrawCube(RotateAroundPivot(transform.TransformPoint(positions2offset[checker]), wall.transform.position, wall.transform.eulerAngles), Vector3.one);



                //Vector3 posy = ( (realpos*1000) -(closestpositions2[i]*1000));
                

//Debug.Log(posy);
                
                
                
                //posy.y = 0;
                //target.transform.position = realpos + ( posy* coverdistance);
                target.transform.position = hit.point + ( (realpos2-realpos)* coverdistance);
                //target.transform.rotation = Quaternion.LookRotation(hit.point - (hit.point-closestpositions2[i]  ),Vector3.up);
                //target.transform.LookAt(closestpositions2[2]);
                   
                        StartCoroutine(Check(i, covers));

                      

                
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
        Vector3 realpos = RotateAroundPivot(transform.TransformPoint(positions2[i]), wall.transform.position, wall.transform.eulerAngles);
        Vector3 realpos2 = RotateAroundPivot(transform.TransformPoint(positions2offset[i]), wall.transform.position, wall.transform.eulerAngles);

        if (check_standing)
        {
            anim.CrossFade("wall_cover_stand", 0, 0, 1);
            yield return new WaitForSeconds(0.01f);

            Debug.Log("stand");

            Physics.Raycast(head.transform.position, (new Vector3(realpos.x, head.transform.position.y, realpos.z) - new Vector3(head.transform.position.x, head.transform.position.y, head.transform.position.z)), out hit, 100, wall.gameObject.layer);

            Debug.DrawRay(head.transform.position, (new Vector3(realpos.x, head.transform.position.y, realpos.z) - new Vector3(head.transform.position.x, head.transform.position.y, head.transform.position.z)), Color.cyan, 10);

            if (hit.collider != null)
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
            Physics.Raycast(head.transform.position, (new Vector3(realpos.x, head.transform.position.y, realpos.z) - new Vector3(head.transform.position.x, head.transform.position.y, head.transform.position.z)), out hit, 100);

            Debug.DrawRay(head.transform.position, (new Vector3(realpos.x, head.transform.position.y, realpos.z) - new Vector3(head.transform.position.x, head.transform.position.y, head.transform.position.z)), Color.magenta, 10);

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
            //Debug.Log(frontc[i].crouch_covered + " " + i);
        }
        yield return new WaitForSeconds(0.01f);
        if (check_prone)
        {
            anim.CrossFade("wall_cover_prone", 0, 0, 1);
            yield return new WaitForSeconds(0.01f);

            Debug.Log("prone");
            Physics.Raycast(head.transform.position, (new Vector3(realpos.x, head.transform.position.y, realpos.z) - new Vector3(head.transform.position.x, head.transform.position.y, head.transform.position.z)), out hit, 100);
            if (hit.collider != null)
            {
                Debug.DrawRay(head.transform.position, (new Vector3(realpos.x, head.transform.position.y, realpos.z) - new Vector3(head.transform.position.x, head.transform.position.y, head.transform.position.z)), Color.red, 10);

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

    void RepeatRaycast(Vector3 pos, Vector3 direction, bool oppositeside)
    {
        //Debug.Log("this2");
        RaycastHit hit5 = new RaycastHit();
        Physics.Raycast(pos + (direction * 0.1f), direction, out hit5, 100, ground);
        Gizmos.DrawRay(pos, direction);

        //Gizmos.color = Color.blue; 
        //Gizmos.DrawWireSphere(Vector3.Lerp(hit5.point, pos, 0.05f), 0.5f);

        if (hit5.collider != null)
        {
            if (selfonly && hit5.collider.name == transform.name)
        {
            
                //Gizmos.color = Color.black;
                Gizmos.DrawWireSphere(hit5.point, 0.5f);
                if (oppositeside)
                {
                    wallhit.Add(hit5.point);
                    wallhitoffset.Add(Vector3.Lerp(hit5.point, pos, 0.05f));
                }
                else
                {
                    oppositewallhitoffset.Add(Vector3.Lerp(hit5.point, pos, 0.05f));
                    oppositewallhit.Add(hit5.point);
                }
                RepeatRaycast(hit5.point, direction, oppositeside);
            }
        }
        else if (selfonly == false)
        {
            if (hit5.collider != null)
            {
                //Gizmos.color = Color.black;
                Gizmos.DrawWireSphere(hit5.point, 0.5f);
                if (oppositeside)
                {
                    wallhitoffset.Add(Vector3.Lerp(hit5.point, pos, 0.05f));
                    wallhit.Add(hit5.point);
                }
                else
                {
                    oppositewallhitoffset.Add(Vector3.Lerp(hit5.point, pos, 0.05f));
                    oppositewallhit.Add(hit5.point);
                }
                RepeatRaycast(hit5.point, direction, oppositeside);
            }
        }
    }
}
public enum DoubleSideOrientation { Y_nY,Z_nZ,X_nX };