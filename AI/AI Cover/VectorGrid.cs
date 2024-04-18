using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorGrid : MonoBehaviour
{
    public Collider wall;
    public MeshFilter mesh;
    public Collider target_object;
    public Transform head;
    public Collider player;
    public List<Transform> enemy;
    public Animator anim;


    public float gridwidth;
    public float gridheight;
    public float line1size;

    public Orientation orientation = Orientation.nY;

    public bool show_mid;
    public bool show_division;
    public LayerMask ground;
    public Vector3[,] grid;
    public Vector3[,] midgrid;
    public Vector3 rot;
    public Vector3 direction;
    public bool refresh;
    public bool add;
    public List<Vector3> positions;
    public bool drawboxdetection;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            case Orientation.Y:
                {

                    // Top
                    line1[0] = clear;
                    line1[1] = magenta;
                    line1[2] = cyan;
                    line1[3] = red;

                    Gizmos.color = Color.green;
                    rot = new Vector3(0, 0, 0);
                    direction = Vector3.down;
                    break;
                }
            case Orientation.nY:
                {
                    // Bottom
                    line1[0] = blue;
                    line1[1] = grey;
                    line1[2] = yellow;
                    line1[3] = green;
                    Gizmos.color = Color.green;

                    rot = new Vector3(180, 0, 0);
                    direction = Vector3.up;

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

                    rot = new Vector3(90, 0, 0);
                    direction = Vector3.back;
                    break;
                }
            case Orientation.nZ:
                {
                    // Back
                    line1[0] = blue;
                    line1[1] = magenta;
                    line1[2] = clear;
                    line1[3] = green;

                    Gizmos.color = Color.blue;

                    rot = new Vector3(0,90 , -90);
                    direction = Vector3.forward;
                    break;
                }
            case Orientation.nX:
                {
                    // Right
                    line1[0] = cyan;
                    line1[1] = magenta;
                    line1[2] = blue;
                    line1[3] = grey;

                    Gizmos.color = Color.red;
                    rot = new Vector3(90, 0, 90);
                    direction = Vector3.right;
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
                    rot = new Vector3(90, 180, 90);
                    direction = -Vector3.right;
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
            grid = new Vector3[(int)gridwidth+1, (int)gridheight+1];
            for (int x = 0; x <= gridwidth; x++)
            {
                for (int y = 0; y <= gridheight; y++)
                {
                    Vector3 posxy = new Vector3(x * width, 10, (y * height)) + line1[1];
                    Vector3 targetposxy = line1[1] - (line1[1] - posxy);
                    Vector3 finalxy = Vector3.zero;
                    finalxy = RotateAroundPivot2(targetposxy, line1[1], Quaternion.Euler(rot));
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(finalxy,0.5f);
                    Ray ray = new Ray(finalxy,direction*100);
                    Gizmos.color=Color.blue;
                    Gizmos.DrawRay(ray);
                    RaycastHit hit= new RaycastHit();
                    Physics.Raycast(ray, out hit, 100, ground);
                    grid[x, y] = hit.point;
                }
            }

            midgrid = new Vector3[grid.GetLength(0) - 1, grid.GetLength(1) - 1];
            for (int x = 0; x < grid.GetLength(0)-1; x++)
            {
                for (int y = 0; y < grid.GetLength(1)-1; y++)
                {
                    Vector3 pos = (grid[x, y] + grid[x + 1, y] + grid[x, y + 1] + grid[x + 1, y + 1]) / 4;
                    
                    midgrid[x,y] = pos;
                }
            }

            Gizmos.color = Color.green;
            for (int x = 0; x < midgrid.GetLength(0); x++)
            {
                for (int y = 0; y < midgrid.GetLength(1); y++)
                {
                    Gizmos.DrawCube(midgrid[x, y], Vector3.one);
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

        #endregion

        //Debug.Log(Vector3.Distance(red, cyan));
        if (refresh)
        {
            positions.Clear();
            foreach (Vector3 pos1 in midgrid)
            {
                positions.Add(pos1);
            }
            
            refresh = false;
        }
        if (add)
        {
            foreach (Vector3 pos1 in midgrid)
            {
                positions.Add(pos1);
            }
            
            add = false;
        }
        foreach (Vector3 finalpos in positions)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(RotateAroundPivot2(finalpos, transform.position, Quaternion.Euler(transform.eulerAngles)), 0.5f);
        
        }
        if (drawboxdetection)
        {
            Collider[] objects = Physics.OverlapBox(transform.position + (Vector3.Scale(transform.localScale, Vector3.up)), transform.localScale * .5f);
            //Gizmos.DrawCube(transform.position + (Vector3.Scale(transform.localScale, Vector3.up)), transform.localScale);
            Gizmos.DrawCube(transform.position + (Vector3.Scale(transform.localScale*.5f, Vector3.up)), Vector3.Scale(transform.localScale, new Vector3(1,2,1)));
        
        }
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

}

