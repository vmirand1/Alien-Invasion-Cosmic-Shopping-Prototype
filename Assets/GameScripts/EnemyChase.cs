using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private float timer;

    public Transform player;
    public float chaseDistance = 5f;
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;

    private Character playerScript; // Reference to the player's script
    private AudioManager audioManager;

    private bool isChasing = false; // Track if this enemy is currently chasing

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        timer = wanderTimer;
        playerScript = player.GetComponent<Character>(); // Get the player's Character script
        audioManager = AudioManager.instance; // Get the AudioManager
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if the player is hiding
        if (playerScript != null && playerScript.IsHiding())
        {
            // Stop chasing and return to patrol
            if (isChasing)
            {
                isChasing = false;
                audioManager.ResumeNormalMusic(); // Resume normal music
            }
            animator.SetBool("isRunning", false);
            ReturnToPatrol();
            Debug.Log("Player is hiding. Enemy returns to patrol.");
        }
        else if (distanceToPlayer <= chaseDistance)
        {
            // Chase the player
            agent.SetDestination(player.position);
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);

            if (!isChasing)
            {
                isChasing = true;
                audioManager.PlayChaseMusic(); // Play chase music
            }
            Debug.Log("Enemy is chasing the player.");
        }
        else
        {
            // Wander around
            if (isChasing)
            {
                isChasing = false;
                audioManager.ResumeNormalMusic(); // Resume normal music
            }
            ReturnToPatrol();
        }
    }

    private void ReturnToPatrol()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }

        if (agent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player has the tag "Player"
        {
            Counter.Instance.DecreaseTotal(); // Decrease the item count in the Counter script
            Debug.Log("Player touched by enemy. Decreasing item count.");
        }
    }
}


