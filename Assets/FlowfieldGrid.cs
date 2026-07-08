using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GridNode
{
    public int x, y, moveCost, cost;
    public Vector2Int moveDir = Vector2Int.zero;
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
    Vector3Int[] offsets = { Vector3Int.forward, Vector3Int.back, Vector3Int.left, Vector3Int.right };
    int width, height;
    float cellSize;
    Vector2 originPos;

    public FlowfieldGrid(int width, int height, float cellSize, Vector2 originPos)
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
                Vector2 pos = GetWorldPos(i, j);

                //Draws GridTiles
                Debug.DrawLine(pos, pos + Vector2.up * cellSize);
                Debug.DrawLine(pos, pos + Vector2.right * cellSize);

                //Draws Travel Direction
                Vector2 dir = grid[i, j].moveDir;
                Debug.DrawLine(pos + new Vector2(cellSize / 2, cellSize / 2), (pos + dir * cellSize) + new Vector2(cellSize / 2, cellSize / 2), Color.green);
            }
        }

        Debug.DrawLine(GetWorldPos(width, 0), GetWorldPos(width, height));
        Debug.DrawLine(GetWorldPos(0, height), GetWorldPos(width, height));
    }

    void GetGridPos(Vector2 pos, out int x, out int y)
    {
        pos -= originPos;

        x = (int)(pos.x / cellSize);
        y = (int)(pos.y / cellSize);
    }

    Vector2 GetWorldPos(int x, int y)
    {
        return new Vector2(x * cellSize, y * cellSize) + originPos;
    }

    public void SetWall(Vector2 pos)
    {
        GetGridPos(pos, out int x, out int y);

        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return;
        }

        grid[x, y].SetWall(!grid[x, y].wall);
    }

    public Vector2 GetDir(Vector2 pos)
    {
        GetGridPos(pos, out int x, out int y);

        if (x < 0)
        {
            x = 0;
        }
        else if (x >= width)
        {
            x = width - 1;
        }

        if (y < 0)
        {
            y = 0;
        }
        else if (y >= height)
        {
            y = height - 1;
        }

        if (grid[x, y].wall)
        {
            //return ((Vector2)grid[x, y].tile.gameObject.transform.position - pos.normalized);
        }

        return grid[x, y].moveDir;
    }
    public void CreateFlowField(Vector2 pos)
    {
        GetGridPos(pos, out int x, out int y);

        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return;
        }


        Queue<GridNode> queue = new Queue<GridNode>();
        bool[,] visited = new bool[width, height];
        queue.Enqueue(grid[x, y]);
        grid[x, y].SetCost(0);
        visited[x, y] = true;

        while (queue.Count > 0)
        {
            GridNode node = queue.Dequeue();

            foreach (Vector2Int offset in offsets)
            {
                int newX = node.x + offset.x;
                int newY = node.y + offset.y;

                if (newX >= 0 && newX < width && newY >= 0 && newY < height && Mathf.Abs(offset.x) != Mathf.Abs(offset.y))
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
                //node.tile.DisableDisplay();
            }


            foreach (Vector2Int offset in offsets)
            {
                //Debug.Log(offset);
                int newX = node.x + offset.x;
                int newY = node.y + offset.y;

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
