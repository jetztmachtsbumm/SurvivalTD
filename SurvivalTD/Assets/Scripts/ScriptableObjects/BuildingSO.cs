using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Building")]
public class BuildingSO : ScriptableObject
{

    public Transform prefab;
    public string nameString;
    public ItemCost constructionCost;
    public float timeToConstruct;

}
