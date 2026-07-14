using UnityEngine;
using System.Collections.Generic;

enum GameState 
{ 
    Setup, Play, End, Idle
}

public class GameManager : MonoBehaviour
{
    public PathfindingGrid pG;
    public SpawningManager sM;

    public int StartAgents;



    BossScript curBoss;
    List<Unit> units = new List<Unit>();
    Stack<Unit> unitsToRemove = new Stack<Unit>();

    GameState gs = GameState.Setup;

    float frameDamage = 0;

    private void Update()
    {
        CheckState();
    }

    void CheckState() 
    {
        switch (gs) 
        {
            case GameState.Setup:

                SetUp();
                break;
            case GameState.Play:
                Play();
                break;
            case GameState.End:
                break;
        }
    }

    // Setup new bosses and units
    void SetUp() 
    {
        curBoss = sM.SpawnBoss(new Vector3(5, 1, 5));

        pG.SetTarget(curBoss.gameObject.transform);

        for (int i = 0; i < StartAgents; i++) 
        {
            GameObject newUnit = sM.SpawnUnit(new Vector3(1 + i, 1, 1));

            pG.AddAgent(newUnit.GetComponent<Agent>());
            units.Add(newUnit.GetComponent<Unit>());
        }

        

        gs = GameState.Play;
    }

    //handle all game updates here
    void Play() 
    {
        //Handle boss frame
        
        frameDamage = 0;
        curBoss.CheckState();
        frameDamage = 0;
        //Update the pathfinding grid
        pG.DoUpdate();

        //Handle all the units
        foreach (Unit u in units) 
        { 
            u.CheckRange(curBoss.transform.position);
            u.DoUpdate();
            frameDamage += u.GetFrameDamage();


            if (u.death) 
            {
                unitsToRemove.Push(u);
            }
        }


        //Remove any dead units
        while (unitsToRemove.Count > 0) 
        { 
            Unit u = unitsToRemove.Pop();
            units.Remove(u);
            pG.RemoveAgent(u.gameObject.GetComponent<Agent>());

            //Redundant - Do animation here instead
            Destroy(u.gameObject);
        }

        if (curBoss.TakeDamage(frameDamage)) 
        {
            gs = GameState.End;
        }
    }

}
