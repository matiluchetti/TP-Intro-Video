using UnityEngine;
using Strategy;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour, IMoveable
{
    [SerializeField] private float _speed = 10;
    public float Speed => _speed;

    private Rigidbody _rb;
    private Vector3 _moveDirection = Vector3.zero;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 direction)
    {
        // Acumula el movimiento para este frame
        _moveDirection += direction.normalized;
    }

    public void RotateTowards(Vector3 point)
    {
        Vector3 direction = (point - transform.position);
        direction.y = 0; // Mantener solo la rotación en el plano XZ
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }

    private void FixedUpdate()
    {
        if (_moveDirection != Vector3.zero)
        {
            Vector3 newPos = _rb.position + _moveDirection.normalized * Speed * Time.fixedDeltaTime;
            newPos.y = 0;
            _rb.MovePosition(newPos);
        }

        _moveDirection = Vector3.zero; // Limpiar después de FixedUpdate
    }
}
