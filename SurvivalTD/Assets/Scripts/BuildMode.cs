using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMode : MonoBehaviour
{

    [SerializeField] BuildingSO selectedBuilding;
    [SerializeField] Transform buildingGhost;

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
                    if (material.GetColor("_MainColor") != Color.cyan)
                    {
                        material.SetColor("_MainColor", Color.cyan);
                    }
                }
                else
                {
                    buildingGhost.transform.position = hit.point + new Vector3(0, 0.5f, 0);
                    if (material.GetColor("_MainColor") != Color.red)
                    {
                        material.SetColor("_MainColor", Color.red);
                    }
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
