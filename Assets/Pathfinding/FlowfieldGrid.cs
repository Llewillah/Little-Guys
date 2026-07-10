using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GridNode
{
    public int x, y, moveCost, cost;
    public Vector3Int moveDir = Vector3Int.zero;
    //public Tile tile;
    public bool wall = false;


    public GridNode(int x, int y, int moveCost)
    {
        this.x = x;
        this.y = y;
        this.moveCost = moveCost;

    }

    public void SetCost(int cost)
    {
        this.cost = cost + moveCost;
    }

    public void SetWall(bool wall)
    {
        Debug.Log(wall);
        this.wall = wall;
        //tile.SetWall(!wall);
    }
}

public class FlowfieldGrid
{
    GridNode[,] grid;

    //set up all offset directions
    Vector3Int[] offsets = 
        { Vector3Int.forward, Vector3Int.back, Vector3Int.left, Vector3Int.right,
            new Vector3Int(1,0,1),new Vector3Int(-1,0,1),new Vector3Int(1,0,-1),new Vector3Int(-1,0,-1)
            };
    int width, height;
    float cellSize;
    Vector3 originPos;

    public FlowfieldGrid(int width, int height, float cellSize, Vector3 originPos)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPos = originPos;

        grid = new GridNode[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                grid[i, j] = new GridNode(i, j, 1);
                //grid[i, j].tile = UnitSpawner.Instance.SpawnTile(GetWorldPos(i, j) + Vector2.one * cellSize / 2);
            }
        }
    }

    public void DrawGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 pos = GetWorldPos(i, j);

                //Draws GridTiles
                Debug.DrawLine(pos, pos + Vector3.forward * cellSize);
                Debug.DrawLine(pos, pos + Vector3.right * cellSize);

                //Draws Travel Direction
                Vector3 dir = grid[i, j].moveDir;
                Debug.DrawLine(pos + new Vector3(cellSize / 2, 0, cellSize / 2), (pos + dir * cellSize), Color.green);
            }
        }

        Debug.DrawLine(GetWorldPos(width, 0), GetWorldPos(width, height));
        Debug.DrawLine(GetWorldPos(0, height), GetWorldPos(width, height));
    }

    void GetGridPos(Vector3 pos, out int x, out int y)
    {
        pos -= originPos;

        x = (int)(pos.x / cellSize);
        y = (int)(pos.z / cellSize);
    }

    Vector3 GetWorldPos(int x, int y)
    {
        return new Vector3(x * cellSize + cellSize / 2, 0 ,  y * cellSize + cellSize / 2) + originPos;
    }

    public Vector3 GetWorldGridTest(Vector3 pos) 
    {
        GetGridPos(pos, out int x, out int y);
        return GetWorldPos(x, y);
    }

    public Vector3 GetDir(Vector3 pos)
    {
        GetGridPos(pos, out int x, out int y);

        //Checks if pos is outside of the grid, returns value to return to grid

        if (x < 0)
        {
            return Vector3.right;
        }
        else if (x >= width)
        {
            return Vector3.left;
        }

        if (y < 0)
        {
            return Vector3.forward;
        }
        else if (y >= height)
        {
            return Vector3.back;
        }

        if (grid[x, y].wall)
        {
            //return ((Vector2)grid[x, y].tile.gameObject.transform.position - pos.normalized);
        }

        return grid[x, y].moveDir;
    }
    public void CreateFlowField(Vector3 pos)
    {

        GetGridPos(pos, out int x, out int y);


        //Ensures pos is within grid
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return;
        }


        //Creates a queue to do a BFS of the grid
        Queue<GridNode> queue = new Queue<GridNode>();
        bool[,] visited = new bool[width, height];

        //handles target node of the grid
        queue.Enqueue(grid[x, y]);
        grid[x, y].SetCost(0);
        visited[x, y] = true;

        while (queue.Count > 0)
        {
            GridNode node = queue.Dequeue();

            foreach (Vector3Int offset in offsets)
            {
                int newX = node.x + offset.x;
                int newY = node.y + offset.z;

                if (newX >= 0 && newX < width && newY >= 0 && newY < height && Mathf.Abs(offset.x) != Mathf.Abs(offset.z))
                {
                    if (!visited[newX, newY] && !grid[newX, newY].wall)
                    {
                        queue.Enqueue(grid[newX, newY]);
                        visited[newX, newY] = true;
                        grid[newX, newY].SetCost(node.cost);
                        //node.tile.DisplayText(node.cost);
                    }
                }
            }
        }


        foreach (GridNode node in grid)
        {
            int highestCost = node.cost;
            if (node.cost == 1)
            {
                node.moveDir = Vector3Int.zero;
                //node.tile.DisableDisplay();
            }


            foreach (Vector3Int offset in offsets)
            {
                //Debug.Log(offset);
                int newX = node.x + offset.x;
                int newY = node.y + offset.z;

                if (newX >= 0 && newX < width && newY >= 0 && newY < height)
                {
                    if (grid[newX, newY].cost < highestCost && !grid[newX, newY].wall)
                    {
                        node.moveDir = offset;
                        highestCost = grid[newX, newY].cost;
                        //node.tile.SetArrowRotaion(offset);
                    }
                }
            }
        }
    }
}
