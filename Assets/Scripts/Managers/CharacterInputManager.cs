using System.Collections;
using System.Collections.Generic;
using Commands;
using Strategy;
using UnityEngine;
using Managers;

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
    private KeyCode _shoot = KeyCode.Mouse0;
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
        Vector3 backward = rotation * transform.forward * -1;
        Vector3 forward = rotation * transform.forward;
        Vector3 left = rotation * transform.right * -1;
        Vector3 right = rotation * transform.right;
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
        bool isUzi = _currentWeapon is Machingun; 


        // Disparo con cooldown para otras armas
        if (Input.GetKeyDown(_shoot) && _shotCooldownTimer >= shotCooldown)
        {
            EventQueueManager.instance.AddEvent(_cmdShoot);
            _shotCooldownTimer = 0;
        }
        else
        {
            if (isUzi)
            {
                _shotCooldownTimer += (int)(Time.deltaTime * 500);;
            }
            else
            {
                _shotCooldownTimer += (int)(Time.deltaTime * 1000);
            }
        }
        
        // Disparo

        if (Input.GetKeyDown(_shoot) && (isUzi || _shotCooldownTimer >= shotCooldown))
        {
            EventQueueManager.instance.AddEvent(_cmdShoot);
            _shotCooldownTimer = 0;
        }
        else if (!isUzi)
        {
            _shotCooldownTimer += (int)(Time.deltaTime * 1000);
        }


        // Recarga
        if (Input.GetKeyDown(_reload))
        {
            EventQueueManager.instance.AddEvent(_cmdReload);
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
        EventQueueManager.instance.AddEvent(_cmdRotateTowardsMouse);

        // Direcciones actualizadas en tiempo real según rotación del personaje
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 backward = -forward;
        Vector3 left = -right;

        Vector3 forwardLeft = Vector3.Normalize(forward + left);
        Vector3 forwardRight = Vector3.Normalize(forward + right);
        Vector3 backwardLeft = Vector3.Normalize(backward + left);
        Vector3 backwardRight = Vector3.Normalize(backward + right);

        if (Input.GetKey(KeyCode.Alpha4))
        {
            Debug.Log("bFixedUpdate");
            EventManager.instance.EventGameOver(false);
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            Debug.Log("cFixedUpdate");
            EventManager.instance.EventGameOver(true);
        }

        if (Input.GetKey(_moveForward))
        {
            if (Input.GetKey(_moveLeft))
                EventQueueManager.instance.AddEvent(_cmdMoveForwardLeft);
            else if (Input.GetKey(_moveRight))
                EventQueueManager.instance.AddEvent(_cmdMoveForwardRight);
            else
                EventQueueManager.instance.AddEvent(_cmdMoveForward);
        }
        else if (Input.GetKey(_moveBackward))
        {
            if (Input.GetKey(_moveLeft))
                EventQueueManager.instance.AddEvent(_cmdMoveBackwardLeft);
            else if (Input.GetKey(_moveRight))
                EventQueueManager.instance.AddEvent(_cmdMoveBackwardRight);
            else
                EventQueueManager.instance.AddEvent(_cmdMoveBackward);
        }
        else if (Input.GetKey(_moveLeft))
                EventQueueManager.instance.AddEvent(_cmdMoveLeft);
        else if (Input.GetKey(_moveRight))
                EventQueueManager.instance.AddEvent(_cmdMoveRight);
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

        EventManager.instance.EventGunUpdate(_weapons[weaponIndex]);

        Gun gun = _weapons[weaponIndex];
        gun.gameObject.SetActive(true);
        _currentWeapon = gun;

        _cmdShoot =  new CmdShoot(_currentWeapon);
        _cmdReload = new CmdReload(_currentWeapon);
    }
}




