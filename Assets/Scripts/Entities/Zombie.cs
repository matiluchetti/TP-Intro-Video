using UnityEngine;
using Strategy;
using System.Collections;

public class ZombieAI : MonoBehaviour, IDamagable
{
    public Transform target;
    [SerializeField] private EnemyTypeSO enemyType;
    [SerializeField] private GameObject bloodEffectPrefab;
    private float _currentLife;
    private bool isBeingPushedBack = false;
    private float originalSpeed;
    private Rigidbody _rb;
    public System.Action onDeath;
    private Animator anim;

    public float Life => _currentLife;
    public float MaxLife => enemyType.maxLife;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _currentLife = enemyType.maxLife;

        if (player != null)
        {
            target = player.transform;
        }
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position);
        direction.y = 0f; 
        direction.Normalize();

        Vector3 newPos = _rb.position + direction * enemyType.speed * Time.fixedDeltaTime;
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

        if (bloodEffectPrefab != null)
        {
            Vector3 bloodPosition = transform.position + Vector3.forward * (-1.5f) + Vector3.up * 6f;
            Instantiate(bloodEffectPrefab, bloodPosition, Quaternion.identity, transform);
        }

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
            anim.SetTrigger("ZombieAttack");
            IDamagable player = collision.gameObject.GetComponent<IDamagable>();
            player?.TakeDamage(Mathf.RoundToInt(enemyType.damage));

            Vector3 pushDir = (transform.position - collision.transform.position).normalized;
            StartCoroutine(PushBack(pushDir * 10f, 0.3f));
        }
    }

    private IEnumerator PushBack(Vector3 pushVector, float duration)
    {
        if (isBeingPushedBack) yield break;

        isBeingPushedBack = true;
        originalSpeed = enemyType.speed;

        float timer = 0f;
        while (timer < duration)
        {
            _rb.MovePosition(_rb.position + pushVector * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        isBeingPushedBack = false;
    }

    public void SetStats(float life, float speed, float damage)
    {
        _currentLife = life;
    }
}
