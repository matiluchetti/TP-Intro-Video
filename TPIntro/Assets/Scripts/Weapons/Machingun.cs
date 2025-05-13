using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Machingun : Gun
{
    [SerializeField] private int _shotCount = 5;
    public override void Attack()
    {
        for (int i = 0; i < _shotCount; i++)
        {
            Instantiate(
                BulletPrefab,
                transform.position + Vector3.forward * i * .6f,
                Quaternion.identity);
        }
    }

    public override void Reload() => base.Reload();
}
