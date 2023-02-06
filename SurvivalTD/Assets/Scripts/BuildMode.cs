using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildMode : MonoBehaviour
{

    public static BuildMode Instance { get; private set; }

    [SerializeField] private BuildingSO selectedBuilding;
    [SerializeField] private Transform buildingGhost;
    [SerializeField] private GameObject errorMessage;

    [ColorUsage(true, true)]
    [SerializeField] private Color validColor;

    [ColorUsage(true, true)]
    [SerializeField] private Color invalidColor;

    private bool inBuildMode = false;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("It seems like there is more than one BuildMode object active in the scene!");
            Destroy(gameObject);
        }
        Instance = this;
    }

    private void Update()
    {
        HandleInput();
        HandleBuilding();
    }

    public void UpdateBuildingGhostMesh()
    {
        buildingGhost.GetComponent<MeshFilter>().mesh = UtilsClass.CombineMeshes(selectedBuilding.prefab.gameObject);
        buildingGhost.gameObject.SetActive(true);
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (inBuildMode)
            {
                buildingGhost.gameObject.SetActive(false);
                inBuildMode = false;
            }
            else
            {
                UpdateBuildingGhostMesh();
                inBuildMode = true;
            }
        }
    }

    private void HandleBuilding()
    {
        if (!inBuildMode)
        {
            errorMessage.SetActive(false);
            return;
        }

        if (!Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 50f))
        {
            errorMessage.SetActive(true);
            errorMessage.GetComponentInChildren<TextMeshProUGUI>().text = "Invalid aim position!";
            return;
        }

        GridPosition hitGridPosition = LevelGrid.Instance.GetGridPosition(hit.point);
        Material material = buildingGhost.GetComponent<MeshRenderer>().material;

        if (!LevelGrid.Instance.IsValidGridPosition(hitGridPosition))
        {
            material.SetColor("_MainColor", invalidColor);
            buildingGhost.transform.position = hit.point + new Vector3(0, 0.5f, 0);
            errorMessage.SetActive(true);
            errorMessage.GetComponentInChildren<TextMeshProUGUI>().text = "Invalid position!";
            return;
        }

        buildingGhost.transform.position = LevelGrid.Instance.GetWorldPosition(hitGridPosition);

        if (LevelGrid.Instance.IsGridPositionOccupied(hitGridPosition))
        {
            material.SetColor("_MainColor", invalidColor);
            errorMessage.SetActive(true);
            errorMessage.GetComponentInChildren<TextMeshProUGUI>().text = "Position is occupied";
            return;
        }

        if (!selectedBuilding.constructionCost.CanAfford(Inventory.Instance.GetAllItems()))
        {
            material.SetColor("_MainColor", invalidColor); errorMessage.SetActive(true);
            errorMessage.GetComponentInChildren<TextMeshProUGUI>().text = "Cannot afford!";
            return;
        }

        BaseBuilding baseBuilding = selectedBuilding.prefab.GetComponent<BaseBuilding>();

        if(!baseBuilding.IsBuildingConditionMet(hitGridPosition, out string error))
        {
            material.SetColor("_MainColor", invalidColor); errorMessage.SetActive(true);
            errorMessage.GetComponentInChildren<TextMeshProUGUI>().text = error;
            return;
        }

        errorMessage.SetActive(false);

        material.SetColor("_MainColor", validColor);

        if (Input.GetMouseButtonDown(0))
        {
            BuildingConstruction.Create(LevelGrid.Instance.GetWorldPosition(hitGridPosition), buildingGhost.transform.localRotation, selectedBuilding);
            LevelGrid.Instance.GetGridObject(hitGridPosition).SetBuilding(selectedBuilding);
            Inventory.Instance.SpendItems(selectedBuilding.constructionCost);
        }
    }

}
