using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    public override void Attack() => Instantiate(
        BulletPrefab, 
        transform.position, 
        Quaternion.identity);

    public override void Reload() => base.Reload();
}
