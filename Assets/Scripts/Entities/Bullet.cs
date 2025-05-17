using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Strategy;

public class Bullet : MonoBehaviour, IBullet
{
    #region PRIVATE_PROPERTEIS
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _lifetime = 5;
    [SerializeField] private List<int> _layerMasks;
    private float _timeRemaining;
    #endregion

    #region I_BULLET_PROPERTIES
    public int Damage => _damage;
    public float Speed => _speed;
    public float LifeTime => _lifetime;
    #endregion

    #region I_BULLET_METHODS
    public void Travel(){
        GameObject player = GameObject.FindWithTag("Player");
         transform.position += player.transform.rotation * transform.forward * Time.deltaTime * Speed;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (_layerMasks.Contains(collision.gameObject.layer))
        {
            IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
            damagable?.TakeDamage(Damage);

            Destroy(this.gameObject);
        }
    }
    #endregion

    #region UNITY_EVENTS
    void Start() { 

        _timeRemaining = _lifetime;
    }

    void Update()
    {
        Travel();

        _timeRemaining -= Time.deltaTime;
        if (_timeRemaining <= 0) Destroy(this.gameObject);
    }
    #endregion
}