using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] Color resourceColor;
    [SerializeField] DamagePopup damagePopup;
    [SerializeField] ResourceType type;
    [SerializeField] int maxAmount;

    int currentAmount;

    private void Start()
    {
        currentAmount = maxAmount;
    }

    public void Collect(int amount)
    {
        DamagePopup collect = Instantiate(damagePopup);
        collect.transform.position = transform.position;
        collect.Setup(amount, resourceColor);

        currentAmount -= amount;
        ResourceSystem.Instance.AddResource(amount, type);
        if(currentAmount <= 0)
        {
            Destroy(gameObject);
        }
    }
}
