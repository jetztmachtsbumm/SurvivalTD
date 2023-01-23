using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    
    public static ResourceManager Instance { get; private set; }

    private Dictionary<ResourceTypeSO, int> resourceAmountDictionary;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("There's already a ResourceManager active!");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        resourceAmountDictionary = new Dictionary<ResourceTypeSO, int>();

        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        foreach(ResourceTypeSO resourceType in resourceTypeList.list)
        {
            resourceAmountDictionary[resourceType] = 0;
        }
    }

    public void AddResource(ResourceTypeSO resourceType, int amount)
    {
        resourceAmountDictionary[resourceType] += amount;
    }

    public int GetResourceAmount(ResourceTypeSO resourceType)
    {
        return resourceAmountDictionary[resourceType];
    }

    public bool CanAfford(ResourceAmount[] resourceAmountArray)
    {
        foreach (ResourceAmount resourceAmount in resourceAmountArray)
        {
            if (GetResourceAmount(resourceAmount.resourceType) < resourceAmount.amount)
            {
                return false;
            }
        }
        return true;
    }

    public void SpendResources(ResourceAmount[] resourceAmountArray)
    {
        foreach (ResourceAmount resourceAmount in resourceAmountArray)
        {
            resourceAmountDictionary[resourceAmount.resourceType] -= resourceAmount.amount;
        }
    }

}
