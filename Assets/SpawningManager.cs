using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SpawningManager : MonoBehaviour
{
    public GameObject test1, tile, unit, boss;

    public void SpawnTest(Vector3 pos) 
    { 
        Instantiate(test1, pos, Quaternion.identity);
    }

    public void SpawnTile(Vector3 pos, float scale) 
    { 
        GameObject obj = Instantiate(tile, pos, Quaternion.identity);
        tile.transform.localScale = new Vector3(scale,1,scale);
    }

    public GameObject SpawnUnit(Vector3 pos) 
    { 
        return Instantiate(unit, pos, Quaternion.identity);
    }

    public BossScript SpawnBoss(Vector3 pos) 
    {
        GameObject obj = GameObject.Instantiate(boss, pos, Quaternion.identity);
        return obj.GetComponent<BossScript>();
    }
}
