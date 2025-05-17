using System.Collections;
using System.Collections.Generic;
using Commands;
using Strategy;
using UnityEngine;

public enum WeaponIndex
{
    Pistol = 0,
    Uzi = 1,
    Shotgun = 2
}

public class CharacterInputManager : MonoBehaviour
{
    private IGun _currentWeapon;
    private IMoveable _player;
    private List<Gun> _weapons;

    private KeyCode _moveForward = KeyCode.W;
    private KeyCode _moveBackward = KeyCode.S;
    private KeyCode _moveLeft = KeyCode.A;
    private KeyCode _moveRight = KeyCode.D;
    private KeyCode _shoot = KeyCode.M;
    private KeyCode _reload = KeyCode.R;
    private KeyCode _switchPistol = KeyCode.Alpha1;
    private KeyCode _switchUzi = KeyCode.Alpha2;
    private KeyCode _switchShotgun = KeyCode.Alpha3;

    private CmdRotateTowardsMouse _cmdRotateTowardsMouse;
    private CmdMovement _cmdMoveForward;
    private CmdMovement _cmdMoveBackward;
    private CmdMovement _cmdMoveLeft;
    private CmdMovement _cmdMoveRight;
    private CmdMovement _cmdMoveForwardLeft;
    private CmdMovement _cmdMoveBackwardLeft;
    private CmdMovement _cmdMoveForwardRight;
    private CmdMovement _cmdMoveBackwardRight;
    private CmdShoot _cmdShoot;
    private CmdReload _cmdReload;

    [SerializeField] private int shotCooldown = 500; // in ms
    private int _shotCooldownTimer = 0;

    [Header("Camera Follow")]
    public Transform camTarget;
    public float camPLerp = 0.2f;
    public float camRLerp = 0.2f;
    private Transform _cameraTransform;

    [Header("Camera Rotation")]
    public float mouseSensitivity = 2f;
    private Vector2 _turn;

    void Start()
    {
        _player = GetComponent<IMoveable>();
        _weapons = new List<Gun>(GetComponentsInChildren<Gun>(true));
        SwitchWeapon((int)WeaponIndex.Pistol);

        Quaternion rotation = Quaternion.AngleAxis(-45, Vector3.up);
        Vector3 backward = rotation * new Vector3(0, 0, -1);
        Vector3 forward = rotation * new Vector3(0, 0, 1);
        Vector3 left = rotation * new Vector3(-1, 0, 0);
        Vector3 right = rotation * new Vector3(1, 0, 0);
        Vector3 forwardLeft = Vector3.Normalize(forward + left);
        Vector3 forwardRight = Vector3.Normalize(forward + right);
        Vector3 backwardLeft = Vector3.Normalize(backward + left);
        Vector3 backwardRight = Vector3.Normalize(backward + right);

        _cmdRotateTowardsMouse = new CmdRotateTowardsMouse(_player);
        _cmdMoveBackward = new CmdMovement(backward, _player);
        _cmdMoveForward = new CmdMovement(forward, _player);
        _cmdMoveLeft = new CmdMovement(left, _player);
        _cmdMoveRight = new CmdMovement(right, _player);
        _cmdMoveForwardLeft = new CmdMovement(forwardLeft, _player);
        _cmdMoveForwardRight = new CmdMovement(forwardRight, _player);
        _cmdMoveBackwardLeft = new CmdMovement(backwardLeft, _player);
        _cmdMoveBackwardRight = new CmdMovement(backwardRight, _player);

        _cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void Update()
    {
        // Rotación cámara
        _turn.x += Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.localRotation = Quaternion.Euler(-_turn.y, _turn.x, 0);

        // Disparo
        if (Input.GetKeyDown(_shoot) && _shotCooldownTimer >= shotCooldown)
        {
            _cmdShoot.Execute();
            _shotCooldownTimer = 0;
        }
        else
        {
            _shotCooldownTimer += (int)(Time.deltaTime * 1000);
        }

        // Recarga
        if (Input.GetKeyDown(_reload))
        {
            _cmdReload.Execute();
        }

        // Cambiar de arma
        if (Input.GetKeyDown(_switchPistol))
        {
            SwitchWeapon((int)WeaponIndex.Pistol);
        }
        else if (Input.GetKeyDown(_switchUzi))
        {
            SwitchWeapon((int)WeaponIndex.Uzi);
        }
        else if (Input.GetKeyDown(_switchShotgun))
        {
            SwitchWeapon((int)WeaponIndex.Shotgun);
        }
    }

    private void FixedUpdate()
    {
        _cmdRotateTowardsMouse.Do();

        if (Input.GetKey(_moveForward))
        {
            if (Input.GetKey(_moveLeft))
                _cmdMoveForwardLeft.Do();
            else if (Input.GetKey(_moveRight))
                _cmdMoveForwardRight.Do();
            else
                _cmdMoveForward.Do();
        }
        else if (Input.GetKey(_moveBackward))
        {
            if (Input.GetKey(_moveLeft))
                _cmdMoveBackwardLeft.Do();
            else if (Input.GetKey(_moveRight))
                _cmdMoveBackwardRight.Do();
            else
                _cmdMoveBackward.Do();
        }
        else if (Input.GetKey(_moveLeft)) _cmdMoveLeft.Do();
        else if (Input.GetKey(_moveRight)) _cmdMoveRight.Do();
    }

    private void LateUpdate()
    {
        // Cámara sigue al objetivo
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, camTarget.position, camPLerp);
        _cameraTransform.rotation = Quaternion.Lerp(_cameraTransform.rotation, camTarget.rotation, camRLerp);
    }

    private void SwitchWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= _weapons.Count)
        {
            Debug.LogError("Invalid weapon index: " + weaponIndex);
            return;
        }

        foreach (Gun weapon in _weapons)
        {
            weapon.gameObject.SetActive(false);
        }

        Gun gun = _weapons[weaponIndex];
        gun.gameObject.SetActive(true);
        _currentWeapon = gun;

        _cmdShoot =  new CmdShoot(_currentWeapon);
        _cmdReload = new CmdReload(_currentWeapon);
    }
}




