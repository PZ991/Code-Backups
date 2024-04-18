using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverV3 : MonoBehaviour
{
    // Start is called before the first frame update
    public Collider wall;
    public MeshFilter[] mesh;
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

    public Vector3 realpos;
    public bool once;

    public bool check_standing;
    public bool check_crouch;
    public bool check_prone;
    public Transform head;
    public Animator anim;
    public CoverInfo[] covers = new CoverInfo[1];
    public bool check;
    public float coverdistance;
    public Transform target;
    public int checker;
    public Vector3Int checkerV2;
    public bool UseVertex;
    public bool showBoundingBox;
    public bool showSelectedAxis;
    public Vector3[,,] Grid2;
    public float gridlength;

    public bool useNativeGrid;
    public Vector3[,,] nativegrid;
    public Vector3[,,] nativemidgrid;
    public bool shownativecollision;

    public GameObject testcol;
    public bool usebothdetection;

    public List<Vector3[,,]> finalpoints;

    public bool UseInbetweenlines;
    public bool Terrain;
    public Checks[,] list= new Checks[0,0];
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        #region Grid
        Vector3[] line1 = new Vector3[5];


        #region NEW Grid

        Utility.InitializeScanner(this.transform, mesh, ref line1, ref rot, ref direction, ref othersidepos, orientation, showBoundingBox, showSelectedAxis,useNativeGrid);
            if (!useNativeGrid)
            {
                Grid(line1);
            }
            else
            {
                GridBlock(line1);
            }
            #endregion
        
        #endregion
        
            
          
            #region Grid Analyze
            List<float> Ycount = new List<float>();
            foreach (Vector3 item in wallhit)
            {
                if (!Ycount.Contains(item.y))
                {
                    Ycount.Add(item.y);
                }
            }
            foreach (Vector3 item in oppositewallhit)
            {
                if (!Ycount.Contains(item.y))
                {
                    Ycount.Add(item.y);
                }
            }
            Grid2 = new Vector3[Mathf.FloorToInt(gridwidth), Ycount.Count - 1, Mathf.FloorToInt(gridheight)];
            wallhit.Sort(Utility.CompareVectors);
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(wallhit[checker], 5);
            #endregion
        
    }




    public void Grid(Vector3[] line1)
    {
        #region MainGrid

        #region Set Grid Corners
        Vector3 a = line1[2];
        Vector3 b = line1[3];
        Vector3 c = line1[1];

        float width = 0;
        width = Vector3.Distance(a, b) / gridwidth;
        float height = 0;
        height = Vector3.Distance(a, c) / gridheight;
        //GRID ARE THE INITIAL CORNERS
        grid = new Vector3[(int)gridwidth + 1, (int)gridheight + 1];
        oppositegrid = new Vector3[(int)gridwidth + 1, (int)gridheight + 1];
        for (int x = 0; x <= gridwidth; x++)
        {
            for (int y = 0; y <= gridheight; y++)
            {
                Vector3 posxy = new Vector3(x * width, 10, (y * height)) + line1[1];
                Vector3 targetposxy = line1[1] - (line1[1] - posxy);
                Vector3 finalxy = Vector3.zero;

                finalxy = Utility.RotateAroundPivot2(targetposxy, line1[1], wall.transform.localRotation * Quaternion.Euler(rot));
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

            }
        }
        for (int x = 0; x <= gridwidth; x++)
        {
            for (int y = 0; y <= gridheight; y++)
            {
                Vector3 posxy = new Vector3(x * width, 10, (y * height)) + line1[1];
                Vector3 targetposxy = line1[1] - (line1[1] - posxy);
                Vector3 finalxy = Vector3.zero;

                finalxy = Utility.RotateAroundPivot2(targetposxy + (othersidepos), line1[1], wall.transform.localRotation * Quaternion.Euler(rot));
                //finalxy = RotateAroundPivot2(RotateAroundPivot2(targetposxy, line1[1], Quaternion.Euler(rot)),transform.position,Quaternion.Euler(transform.eulerAngles));
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(finalxy, 0.5f);

                oppositegrid[x, y] = finalxy;
            }
        }

        #endregion

        #region Set Middle Of Grid
        midgrid = new Vector3[grid.GetLength(0) - 1, grid.GetLength(1) - 1];
        list = new Checks[grid.GetLength(0) - 1, grid.GetLength(1) - 1];
        oppositemidgrid = new Vector3[grid.GetLength(0) - 1, grid.GetLength(1) - 1];
        for (int x = 0; x < grid.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < grid.GetLength(1) - 1; y++)
            {
                Vector3 pos = (grid[x, y] + grid[x + 1, y] + grid[x, y + 1] + grid[x + 1, y + 1]) / 4;
                Vector3 pos2 = (oppositegrid[x, y] + oppositegrid[x + 1, y] + oppositegrid[x, y + 1] + oppositegrid[x + 1, y + 1]) / 4;

                midgrid[x, y] = pos;
                oppositemidgrid[x, y] = pos2;
                list[x, y] = new Checks();
                
                list[x, y].beginning = pos;
                list[x, y].end = pos2;

                
            }
        }

        #endregion

        #region Start Raycast
        Utility.ClearMultipleList(wallhit, wallhitoffset, oppositewallhit, oppositewallhitoffset);

        for (int x = 0; x < midgrid.GetLength(0); x++)
        {
            for (int y = 0; y < midgrid.GetLength(1); y++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(midgrid[x, y], Vector3.one);
                Gizmos.color = Color.black;

                Gizmos.DrawCube(oppositemidgrid[x, y], Vector3.one);
                Gizmos.DrawRay(midgrid[x, y], (oppositemidgrid[x, y] - midgrid[x, y]));
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
                Vector3 NormDirect = (oppositemidgrid[x, y] - midgrid[x, y]);
                Vector3 OtherDirect = (midgrid[x, y] - oppositemidgrid[x, y]);

                if (Physics.Raycast(midgrid[x, y], NormDirect, out hitting, 1000))
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
                            if (list[x, y].starts == null)
                                list[x, y].starts = new List<Vector3>();
                            if (list[x, y].ends == null)
                                list[x, y].ends = new List<Vector3>();

                            list[x, y].starts.Add(hitting.point);
                            Utility.RepeatRaycast(transform,ref wallhit,ref oppositewallhit,ref list,hitting.point, NormDirect, false,x,y,selfonly);
                        }

                    }
                    else if (selfonly == false)
                    {
                        if (hitting.collider != null)
                        {
                            Gizmos.color = Color.black;
                            Gizmos.DrawWireSphere(hitting.point, 0.5f);
                            wallhit.Add(hitting.point);
                            list[x, y].starts.Add(hitting.point);
                            Utility.RepeatRaycast(transform, ref wallhit, ref oppositewallhit, ref list, hitting.point, NormDirect, false, x, y, selfonly);
                        }

                    }
                    if (selfonly && hitting.collider.name != transform.name)
                    {
                        list[x, y].starts.Add(hitting.point);
                            Utility.RepeatRaycast(transform,ref wallhit,ref oppositewallhit,ref list,hitting.point, NormDirect, false,x,y,selfonly);
                    }
                }

                if (Physics.Raycast(oppositemidgrid[x, y], OtherDirect, out hitting2, 1000))
                {
                    if (selfonly && hitting2.collider.name == transform.name)
                    {
                        if (hitting2.collider != null)
                        {
                            Gizmos.color = Color.cyan;
                            Gizmos.DrawWireSphere(hitting2.point, 0.5f);
                            oppositewallhit.Add(hitting2.point);
                            oppositewallhitoffset.Add(Vector3.Lerp(hitting2.point, oppositemidgrid[x, y], 0.1f));
                            //Gizmos.color = Color.cyan;
                            //Gizmos.DrawWireSphere(Vector3.Lerp(hitting2.point, oppositemidgrid[x, y], 0.1f), 0.5f);
                            list[x, y].ends.Insert(0,hitting2.point);
                            Utility.RepeatRaycast(transform, ref wallhit, ref oppositewallhit, ref list, hitting2.point, OtherDirect, true, x, y, selfonly);

                        }
                    }
                    else if (!selfonly)
                    {
                        if (hitting2.collider != null)
                        {
                            Gizmos.color = Color.cyan;
                            Gizmos.DrawWireSphere(hitting2.point, 0.5f);
                            oppositewallhit.Add(hitting2.point);
                            list[x, y].ends.Insert(0, hitting2.point);
                            Utility.RepeatRaycast(transform, ref wallhit, ref oppositewallhit, ref list, hitting2.point, OtherDirect, true, x, y, selfonly);
                        }
                    }
                    else if (selfonly && hitting2.collider.name != transform.name)
                    {
                        list[x, y].ends.Insert(0, hitting2.point);
                        Utility.RepeatRaycast(transform, ref wallhit, ref oppositewallhit, ref list, hitting2.point, OtherDirect, true, x, y, selfonly);
                    }
                }
                
                //Gizmos.DrawSphere(new Vector3(0, -Vector3.Distance(red, yellow)-10, 0) + midgrid[x, y], 0.5f);
            }
        }

        #endregion

        #endregion
    }

    public void GridBlock(Vector3[] line1)
    {
        #region MainGrid

        #region Set Grid Corners

        Vector3 a = line1[2];
        Vector3 b = line1[3];
        Vector3 c = line1[1];
        Vector3 d = line1[4];

        float width = 0;

        width = Vector3.Distance(a, b) / gridwidth;
        float height = 0;
        height = Vector3.Distance(a, c) / gridheight;
        float length = Vector3.Distance(c, d)/gridlength;
        //Debug.Log(height);
        nativegrid = new Vector3[(int)gridwidth + 1, (int)gridheight + 1, (int)gridlength+1];
        for (int x = 0; x <= gridwidth; x++)
        {
            for (int y = 0; y <= gridheight; y++)
            {
                for (int z = 0; z <= gridlength; z++)
                {
                    Vector3 posxy = new Vector3(x * width, z*length, (y * height)) + line1[4]; 
                    Vector3 targetposxy = line1[4] - (line1[4] - posxy);
                    Vector3 finalxy = Vector3.zero;

                    finalxy = Utility.RotateAroundPivot2(targetposxy, line1[1], wall.transform.localRotation * Quaternion.Euler(rot));
                    //finalxy = RotateAroundPivot2(RotateAroundPivot2(targetposxy, line1[1], Quaternion.Euler(rot)),transform.position,Quaternion.Euler(transform.eulerAngles));
                    
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(finalxy, 0.25f);

                    //Ray ray3 = new Ray(finalxy, direction * 100);
                    //Gizmos.color = Color.blue;
                    //Gizmos.DrawRay(ray3);
                    //RaycastHit hit3 = new RaycastHit();
                    //Physics.Raycast(ray3, out hit3, 100, ground);
                    nativegrid[x, y, z] = finalxy;
                }
            }
        }

        #endregion

        #region Set Middle of Grid
        nativemidgrid = new Vector3[nativegrid.GetLength(0) - 1, nativegrid.GetLength(1) - 1, nativegrid.GetLength(2)-1];
        for (int x = 0; x < nativegrid.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < nativegrid.GetLength(1) - 1; y++)
            {
                for (int z = 0; z < nativegrid.GetLength(2) - 1; z++)
                {
                    Vector3 pos = (nativegrid[x, y, z] + nativegrid[x + 1, y, z] + nativegrid[x, y + 1, z] + nativegrid[x + 1, y + 1, z]+
                                   nativegrid[x, y, z+1] + nativegrid[x + 1, y, z+1] + nativegrid[x, y + 1, z+1] + nativegrid[x + 1, y + 1, z+1]) / 8;

                    nativemidgrid[x, y, z] = pos;

                }
            }
        }
        #endregion

        #region Get all Cube Position
        if (usebothdetection)
        {
            for (int x = 0; x < nativemidgrid.GetLength(0); x++)
            {
                for (int y = 0; y < nativemidgrid.GetLength(1); y++)
                {
                    for (int z = 0; z < nativemidgrid.GetLength(2); z++)
                    {
                        Gizmos.color = Color.green;
                        // Gizmos.DrawCube(nativemidgrid[x, y, z], new Vector3(width, length, height));
                        //Gizmos.color = Color.black;
                    }
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

                    //Gizmos.DrawSphere(new Vector3(0, -Vector3.Distance(red, yellow)-10, 0) + midgrid[x, y], 0.5f);
                }
            }
        }
        #endregion

        #region Detect Collision
        if (usebothdetection)
        {
            list = new Checks[nativemidgrid.GetLength(0), nativemidgrid.GetLength(1)];
            for (int x = 0; x <= nativemidgrid.GetLength(0)-1; x++)
            {
                for (int y = 0; y <= nativemidgrid.GetLength(1)-1; y++)
                {
                    for (int z = 0; z <= nativemidgrid.GetLength(2) - 1; z++)
                    {

                        if (!Terrain)
                        {


                            Bounds bound = new Bounds(nativemidgrid[x, y, z], new Vector3(width, length, height));
                            Utility.CheckBoundsCollision( bound.center,bound.extents, shownativecollision);

                            //foreach (Vector3 item in wallhit)
                            foreach (MeshFilter item in mesh)
                            {
                                //if (bound.Contains(item))
                                if (Utility.CheckBoundsCollision(bound.center, bound.extents, shownativecollision))
                                {

                                    Gizmos.color = Color.green;
                                    Gizmos.DrawCube(nativemidgrid[x, y, z], new Vector3(width, length, height));

                                }
                                else
                                {
                                    //Gizmos.color = Color.red;
                                   // Gizmos.DrawCube(nativemidgrid[x, y, z], new Vector3(width, length, height));
                                }
                            }
                        }
                        else
                        {

                            //Debug.Log(list.Length + " / "  + "/" + x+"/"+y);

                            bool bottom=false;
                            bool top=false;

                            
                            for (int w = 0; w < list[x, y].starts.Count; w++)
                            {
                                

                                if (nativemidgrid[x, y, z].y <= list[x, y].starts[w].y+1)
                                {
                                    top = true;
                                }
                                if (nativemidgrid[x, y, z].y >= list[x, y].ends[w].y-1)
                                {
                                    bottom = true;
                                }
                                if (top && bottom)
                                    break;
                                bottom = false;
                                top = false;

                            }


                            
                            for (int v = list[x, y].ends.Count - 1; v > 0; v--)
                            {
                                if (nativemidgrid[x, y, z].y >= list[x, y].ends[v].y)
                                {
                                    bottom = true;
                                    break;
                                }
                            }
                           
                            if (top&&bottom)
                            {
                                Gizmos.color = Color.green;
                                Gizmos.DrawCube(nativemidgrid[x, y, z], new Vector3(width, length, height));
                            }
                        }
                        /*
                        if (z <= nativemidgrid.GetLength(2) - 3&&y <= nativemidgrid.GetLength(2) - 2&&x <= nativemidgrid.GetLength(2) - 2
                            && z > 0&& x > 0&& y > 0)
                        {
                            if (CheckGrid(x,y,z, new Vector3(width, length, height)))
                            {
                               
                            }

                            else
                            {
                                Gizmos.color = Color.blue;
                                // Gizmos.DrawCube(nativemidgrid[x, y, z], new Vector3(width, length, height));
                            }
                        }
                        
                        else
                        {
                            if (Physics.OverlapBox(nativemidgrid[x, y, z], new Vector3(width, length, height) / 2, transform.rotation).Length > 0)
                            {
                                Gizmos.color = Color.green;
                                Gizmos.DrawCube(nativemidgrid[x, y, z], new Vector3(width, length, height));
                            }
                        }
                        */
                    }
                }
            }
            
        }

        #endregion

 
        #region Test Collision
        if (testcol!=null)
        {
            if(Physics.CheckBox(testcol.transform.position, new Vector3(width, length, height) / 2, transform.rotation))
                Gizmos.DrawCube(testcol.transform.position, new Vector3(width, length, height));
        }
        Gizmos.color = Color.black; 
        Gizmos.DrawCube(nativemidgrid[checkerV2.x,checkerV2.y,checkerV2.z], Vector3.one*5);
        #endregion

        #endregion
    }



}
public struct Checks
{
    public Vector3 beginning;
    public Vector3 end;
    public List<Vector3> starts;
    public List<Vector3> ends;

}
