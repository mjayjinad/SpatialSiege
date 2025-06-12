using UnityEngine;
using UnityEngine.AI;

public class OrbNavmenshAgent : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        Vector3 originalPos = transform.position;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(originalPos, out hit, 5.0f, NavMesh.AllAreas))
        {
            Vector3 adjustedPos = new Vector3(hit.position.x, originalPos.y, hit.position.z);
            agent.Warp(adjustedPos);  // Move to valid NavMesh position, keep original Y
        }
        else
        {
            Debug.LogWarning("No nearby NavMesh position found for orb.");
        }
    }
}