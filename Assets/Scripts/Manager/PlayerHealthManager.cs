using Unity.XR.CoreUtils;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField] private float health = 100;

    public bool isPlayerDead = false;

    private void Start()
    {
        GameEventManager.Instance.OnWaveEndedEvent += InitializeHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isPlayerDead) return;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            health -= damage;
        }
    }

    private void Die()
    {
        // Handle player death here (e.g., show Game Over screen, respawn, etc.)
        Debug.Log("Player is dead!");
        isPlayerDead = true;
        GameManager.Instance.GameOver();
        gameObject.tag = "Untagged";
    }

    private void InitializeHealth(Wave value)
    {
        switch (value)
        {
            case Wave.Wave1:
                health = 8;
                break;
            case Wave.Wave2:
                health = 16;
                break;
            case Wave.Wave3:
                health = 24;
                break;
            case Wave.Wave4:
                health = 32;
                break;
            case Wave.Wave5:
                health = 40;
                break;
        }
    }

    private void OnDestroy()
    {
        GameEventManager.Instance.OnWaveEndedEvent -= InitializeHealth;
    }
}
