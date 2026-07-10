using UnityEngine;

public class Unit : MonoBehaviour
{
    Agent a;

    private void Start()
    {
        a = GetComponent<Agent>();
    }

    public void TakeDamage(float damage, Vector3 knockPos, float knockForce)  
    { 
        a.AddKnockBack(knockPos, knockForce);
    }
}
