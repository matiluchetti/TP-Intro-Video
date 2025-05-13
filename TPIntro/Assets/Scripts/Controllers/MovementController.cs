using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour, IMoveablle
{
    #region IMOVEABLE_PROPERTIES
    public float Speed => _speed;
    private float _speed = 10;
    #endregion

    #region IMOVEABLE_METHODS
    public void Move(Vector3 direction)
    {
        transform.position += direction * Time.deltaTime * Speed;
    }
    #endregion

    //[SerializeField] private float _timer = 10;
    //void Update()
    //{
    //    _timer -= Time.deltaTime;
    //    if (_timer <= 0)
    //    {
    //        Debug.Log("Do something");
    //    }
    //}
}
