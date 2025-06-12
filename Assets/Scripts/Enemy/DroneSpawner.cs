using Meta.XR.MRUtilityKit;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Wave
{
    Wave1,
    Wave2, 
    Wave3,
    Wave4,
    Wave5
}

public class DroneSpawner : MonoBehaviour
{
    [SerializeField] private EnemyManager[] drones;
    [SerializeField] private float _spawnWaitTime;
    [SerializeField] private float normalOffset;
    [SerializeField] private float targetPosOffset;
    [SerializeField] private float minEdgeDistance;
    [SerializeField] private MRUKAnchor.SceneLabels spawnLabel;

    private int _maxSpawnCount;
    private int completedEnemies = 0;

    // Start is called before the first frame update
    void Awake()
    {
        GameEventManager.Instance.OnWaveEndedEvent += EnemyWave;
    }

    private void EnemyWave(Wave waveIndex)
    {
        InitializeWaveCount(waveIndex);
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        int spawnCount = 0; // Counter for how many times we spawn
        while (spawnCount < _maxSpawnCount)
        {
            GameObject enemySpawned;
            Vector3 spawnPos = SpawnPosition();

            enemySpawned = Instantiate(EnemyToSpawn().gameObject, spawnPos, Quaternion.identity);
            StartCoroutine(LerpEnemyToRoomPos(enemySpawned, spawnPos));
            EnemySpawnerHandler.Instance.spawnedDrone.Add(enemySpawned);
            spawnCount++;

            yield return new WaitForSeconds(_spawnWaitTime);
        }

        Debug.Log("Finished spawning objects!");
    }

    private Vector3 SpawnPosition()
    {
        MRUKRoom room = MRUK.Instance.GetCurrentRoom();
        LabelFilter labelFilter = new LabelFilter(spawnLabel);

        room.GenerateRandomPositionOnSurface(MRUK.SurfaceType.FACING_DOWN, minEdgeDistance, labelFilter, out Vector3 surfacePosition, out Vector3 norm);

        Vector3 spawnPos = surfacePosition;
        spawnPos.y = surfacePosition.y * normalOffset;
        return spawnPos;
    }

    private IEnumerator LerpEnemyToRoomPos(GameObject enemySpawned, Vector3 spawnPos)
    {
        Vector3 targetPosition = spawnPos;
        targetPosition.y = Mathf.Abs(spawnPos.y * targetPosOffset/2);

        float lerpDuration = 2f;
        float timeElapsed = 0f;

        while (timeElapsed < lerpDuration)
        {
            timeElapsed += Time.deltaTime;
            float lerpFactor = timeElapsed / lerpDuration;

            enemySpawned.transform.position = Vector3.Lerp(spawnPos, targetPosition, lerpFactor);

            yield return null;
        }

        enemySpawned.transform.position = targetPosition;

        // Increment the completed enemies counter
        completedEnemies++;

        // Check if all enemies have completed their movement
        if (completedEnemies >= _maxSpawnCount)
        {
            Debug.Log("All enemies have reached their target position!");
            completedEnemies = 0;
            GameEventManager.Instance.OnEnemyReachedTargetPos(true);
        }
    }

    private void InitializeWaveCount(Wave value)
    {
        switch (value)
        {
            case Wave.Wave1:
                _maxSpawnCount = 4;
                break;
            case Wave.Wave2:
                _maxSpawnCount = 8;
                break;
            case Wave.Wave3:
                _maxSpawnCount = 12;
                break;
            case Wave.Wave4:
                _maxSpawnCount = 16;
                break;
            case Wave.Wave5:
                _maxSpawnCount = 20;
                break;
        }
    }

    private EnemyManager EnemyToSpawn()
    {
        return drones[Random.Range(0, drones.Length)];
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.OnWaveEndedEvent -= EnemyWave;
    }
}
