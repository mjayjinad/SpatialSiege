using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;

    [SerializeField] private bool isKillerDrone;

    //private Target _target;

    private bool isAgentActive =false;
    private Vector3 _investigationPoint;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        GameEventManager.Instance.OnEnemyReachedTargetPosEvent += InitializeNavmeshAgent;
        //_target = GetComponent<Target>();
    }

    private void Update()
    {
        //if (_target._isDead) return;
        if (!isAgentActive) return;
        SetInvestigationPoint();

    }

    private void InitializeNavmeshAgent(bool value)
    {
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
}
