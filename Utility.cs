using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility 
{
    #region OG
    public static Vector3 RotateAroundPivot2(Vector3 point, Vector3 pivot, Quaternion angles)
    {
        var finalpos = point - pivot;
        finalpos = angles * finalpos;
        finalpos += pivot;
        return finalpos;
        
    }
    public static int CompareVectors(Vector3 v1, Vector3 v2)
    {
        
        if (v1 == v2) return 0;

        if (Mathf.Approximately(v1.x, v2.x))
        {
            if (Mathf.Approximately(v1.z, v2.z))
                return v1.y > v2.y ? -1 : 1;
            else
                return v1.z > v2.z ? -1 : 1;
        }
        return v1.x > v2.x ? -1 : 1;
    }
    public static void InitializeScanner(Transform transform, MeshFilter[] meshfilter, ref Vector3[] line1,ref Vector3 rot, ref Vector3 direction, ref Vector3 othersidepos,DoubleSideOrientation orientation=DoubleSideOrientation.Y_nY, bool showBoundingBox=false, bool showSelectedAxis=false,bool nativegrid=false)
    {
        # region Get all Vertices from all mesh
        List<Vector3> verts = new List<Vector3>();
        foreach (MeshFilter mesh in meshfilter)
        {
            foreach (Vector3 Vertices in mesh.sharedMesh.vertices)
            {
                verts.Add(Vertices);
                Gizmos.color = Color.black;
                Vector3 worldPos7 = transform.TransformPoint(Vertices);
                //Gizmos.DrawCube(worldPos7, Vector3.one * 2f);
            }
        }
        Vector3[] vertices = verts.ToArray();

        #endregion

        # region Get Min Max Vertex positions
        float[] points = new float[6];
        points[0] = vertices[0].x;
        points[1] = vertices[0].x;
        points[2] = vertices[0].y;
        points[3] = vertices[0].y;
        points[4] = vertices[0].z;
        points[5] = vertices[0].z;
        foreach (Vector3 pos in vertices)
        {
            Gizmos.color = Color.black;
            if (pos.x < points[0])
            {
                points[0] = pos.x;

            }
            if (pos.x > points[1])
            {
                points[1] = pos.x;

            }
            if (pos.y < points[2])
            {
                points[2] = pos.y;

            }
            if (pos.y > points[3])
            {
                points[3] = pos.y;

            }
            if (pos.z < points[4])
            {
                points[4] = pos.z;

            }
            if (pos.z > points[5])
            {
                points[5] = pos.z;
            }
            //Vector3 worldPos = transform.TransformPoint(pos);
            // Gizmos.DrawCube(worldPos, Vector3.one * 2f);

        }
        float minX = points[0];
        float maxX = points[1];
        float minY = points[2];
        float maxY = points[3];
        float minZ = points[4];
        float maxZ = points[5];

        Gizmos.color = Color.red;
        Vector3 blue = transform.TransformPoint(new Vector3(minX, minY, minZ));
        Vector3 grey = transform.TransformPoint(new Vector3(minX, minY, maxZ));
        Vector3 cyan = transform.TransformPoint(new Vector3(minX, maxY, maxZ));
        Vector3 red = transform.TransformPoint(new Vector3(maxX, maxY, maxZ));

        Vector3 clear = transform.TransformPoint(new Vector3(maxX, maxY, minZ));
        Vector3 yellow = transform.TransformPoint(new Vector3(maxX, minY, maxZ));
        Vector3 green = transform.TransformPoint(new Vector3(maxX, minY, minZ));
        Vector3 magenta = transform.TransformPoint(new Vector3(minX, maxY, minZ));
        if (showBoundingBox)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(blue, Vector3.one * 5f);
            Gizmos.color = Color.red;

            Gizmos.DrawCube(red, Vector3.one * 5f);
            Gizmos.color = Color.yellow;

            Gizmos.DrawCube(yellow, Vector3.one * 5f);
            Gizmos.color = Color.green;

            Gizmos.DrawCube(green, Vector3.one * 5f);
            Gizmos.color = Color.magenta;

            Gizmos.DrawCube(magenta, Vector3.one * 5f);
            Gizmos.color = Color.cyan;

            Gizmos.DrawCube(cyan, Vector3.one * 5f);
            Gizmos.color = Color.grey;

            Gizmos.DrawCube(grey, Vector3.one * 5f);
            Gizmos.color = Color.clear;
            Gizmos.DrawCube(clear, Vector3.one * 5f);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(green, yellow);
            Gizmos.DrawLine(green, clear);
            Gizmos.DrawLine(magenta, clear);
            Gizmos.DrawLine(magenta, blue);
            Gizmos.DrawLine(green, blue);
            Gizmos.DrawLine(magenta, cyan);
            Gizmos.DrawLine(red, cyan);
            Gizmos.DrawLine(red, clear);
            Gizmos.DrawLine(red, yellow);
            Gizmos.DrawLine(grey, yellow);
            Gizmos.DrawLine(grey, cyan);
            Gizmos.DrawLine(grey, blue);

            Vector3 midpos = (green + yellow + clear + magenta + blue + cyan + red + grey) / 8;
            Gizmos.DrawLine(green, midpos);
            Gizmos.DrawLine(midpos, clear);
            Gizmos.DrawLine(magenta, midpos);
            Gizmos.DrawLine(midpos, blue);
            Gizmos.DrawLine(midpos, cyan);
            Gizmos.DrawLine(red, midpos);
            Gizmos.DrawLine(midpos, yellow);
            Gizmos.DrawLine(grey, midpos);
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(midpos, Vector3.one * 5);
        }
        #endregion

        #region Orientation of Min Max Face
        if (nativegrid)
        {
            switch (orientation)
            {
                case DoubleSideOrientation.Y_nY:
                    {

                        // Top
                        line1[0] = clear;
                        line1[1] = magenta;
                        line1[2] = cyan;
                        line1[3] = red;
                        line1[4] = blue;

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
                        line1[4] = magenta;

                        Gizmos.color = Color.blue;
                        othersidepos = new Vector3(0, -(Vector3.Distance(red, yellow) + 20), 0);
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
                        line1[4] = blue;

                        Gizmos.color = Color.red;
                        othersidepos = new Vector3(0, -(Vector3.Distance(red, yellow) + 30), 0);
                        rot = new Vector3(90, 0, 90);
                        direction = Vector3.right;
                        break;
                    }

            }
        }
        else
        {
            line1[0] = red;
            line1[1] = cyan;
            line1[2] = grey;
            line1[3] = yellow;
            line1[4] = magenta;

            Gizmos.color = Color.blue;
            othersidepos = new Vector3(0, -(Vector3.Distance(red, yellow) + 20), 0);
            rot = new Vector3(90, 0, 0);
            direction = Vector3.back;
        }
        if (showSelectedAxis)
        {
            Gizmos.DrawLine(line1[0], line1[1]);
            Gizmos.DrawLine(line1[1], line1[2]);
            Gizmos.DrawLine(line1[2], line1[3]);
            Gizmos.DrawLine(line1[3], line1[0]);
        }
        #endregion
    }

    public static void RepeatRaycast(Transform transform,ref List<Vector3> wallhit,ref List<Vector3> oppositewallhit,ref Checks[,] list,Vector3 pos, Vector3 direction, bool oppositeside, int x, int y,bool selfonly=false)
    {
        //Debug.Log("this2");
        RaycastHit hit5 = new RaycastHit();
        Physics.Raycast(pos + (direction * 0.05f), direction, out hit5, 1000);
        //Gizmos.color = Color.magenta;
        //Gizmos.DrawRay(pos, direction);
        if (hit5.collider != null)
        {
            //Gizmos.color = Color.blue; 
            //Gizmos.DrawWireSphere(hit5.point,0.1f);
            // Debug.Log(hit5.collider.name);
            if (selfonly && hit5.collider == transform.GetComponent<Collider>())
            {

                if (list[x, y].starts == null)
                    list[x, y].starts = new List<Vector3>();
                if (list[x, y].ends == null)
                    list[x, y].ends = new List<Vector3>();



                if (!oppositeside)
                {
                    wallhit.Add(hit5.point);
                    wallhit.Add(Vector3.Lerp(hit5.point, pos, 0.05f));
                    //wallhitoffset.Add(Vector3.Lerp(hit5.point, pos, 0.05f));
                    Gizmos.color = Color.black;
                    Gizmos.DrawWireSphere(hit5.point, 0.5f);
                    list[x, y].starts.Add(hit5.point);
                }
                else
                {
                    //oppositewallhitoffset.Add(Vector3.Lerp(hit5.point, pos, 0.05f));
                    oppositewallhit.Add(Vector3.Lerp(hit5.point, pos, 0.05f));
                    oppositewallhit.Add(hit5.point);
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawWireSphere(hit5.point, 0.5f);
                    list[x, y].ends.Insert(0, hit5.point);

                }

                RepeatRaycast(transform,ref wallhit,ref oppositewallhit,ref list,hit5.point, direction, oppositeside, x, y,selfonly);

            }

            else if (selfonly == false)
            {



                //Gizmos.color = Color.black;
                if (!oppositeside)
                {
                    //wallhitoffset.Add(Vector3.Lerp(hit5.point, pos, 0.05f));
                    wallhit.Add(Vector3.Lerp(hit5.point, pos, 0.05f));
                    wallhit.Add(hit5.point);
                    Gizmos.color = Color.black;
                    Gizmos.DrawWireSphere(hit5.point, 0.5f);
                    list[x, y].starts.Add(hit5.point);
                }
                else
                {
                    //oppositewallhitoffset.Add(Vector3.Lerp(hit5.point, pos, 0.05f));
                    oppositewallhit.Add(Vector3.Lerp(hit5.point, pos, 0.05f));
                    oppositewallhit.Add(hit5.point);
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawWireSphere(hit5.point, 0.5f);
                    list[x, y].ends.Insert(0, hit5.point);

                }

                RepeatRaycast(transform, ref wallhit, ref oppositewallhit, ref list, hit5.point, direction, oppositeside, x, y, selfonly);

            }
            else
            {
                //Debug.Log("this2");



                RepeatRaycast(transform, ref wallhit, ref oppositewallhit, ref list, hit5.point, direction, oppositeside, x, y, selfonly);


            }
        }
    }

    public static bool CheckGrid(int x, int y, int z, Vector3 size, Vector3[,,] nativemidgrid,Transform transform)
    {
        bool U = Physics.OverlapBox(nativemidgrid[x, y, z + 1], size / 2, transform.rotation).Length > 0;
        /*
        bool F = Physics.OverlapBox(nativemidgrid[x + 1, y, z], size / 2, transform.rotation).Length > 0;
        bool B = Physics.OverlapBox(nativemidgrid[x - 1, y, z], size / 2, transform.rotation).Length > 0;
        bool R = Physics.OverlapBox(nativemidgrid[x , y+1, z], size / 2, transform.rotation).Length > 0;
        bool L = Physics.OverlapBox(nativemidgrid[x , y-1, z], size / 2, transform.rotation).Length > 0;
        bool U2 = Physics.OverlapBox(nativemidgrid[x , y, z+2], size / 2, transform.rotation).Length > 0;
        bool D = Physics.OverlapBox(nativemidgrid[x , y, z-1], size / 2, transform.rotation).Length > 0;
        bool FU = Physics.OverlapBox(nativemidgrid[x + 1, y, z+1], size / 2, transform.rotation).Length > 0;
        bool FD = Physics.OverlapBox(nativemidgrid[x + 1, y, z-1], size / 2, transform.rotation).Length > 0;
        bool FL = Physics.OverlapBox(nativemidgrid[x + 1, y-1, z], size / 2, transform.rotation).Length > 0;
        bool FLU = Physics.OverlapBox(nativemidgrid[x + 1, y-1, z+1], size / 2, transform.rotation).Length > 0;
        bool FLD = Physics.OverlapBox(nativemidgrid[x + 1, y-1, z-1], size / 2, transform.rotation).Length > 0;
        bool FR = Physics.OverlapBox(nativemidgrid[x + 1, y+1, z], size / 2, transform.rotation).Length > 0;
        bool FRU = Physics.OverlapBox(nativemidgrid[x + 1, y+1, z+1], size / 2, transform.rotation).Length > 0;
        bool FRD = Physics.OverlapBox(nativemidgrid[x + 1, y+1, z-1], size / 2, transform.rotation).Length > 0;
        bool BU = Physics.OverlapBox(nativemidgrid[x - 1, y, z+1], size / 2, transform.rotation).Length > 0;
        bool BD = Physics.OverlapBox(nativemidgrid[x - 1, y, z-1], size / 2, transform.rotation).Length > 0;
        bool BL = Physics.OverlapBox(nativemidgrid[x - 1, y-1, z], size / 2, transform.rotation).Length > 0;
        bool BLU = Physics.OverlapBox(nativemidgrid[x + 1, y-1, z+1], size / 2, transform.rotation).Length > 0;
        bool BLD = Physics.OverlapBox(nativemidgrid[x + 1, y-1, z-1], size / 2, transform.rotation).Length > 0;
        bool BR = Physics.OverlapBox(nativemidgrid[x - 1, y+1, z], size / 2, transform.rotation).Length > 0;
        bool BRU = Physics.OverlapBox(nativemidgrid[x - 1, y+1, z+1], size / 2, transform.rotation).Length > 0;
        bool BRD = Physics.OverlapBox(nativemidgrid[x + 1, y+1, z-1], size / 2, transform.rotation).Length > 0;
        bool LU = Physics.OverlapBox(nativemidgrid[x , y-1, z+1], size / 2, transform.rotation).Length > 0;
        bool LD = Physics.OverlapBox(nativemidgrid[x, y - 1, z - 1], size / 2, transform.rotation).Length > 0;
        bool RU = Physics.OverlapBox(nativemidgrid[x, y + 1, z + 1], size / 2, transform.rotation).Length > 0;
        bool RD = Physics.OverlapBox(nativemidgrid[x, y + 1, z - 1], size / 2, transform.rotation).Length > 0;
        */
        bool C = Physics.OverlapBox(nativemidgrid[x, y, z], size / 2, transform.rotation).Length > 0;

        if ((C && !U))//||(C&&U&&D&&L&&R&&!U2) ||(C&&U&&D&&L&&!R&&!U2) )
        {

            return true;
        }

        return false;
    }

    public static void ClearMultipleList(params List<Vector3>[] list)
    {
        foreach (var item in list)
        {
            item.Clear();
        }
    }


    public static bool CheckBoundsCollision(Vector3 pos, Vector3 sizehalf,bool showgizmos)
    {
        Vector3[] corners = new Vector3[8];
        corners[0] = pos + new Vector3(sizehalf.x, sizehalf.y, sizehalf.z);     //XYZ
        corners[1] = pos + new Vector3(sizehalf.x, sizehalf.y, -sizehalf.z);    //XY-Z
        corners[2] = pos + new Vector3(sizehalf.x, -sizehalf.y, sizehalf.z);    //X-YZ
        corners[3] = pos + new Vector3(sizehalf.x, -sizehalf.y, -sizehalf.z);   //X-Y-Z
        corners[4] = pos + new Vector3(-sizehalf.x, sizehalf.y, sizehalf.z);    //-XYZ
        corners[5] = pos + new Vector3(-sizehalf.x, sizehalf.y, -sizehalf.z);   //-XY-Z
        corners[6] = pos + new Vector3(-sizehalf.x, -sizehalf.y, sizehalf.z);   //-X-YZ
        corners[7] = pos + new Vector3(-sizehalf.x, -sizehalf.y, -sizehalf.z);  //-X-Y-Z

        if (showgizmos)
        {
            Gizmos.color = Color.white;

            //cubic
            Gizmos.DrawLine(corners[0], corners[1]);
            Gizmos.DrawLine(corners[0], corners[2]);
            Gizmos.DrawLine(corners[0], corners[4]);
            Gizmos.DrawLine(corners[1], corners[3]);
            Gizmos.DrawLine(corners[1], corners[5]);
            Gizmos.DrawLine(corners[2], corners[3]);
            Gizmos.DrawLine(corners[2], corners[6]);
            Gizmos.DrawLine(corners[3], corners[7]);
            Gizmos.DrawLine(corners[4], corners[5]);
            Gizmos.DrawLine(corners[4], corners[6]);
            Gizmos.DrawLine(corners[5], corners[7]);
            Gizmos.DrawLine(corners[6], corners[7]);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(corners[0], corners[7]);
            Gizmos.DrawLine(corners[2], corners[5]);
            Gizmos.DrawLine(corners[6], corners[1]);
            Gizmos.DrawLine(corners[4], corners[3]);

            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(corners[0], corners[3]);
            Gizmos.DrawLine(corners[2], corners[1]);
            Gizmos.DrawLine(corners[0], corners[6]);
            Gizmos.DrawLine(corners[2], corners[4]);
            Gizmos.DrawLine(corners[6], corners[5]);
            Gizmos.DrawLine(corners[4], corners[7]);
            Gizmos.DrawLine(corners[3], corners[5]);
            Gizmos.DrawLine(corners[1], corners[7]);
        }
        // Gizmos.DrawCube(corners[0],Vector3.one*0.5f); //XYZ
        //Gizmos.DrawCube(corners[1],Vector3.one*0.5f); //XY-Z


        RaycastHit hit;

        for (int i = 0; i < corners.Length; i++)
        {
            for (int j = i + 1; j < corners.Length; j++)
            {
                if (Physics.Linecast(corners[i], corners[j], out hit))
                {
                    return true;
                }
            }
        }

        

        // No collision detected
        return false;
    }
    #endregion

    #region D*
    /*

public class DStarLite
{
    private class Cell
    {
        public int X;
        public int Y;
        public double G;
        public double Rhs;
        public double Key;
        public List<Cell> Neighbors;

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            G = double.PositiveInfinity;
            Rhs = double.PositiveInfinity;
            Key = 0;
            Neighbors = new List<Cell>();
        }
    }

    private Cell[,] grid;
    private Cell start;
    private Cell goal;
    private List<Cell> openList;

    public DStarLite(int width, int height, Vector2Int start, Vector2Int goal)
    {
        grid = new Cell[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new Cell(x, y);
            }
        }

        this.start = grid[start.x, start.y];
        this.goal = grid[goal.x, goal.y];
        openList = new List<Cell>();

        Initialize();
    }

    private void Initialize()
    {
        goal.Rhs = 0;
        goal.Key = CalculateKey(goal);

        openList.Add(goal);
    }

    private double CalculateKey(Cell cell)
    {
        return Mathf.Min((float)cell.G, (float)cell.Rhs) + Heuristic(cell, start) + start.G;
    }

    private double Heuristic(Cell a, Cell b)
    {
        return Mathf.Abs(a.X - b.X) + Mathf.Abs(a.Y - b.Y); // Manhattan distance
    }

    private void UpdateVertex(Cell cell)
    {
        if (cell != goal)
        {
            double minRhs = double.PositiveInfinity;

            foreach (var neighbor in cell.Neighbors)
            {
                double rhs = neighbor.G + Cost(cell, neighbor);
                if (rhs < minRhs)
                {
                    minRhs = rhs;
                }
            }

            cell.Rhs = minRhs;
        }

        if (openList.Contains(cell))
        {
            openList.Remove(cell);
        }

        if (cell.G != cell.Rhs)
        {
            cell.Key = CalculateKey(cell);
            openList.Add(cell);
        }
    }

    private double Cost(Cell a, Cell b)
    {
        return Mathf.Abs(a.X - b.X) + Mathf.Abs(a.Y - b.Y); // Manhattan distance
    }

    private Cell GetSmallestCell()
    {
        Cell smallest = openList[0];
        for (int i = 1; i < openList.Count; i++)
        {
            if (openList[i].Key < smallest.Key || (openList[i].Key == smallest.Key && openList[i].Rhs < smallest.Rhs))
            {
                smallest = openList[i];
            }
        }
        return smallest;
    }

    public List<Vector2Int> Plan()
    {
        List<Vector2Int> path = new List<Vector2Int>();

        while (openList.Count > 0 && (start.Rhs > start.G || !Mathf.Approximately(start.Rhs, start.G)))
        {
            Cell current = GetSmallestCell();

            if (current.G > current.Rhs)
            {
                current.G = current.Rhs;
                openList.Remove(current);

                foreach (var neighbor in current.Neighbors)
                {
                    UpdateVertex(neighbor);
                }
            }
            else
            {
                double oldG = current.G;
                current.G = double.PositiveInfinity;

                foreach (var neighbor in current.Neighbors)
                {
                    if (neighbor.Rhs == Cost(neighbor, current) + oldG)
                    {
                        UpdateVertex(neighbor);
                    }
                }

                UpdateVertex(current);
            }
        }

        if (Mathf.Approximately(start.Rhs, double.PositiveInfinity))
        {
            Debug.Log("No valid path found.");
            return path;
        }

        Cell currentCell = start;
        path.Add(new Vector2Int(currentCell.X, currentCell.Y));

        while (currentCell != goal)
        {
            double minCost = double.PositiveInfinity;
            Cell nextCell = null;

            foreach (var neighbor in currentCell.Neighbors)
            {
                double cost = Cost(currentCell, neighbor);
                if (neighbor.G + cost < minCost)
                {
                    minCost = neighbor.G + cost;
                    nextCell = neighbor;
                }
            }

            if (nextCell == null)
            {
                Debug.Log("No valid path found.");
                return path;
            }

            currentCell = nextCell;
            path.Add(new Vector2Int(currentCell.X, currentCell.Y));
        }

        return path;
    }
}



     */
    #endregion

    #region Theta*
    /*


 public class ThetaStar
 {
     private class Cell
     {
         public int X;
         public int Y;
         public float G;
         public float H;
         public Cell Parent;
         public List<Cell> Neighbors;

         public Cell(int x, int y)
         {
             X = x;
             Y = y;
             G = float.PositiveInfinity;
             H = 0;
             Parent = null;
             Neighbors = new List<Cell>();
         }
     }

     private Cell[,] grid;
     private Cell start;
     private Cell goal;

     public ThetaStar(int width, int height, Vector2Int start, Vector2Int goal)
     {
         grid = new Cell[width, height];
         for (int x = 0; x < width; x++)
         {
             for (int y = 0; y < height; y++)
             {
                 grid[x, y] = new Cell(x, y);
             }
         }

         this.start = grid[start.x, start.y];
         this.goal = grid[goal.x, goal.y];

         Initialize();
     }

     private void Initialize()
     {
         start.G = 0;
         start.H = Heuristic(start, goal);

         foreach (var cell in grid)
         {
             foreach (var neighbor in GetNeighbors(cell))
             {
                 if (!IsObstacleInPath(cell, neighbor))
                 {
                     cell.Neighbors.Add(neighbor);
                 }
             }
         }
     }

     private float Heuristic(Cell a, Cell b)
     {
         return Mathf.Abs(a.X - b.X) + Mathf.Abs(a.Y - b.Y); // Manhattan distance
     }

     private List<Cell> GetNeighbors(Cell cell)
     {
         List<Cell> neighbors = new List<Cell>();

         int startX = Mathf.Max(0, cell.X - 1);
         int endX = Mathf.Min(grid.GetLength(0) - 1, cell.X + 1);
         int startY = Mathf.Max(0, cell.Y - 1);
         int endY = Mathf.Min(grid.GetLength(1) - 1, cell.Y + 1);

         for (int x = startX; x <= endX; x++)
         {
             for (int y = startY; y <= endY; y++)
             {
                 if (x == cell.X && y == cell.Y)
                 {
                     continue;
                 }

                 neighbors.Add(grid[x, y]);
             }
         }

         return neighbors;
     }

     private bool IsObstacleInPath(Cell a, Cell b)
     {
         Vector2Int cellA = new Vector2Int(a.X, a.Y);
         Vector2Int cellB = new Vector2Int(b.X, b.Y);

         foreach (var position in GetLine(cellA, cellB))
         {
             if (IsObstacle(position))
             {
                 return true;
             }
         }

         return false;
     }

     private IEnumerable<Vector2Int> GetLine(Vector2Int from, Vector2Int to)
     {
         int dx = Mathf.Abs(to.x - from.x);
         int dy = Mathf.Abs(to.y - from.y);
         int sx = (from.x < to.x) ? 1 : -1;
         int sy = (from.y < to.y) ? 1 : -1;
         int err = dx - dy;

         while (true)
         {
             yield return from;

             if (from == to)
             {
                 break;
             }

             int e2 = 2 * err;
             if (e2 > -dy)
             {
                 err -= dy;
                 from.x += sx;
             }
             if (e2 < dx)
             {
                 err += dx;
                 from.y += sy;
             }
         }
     }

     private List<Vector2Int> ReconstructPath(Cell cell)
     {
         List<Vector2Int> path = new List<Vector2Int>();

         while (cell != null)
         {
             path.Insert(0, new Vector2Int(cell.X, cell.Y));
             cell = cell.Parent;
         }

         return path;
     }

     public List<Vector2Int> Plan()
     {
         List<Cell> openList = new List<Cell>();
         HashSet<Cell> closedSet = new HashSet<Cell>();

         start.G = 0;
         start.H = Heuristic(start, goal);
         openList.Add(start);

         while (openList.Count > 0)
         {
             Cell current = openList[0];

             for (int i = 1; i < openList.Count; i++)
             {
                 if (openList[i].G + openList[i].H < current.G + current.H)
                 {
                     current = openList[i];
                 }
             }

             openList.Remove(current);
             closedSet.Add(current);

             if (current == goal)
             {
                 return ReconstructPath(current);
             }

             foreach (var neighbor in current.Neighbors)
             {
                 if (closedSet.Contains(neighbor))
                 {
                     continue;
                 }

                 float tentativeG = current.G + Heuristic(current, neighbor);

                 if (tentativeG < neighbor.G || !openList.Contains(neighbor))
                 {
                     neighbor.Parent = current;
                     neighbor.G = tentativeG;
                     neighbor.H = Heuristic(neighbor, goal);

                     if (!openList.Contains(neighbor))
                     {
                         openList.Add(neighbor);
                     }
                 }
             }
         }

         return null; // No valid path found
     }

     private bool IsObstacle(Vector2Int position)
     {
         // Replace this with your own obstacle checking logic
         // For example, you can check if the position is blocked by a collider in the scene

         // In this example, all positions outside the range of the grid are considered obstacles
         return position.x < 0 || position.x >= grid.GetLength(0) || position.y < 0 || position.y >= grid.GetLength(1);
     }
 }


     */
    #endregion

    #region A*

    /*
     using UnityEngine;
using System.Collections.Generic;

public class AStar : MonoBehaviour
{
    public Transform startTransform;
    public Transform goalTransform;

    private void Start()
    {
        List<Node> path = FindPath(startTransform.position, goalTransform.position);

        if (path != null)
        {
            // Path found, do something with it
            foreach (Node node in path)
            {
                Debug.Log("Node: " + node.position);
            }
        }
        else
        {
            Debug.Log("Path not found!");
        }
    }

    public List<Node> FindPath(Vector3 startPos, Vector3 goalPos)
    {
        Node startNode = new Node(startPos);
        Node goalNode = new Node(goalPos);

        // Create open and closed lists
        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];

            // Find node with lowest F cost
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentNode.FCost || (openList[i].FCost == currentNode.FCost && openList[i].hCost < currentNode.hCost))
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == goalNode)
            {
                // Path found, reconstruct and return path
                return RetracePath(startNode, goalNode);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (closedList.Contains(neighbor))
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openList.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, goalNode);
                    neighbor.parent = currentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        // No path found
        return null;
    }

    private List<Node> RetracePath(Node startNode, Node goalNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = goalNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    private List<Node> GetNeighbors(Node node)
    {
        // Implement your own logic to retrieve the neighboring nodes of a given node
        List<Node> neighbors = new List<Node>();
        // Add the neighboring nodes based on your grid/graph structure
        // For example:
        // neighbors.Add(new Node(node.position + Vector3.up));
        // neighbors.Add(new Node(node.position + Vector3.down));
        // neighbors.Add(new Node(node.position + Vector3.left));
        // neighbors.Add(new Node(node.position + Vector3.right));
        // ...

        return neighbors;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        // Implement your own heuristic function to calculate the distance between two nodes
        int distX = Mathf.Abs((int)nodeA.position.x - (int)nodeB.position.x);
        int distY = Mathf.Abs((int)nodeA.position.y - (int)nodeB.position.y);

        return distX + distY; // Manhattan distance
    }

    public class Node
    {
        public Vector3 position;
        public int gCost; // Cost from start node to this node
        public int hCost; // Estimated cost from this node to goal node
        public Node parent; // Parent node

        public int FCost { get { return gCost + hCost; } }

        public Node(Vector3 pos)
        {
            position = pos;
        }
    }
}

     */

    #endregion

    #region Djikistra

    /*
     


     */
    #endregion

    #region BFA

    /*
     
public class BreadthFirstSearch : MonoBehaviour
{
    public Transform startTransform;
    public Transform goalTransform;

    private void Start()
    {
        List<Node> path = FindPath(startTransform.position, goalTransform.position);

        if (path != null)
        {
            // Path found, do something with it
            foreach (Node node in path)
            {
                Debug.Log("Node: " + node.position);
            }
        }
        else
        {
            Debug.Log("Path not found!");
        }
    }

    public List<Node> FindPath(Vector3 startPos, Vector3 goalPos)
    {
        Node startNode = new Node(startPos);
        Node goalNode = new Node(goalPos);

        Queue<Node> queue = new Queue<Node>();
        HashSet<Node> visited = new HashSet<Node>();
        Dictionary<Node, Node> parent = new Dictionary<Node, Node>();

        queue.Enqueue(startNode);
        visited.Add(startNode);

        while (queue.Count > 0)
        {
            Node currentNode = queue.Dequeue();

            if (currentNode == goalNode)
            {
                // Path found, reconstruct and return path
                return RetracePath(startNode, goalNode, parent);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!visited.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                    parent[neighbor] = currentNode;
                }
            }
        }

        // No path found
        return null;
    }

    private List<Node> RetracePath(Node startNode, Node goalNode, Dictionary<Node, Node> parent)
    {
        List<Node> path = new List<Node>();
        Node currentNode = goalNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = parent[currentNode];
        }

        path.Reverse();
        return path;
    }

    private List<Node> GetNeighbors(Node node)
    {
        // Implement your own logic to retrieve the neighboring nodes of a given node
        List<Node> neighbors = new List<Node>();
        // Add the neighboring nodes based on your grid/graph structure
        // For example:
        // neighbors.Add(new Node(node.position + Vector3.up));
        // neighbors.Add(new Node(node.position + Vector3.down));
        // neighbors.Add(new Node(node.position + Vector3.left));
        // neighbors.Add(new Node(node.position + Vector3.right));
        // ...

        return neighbors;
    }

    public class Node
    {
        public Vector3 position;

        public Node(Vector3 pos)
        {
            position = pos;
        }
    }
}


     */
    #endregion

    #region DFA

    /*
     

    public class DepthFirstSearch : MonoBehaviour
{
    public Transform startTransform;
    public Transform goalTransform;

    private void Start()
    {
        List<Node> path = FindPath(startTransform.position, goalTransform.position);

        if (path != null)
        {
            // Path found, do something with it
            foreach (Node node in path)
            {
                Debug.Log("Node: " + node.position);
            }
        }
        else
        {
            Debug.Log("Path not found!");
        }
    }

    public List<Node> FindPath(Vector3 startPos, Vector3 goalPos)
    {
        Node startNode = new Node(startPos);
        Node goalNode = new Node(goalPos);

        Stack<Node> stack = new Stack<Node>();
        HashSet<Node> visited = new HashSet<Node>();
        Dictionary<Node, Node> parent = new Dictionary<Node, Node>();

        stack.Push(startNode);
        visited.Add(startNode);

        while (stack.Count > 0)
        {
            Node currentNode = stack.Pop();

            if (currentNode == goalNode)
            {
                // Path found, reconstruct and return path
                return RetracePath(startNode, goalNode, parent);
            }

            foreach (Node neighbor in GetNeighbors(currentNode))
            {
                if (!visited.Contains(neighbor))
                {
                    stack.Push(neighbor);
                    visited.Add(neighbor);
                    parent[neighbor] = currentNode;
                }
            }
        }

        // No path found
        return null;
    }

    private List<Node> RetracePath(Node startNode, Node goalNode, Dictionary<Node, Node> parent)
    {
        List<Node> path = new List<Node>();
        Node currentNode = goalNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = parent[currentNode];
        }

        path.Reverse();
        return path;
    }

    private List<Node> GetNeighbors(Node node)
    {
        // Implement your own logic to retrieve the neighboring nodes of a given node
        List<Node> neighbors = new List<Node>();
        // Add the neighboring nodes based on your grid/graph structure
        // For example:
        // neighbors.Add(new Node(node.position + Vector3.up));
        // neighbors.Add(new Node(node.position + Vector3.down));
        // neighbors.Add(new Node(node.position + Vector3.left));
        // neighbors.Add(new Node(node.position + Vector3.right));
        // ...

        return neighbors;
    }

    public class Node
    {
        public Vector3 position;

        public Node(Vector3 pos)
        {
            position = pos;
        }
    }
}
     */
    #endregion



    public static bool CheckMatch<T>(List<List<T>> l0, List<T> l2)
    {
        bool probable = false;
        for (int k = 0; k < l0.Count; k++)
        {
            if (l0[k].Count != l2.Count)
            {
                probable = false;
                continue;
            }

            for (int i = 0; i < l0[k].Count; i++)
            {
                if (!l0[k].Contains(l2[k]))
                {
                    probable = false;
                    break;
                }
                else
                {
                    probable = true;
                }
            }
        }

        return probable;
    }

    public static bool CheckMatch<T>(List<T> l1, List<T> l2)
    {
        if (l1.Count != l2.Count)
            return false;

        for (int i = 0; i < l1.Count; i++)
        {
            if (!l1.Contains(l2[i]))
                return false;
        }

        return true;
    }


    public static Vector3 ClosestPointOnMesh(Mesh mesh, Vector3 point)
    {
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 closestPoint = Vector3.zero;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        // Iterate through each triangle in the mesh
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p0 = vertices[triangles[i]];
            Vector3 p1 = vertices[triangles[i + 1]];
            Vector3 p2 = vertices[triangles[i + 2]];

            // Calculate the closest point on the triangle to the given point
            Vector3 closest = ClosestPointOnTriangle(p0, p1, p2, point);

            // Calculate the squared distance between the point and the closest point
            float distanceSqr = (closest - point).sqrMagnitude;

            // Update the closest point if this distance is smaller
            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestPoint = closest;
            }
        }

        return closestPoint;
    }

    private static Vector3 ClosestPointOnTriangle(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 point)
    {
        // Calculate the barycentric coordinates of the point within the triangle
        Vector3 v0 = p1 - p0;
        Vector3 v1 = p2 - p0;
        Vector3 v2 = point - p0;

        float dot00 = Vector3.Dot(v0, v0);
        float dot01 = Vector3.Dot(v0, v1);
        float dot02 = Vector3.Dot(v0, v2);
        float dot11 = Vector3.Dot(v1, v1);
        float dot12 = Vector3.Dot(v1, v2);

        float invDenom = 1.0f / (dot00 * dot11 - dot01 * dot01);
        float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
        float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

        // Clamp the barycentric coordinates within the valid range
        u = Mathf.Clamp01(u);
        v = Mathf.Clamp01(v);
        float w = 1.0f - u - v;

        // Calculate the closest point on the triangle using the barycentric coordinates
        Vector3 closest = w * p0 + u * p1 + v * p2;

        return closest;
    }

    public static Collider[] SphereCastDirection(Vector3 start, Vector3 end, float radius)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;

        RaycastHit[] hits = Physics.SphereCastAll(start, radius, direction, distance);

        Collider[] hitColliders = new Collider[hits.Length];
        for (int i = 0; i < hits.Length; i++)
        {
            hitColliders[i] = hits[i].collider;
        }

        return hitColliders;
    }
}
