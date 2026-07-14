using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Diagnostics.Contracts;
using System.Net.Http.Headers;

public class BasicAttack : MonoBehaviour, IAttackable
{
    public int range;
    public float damage;
    public float knockBack = 500;

    public float chargeTime;

    public Collider[] col;

    List<Unit> targets = new List<Unit>();
    public void DoAttack() 
    {
        foreach (Unit target in targets) 
        {
            target.TakeDamage(damage, transform.parent.position, knockBack);
        }

        targets.Clear();
    }

    public int GetRange() 
    {
        return range;
    }

    public float GetChargeTime() 
    {
        return chargeTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Unit>(out Unit u))
        {
            targets.Add(u);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Unit>(out Unit u))
        {
            targets.Remove(u);
        }
    }
}
