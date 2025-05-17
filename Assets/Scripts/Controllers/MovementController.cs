using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Strategy;
public class MovementController : MonoBehaviour, IMoveable
{
    #region IMOVEABLE_PROPERTIES
    public float Speed => _speed;
    [SerializeField] private float _speed = 10;
    #endregion

    #region IMOVEABLE_METHODS
    public void Move(Vector3 direction)
    {
        transform.position += direction * Time.deltaTime * Speed;
    }

    public void RotateTowards(Vector3 point)
    {
        transform.LookAt(new Vector3(point.x, transform.position.y, point.z));
    }
    #endregion
}
