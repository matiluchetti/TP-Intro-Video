using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Strategy;

public class Gun : MonoBehaviour, IGun
{
    #region GUN_PROPERTIES
    private int _damage = 10;
    protected int _maxBulletCount = 30;
    [SerializeField] protected int _currentBulletCount;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] protected AudioClip _shotClip; // Clip de sonido a asignar en el Inspector
    [SerializeField] private Sprite _gunSprite; // Sprite de la pistola a asignar en el Inspector
    
    #endregion

    #region COMPONENTS
    protected AudioSource _audioSource;
    #endregion

    #region I_GUN_PROPERTIES
    public int Damage => _damage;
    public int MaxBulletCount => _maxBulletCount;
    public GameObject BulletPrefab => _bulletPrefab;
    public Sprite GunSprite => _gunSprite;
    public int CurrentBullets => _currentBulletCount;
    #endregion

    public AudioClip ShotClip => _shotClip;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogWarning($"No AudioSource found on {gameObject.name}, adding one.");
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public virtual void Attack()
    {
        Instantiate(_bulletPrefab, transform.position, transform.rotation);

        if (_shotClip != null && _audioSource != null)
        {
            Debug.Log($"Disparo ejecutado. Clip asignado: {_shotClip != null}, AudioSource: {_audioSource != null}");
            _audioSource.PlayOneShot(_shotClip);
        }
    }

    public virtual void Reload()
    {
        _currentBulletCount = _maxBulletCount;
        EventManager.instance.EventGunUpdate(this);
    }
}
