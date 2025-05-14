using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private MovementController _movementController;
    [SerializeField] private Gun _gun;

    // BINDING ATTACK KEYS
    [SerializeField] private KeyCode _shoot = KeyCode.Mouse0;
    [SerializeField] private KeyCode _reload = KeyCode.R;
    // BINDING MOVEMENT KEYS
    [SerializeField] private KeyCode _moveForward = KeyCode.W;
    [SerializeField] private KeyCode _moveBack = KeyCode.S;
    [SerializeField] private KeyCode _MoveLeft = KeyCode.A;
    [SerializeField] private KeyCode _moveRight = KeyCode.D;

    private Vector3 pushBackOffset = Vector3.zero;
    private float pushBackSpeed = 10f;

    private bool inputBlocked = false;

    void Start()
    {
        _movementController = GetComponent<MovementController>();
    }

    void Update()
    {
        if (inputBlocked)
        {
            // Movimiento por empuje si se bloqueó el input
            transform.position += pushBackOffset * Time.deltaTime * pushBackSpeed;
            pushBackOffset = Vector3.Lerp(pushBackOffset, Vector3.zero, Time.deltaTime * 5f);
            return;
        }

        // Movimiento normal por input
        if (Input.GetKey(_moveForward)) _movementController.Move(transform.forward);
        if (Input.GetKey(_moveBack)) _movementController.Move(-transform.forward);
        if (Input.GetKey(_moveRight)) _movementController.Move(transform.right);
        if (Input.GetKey(_MoveLeft)) _movementController.Move(-transform.right);

        if (Input.GetKeyDown(_shoot)) _gun.Attack();
        if (Input.GetKeyDown(_reload)) _gun.Reload();

        if (Input.GetKeyDown(KeyCode.Return)) EventManager.instance.EventGameOver(true);
        if (Input.GetKeyDown(KeyCode.Backspace)) GetComponent<IDamagable>().TakeDamage(10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zombie") && !inputBlocked)
        {
            Vector3 pushDir = (transform.position - collision.transform.position).normalized;
            pushBackOffset = pushDir * 1f;

            StartCoroutine(BlockInputTemporarily(0.2f)); // Bloquea input por 0.2 segundos
        }
    }

    private IEnumerator BlockInputTemporarily(float duration)
    {
        inputBlocked = true;
        yield return new WaitForSeconds(duration);
        inputBlocked = false;
        pushBackOffset = Vector3.zero; // por si quedaba empuje acumulado
    }
}
