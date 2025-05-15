using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private MovementController _movementController;
    [SerializeField] private Gun _gun;
    [SerializeField] private CharacterCamera _camera;

    // BINDING ATTACK KEYS
    [SerializeField] private KeyCode _shoot = KeyCode.Mouse0;
    [SerializeField] private KeyCode _reload = KeyCode.R;
    // BINDING MOVEMENT KEYS
    [SerializeField] private KeyCode _moveForward = KeyCode.W;
    [SerializeField] private KeyCode _moveBack = KeyCode.S;
    [SerializeField] private KeyCode _moveLeft = KeyCode.A;
    [SerializeField] private KeyCode _moveRight = KeyCode.D;

    private Vector3 pushBackOffset = Vector3.zero;
    private float pushBackSpeed = 10f;

    private bool inputBlocked = false;

    void Start()
    {
        _movementController = GetComponent<MovementController>();
        if (_camera == null)
        {
            _camera = FindObjectOfType<CharacterCamera>();
        }
    }

    void Update()
    {
        if (inputBlocked)
        {
            transform.position += pushBackOffset * Time.deltaTime * pushBackSpeed;
            pushBackOffset = Vector3.Lerp(pushBackOffset, Vector3.zero, Time.deltaTime * 5f);
            return;
        }

        // MOVEMENT INPUT
        Vector3 inputDir = Vector3.zero;
        if (Input.GetKey(_moveForward)) inputDir += Vector3.forward;
        if (Input.GetKey(_moveBack)) inputDir += Vector3.back;
        if (Input.GetKey(_moveRight)) inputDir += Vector3.right;
        if (Input.GetKey(_moveLeft)) inputDir += Vector3.left;

        inputDir.Normalize();

        if (inputDir != Vector3.zero)
        {
            // Rotate input direction based on camera rotation
            inputDir = _camera.transform.rotation * inputDir;
            _movementController.Move(inputDir);
        }

        // SHOOTING & RELOADING
        if (Input.GetKeyDown(_shoot)) _gun.Attack();
        if (Input.GetKeyDown(_reload)) _gun.Reload();

        // DEBUG INPUTS
        if (Input.GetKeyDown(KeyCode.Return)) EventManager.instance.EventGameOver(true);
        if (Input.GetKeyDown(KeyCode.Backspace)) GetComponent<IDamagable>().TakeDamage(10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zombie") && !inputBlocked)
        {
            Vector3 pushDir = (transform.position - collision.transform.position).normalized;
            pushBackOffset = pushDir * 1f;
            StartCoroutine(BlockInputTemporarily(0.2f));
        }
    }

    private IEnumerator BlockInputTemporarily(float duration)
    {
        inputBlocked = true;
        yield return new WaitForSeconds(duration);
        inputBlocked = false;
        pushBackOffset = Vector3.zero;
    }
}
