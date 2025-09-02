using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] DamagePopup damagePopup;

    int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(int amount)
    {
        DamagePopup damage = Instantiate(damagePopup);
        damage.transform.position = transform.position;
        damage.Setup(amount, Color.red);

        currentHealth -= amount;
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
