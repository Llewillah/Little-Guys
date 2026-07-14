using UnityEngine;
using System.Collections.Generic;
using System.Threading;

enum BossState
{
    Idle, CheckAttack, Attack, Move
}

public class BossScript : MonoBehaviour
{
    BossState state = BossState.CheckAttack;
    public IAttackable[] attacks;
    IAttackable selectedAttack;

    public float health = 1;
    public float speed;

    public float attackCooldown;
    float attackTimer = 0;
    float chargeTimer = 0;

    LayerMask UNIT_LAYER_MASK;

    Rigidbody rb;

    Vector3 targetDir;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        attacks = GetComponentsInChildren<IAttackable>();

        UNIT_LAYER_MASK = LayerMask.GetMask("Unit");
    }

    public void CheckState()
    {
        switch (state)
        {
            case BossState.Idle:
                Idle();
                break;
            case BossState.CheckAttack:
                CheckAttack();
                break;
            case BossState.Attack:
                Attack();
                break;
            case BossState.Move:
                Move();
                break;
        }
    }

    void Idle() 
    {
        if (health < 0) 
        { 
            //DO ALL DYING STUFF HERE
        }

        attackTimer += Time.deltaTime;

        if (attackTimer > attackCooldown) 
        {
            state = BossState.CheckAttack;
        }
    }

    void CheckAttack() 
    {
        //Get range of random attack and check if any units are in range
        int rand = Random.Range(0, attacks.Length);
        selectedAttack = attacks[rand];

        int range = selectedAttack.GetRange();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, UNIT_LAYER_MASK);

        //ensure minDist will always start higher than any unit
        float minDist = range + 1;
        Vector3 target = Vector3.zero;


        foreach (Collider c in hitColliders)
        {
            float dist = (c.gameObject.transform.position - transform.position).magnitude;
            if (minDist > dist)
            {
                target = c.gameObject.transform.position;
                minDist = dist;
            }
        }

        //Look towards the closest unit within range & do attack
        if (target != Vector3.zero)
        {
            transform.LookAt(new Vector3(target.x, transform.position.y, target.z), Vector3.up);
            state = BossState.Attack;
            attackTimer = 0;
        }
        else
        {
            state = BossState.Idle;
        }
    }

    void Attack() 
    {
        //Check if attack has charged
        chargeTimer += Time.deltaTime;

        //Setall animations here

        if (chargeTimer > selectedAttack.GetChargeTime()) 
        {
            //Do Attack
            selectedAttack.DoAttack();
            chargeTimer = 0;
            state = BossState.Idle;
        }

    }
    void Move() 
    {
        rb.MovePosition(transform.position + targetDir.normalized * speed * Time.deltaTime);
        state = BossState.Idle;
    }

    public bool TakeDamage(float damage) 
    {
        health -= damage;

        if (health <= 0) 
        {
            return true;
        }

        return false;
    }
}
