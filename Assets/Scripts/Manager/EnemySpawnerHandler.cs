using Meta.XR.MRUtilityKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerHandler : MonoBehaviour
{
    public List<GameObject> spawnedDrone;
    public Wave currentWave;
    private int waveIndex = 0;
    public bool waveInitialized = false;
    public bool isGameOver;

    // Singleton instance
    public static EnemySpawnerHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void InitializeWave()
    {
        currentWave = Wave.Wave1;
        waveIndex = 0;
        GameEventManager.Instance.OnWaveEnded(currentWave);
        waveInitialized = true;
    }

    private void Update()
    {
        if (!waveInitialized) return;
        if(spawnedDrone.Count == 0 && isGameOver == false)
        {
            LoadNextWave();
        }
    }

    private void LoadNextWave()
    {
        if(currentWave != Wave.Wave4)
        {
            waveIndex++;
            currentWave = (Wave)waveIndex;
            GameEventManager.Instance.OnWaveEnded(currentWave);
        }
        else
        {
            GameWon();
        }
    }

    public void GameOver()
    {
        foreach (GameObject orb in spawnedDrone)
        {
            Destroy(orb);
        }

        spawnedDrone.Clear();
        waveInitialized = false;
    }

    private void GameWon()
    {
        GameManager.Instance.WinPlayerState();
        isGameOver = true;
    }
}
