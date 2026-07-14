using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using System.Diagnostics.Contracts;

public class PathfindingGrid : MonoBehaviour
{
    Vector3 prevFrame = Vector3.zero;
    Transform target;
    FlowfieldGrid grid;

    public int gridWidth, gridHeight;
    public float cellSize;
    public Vector3 gridStartPoint;

    int LAYER_MASK_GRID;

    List<Agent> pathfindingAgents = new List<Agent>();

    int totalAgents;

    Camera cam;

    private void Start()
    {
        cam = Camera.main;

        LAYER_MASK_GRID = LayerMask.GetMask("Grid");

        grid = new FlowfieldGrid(gridWidth, gridHeight, cellSize, gridStartPoint);
    }

    public void DoUpdate() 
    {
        grid.DrawGrid();

        if ((int)prevFrame.x / cellSize != (int)target.position.x / cellSize || (int)prevFrame.z / cellSize != (int)target.position.z / cellSize) 
        {
            grid.CreateFlowField(target.position);
        }


        //Set the direction of all the agents in the flowfield
        for (int i = 0; i < totalAgents; i++)
        {
            if (pathfindingAgents[i].active)
            {
                //Check if agent is not in same tile as boss
                if (grid.GetDir(pathfindingAgents[i].transform.position) != Vector3.zero)
                {
                    pathfindingAgents[i].SetDir(grid.GetDir(pathfindingAgents[i].transform.position));
                }
                else
                {
                    pathfindingAgents[i].SetDir(target.position - pathfindingAgents[i].transform.position);
                }

                pathfindingAgents[i].DoUpdate();
            }
        }
    }

    public void AddAgent(Agent agent) 
    { 
        pathfindingAgents.Add(agent);
        totalAgents++;
    }

    public void RemoveAgent(Agent agent) 
    { 
        pathfindingAgents.Remove(agent);
        totalAgents--;
    }

    public void SetTarget(Transform target) 
    {
        this.target = target;
        grid.CreateFlowField(target.position);
        prevFrame = target.position;
    }

    //Will be useful for A* pathfinding
    /*void OnLeftClick() 
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        //find position mouse is clicking
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LAYER_MASK_GRID)) 
        {
            grid.CreateFlowField(hit.point);
        }
        
    }*/
}
