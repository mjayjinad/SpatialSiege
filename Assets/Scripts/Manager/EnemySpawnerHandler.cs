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
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MRUK.Instance.SceneLoadedEvent.AddListener(InitializeWave);
    }

    private void InitializeWave()
    {
        currentWave = Wave.Wave1;
        GameEventManager.Instance.OnWaveEnded(currentWave);
        waveInitialized = true;
    }

    private void Update()
    {
        if (!waveInitialized) return;
        if(spawnedDrone == null || spawnedDrone.Count == 0)
        {
            LoadNextWave();
        }
    }

    private void LoadNextWave()
    {
        waveIndex++;
        currentWave = (Wave)waveIndex;
        GameEventManager.Instance.OnWaveEnded(currentWave);
    }
}
