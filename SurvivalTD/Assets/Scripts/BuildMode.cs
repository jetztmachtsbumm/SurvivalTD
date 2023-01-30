using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMode : MonoBehaviour
{

    [SerializeField] private BuildingSO selectedBuilding;
    [SerializeField] private Transform buildingGhost;

    [ColorUsage(true, true)]
    [SerializeField] private Color validColor;

    [ColorUsage(true, true)]
    [SerializeField] private Color invalidColor;

    private bool inBuildMode = false;
    private Mesh buildingGhostMesh;

    private void Awake()
    {
        buildingGhostMesh = selectedBuilding.prefab.GetComponent<MeshFilter>().sharedMesh;
    }

    private void Update()
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

        if (inBuildMode)
        {
            if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 50f))
            {
                GridPosition hitGridPosition = LevelGrid.Instance.GetGridPosition(hit.point);
                Material material = buildingGhost.GetComponent<MeshRenderer>().material;

                if (LevelGrid.Instance.IsValidGridPosition(hitGridPosition))
                {
                    buildingGhost.transform.position = LevelGrid.Instance.GetWorldPosition(hitGridPosition);

                    if (!LevelGrid.Instance.IsGridPositionOccupied(hitGridPosition))
                    {
                        material.SetColor("_MainColor", validColor);

                        if (Input.GetMouseButtonDown(0))
                        {
                            Instantiate(selectedBuilding.prefab, buildingGhost.transform.position, buildingGhost.transform.rotation);
                            LevelGrid.Instance.GetGridObject(hitGridPosition).SetBuilding(selectedBuilding);
                        }
                    }
                    else
                    {
                        material.SetColor("_MainColor", invalidColor);
                    }
                }
                else
                {
                    material.SetColor("_MainColor", invalidColor);
                    buildingGhost.transform.position = hit.point + new Vector3(0, 0.5f, 0);
                }
            }
        }
    }

    public void UpdateBuildingGhostMesh()
    {
        buildingGhost.GetComponent<MeshFilter>().mesh = buildingGhostMesh;
        buildingGhost.gameObject.SetActive(true);
    }

}
