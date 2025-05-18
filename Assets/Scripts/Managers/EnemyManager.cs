using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int enemiesPerWave = 10;

    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        SpawnWave();
    }

    void Update()
    {
        // Limpia enemigos nulos (ya destruidos)
        activeEnemies.RemoveAll(enemy => enemy == null);

        // Si no quedan enemigos vivos, inicia nueva oleada
        if (activeEnemies.Count == 0)
        {
            SpawnWave();
        }
    }

    private void SpawnWave()
    {
        List<Transform> availableSpawns = new List<Transform>(spawnPoints);

        for (int i = 0; i < enemiesPerWave && availableSpawns.Count > 0; i++)
        {
            int index = Random.Range(0, availableSpawns.Count);
            Transform spawnPoint = availableSpawns[index];
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            activeEnemies.Add(enemy);
            availableSpawns.RemoveAt(index);
        }

        Debug.Log("Nueva oleada generada.");
    }
}
