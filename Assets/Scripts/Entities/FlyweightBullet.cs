using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Strategy;

public class FlyweightBullet : MonoBehaviour, IBullet
{
    [SerializeField] private BulletTypeSO bulletType;
    [SerializeField] private List<int> _layerMasks;

    private Vector3 _direction;
    private float _timeRemaining;

    public int Damage => bulletType.damage;
    public float Speed => bulletType.speed;
    public float LifeTime => bulletType.lifetime;

    public void Travel()
    {
        transform.position += _direction * Time.deltaTime * Speed;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (_layerMasks.Contains(collision.gameObject.layer))
        {
            IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
            damagable?.TakeDamage(Damage);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(Vector3 direction)
    {
        _direction = direction.normalized;
        _timeRemaining = LifeTime;
        
        // Si no hay bulletType asignado, usar valores por defecto
        if (bulletType == null)
        {
            Debug.LogWarning("No BulletTypeSO assigned to FlyweightBullet!");
        }
    }

    void Update()
    {
        Travel();
        _timeRemaining -= Time.deltaTime;
        if (_timeRemaining <= 0)
            Destroy(gameObject);
    }
}
