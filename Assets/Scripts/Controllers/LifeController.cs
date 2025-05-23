using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Strategy;


public class LifeController : MonoBehaviour, IDamagable
{
    #region I_DAMAGABLE_PROPERTIES
    public float Life => _life;
    [SerializeField] private float _life;

    public float MaxLife => _maxLife;
    [SerializeField] private float _maxLife = 100;
    #endregion

    #region UNITY_EVENTS
    void Start()
    {
        _life = MaxLife;
    }
    #endregion

    #region I_DAMAGABLE_METHODS
    public void TakeDamage(int damage)
    {
        Debug.Log("TakeDamage: " + damage);
        Debug.Log("Life: " + _life);
        Debug.Log("newLife: " + (_life - damage));
        _life -= damage;
        if (IsDead()) Die();
    }
    #endregion

    #region PRIVATE_METHODS
    private bool IsDead() => _life <= 0;

    private void Die() 
    { 
        if(name == "Player") EventManager.instance.EventGameOver(false);
        Destroy(this.gameObject); 
    }
    #endregion
}
