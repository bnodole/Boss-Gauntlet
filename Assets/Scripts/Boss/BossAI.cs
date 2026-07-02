using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    public enum BossStates
    {
        Idle,
        Chase,
        Attack,
        Enraged,
        Dead
    }

    Animator bossAnimator;
    NavMeshAgent agent;

    public Transform player;

    public float detectRange = 15f;
    public float attackRange = 3f;
    public float attackCooldown = 2f;

    BossStates currentState;
    float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        currentState = BossStates.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case BossStates.Idle:
                IdleState();
                break;
            case BossStates.Chase:
                ChaseState();
                break;
            case BossStates.Attack:
                AttackState();
                break;
        }
    }

    private void AttackState()
    {
        timer += Time.deltaTime;

        transform.LookAt(player);

        float distance =
            Vector3.Distance(
                transform.position,
                player.position);

        if (timer >= attackCooldown)
        {
            ChooseAttack();
            timer = 0;
        }

        // Player moved outside attack range
        if (distance > attackRange)
        {
            currentState = BossStates.Chase;

            agent.isStopped = false;
        }

        // Player escaped completely
        if (distance > detectRange)
        {
            currentState = BossStates.Idle;

            bossAnimator.SetBool("isActive", false);
            bossAnimator.SetBool("canChase", false);

            agent.isStopped = true;
        }
    }

    private void ChooseAttack()
    {
        int randomAttack = UnityEngine.Random.Range(1, 7);

        bossAnimator.SetInteger(
            "Atk_type",
            randomAttack);

        bossAnimator.SetTrigger("Attack");

        Debug.Log("Boss Attack: " + randomAttack);
    }

    private void ChaseState()
    {
        bossAnimator.SetBool("canChase", true);
        agent.isStopped = false;
        agent.SetDestination(player.position);
        float distance =
            Vector3.Distance(
                transform.position,
                player.position);

        if (distance <= attackRange)
        {
            currentState = BossStates.Attack;

            agent.isStopped = true;

            bossAnimator.SetBool("canChase", false);
        }
        else if (distance > detectRange)
        {
            currentState = BossStates.Idle;

            agent.isStopped = true;

            bossAnimator.SetBool("canChase", false);
            bossAnimator.SetBool("isActive", false);

            Debug.Log("Back to Idle");
        }
    }

    private void IdleState()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if(distance <= detectRange)
        {
            bossAnimator.SetBool("isActive", true);
            currentState = BossStates.Chase;
        }
    }
}
