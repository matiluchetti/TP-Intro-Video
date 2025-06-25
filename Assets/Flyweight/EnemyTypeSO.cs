using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "Flyweight/EnemyTypeSO")]
public class EnemyTypeSO : ScriptableObject
{
    public string enemyName;
    public float maxLife = 100f;
    public float speed = 2f;
    public float damage = 10f;
} 