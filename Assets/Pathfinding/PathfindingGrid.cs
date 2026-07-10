using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PathfindingGrid : MonoBehaviour
{
    FlowfieldGrid grid;

    public int gridWidth, gridHeight;
    public float cellSize;
    public Vector3 gridStartPoint;

    InputAction leftClick;

    int LAYER_MASK_GRID;

    List<Agent> pathfindingAgents = new List<Agent>();

    int prevFrameAgents;
    int totalAgents;

    Camera cam;

    private void Start()
    {
        cam = Camera.main;

        LAYER_MASK_GRID = LayerMask.GetMask("Grid");

        grid = new FlowfieldGrid(gridWidth, gridHeight, cellSize, gridStartPoint);
        leftClick = InputSystem.actions.FindAction("Attack");

        leftClick.performed += ctx => OnLeftClick();
    }

    private void Update()
    {
        grid.DrawGrid();

        //Set the direction of all the agents in the flowfield
        for (int i = 0; i < prevFrameAgents; i++) 
        {
            pathfindingAgents[i].SetDir(grid.GetDir(pathfindingAgents[i].transform.position));
        }

        if (totalAgents != prevFrameAgents) 
        { 
            prevFrameAgents = totalAgents;
        }
    }

    void OnLeftClick() 
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        //find position mouse is clicking
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LAYER_MASK_GRID)) 
        {
            Debug.Log(hit.point);
            grid.CreateFlowField(hit.point);
        }
        
    }

    public void AddAgent(Agent agent) 
    { 
        pathfindingAgents.Add(agent);
        totalAgents++;
    }
}
