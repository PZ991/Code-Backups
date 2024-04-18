using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePathFinding : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 GridSpec;
    public Vector3 GridSize;
    public Gridmap[,] grid;
    public Vector2Int userposition;
    public Vector2Int targetposition;
    public Gridmap currentsearching;
    public bool generated;
    public bool finished;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        if (!generated)
        {
            grid = new Gridmap[Mathf.RoundToInt(GridSize.x), Mathf.RoundToInt(GridSize.y)];
            for (int x = 0; x < GridSize.x; x++)
            {
                for (int y = 0; y < GridSize.y; y++)
                {
                    Vector3 pos = new Vector3(transform.position.x + (x), transform.position.y + (y), transform.position.z);
                    //Gizmos.DrawCube( pos, GridSpec);
                    grid[x, y].position = pos;
                    grid[x, y].Gridposition = new Vector2Int(x,y);
                    grid[x, y].ucost = Mathf.Abs(Vector2.Distance(userposition, new Vector2(x, y)));
                    grid[x, y].tcost = Mathf.Abs(Vector2.Distance(targetposition, new Vector2(x, y)));
                    grid[x, y].ocost = grid[x, y].ucost + grid[x, y].tcost;
                    currentsearching = grid[userposition.x, userposition.y];
                    
                    generated = true;
                }
            }
        }
        else
        {
            //while (!finished)
            {
                SearchPath();
                foreach (var item in grid)
                {
                   // if(real2.Gridposition== item.Gridposition)
                    {

                    }
                     if(item.Gridposition == grid[targetposition.x,targetposition.y].Gridposition)
                        Gizmos.color = Color.blue;
                   // else if (!neighbors.Contains(item))

                    //    Gizmos.color = Color.green;
                    else
                        Gizmos.color = Color.red;
                    Gizmos.DrawCube( item.position, GridSpec);

                }
            }
        }

    }
    public void SearchPath()
    {
        /*
        bool found = false ;
        List<Gridmap> neighbors = Neighbors(currentsearching);
        Gridmap real2 = new Gridmap();
        foreach (var item in neighbors)
        {
            Gridmap real = item;
            if (item.ocost < real.ocost)
            {
                real = item;
            }
            real2 = real;
            if(item.Gridposition==targetposition)
            {
                Debug.Log("Found");
                found = true;
            }
        }
        currentsearching = real2;
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(currentsearching.position, 1f);
        if (!found)
            SearchPath();
        Debug.Log(real2.Gridposition);
        */
    }
    public List<Gridmap> Neighbors(Gridmap grid2)
    {
        List<Gridmap> nb = new List<Gridmap>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <=1; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                int checkX = grid2.Gridposition.x + x;
                int checkY = grid2.Gridposition.y + y;
                if (checkX >= 0 && checkX < GridSize.x && checkY >= 0 && checkY < GridSize.y)
                    nb.Add(grid[checkX, checkY]);
            }
        }
        return nb;
    }
}

public struct Gridmap
{
    public Vector3 position;
    public Vector2Int Gridposition;
    public float tcost;         //target cose
    public float ucost;         //user cost
    public float ocost;         //overall cost
    public bool done;
}
