using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public enum Patrol
    {
        Player,
        Orb
    }

    [SerializeField] private Patrol patrol;
    [SerializeField] private bool isKillerDrone;
    [SerializeField] private float deathDelay = 5;
    [SerializeField] private float eatDistance = 0.2f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private LayerMask playerDetectionLayer;
    [SerializeField] private LayerMask orbDetectionLayer;

    private NavMeshAgent _agent;
    private EnemyHealthManager healthManager;
    private Animator _animator;
    private Rigidbody rb;
    private RayGun rayGun;
    private bool isAgentActive =false;
    private Vector3 playerPos;
    private Vector3 investigationPoint;
    private float stopingDistance;

    private void Start()
    {
        GameEventManager.Instance.OnEnemyReachedTargetPosEvent += InitializeNavmeshAgent;

        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        rayGun = GetComponent<RayGun>();
        healthManager = GetComponentInChildren<EnemyHealthManager>();

        playerPos = Camera.main.transform.position;
        stopingDistance = _agent.stoppingDistance;
    }

    private void Update()
    {
        if (healthManager.isDead) return;
        if (!isAgentActive) return;

        if (IsPlayerInRange())
        {
            patrol = Patrol.Player;
            _agent.stoppingDistance = stopingDistance;
            SetInvestigationPoint(playerPos);
            rayGun.Shoot();
        }
        else
        {
            Debug.Log("going to orb");
            Transform closest = GetClosestOrb();
            if (closest)
            {
                patrol = Patrol.Orb;
                investigationPoint = closest.position;
                _agent.stoppingDistance = 0;
                SetInvestigationPoint(investigationPoint);
            }
        }
    }

    //public Transform GetClosestOrb()
    //{
    //    Transform closest = null;
    //    float minDistance = Mathf.Infinity;

    //    List<Transform> orbs = OrbSpawner.Instance.spawnedOrbs;

    //    // Use OverlapSphere to detect orbs within range
    //    Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange, orbDetectionLayer);

    //    // Loop through all colliders detected by OverlapSphere
    //    foreach (var hitCollider in hitColliders)
    //    {
    //        // Check if the collider is an orb
    //        if (orbs.Contains(hitCollider.transform))
    //        {
    //            Vector3 orbPosition = hitCollider.transform.position;
    //            orbPosition.y = 0;  // Ensure the y-position is neutral for comparison

    //            // Calculate the distance to this orb
    //            Vector3 directionToOrb = orbPosition - transform.position;
    //            directionToOrb.y = 0; // Ignore the vertical axis to make it horizontal

    //            float distance = directionToOrb.magnitude;

    //            // Check if this orb is closer than the previous closest
    //            if (distance < minDistance)
    //            {
    //                minDistance = distance;
    //                closest = hitCollider.transform;
    //            }
    //        }
    //    }

    //    // If the closest orb is within the eating distance, destroy it
    //    if (minDistance < eatDistance && closest != null)
    //    {
    //        OrbSpawner.Instance.DestroyOrb(closest.gameObject);
    //    }

    //    return closest;
    //}

    public Transform GetClosestOrb()
    {
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        List<Transform> orbs = OrbSpawner.Instance.spawnedOrbs;

        foreach (var item in orbs)
        {
            Vector3 dronePosition = transform.position;
            dronePosition.y = 0;
            Vector3 orbPosition = item.transform.position;
            orbPosition.y = 0;

            float distance = Vector3.Distance(dronePosition, orbPosition);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = item;
            }
        }

        if (minDistance < eatDistance)
        {
            OrbSpawner.Instance.DestroyOrb(closest.gameObject);
        }

        return closest;
    }

    private bool IsPlayerInRange()
    {
        // Perform a SphereCast to check if the player is within range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange, playerDetectionLayer);

        // Loop through the colliders hit by the SphereCast and check if the player is in the range
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player")) // Assuming player has the "Player" tag
            {
                Debug.Log(hitCollider.transform.name);
                return true;  // Player detected within range
            }
        }

        return false; // Player not within range
    }

    private void InitializeNavmeshAgent(bool value)
    {
        if (healthManager.isDead) return;
        _agent.enabled = value;
        isAgentActive = value;
        if (isKillerDrone)
            _animator.SetBool("Attack", true);
    }

    private void SetInvestigationPoint(Vector3 investigationPoint)
    {
        if (_agent.enabled == false) return;
        _agent.SetDestination(investigationPoint);
    }

    public void Die()
    {
        if (isKillerDrone)
            _animator.SetBool("IsDead", true);
        _animator.enabled = false;
        _agent.enabled = false;
        rb.isKinematic = false;
        rb.useGravity = true;
        StartCoroutine(DeathDelay());
    }

    private IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(deathDelay);

        EnemySpawnerHandler.Instance.spawnedDrone.Remove(gameObject);
        Destroy(gameObject);
    }


    private void OnDrawGizmos()
    {
        // Only draw the Gizmo if the enemy is not dead
        if (healthManager != null && !healthManager.isDead)
        {
            Gizmos.color = Color.red;  // Color the Gizmo to indicate danger or detection range
            Gizmos.DrawWireSphere(transform.position, detectionRange);  // Draw the sphere at the drone's position
        }
    }
}
