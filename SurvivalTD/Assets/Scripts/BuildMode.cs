using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildMode : MonoBehaviour
{

    public static BuildMode Instance { get; private set; }

    [SerializeField] private Transform buildingSelectUI;
    [SerializeField] private Transform buildingGhost;
    [SerializeField] private Transform buildingInventoryUI;
    [SerializeField] private GameObject errorMessage;

    [ColorUsage(true, true)]
    [SerializeField] private Color validColor;

    [ColorUsage(true, true)]
    [SerializeField] private Color invalidColor;
    
    private BuildingSO selectedBuilding;
    private bool inBuildingSelectUI = false;
    private bool inBuildMode = false;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("It seems like there is more than one BuildMode object active in the scene!");
            Destroy(gameObject);
        }
        Instance = this;

        CreateUI();
    }

    private void Update()
    {
        HandleInput();
        HandleBuilding();
    }

    public void UpdateBuildingGhostMesh()
    {
        buildingGhost.GetComponent<MeshFilter>().mesh = Utils.CombineMeshes(selectedBuilding.prefab.gameObject);
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (GameManager.Instance.GetCurrentGamePhase() == GameManager.GamePhase.FARMING_PHASE)
            {
                if (inBuildMode)
                {
                    inBuildMode = false;
                    buildingGhost.gameObject.SetActive(false);
                }
                else
                {
                    ToggleBuildingSelectUI(!inBuildingSelectUI);
                }
            }
        }
    }

    private void HandleBuilding()
    {
        if (!inBuildMode)
        {
            errorMessage.SetActive(false);
            buildingGhost.gameObject.SetActive(false);
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

        if (!selectedBuilding.constructionCost.CanAfford(PlayerInventory.Instance.GetItems().ToArray()))
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
            BuildingConstruction.Create(LevelGrid.Instance.GetWorldPosition(hitGridPosition), buildingGhost.transform.localRotation, selectedBuilding, buildingInventoryUI);
            LevelGrid.Instance.GetGridObject(hitGridPosition).SetBuilding(selectedBuilding);
            PlayerInventory.Instance.SpendItems(selectedBuilding.constructionCost);
        }
    }

    private void CreateUI()
    {
        foreach(BuildingSO building in Resources.Load<BuildingTypeHolder>("BuildingTypes").buildingTypes)
        {
            Transform template = Resources.Load<Transform>("BuildingSelectUIButton");
            Transform uiElement;
            switch (building.buidlingMenuType)
            {
                case BuildingSO.BuildingMenuType.DEFENSE:
                    uiElement = Instantiate(template, buildingSelectUI.Find("DefenseBuildings"));
                    uiElement.GetComponent<Image>().sprite = building.uiTexture;
                    uiElement.GetComponent<Button>().onClick.AddListener(() => OnClickListener(building));
                    break;

                case BuildingSO.BuildingMenuType.HARVESTING:
                    uiElement = Instantiate(template, buildingSelectUI.Find("HarvestingBuildings"));
                    uiElement.GetComponent<Image>().sprite = building.uiTexture;
                    uiElement.GetComponent<Button>().onClick.AddListener(() => OnClickListener(building));
                    break;
            }
        }
    }

    private void OnClickListener(BuildingSO building)
    {
        selectedBuilding = building;
        UpdateBuildingGhostMesh();
        ToggleBuildingSelectUI(false);
        inBuildMode = true;
        buildingGhost.gameObject.SetActive(true);
    }

    public void ToggleBuildingSelectUI(bool on)
    {
        if (on)
        {
            inBuildingSelectUI = true;
            PlayerControlls.Instance.ToggleControllsOff();
        }
        else
        {
            inBuildingSelectUI = false;
            PlayerControlls.Instance.ToggleControllsOn();
        }

        buildingSelectUI.gameObject.SetActive(inBuildingSelectUI);
        ActivateMenu("NONE");
    }

    public void ActivateMenu(string menuType)
    {
        foreach(Transform transform in buildingSelectUI)
        {
            if(transform.name != "Background")
            {
                transform.gameObject.SetActive(false);
            }
        }

        Transform menu = null;
        Enum.TryParse(menuType, out BuildingSO.BuildingMenuType enumerator);
        switch (enumerator)
        {
            case BuildingSO.BuildingMenuType.DEFENSE:
                menu = buildingSelectUI.Find("DefenseBuildings");
                break;
            case BuildingSO.BuildingMenuType.HARVESTING:
                menu = buildingSelectUI.Find("HarvestingBuildings");
                break;
            case BuildingSO.BuildingMenuType.NONE:
                menu = buildingSelectUI.Find("Menu");
                break;
        }
        menu.gameObject.SetActive(true);
    }

    public void SetInBuildMode(bool inBuildMode)
    {
        this.inBuildMode = inBuildMode;
    }
}
