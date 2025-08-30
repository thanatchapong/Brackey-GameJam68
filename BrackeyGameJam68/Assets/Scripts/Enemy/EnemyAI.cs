using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] public int id;

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.avoidancePriority = Random.Range(30, 60);
        agent.obstacleAvoidanceType = (ObstacleAvoidanceType)2;
        agent.radius = 0.3f;
        agent.stoppingDistance = 0.5f;
    }

    private void Update()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 dir = (transform.position - collision.transform.position).normalized;
            agent.Move(dir * 0.05f);
        }
    }
}
