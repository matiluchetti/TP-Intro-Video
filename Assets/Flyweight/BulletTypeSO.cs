using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BulletType", menuName = "Flyweight/BulletTypeSO")]
public class BulletTypeSO : ScriptableObject
{
    public int damage = 10;
    public float speed = 10f;
    public float lifetime = 5f;
    public List<int> layerMasks;
}
