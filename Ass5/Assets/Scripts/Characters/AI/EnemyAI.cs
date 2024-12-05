using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    //public enum State { Wander, Pursue, Attack }
    //private State currentState = State.Wander;

    [SerializeField]
    private CharacterStats characterStats;
    public float DetectionRange { get; private set; } = 10f; // How far the enemy can detect the player
    public float WanderRadius { get; private set; } = 5f; // Radius for random wandering
    public float WanderInterval { get; private set; } = 3f; // Time between wander points
    public float CurrentHP { get; set; }

    private NavMeshAgent agent;
    private float lastWanderTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= DetectionRange)
        {
            if (distanceToPlayer <= characterStats.attackRange)
                AttackPlayer();
            else
                PursuePlayer();
        }
        else
            Wander();
    }

    void PursuePlayer()
    {
        agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        agent.ResetPath(); // Stop moving
        Debug.Log("Attacking the player!");
        // Add attack logic here
    }

    void Wander()
    {
        if (Time.time - lastWanderTime > WanderInterval)
        {
            Vector3 randomDirection = Random.insideUnitSphere * WanderRadius;
            randomDirection += transform.position;
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(randomDirection, out navHit, WanderRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(navHit.position);
            }
            lastWanderTime = Time.time;
        }
    }

    void Flee()
    {
        // Flee from combat if low hp
    }

    //void OnDrawGizmosSelected()
    //{
    //    // Draw detection and attack ranges for debugging
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, detectionRange);
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, attackRange);
    //}
}
