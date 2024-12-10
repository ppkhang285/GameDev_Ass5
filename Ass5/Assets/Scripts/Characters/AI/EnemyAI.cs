using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Animator animator { get; private set; }

    [SerializeField]
    private CharacterStats characterStats;
    public CharacterStats Stats;

    public float DetectionRange { get; private set; } = 20f; // How far the enemy can detect the player
    public float WanderRadius { get; private set; } = 10f; // Radius for random wandering
    public float WanderInterval { get; private set; } = 3f; // Time between wander points
    private GameObject player; 

    private float hp;
    public float CurrentHP
    {
        get { return hp; }
        set
        {
            if (value < 0) hp = 0;
            else if (value > Stats.hp) hp = Stats.hp;
            else hp = value;
        }
    }

    public float CurrentDamage;
    public float Speed;
    public float AttackRange;
    public float AttackCooldown;

    private float lastWanderTime;

    public float TimeSinceLastAttack;
    private bool isDead;
    private Vector3 destination;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        Stats = Instantiate(characterStats);

        CurrentHP = Stats.hp;
        CurrentDamage = Stats.damage;
        Speed = Stats.speed;
        AttackRange = Stats.attackRange;
        AttackCooldown = Stats.attackCooldown;
        TimeSinceLastAttack = AttackCooldown;
        isDead = false;
    }

    void Start()
    {
        player = GameplayManager.Instance.player;
    }

    void Update()
    {
        if (isDead)
            return;
        TimeSinceLastAttack += Time.deltaTime;
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= DetectionRange)
        {
            if (distanceToPlayer <= characterStats.attackRange)
            {
                if (TimeSinceLastAttack >= AttackCooldown)
                    Attack();
            }
            else
                Pursue();
        }
        else
            Wander();
    }

    private void Move()
    {
        Vector3 direction = (destination - transform.position).normalized;
        animator.SetFloat("speed", direction.magnitude);
        if (direction.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10); // Smooth rotation
        }
        transform.Translate(direction * Speed * Time.deltaTime, Space.World);
    }

    private void Attack()
    {
        TimeSinceLastAttack = 0;
        animator.SetTrigger("attack");
    }

    private void Pursue()
    {
        destination = player.transform.position;
        Move();
    }

    private void Wander()
    {
        if (Time.time - lastWanderTime > WanderInterval)
        {
            destination = Random.insideUnitSphere;
            destination.y = 0;
            destination = destination.normalized * WanderRadius;
            Move();
            lastWanderTime = Time.time;
        }
        else
            Move();
    }

    public virtual void TakeDamage(float damage)
    {
        Debug.Log(CurrentHP);
        animator.SetTrigger("hit");
        CurrentHP -= damage;
        if (CurrentHP <= 0)
            Die();
    }

    public virtual void Die()
    {
        animator.SetTrigger("dead");
        isDead = true;
        gameObject.GetComponent<Collider>().enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Missile"))
        {
            float damage = ArrowManager.Instance.NotifyArrowHit(other.gameObject, gameObject);
            TakeDamage(damage);
        }
        else if (other.CompareTag("Melee"))
        {
            GameObject damagerObject = other.gameObject.transform.root.gameObject;
            if (damagerObject.GetComponent<Character>())
            {
                Character damager = damagerObject.GetComponent<Character>();
                TakeDamage(damager.CurrentDamage);
                if (damager.Stats.charType == CharacterType.Berserker)
                    (damager as Berserker).HitTarget();
            }
        }
    }
}
