using UnityEngine;

public enum ChaseEnemyState
{
    Hidden, Chasing, Attacking, Returning
}

public class ChaseEnemy : MonoBehaviour
{
    [SerializeField] Color realColor;
    [SerializeField] float detectionRadius;
    [SerializeField] float attackRange;
    [SerializeField] float moveSpeed;
    [SerializeField] float attackCooldown;
    [SerializeField] int damage;

    SpriteRenderer sr;
    Rigidbody2D rb;
    ChaseEnemyState currentState;
    
    Vector3 originalPosition;
    float lastAttackTime;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        switch(currentState)
        {
            case ChaseEnemyState.Hidden:
                HandleHidden();
                break;
            case ChaseEnemyState.Chasing:
                HandleChasing();
                break;
            case ChaseEnemyState.Attacking:
                HandleAttacking();
                break;
            case ChaseEnemyState.Returning:
                HandleReturning();
                break;
        }
    }

    void HandleHidden()
    {
        float sqrDistance = (Player.Instance.transform.position - transform.position).sqrMagnitude;
        if(sqrDistance <= detectionRadius * detectionRadius)
        {
            Activate();
        }
    }

    void HandleChasing()
    {
        float sqrDistance = (Player.Instance.transform.position - transform.position).sqrMagnitude;
        
        // Check if player left detection range
        if (sqrDistance > detectionRadius * detectionRadius)
        {
            currentState = ChaseEnemyState.Returning;
            return;
        }
        
        // Check if close enough to attack
        if (sqrDistance <= attackRange * attackRange)
        {
            currentState = ChaseEnemyState.Attacking;
            rb.velocity = Vector2.zero; // Stop moving
            return;
        }
        
        // Move toward player
        Vector2 direction = (Player.Instance.transform.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    void HandleAttacking()
    {
        rb.velocity = Vector2.zero; // Stay still while attacking
        
        float sqrDistance = (Player.Instance.transform.position - transform.position).sqrMagnitude;
        
        // Player moved out of attack range, go back to chasing
        if (sqrDistance > attackRange * attackRange)
        {
            // Check if still in detection range
            if (sqrDistance <= detectionRadius * detectionRadius)
            {
                currentState = ChaseEnemyState.Chasing;
            }
            else
            {
                currentState = ChaseEnemyState.Returning;
            }
            return;
        }
        
        // Attack if cooldown is ready
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
        }
    }

    void HandleReturning()
    {
        // Move back to original position
        Vector2 direction = (originalPosition - transform.position).normalized;
        rb.velocity = direction * (moveSpeed * 0.7f); // Move slightly slower when returning
        
        // Check if back at original position
        if (Vector2.Distance(transform.position, originalPosition) < 0.3f)
        {
            ReturnToIdle();
        }

        float sqrDistance = (Player.Instance.transform.position - transform.position).sqrMagnitude;
        if (sqrDistance <= detectionRadius * detectionRadius)
        {
            currentState = ChaseEnemyState.Chasing;
        }
    }

    void Activate()
    {
        sr.color = realColor;
        currentState = ChaseEnemyState.Chasing;
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        
        if (Player.Instance != null && Player.Instance.TryGetComponent(out Health health))
        {
            health.Damage(damage);
        }
    }

    void ReturnToIdle()
    {
        rb.velocity = Vector2.zero;
        transform.position = originalPosition;
        currentState = ChaseEnemyState.Hidden;
    }
}
