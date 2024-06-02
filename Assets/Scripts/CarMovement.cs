using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] public float speed { get; private set; } = 10f;
    [SerializeField] private float viewDistance = 5f;
    [SerializeField] private Rigidbody rb;

    private Vector3 leftRay;
    private Vector3 leftForwardRay;
    private Vector3 leftBackRay;

    private Vector3 forwardRay;

    private Vector3 rightForwardRay;
    private Vector3 rightRay;
    private Vector3 rightBackRay;


    private Vector3 forwardDirection;

    public bool isDead = false;

    private void Start()
    {
        rb.velocity = transform.forward * speed;
    }

    private void Update()
    {
        if (isDead) return;
        forwardDirection = transform.forward;

        rb.velocity = forwardDirection * speed;
        
        leftRay = Quaternion.Euler(0, -90, 0) * forwardDirection;
        leftForwardRay = Quaternion.Euler(0, -45, 0) * forwardDirection;
        leftBackRay = Quaternion.Euler(0, -165, 0) * forwardDirection;

        forwardRay = Quaternion.Euler(0, 0, 0) * forwardDirection;

        rightForwardRay = Quaternion.Euler(0, 45, 0) * forwardDirection;
        rightRay = Quaternion.Euler(0, 90, 0) * forwardDirection;
        rightBackRay = Quaternion.Euler(0, 165, 0) * forwardDirection;
    }

    public float[] returnRayCast()
    {
        float[] test = new float[7];

        test[0] = CheckForObstacles(leftRay);
        test[1] = CheckForObstacles(leftForwardRay);
        test[2] = CheckForObstacles(forwardRay);

        test[3] = CheckForObstacles(rightForwardRay);

        test[4] = CheckForObstacles(rightRay);
        test[5] = CheckForObstacles(rightBackRay);
        test[6] = CheckForObstacles(leftBackRay);

        return test;
    }

    public void Action(int _i)
    {
        if (_i == 0)
            transform.Rotate(Vector3.up * 0.3f);
        if (_i == 1)
            speed += Time.deltaTime * 4f;
        if (_i == 2)
            transform.Rotate(Vector3.down * 0.3f);
        speed -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer != 3 && other.gameObject.layer != 6)
        {
            isDead = true;
            Debug.Log("Dead");
            gameObject.SetActive(false);
        }
    }

    private float CheckForObstacles(Vector3 direction)
    {
        Ray ray = new Ray(transform.position, transform.forward + direction);

        RaycastHit hit;

        float maxRaycastDistance = 20f;

        if (Physics.Raycast(ray, out hit, maxRaycastDistance))
            return hit.distance;

        return maxRaycastDistance;
    }

    void OnDrawGizmos()
    {
        DrawGizmo(leftRay, Color.red);
        DrawGizmo(leftForwardRay, Color.red);
        DrawGizmo(leftBackRay, Color.red);

        DrawGizmo(forwardRay, Color.cyan);

        DrawGizmo(rightForwardRay, Color.blue);
        DrawGizmo(rightRay, Color.blue);
        DrawGizmo(rightBackRay, Color.blue);
    }

    void DrawGizmo(Vector3 direction, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawRay(transform.position, direction * viewDistance + transform.forward);
    }
}