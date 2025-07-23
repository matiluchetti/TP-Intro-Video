using UnityEngine;
using Strategy;
using System.Collections;

public class DevilAI : MonoBehaviour, IDamagable
{
    public Transform target;
    [SerializeField] private EnemyTypeSO enemyType;
    private float _currentLife;
    private bool isBeingPushedBack = false;
    private float originalSpeed;
    private Rigidbody _rb;
    public System.Action onDeath;
    private Animator anim;

    [Header("Disparo")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 2f;
    [SerializeField] private float fireRange = 15f;
    [SerializeField] private float bulletSpreadRadius = 1f;
    [SerializeField] private AudioClip shotClip;
    private float nextFireTime = 0f;
    private AudioSource _audioSource;

    public float Life => _currentLife;
    public float MaxLife => enemyType.maxLife;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _currentLife = enemyType.maxLife;

        if (player != null)
        {
            target = player.transform;
        }
    }

    void Awake()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = target.position;
        targetPos.y = transform.position.y;

        Vector3 direction = (targetPos - transform.position);
        float distance = direction.magnitude;
        direction.Normalize();

        float stopDistance = 1.5f; 
        if (distance > stopDistance)
        {
            Vector3 newPos = _rb.position + direction * enemyType.speed * Time.fixedDeltaTime;
            _rb.MovePosition(newPos);
        }

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, targetRotation, 0.15f));
        }

        TryShoot();
    }

    private void TryShoot()
    {
        if (Time.time >= nextFireTime && bulletPrefab != null && firePoint != null && target != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);
            if (distanceToPlayer <= fireRange && CanShootPlayer())
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    private void Shoot()
    {
        anim.SetTrigger("DevilShot");
        Vector3 targetPos = target.position;
        targetPos.y = firePoint.position.y;
        Vector3 direction = (targetPos - firePoint.position).normalized;
        Quaternion bulletRotation = Quaternion.LookRotation(direction);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
        if (shotClip != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(shotClip);
        }
    }

    public void TakeDamage(int amount)
    {
        _currentLife -= amount;
        Debug.Log($"Devil recibió {amount} de daño. Vida restante: {_currentLife}");

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
            anim.SetTrigger("DevilShot");
            IDamagable player = collision.gameObject.GetComponent<IDamagable>();
            player?.TakeDamage(Mathf.RoundToInt(enemyType.damage));
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

    private bool CanShootPlayer()
    {
        Vector3 directionToPlayer = target.position - transform.position;
        Ray ray = new Ray(transform.position, directionToPlayer.normalized);
        Debug.DrawRay(ray.origin, ray.direction * directionToPlayer.magnitude, Color.red);
        RaycastHit[] hits = Physics.RaycastAll(ray, directionToPlayer.magnitude);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        
        foreach (var hit in hits)
        {
            if (hit.transform.CompareTag("Devil"))
                continue;
            
            if (hit.transform.gameObject.layer == 8)
            {
                nextFireTime = Time.time + fireRate;
                return false;
            }
            
            // Si es el jugador, permitir disparo
            if (hit.transform == target)
            {
                return true;
            }
        }
        
        return true;
    }
}
