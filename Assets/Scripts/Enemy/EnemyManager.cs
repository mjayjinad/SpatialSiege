using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private bool isKillerDrone;
    [SerializeField] private float deathDelay = 5;

    private NavMeshAgent _agent;
    private EnemyHealthManager healthManager;
    private Animator _animator;
    private Rigidbody rb;
    private bool isAgentActive =false;
    private Vector3 _investigationPoint;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        GameEventManager.Instance.OnEnemyReachedTargetPosEvent += InitializeNavmeshAgent;
        healthManager = GetComponentInChildren<EnemyHealthManager>();
    }

    private void Update()
    {
        if (healthManager.isDead) return;
        if (!isAgentActive) return;
        SetInvestigationPoint();
    }

    private void InitializeNavmeshAgent(bool value)
    {
        if (healthManager.isDead) return;
        _agent.enabled = value;
        isAgentActive = value;
        if (isKillerDrone)
            _animator.SetBool("Attack", true);
    }

    private void SetInvestigationPoint()
    {
        _investigationPoint = Camera.main.transform.position;

        if (_agent.enabled == false) return;
        _agent.SetDestination(_investigationPoint);
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
}
