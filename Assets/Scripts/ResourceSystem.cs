using TMPro;
using UnityEngine;

public enum ResourceType
{
    Wood, Stone, Metal
}

public class ResourceSystem : MonoBehaviour
{
    public static ResourceSystem Instance;

    [SerializeField] TextMeshProUGUI woodLabel;
    [SerializeField] TextMeshProUGUI stoneLabel;
    [SerializeField] TextMeshProUGUI metalLabel;

    int currentWood;
    int currentStone;
    int currentMetal;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        woodLabel.text = currentWood.ToString();
        stoneLabel.text = currentStone.ToString();
        metalLabel.text = currentMetal.ToString();
    }

    public void AddResource(int amount, ResourceType type)
    {
        if(type == ResourceType.Wood)
        {
            currentWood += amount;
            woodLabel.text = currentWood.ToString();
        }
        if(type == ResourceType.Stone)
        {
            currentStone += amount;
            stoneLabel.text = currentStone.ToString();
        }
        if(type == ResourceType.Metal)
        {
            currentMetal += amount;
            metalLabel.text = currentMetal.ToString();
        }
    }
}
