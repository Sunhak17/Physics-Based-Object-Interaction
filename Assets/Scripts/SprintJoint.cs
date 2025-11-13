using UnityEngine;

public class AutoSpringMove : MonoBehaviour
{
    public float force = 5f;
    public float frequency = 2f;
    private Rigidbody rb;
    private float time = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        float direction = Mathf.Sin(time * frequency); // goes left & right
        rb.AddForce(Vector3.right * direction * force);
    }
}
