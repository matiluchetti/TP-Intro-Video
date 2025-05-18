using UnityEngine;
using Strategy;

public class ZombieAI : MonoBehaviour, IDamagable
{
    public Transform target;
    public float speed = 2f;


    [SerializeField] private float _maxLife = 100f;
    private float _currentLife;

    public float Life => _currentLife;
    public float MaxLife => _maxLife;

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

        // Moverse hacia el jugador
        Vector3 newPos = _rb.position + direction * speed * Time.fixedDeltaTime;
        _rb.MovePosition(newPos);

        // Mirar hacia el jugador
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, targetRotation, 10f * Time.fixedDeltaTime));
        }
    }

    // Implementación de daño
    public void TakeDamage(int amount)
    {
        _currentLife -= amount;
        Debug.Log($"Zombie recibió {amount} de daño. Vida restante: {_currentLife}");

        if (_currentLife <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Podés agregar animaciones o partículas acá
        Destroy(gameObject);
    }
}
