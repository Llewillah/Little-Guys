using UnityEngine;
using UnityEngine.InputSystem.Android;

public class Unit : MonoBehaviour
{
    float range = 1;
    public bool death = false;
    float damage = 1;
    float maxHealth;
    float curHealth = 1;
    float attackCooldown = 0;

    float attackTimer = 0;

    float frameDamage = 0;

    Agent a;

    private void Start()
    {
        a = GetComponent<Agent>();
    }

    public void DoUpdate() 
    {
        if (!a.active) 
        {
            DoAttack();
        }
    }

    public void TakeDamage(float damage, Vector3 knockPos, float knockForce)  
    { 
        a.AddKnockBack(knockPos, knockForce);
        curHealth -= damage;

        if (curHealth <= 0) 
        {
            DoDeath();
        }
    }

    void DoDeath() 
    { 
        death = true;
    }

    void StartAttack() 
    {
        a.active = false;
    }

    void DoAttack() 
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            frameDamage += damage;
            attackTimer = 0;
        }
    }

    public void CheckRange(Vector3 pos) 
    {
        if ((pos - transform.position).magnitude <= range) 
        {
            Debug.Log("start attack");
            StartAttack();
        }
    }

    public float GetFrameDamage() 
    {
        float temp = frameDamage;
        frameDamage = 0;
        return temp;
    }

}
