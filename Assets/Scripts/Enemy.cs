using UnityEngine;

public enum ShooterEnemyState
{
    Hidden, Attacking
}

public class ShooterEnemy : MonoBehaviour
{
    [SerializeField] Color realColor;
    [SerializeField] float detectionRadius;
    [SerializeField] float attackRange;
    [SerializeField] float attackCooldown;
    [SerializeField] Projectile projectilePrefab;

    SpriteRenderer sr;
    ShooterEnemyState currentState;
    float lastAttackTime;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        switch(currentState)
        {
            case ShooterEnemyState.Hidden:
                HandleHidden();
                break;
            case ShooterEnemyState.Attacking:
                HandleAttacking();
                break;
        }
    }

    void HandleHidden()
    {
        float sqrDistance = (Player.Instance.transform.position - transform.position).sqrMagnitude;
        if(sqrDistance <= detectionRadius * detectionRadius)
        {
            sr.color = realColor;
            currentState = ShooterEnemyState.Attacking;
        }
    }

    void HandleAttacking()
    {
        float sqrDistance = (Player.Instance.transform.position - transform.position).sqrMagnitude;
        if(sqrDistance <= attackRange * attackRange && Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        Vector2 direction = (Player.Instance.transform.position - transform.position).normalized;
        Projectile projectile = Instantiate(projectilePrefab);
        projectile.transform.position = transform.position;
        projectile.transform.up = direction;
    }
}
