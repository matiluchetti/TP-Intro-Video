using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject devilPrefab;
    [SerializeField] private GameObject megaBossPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int enemiesPerWave = 1;
    [SerializeField] private float timeBetweenWaves = 5f;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private int currentWave = 0;
    private const int TOTAL_WAVES = 10;
    private bool isWaitingForNextWave = false;
    public static event System.Action OnWaveEnded;


    void Start()
    {
        StartNextWave();
    }

    void Update()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);

        if (!GameState.IsGameOver && activeEnemies.Count == 0 && !isWaitingForNextWave) {
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
        Debug.Log("Invocando OnWaveEnded. Suscriptores: " + (OnWaveEnded?.GetInvocationList()?.Length ?? 0));
  
        OnWaveEnded?.Invoke(); 
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
            if(currentWave % 10 == 0)
            {
                SpawnMegaBoss();
                EventManager.instance.EventRoundUpdate(currentWave);
            }
            else if (currentWave % 1 == 0)
            {
                SpawnWave();
                EventManager.instance.EventRoundUpdate(currentWave); 
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
        devil.transform.position = new Vector3(devil.transform.position.x, 1f, devil.transform.position.z); 
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
    

    private void SpawnMegaBoss()
    {
        int index = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[index];

        GameObject megaBoss = Instantiate(megaBossPrefab, spawnPoint.position, Quaternion.identity);
        megaBoss.transform.position = new Vector3(megaBoss.transform.position.x, 1f, megaBoss.transform.position.z); 
        MegaBoss megaBossComponent = megaBoss.GetComponent<MegaBoss>();
        if (megaBossComponent != null)
        {
            megaBossComponent.SetStats(
                1500 + currentWave * 30f, // vida
                15f + currentWave * 0.1f,   // velocidad
                100f + currentWave * 10f   // daño
            );
        }

        activeEnemies.Add(megaBoss);
        Debug.Log($"👾 ¡Ha aparecido un Mega Boss en la posicion {spawnPoint.position}!");
        Debug.Log("👾 ¡Ha aparecido un Mega Boss!");
    }
}
