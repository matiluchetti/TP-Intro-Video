using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    public Transform target;
    public float speed = 2f;
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 newPos = _rb.position + direction * speed * Time.fixedDeltaTime;
        _rb.MovePosition(newPos);
    }

}
