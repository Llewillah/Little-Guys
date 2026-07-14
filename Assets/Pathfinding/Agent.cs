using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Agent : MonoBehaviour
{
    public int speed;
    Vector3 dir = Vector3.zero;
    Rigidbody rb;

    public bool active = true;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void DoUpdate() 
    {
        rb.MovePosition(transform.position + dir.normalized * speed * Time.deltaTime);
    }

    public void SetDir(Vector3 dir)
    {
        this.dir = dir;
    }

    public void AddKnockBack(Vector3 forcePos, float knockForce) 
    {
        Vector3 dir = (transform.position - forcePos).normalized;

        rb.AddForce(dir * knockForce);
    }
}
