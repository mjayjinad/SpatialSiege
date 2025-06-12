using System;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    // Singleton instance
    public static GameEventManager Instance { get; private set; }

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


    public event Action<Wave> OnWaveEndedEvent;
    public event Action<bool> OnWaveEndedEventForMaterial;

    public void OnWaveEnded(Wave wave)
    {
        OnWaveEndedEvent?.Invoke(wave);
        OnWaveEndedEventForMaterial?.Invoke(false);
    }

    public event Action<bool> OnEnemyReachedTargetPosEvent;

    public void OnEnemyReachedTargetPos(bool value)
    {
        OnEnemyReachedTargetPosEvent?.Invoke(value);
    }
}
