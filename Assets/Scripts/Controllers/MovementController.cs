using UnityEngine;

public class MovementController : MonoBehaviour, IMoveablle
{
    #region IMOVEABLE_PROPERTIES
    public float Speed => _speed;
    private float _speed = 10;
    #endregion

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    #region IMOVEABLE_METHODS
    public void Move(Vector3 direction)
    {
        _rb.linearVelocity = new Vector3(direction.x * Speed, _rb.linearVelocity.y, direction.z * Speed);
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            Vector3 pushDirection = (transform.position - collision.transform.position).normalized;

            _rb.AddForce(pushDirection * 5f, ForceMode.Impulse);  // Ajusta la fuerza según sea necesario
        }
    }
}
