using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] TextMeshPro damageText;
    [SerializeField] float lifetime = 1f;
    [SerializeField] float floatHeight = 2f; 
    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField] AnimationCurve alphaCurve;
    [SerializeField] AnimationCurve moveCurve;
    
    float timer;
    Vector3 startPosition;
    Vector3 randomOffset;
    Color originalColor;
    
    public void Setup(int damage, Color color)
    {
        damageText.text = damage.ToString();
        damageText.color = color;
        originalColor = color;
        
        startPosition = transform.position;
        
        randomOffset = new Vector3(Random.Range(-0.3f, 0.3f), 0f, 0f);
        
        timer = 0f;
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        float progress = timer / lifetime;
        
        float yMovement = moveCurve.Evaluate(progress) * floatHeight;
        Vector3 currentOffset = randomOffset * progress;
        
        transform.position = startPosition + Vector3.up * yMovement + currentOffset;
        
        if(scaleCurve.keys.Length > 0)
        {
            float scale = scaleCurve.Evaluate(progress);
            transform.localScale = Vector3.one * scale;
        }
        
        if(alphaCurve.keys.Length > 0)
        {
            Color color = originalColor;
            color.a = alphaCurve.Evaluate(progress);
            damageText.color = color;
        }
        
        if(timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}