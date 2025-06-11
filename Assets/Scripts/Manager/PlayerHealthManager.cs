using Unity.XR.CoreUtils;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField] private float health = 100;
    [SerializeField] private GameObject gameUI;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Handle player death here (e.g., show Game Over screen, respawn, etc.)
        Debug.Log("Player is dead!");
        GameManager.Instance.DeadPlayerState();
        gameUI.SetActive(true);
    }
}
