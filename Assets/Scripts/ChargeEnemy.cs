using UnityEngine;

public enum StoneEnemyState
{
    Hidden, Idle, Charging, ChargeCooldown
}

public class StoneEnemy : MonoBehaviour
{
    [SerializeField] Color realColor;
    [SerializeField] float detectionRadius;
    [SerializeField] float chargeRange;
    [SerializeField] float chargeSpeed;
    [SerializeField] float chargeDistance;
    [SerializeField] float chargeCooldown;
    [SerializeField] int damage;

    SpriteRenderer sr;
    Rigidbody2D rb;
    StoneEnemyState currentState = StoneEnemyState.Hidden;
    
    Vector2 chargeDirection;
    Vector2 chargeStartPosition;
    float chargeTimer;
    float cooldownTimer;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        switch(currentState)
        {
            case StoneEnemyState.Hidden:
                HandleHidden();
                break;
            case StoneEnemyState.Idle:
                HandleIdle();
                break;
            case StoneEnemyState.Charging:
                HandleCharging();
                break;
            case StoneEnemyState.ChargeCooldown:
                HandleChargeCooldown();
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

    void HandleIdle()
    {
        float sqrDistance = (Player.Instance.transform.position - transform.position).sqrMagnitude;
        if (sqrDistance <= chargeRange * chargeRange)
        {
            StartCharge();
        }
    }

    void HandleCharging()
    {
        // Move at high speed in charge direction
        rb.velocity = chargeDirection * chargeSpeed;
        
        // Check if we've traveled far enough or hit something
        float distanceTraveled = Vector2.Distance(chargeStartPosition, transform.position);
        
        if (distanceTraveled >= chargeDistance)
        {
            StopCharge();
        }
    }

    void HandleChargeCooldown()
    {
        rb.velocity = Vector2.zero; // Make sure we're stopped
        
        cooldownTimer -= Time.deltaTime;
        
        if (cooldownTimer <= 0)
        {
            currentState = StoneEnemyState.Idle;
        }
    }

    void Activate()
    {
        sr.color = realColor;
        currentState = StoneEnemyState.Idle;
    }

    void StartCharge()
    {
        // Calculate charge direction toward player
        chargeDirection = (Player.Instance.transform.position - transform.position).normalized;
        chargeStartPosition = transform.position;
        
        currentState = StoneEnemyState.Charging;
        
        // Optional: Add charge startup effect
        // Screen shake, particles, sound, etc.
    }

    void StopCharge()
    {
        rb.velocity = Vector2.zero;
        cooldownTimer = chargeCooldown;
        currentState = StoneEnemyState.ChargeCooldown;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(currentState == StoneEnemyState.Charging)
        {
            if (other.TryGetComponent(out Player player))
            {
                if (other.TryGetComponent(out Health health))
                {
                    health.Damage(damage);
                }
                
                // Continue charging past the player (don't stop)
            }
            else
            {
                // Hit a wall, stop charging
                StopCharge();
            }
        }
    }
}