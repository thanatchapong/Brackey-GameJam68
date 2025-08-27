using UnityEngine;
using UnityEngine.AI;

// TODO: When doing kill/die method, also call the `RemoveByKey` method in the respective EnemyTracker -mistertfy64 2025-08-27

public class EnemyAI : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] public int id;

    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        agent.SetDestination(target.position);
    }
}