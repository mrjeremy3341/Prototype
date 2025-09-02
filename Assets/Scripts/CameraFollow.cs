using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float mouseInfluence;
    [SerializeField] float maxMouseDistance;
    [SerializeField] float mouseSmoothing;

    Vector2 smoothMouseOffset;

    private void LateUpdate()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector2 targetMouseOffset = Vector2.ClampMagnitude(mouseWorldPos - player.position, maxMouseDistance) * mouseInfluence;
        smoothMouseOffset = Vector2.Lerp(smoothMouseOffset, targetMouseOffset, mouseSmoothing * Time.deltaTime);

        Vector3 targetPosition = player.position + (Vector3)smoothMouseOffset;
        targetPosition.z = transform.position.z;

        transform.position = targetPosition;
    }
}
