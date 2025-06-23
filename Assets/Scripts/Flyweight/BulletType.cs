using UnityEngine;

[CreateAssetMenu(fileName = "BulletType", menuName = "Scriptable Objects/Flyweight/Bullet Type")]
public class BulletTypeSO : ScriptableObject
{
  public int damage;
  public float speed;
  public float lifetime;
}