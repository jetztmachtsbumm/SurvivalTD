using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{

    [SerializeField] private ItemSO resource;
    [SerializeField] private Vector3 minerPosition;
    [SerializeField] private int resourcesPerSecond;

    public ItemSO GetResource()
    {
        return resource;
    }

    public int GetResourcesPerSecond()
    {
        return resourcesPerSecond;
    }

}
