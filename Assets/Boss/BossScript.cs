using Unity.VisualScripting;
using UnityEngine;

enum BossState
{
    Idle, Attack, Move
}

public class BossScript : MonoBehaviour
{
    BossState state;
    public IAttackable[] attacks;

    private void Update()
    {
        CheckState();
    }

    void CheckState()
    {
        switch (state)
        {
            case BossState.Idle:
                break;
            case BossState.Attack:
                break;
            case BossState.Move:
                break;
        }
    }
}
