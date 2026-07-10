using UnityEngine;


enum GameState 
{ 
    Setup, Idle
}

public class GameManager : MonoBehaviour
{
    public PathfindingGrid pG;
    public SpawningManager sM;

    public int StartAgents;


    GameState gs = GameState.Setup;

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
        }
    }

    void SetUp() 
    {

        for (int i = 0; i < StartAgents; i++) 
        {
            pG.AddAgent(sM.SpawnAgent(new Vector3(1 + i, 1, 1)));
        }


        gs = GameState.Idle;
    }

}
