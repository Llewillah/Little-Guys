using UnityEngine;

public class SpawningManager : MonoBehaviour
{
    public GameObject test1, tile, agent;

    public void SpawnTest(Vector3 pos) 
    { 
        Instantiate(test1, pos, Quaternion.identity);
    }

    public void SpawnTile(Vector3 pos, float scale) 
    { 
        GameObject obj = Instantiate(tile, pos, Quaternion.identity);
        tile.transform.localScale = new Vector3(scale,1,scale);
    }

    public Agent SpawnAgent(Vector3 pos) 
    { 
        GameObject obj = GameObject.Instantiate(agent, pos, Quaternion.identity);
        return obj.GetComponent<Agent>();
    }
}
