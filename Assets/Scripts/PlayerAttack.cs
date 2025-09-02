using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] float attackWidth = 0.5f; // How thick the attack line is
    [SerializeField] int damage = 25;
    [SerializeField] float attackCooldown = 0.5f;
    [SerializeField] LayerMask enemyLayer = -1;
    
    float lastAttackTime;
    LineRenderer slashLine;
    
    void Start()
    {
        GameObject slashObj = new GameObject("SlashVisual");
        slashObj.transform.SetParent(transform);
        slashLine = slashObj.AddComponent<LineRenderer>();
        slashLine.material = new Material(Shader.Find("Sprites/Default"));
        slashLine.material.color = Color.white;
        slashLine.startWidth = 0.1f;
        slashLine.endWidth = 0.05f;
        slashLine.positionCount = 2;
        slashLine.enabled = false;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
        }
    }
    
    void Attack()
{
    lastAttackTime = Time.time;
    
    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    mousePos.z = 0;
    Vector2 attackDirection = (mousePos - transform.position).normalized;
    
    // Show slash visual
    StartCoroutine(ShowSlash(attackDirection));
    
    // Line-based detection using multiple point checks
    Vector2 startPoint = transform.position;
    Vector2 endPoint = startPoint + attackDirection * attackRange;
    
    int checkPoints = 10;
    HashSet<Collider2D> hitObjects = new HashSet<Collider2D>();
    
    for (int i = 0; i <= checkPoints; i++)
    {
        float t = i / (float)checkPoints;
        Vector2 checkPoint = Vector2.Lerp(startPoint, endPoint, t);
        
        // Check for both enemies AND resources
        Collider2D[] nearby = Physics2D.OverlapCircleAll(checkPoint, attackWidth / 2f);
        
        foreach (Collider2D hit in nearby)
        {
            // Skip if we already processed this object
            if (hitObjects.Contains(hit)) continue;
            
            hitObjects.Add(hit);
            
            // Check if it's an enemy first
            if (hit.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (hit.TryGetComponent(out Health health))
                {
                    health.Damage(damage);
                }
            }
            // Check if it's a resource
            else if (hit.TryGetComponent(out Resource resource))
            {
                resource.Collect(damage);
            }
        }
    }
}
    
    IEnumerator ShowSlash(Vector2 direction)
    {
        slashLine.enabled = true;
        
        Vector3 start = transform.position + (Vector3)(direction * 0.3f);
        Vector3 end = transform.position + (Vector3)(direction * attackRange);
        
        slashLine.SetPosition(0, start);
        slashLine.SetPosition(1, end);
        
        yield return new WaitForSeconds(0.15f);
        
        slashLine.enabled = false;
    }
}
