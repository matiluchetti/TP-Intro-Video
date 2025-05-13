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

    void Start()
    {
        _movementController = GetComponent<MovementController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(_moveForward)) _movementController.Move(transform.forward);
        if (Input.GetKey(_moveBack)) _movementController.Move(-transform.forward);
        if (Input.GetKey(_moveRight)) _movementController.Move(transform.right);
        if (Input.GetKey(_MoveLeft)) _movementController.Move(-transform.right);

        if (Input.GetKeyDown(_shoot)) _gun.Attack();
        if (Input.GetKeyDown(_reload)) _gun.Reload();

        if (Input.GetKeyDown(KeyCode.Return)) EventManager.instance.EventGameOver(true);
        if (Input.GetKeyDown(KeyCode.Backspace)) GetComponent<IDamagable>().TakeDamage(10);
    }
}
