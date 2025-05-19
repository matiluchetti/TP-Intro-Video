using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int enemiesPerWave = 10;
    [SerializeField] private float timeBetweenWaves = 5f;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private int currentWave = 0;
    private const int TOTAL_WAVES = 10;
    private bool isWaitingForNextWave = false;

    void Start()
    {
        StartNextWave();
    }

    void Update()
    {
        // Limpia enemigos nulos (ya destruidos)
        activeEnemies.RemoveAll(enemy => enemy == null);

        // Si no quedan enemigos vivos y no estamos esperando, inicia nueva oleada
        if (activeEnemies.Count == 0 && !isWaitingForNextWave && currentWave < TOTAL_WAVES)
        {
            StartCoroutine(WaitForNextWave());
        }
    }

    private IEnumerator WaitForNextWave()
    {
        isWaitingForNextWave = true;
        Debug.Log($"Esperando {timeBetweenWaves} segundos para la siguiente oleada...");
        yield return new WaitForSeconds(timeBetweenWaves);
        isWaitingForNextWave = false;
        StartNextWave();
    }

    private void StartNextWave()
    {
        currentWave++;
        if (currentWave <= TOTAL_WAVES)
        {
            SpawnWave();
            Debug.Log($"Iniciando oleada {currentWave} de {TOTAL_WAVES}");
        }
        else
        {
            Debug.Log("¡Has completado todas las oleadas!");
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
            
            // Aplicar modificadores de dificultad basados en la oleada actual
            ZombieAI zombieComponent = enemy.GetComponent<ZombieAI>();
            if (zombieComponent != null)
            {
                float waveMultiplier = 1f + ((currentWave - 1) * 0.1f); // Aumenta 0.1 por oleada
                float baseLife = 100f; // Vida base del zombie
                float baseSpeed = 8f; // Velocidad base del zombie
                float baseDamage = 10f; // Daño base del zombie

                // Usar SetStats para modificar todas las estadísticas del zombie
                zombieComponent.SetStats(
                    baseLife * waveMultiplier, // Vida aumentada por oleada
                    baseSpeed + ((currentWave - 1) * 0.1f), // Velocidad aumentada por oleada
                    baseDamage * waveMultiplier // Daño aumentado por oleada
                );
            }

            activeEnemies.Add(enemy);
            availableSpawns.RemoveAt(index);
        }
    }
}
