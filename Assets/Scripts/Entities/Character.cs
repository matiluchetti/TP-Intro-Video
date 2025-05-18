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

    private void Start()
    {
        _currentLife = _maxLife;
    }

    private void Update()
    {
        if (_inputBlocked)
        {
            transform.position += _pushBackOffset * Time.deltaTime * _pushBackSpeed;
            _pushBackOffset = Vector3.Lerp(_pushBackOffset, Vector3.zero, Time.deltaTime * 5f);
        }
    }

    public void TakeDamage(int amount)
    {
        _currentLife -= amount;
        Debug.Log($"Character recibió {amount} de daño. Vida restante: {_currentLife}");

        if (_currentLife <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Puedes agregar aquí animaciones, sonidos, efectos, etc.
        Debug.Log("Personaje murió.");
        EventManager.instance.EventGameOver(true);
        Destroy(gameObject); // O alguna lógica de respawn o desactivación
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
}
