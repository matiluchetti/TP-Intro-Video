using System.Collections;
using UnityEngine;
using Strategy;

public class Character : MonoBehaviour, IDamagable
{
    [SerializeField] private float _maxLife = 100f;
    private float _currentLife;

    public float Life => _currentLife;
    public float MaxLife => _maxLife;

    private Vector3 _pushBackOffset = Vector3.zero;
    private float _pushBackSpeed = 10f;
    private bool _inputBlocked = false;

    private Rigidbody _rb;
    public static event System.Action<float> OnPlayerDamaged;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _currentLife = _maxLife;
        StartCoroutine(RegenerateHealth());

    }

    private void FixedUpdate()
    {
        if (_inputBlocked)
        {
            Vector3 moveDelta = _pushBackOffset * Time.fixedDeltaTime * _pushBackSpeed;
            _rb.MovePosition(_rb.position + moveDelta);
            _pushBackOffset = Vector3.Lerp(_pushBackOffset, Vector3.zero, Time.fixedDeltaTime * 5f);
        }
    }

    public void TakeDamage(int amount)
    {
        _currentLife -= amount;
        Debug.Log($"Character recibió {amount} de daño. Vida restante: {_currentLife}");

        _currentLife -= amount;
        OnPlayerDamaged?.Invoke(amount); // Dispara el evento

        if (_currentLife <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Personaje murió.");
        EventManager.instance.EventGameOver(false);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Respawn") && !_inputBlocked)
        {
            Vector3 pushDir = (transform.position - collision.transform.position).normalized;
            _pushBackOffset = pushDir * 1f;
            StartCoroutine(BlockInputTemporarily(0.2f));
        }
    }

    private IEnumerator BlockInputTemporarily(float duration)
    {
        _inputBlocked = true;
        yield return new WaitForSeconds(duration);
        _inputBlocked = false;
        _pushBackOffset = Vector3.zero;
    }

    private IEnumerator RegenerateHealth()
{
    while (_currentLife > 0)
    {
        yield return new WaitForSeconds(1f);
        if (_currentLife < _maxLife)
        {
            _currentLife = Mathf.Min(_currentLife + 1, _maxLife);
        }
    }
}
}
