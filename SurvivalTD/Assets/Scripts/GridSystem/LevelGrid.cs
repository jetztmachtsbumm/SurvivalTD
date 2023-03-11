using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{

    public static LevelGrid Instance { get; private set; }

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;

    private GridSystem<GridObject> gridSystem;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("It seems like there's more than one LevelGrid active in the scene!");
            Destroy(gameObject);
        }
        Instance = this;

        gridSystem = new GridSystem<GridObject>(width, height, cellSize, (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
    }

    private void Start()
    {
        Pathfinding.Instance.Setup(width, height, cellSize);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public GridObject GetGridObject(GridPosition gridPosition) => gridSystem.GetGridObject(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public bool IsGridPositionOccupied(GridPosition gridPosition) => gridSystem.IsGridPositionOccupied(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();

}
