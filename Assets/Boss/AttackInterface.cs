using UnityEngine;

public interface IAttackable 
{
    public void DoAttack();

    public int GetRange();

    public float GetChargeTime();
}
