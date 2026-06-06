using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    public GameObject player;
    public Animator enemyAnimator;
    private NavMeshAgent agent;

    public float damage = 20f;
    public float health = 100f;

    public float attackDistance = 2.2f;
    public float attackCooldown = 1.5f;

    private float attackTimer = 0f;

    public GameManager gameManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null || agent == null) return;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > attackDistance)
        {
            agent.isStopped = false;
            agent.destination = player.transform.position;

            enemyAnimator.SetBool("isRunning", true);
        }
        else
        {
            agent.isStopped = true;

            enemyAnimator.SetBool("isRunning", false);

            TryAttack();
        }

        attackTimer += Time.deltaTime;
    }

    void TryAttack()
    {
        if (attackTimer >= attackCooldown)
        {
            enemyAnimator.SetTrigger("attack");

            PlayerManager playerManager = player.GetComponent<PlayerManager>();
            playerManager.Hit(damage);

            attackTimer = 0f;
        }
    }

    public void Hit(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (gameManager != null)
        {
            gameManager.enemiesAlive--;
        }

        Destroy(gameObject);
    }
}
