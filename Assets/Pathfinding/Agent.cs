using UnityEngine;

public class Agent : MonoBehaviour
{
    public int speed;
    Vector3 dir = Vector3.zero;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + dir.normalized * speed * Time.fixedDeltaTime);
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
