using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject devilPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int enemiesPerWave = 1;
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
        activeEnemies.RemoveAll(enemy => enemy == null);

        if (activeEnemies.Count == 0 && !isWaitingForNextWave) {
            if(currentWave < TOTAL_WAVES) {
                StartCoroutine(WaitForNextWave());
            } else {
                EventManager.instance.EventGameOver(true);
            }
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

            if (currentWave % 1 == 0)
            {
                SpawnDevil();
            }

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
            
            ZombieAI zombieComponent = enemy.GetComponent<ZombieAI>();
            if (zombieComponent != null)
            {
                float waveMultiplier = 1f + ((currentWave - 1) * 0.1f);
                float baseLife = 100f;
                float baseSpeed = 8f;
                float baseDamage = 10f;

                zombieComponent.SetStats(
                    baseLife * waveMultiplier,
                    baseSpeed + ((currentWave - 1) * 0.1f),
                    baseDamage * waveMultiplier
                );
            }

            activeEnemies.Add(enemy);
            availableSpawns.RemoveAt(index);
        }
    }


    private void SpawnDevil()
    {
        if (spawnPoints.Length == 0 || devilPrefab == null) return;

        int index = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[index];

        GameObject devil = Instantiate(devilPrefab, spawnPoint.position, Quaternion.identity);
        devil.transform.position = new Vector3(devil.transform.position.x, 1f, devil.transform.position.z); // fuerza Y=1
        DevilAI devilComponent = devil.GetComponent<DevilAI>();
        if (devilComponent != null)
        {
            devilComponent.SetStats(
                300f + currentWave * 20f, // vida
                10f + currentWave * 0.2f, // velocidad
                50f + currentWave * 5f    // daño
            );
        }

        activeEnemies.Add(devil);
        Debug.Log("👹 ¡Ha aparecido un Devil!");
    }
}
