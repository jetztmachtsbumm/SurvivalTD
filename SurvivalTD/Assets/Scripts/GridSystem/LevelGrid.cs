using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{

    public static LevelGrid Instance { get; private set; }

    private GridSystem gridSystem;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("It seems like there's more than one LevelGrid active in the scene!");
            Destroy(gameObject);
        }
        Instance = this;

        gridSystem = new GridSystem(10, 10, 3f);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();

}
