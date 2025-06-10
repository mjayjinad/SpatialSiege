using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private EnemyManager enemyManager;

    public bool isDead;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Plasma")) return;
        if (isDead) return;

        if (health > 0)
        {
            health--;
        }
        else
        {
            isDead = true;
            enemyManager.Die();
        }
    }
}
