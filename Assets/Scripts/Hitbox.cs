using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] bool showDebug;
    [SerializeField] LayerMask hurtLayer;

    bool isActive;
    Vector2 currentSize;
    Vector2 currentDirection;
    float timer;

    public void Activate(Vector2 size, Vector2 direction, float duration)
    {
        currentSize = size;
        currentDirection = direction;
        timer = duration;

        isActive = true;
    }

    private void Update()
    {
        if(isActive)
        {
            timer -= Time.deltaTime;
            if(timer <= 0f)
            {
                timer = 0f;
                isActive = false;
            }

            Vector3 center = transform.position + (Vector3)(currentDirection * currentSize.x * 0.5f);
            float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
            Collider2D[] colliders = Physics2D.OverlapBoxAll(center, currentSize, angle, hurtLayer);
            foreach(Collider2D c in colliders)
            {
                Debug.Log("Hit: " + c.gameObject.name);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(showDebug && isActive)
        {
            Vector3 center = transform.position + (Vector3)(currentDirection * currentSize.x * 0.5f);
            float zRotation = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
            Gizmos.matrix = Matrix4x4.TRS(center, Quaternion.Euler(0f, 0f, zRotation), Vector3.one);
            Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
            Gizmos.DrawCube(Vector3.zero, new Vector3(currentSize.x, currentSize.y, 0));
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(currentSize.x, currentSize.y, 0));
        }
    }
}
