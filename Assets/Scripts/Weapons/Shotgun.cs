using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    [SerializeField] private int _shotCount = 5;

    public override void Attack()
    {
        for (int i = 0; i < _shotCount; i++)
        {
            Instantiate(
                BulletPrefab,
                transform.position + Random.insideUnitSphere * 1,
                Quaternion.identity);
        }
    }

    public override void Reload() => base.Reload();
}
