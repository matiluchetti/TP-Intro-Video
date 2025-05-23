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
    private Quaternion rotation;
    Vector3 _direction;


    #endregion

    #region I_BULLET_PROPERTIES
    public int Damage => _damage;
    public float Speed => _speed;
    public float LifeTime => _lifetime;
    #endregion

    #region I_BULLET_METHODS
    public void Travel(){
         transform.position +=  _direction* Time.deltaTime * Speed;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (_layerMasks.Contains(collision.gameObject.layer))
        {
            IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
            damagable?.TakeDamage(Damage);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    #region UNITY_EVENTS
    void Start() { 
        GameObject player = GameObject.FindWithTag("Player");
        rotation = player.transform.rotation;
        _direction = player.transform.forward;
        _timeRemaining = _lifetime;
    }

    void Update()
    {
        Travel();
        Debug.DrawRay(transform.position, transform.forward * 2f, Color.green);
        _timeRemaining -= Time.deltaTime;
        if (_timeRemaining <= 0) Destroy(this.gameObject);
    }
    #endregion
}