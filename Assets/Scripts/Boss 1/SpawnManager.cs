using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] Transform spawnArea;
    [SerializeField] GameObject enemy;
    [SerializeField] private int[] waves;

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private int enemiesKilled;

    private int currentWave;
    private float waveTimer;
    private bool waveInProgress;
    private float waveStartOffset = 3;


    void Start()
    {
        StartWave();
    }

    private void Update()
    {
        CheckEnemiesKilled();
        if (waveInProgress && enemiesKilled >= waves[currentWave])
        {
            waveInProgress = false;
            enemiesKilled = 0;
            currentWave++;
            if (currentWave < waves.Length)
            {
                waveTimer = waveStartOffset;
            }
            else
            {
                BossLevelManager.instance.setPhase(3);
            }
        }

        //Handles the wave start delay
        if (!waveInProgress && currentWave < waves.Length)
        {
            waveTimer -= Time.deltaTime;
            if (waveTimer <= 0)
            {
                StartWave();
            }
        }
    }


    private void CheckEnemiesKilled()
    {
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            GameObject enemy = spawnedEnemies[i];
            
            if (enemy == null) //If enemy is killed the gameobject gets destroyed -> check if list entry is null = enemy killed
            {
                enemiesKilled++;
                spawnedEnemies.RemoveAt(i);
            }
        } 
    }

    private void StartWave()
    {
        waveInProgress = true;
        int totalEnemies = waves[currentWave];

        for (int i = 0; i < totalEnemies; i++)
        {
            SpawnEnemy();
        }
    }

     private void SpawnEnemy()
     {
        int randomOffsetZ = Random.Range(-7, 7);
        int randomOffsetX = Random.Range(-5, 0);

        Vector3 offset = new Vector3(randomOffsetX, 0, randomOffsetZ);
        Vector3 spawnPos = spawnArea.position + offset;
        GameObject spawnedEnemy = Instantiate(enemy, spawnPos, Quaternion.identity);
        spawnedEnemies.Add(spawnedEnemy);
     }

}
