using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConstruction : MonoBehaviour
{

    public static void Create(Vector3 position, Quaternion rotation, BuildingSO constructedBuilding)
    {
        GameObject construction = Instantiate(Resources.Load<GameObject>("BuildingConstruction"));

        construction.transform.position = position;
        construction.transform.rotation = rotation;

        MeshFilter meshFilter = construction.GetComponent<MeshFilter>();
        meshFilter.mesh = UtilsClass.CombineMeshes(constructedBuilding.prefab.gameObject);

        MeshRenderer meshRenderer = construction.GetComponent<MeshRenderer>();

        meshRenderer.material.SetFloat("_Dissolve", 1f);

        construction.GetComponent<BuildingConstruction>().Setup(constructedBuilding);
    }

    private BuildingSO constructedBuilding;
    private float constructionTimer;
    private Material constructionMaterial;

    private void Awake()
    {
        constructionMaterial = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        constructionTimer -= Time.deltaTime;
        constructionMaterial.SetFloat("_Dissolve", GetConstructionTimerNormalized());
        if(constructionTimer <= 0)
        {
            Instantiate(constructedBuilding.prefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    private void Setup(BuildingSO constructedBuilding)
    {
        this.constructedBuilding = constructedBuilding;
        constructionTimer = constructedBuilding.timeToConstruct;
    }

    private float GetConstructionTimerNormalized()
    {
        return 0 + constructionTimer / constructedBuilding.timeToConstruct;
    }

}
