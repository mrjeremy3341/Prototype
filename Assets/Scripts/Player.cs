using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Hitbox hitbox;
    [SerializeField] PlayerController pc;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 size = new Vector2(1.5f, 1f);
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            Vector2 direction = (Vector2)(mousePos - transform.position).normalized;
            hitbox.Activate(size, direction, .5f);

            pc.AttackMomentum(direction);
        }
    }
}
