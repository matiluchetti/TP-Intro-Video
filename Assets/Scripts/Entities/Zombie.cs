using UnityEngine;
using Strategy;
using System.Collections;

public class ZombieAI : MonoBehaviour, IDamagable
{
    public Transform target;
    public float speed = 2f;

    [SerializeField] private float _maxLife = 100f;
    private float _currentLife;
    private float _damage = 10f;

    public float Life => _currentLife;
    public float MaxLife => _maxLife;

    private bool isBeingPushedBack = false;
    private float originalSpeed;
    private Rigidbody _rb;

    public System.Action onDeath;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _currentLife = _maxLife;

        if (player != null)
        {
            target = player.transform;
        }
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position);
        direction.y = 0f; 
        direction.Normalize();

        Vector3 newPos = _rb.position + direction * speed * Time.fixedDeltaTime;
        newPos.y = 0f;
        _rb.MovePosition(newPos);

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            _rb.MoveRotation(Quaternion.RotateTowards(_rb.rotation, targetRotation, 360f * Time.fixedDeltaTime));
        }
    }

    public void TakeDamage(int amount)
    {
        _currentLife -= amount;
        Debug.Log($"Zombie recibió {amount} de daño. Vida restante: {_currentLife}");

        if (_currentLife <= 0)
        {
            Die();
        }
        else
        {
            Vector3 pushDir = -transform.forward;
            StartCoroutine(PushBack(pushDir * 10f, 0.3f));
        }
    }

    private void Die()
    {
        onDeath?.Invoke();
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamagable player = collision.gameObject.GetComponent<IDamagable>();
            player?.TakeDamage(Mathf.RoundToInt(_damage));

            Vector3 pushDir = (transform.position - collision.transform.position).normalized;
            StartCoroutine(PushBack(pushDir * 10f, 0.3f));
        }
    }

    private IEnumerator PushBack(Vector3 pushVector, float duration)
    {
        if (isBeingPushedBack) yield break;

        isBeingPushedBack = true;
        originalSpeed = speed;
        speed = 0;

        float timer = 0f;
        while (timer < duration)
        {
            _rb.MovePosition(_rb.position + pushVector * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        speed = originalSpeed;
        isBeingPushedBack = false;
    }

    public void SetStats(float life, float speed, float damage)
    {
        _maxLife = life;
        _currentLife = _maxLife;
        this.speed = speed;
        this._damage = damage;
    }
}
