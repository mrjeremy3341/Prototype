using UnityEngine;

public enum HitboxState
{
    Closed, Open, Colliding
}

public class Hitbox : MonoBehaviour
{
    [SerializeField] bool showDebug;
    [SerializeField] LayerMask hurtLayer;

    Vector2 size;

    
    
    
    private void OnDrawGizmos()
    {
        
    }
}
