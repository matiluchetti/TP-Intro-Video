using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Strategy;

public class Gun : MonoBehaviour, IGun
{
    #region GUN_PROPERTIES
    private int _damage = 10;
    private int _maxBulletCount = 10;
    [SerializeField] private int _currentBulletCount;
    [SerializeField] private GameObject _bulletPrefab;
    #endregion

    #region I_GUN_PROPERTIES
    public int Damage => _damage;
    public int MaxBulletCount => _maxBulletCount;
    public GameObject BulletPrefab => _bulletPrefab;
    #endregion

    #region I_GUN_PROPERTIES
    public virtual void Attack() => Instantiate(
                                        _bulletPrefab, 
                                        transform.position, 
                                        transform.rotation);

    public virtual void Reload() => _currentBulletCount = _maxBulletCount;
    #endregion
}
