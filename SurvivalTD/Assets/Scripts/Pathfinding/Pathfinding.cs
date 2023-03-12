using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    
    public static Pathfinding Instance { get; private set; }

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    [SerializeField] private LayerMask obstaclesLayerMask;

    private GridSystem<PathNode> gridSystem;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("It seems like there is more than one Pathfinding object active in the scene!");
            Destroy(gameObject);
        }
        Instance = this;
    }

    public void Setup(int width, int height, float cellSize)
    {
        gridSystem = new GridSystem<PathNode>(width, height, cellSize, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));

        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float boxOverlapHeight = 100f;
                foreach(Collider coll in Physics.OverlapBox(worldPosition, new Vector3(cellSize / 2, boxOverlapHeight, cellSize / 2)))
                {
                    if (1 << coll.gameObject.layer == obstaclesLayerMask)
                    {
                        GetNode(x, z).SetIsWalkable(false);
                        break;
                    }
                }
            }
        }
    }

    public List<GridPosition> FindPath(GridPosition startingPos, GridPosition destination)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode startingNode = gridSystem.GetGridObject(startingPos);
        PathNode destinationNode = gridSystem.GetGridObject(destination);
        openList.Add(startingNode);

        for(int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for(int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetPreviousNode();
            }
        }

        startingNode.SetGCost(0);
        startingNode.SetHCost(CalculateDistance(startingPos, destination));
        startingNode.CalculateFCost();

        while(openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);

            if(currentNode == destinationNode)
            {
                return CalculatePath(destinationNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach(PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                }

                if (!neighbourNode.IsWalkable())
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                if(tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetPreviousNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), destination));
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        //No path found
        return null;
    }

    public int CalculateDistance(GridPosition a, GridPosition b)
    {
        GridPosition gridPositionDistance = a - b;
        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    public PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodes)
    {
        PathNode lowestFCostPathNode = pathNodes[0];
        foreach(PathNode pathNode in pathNodes)
        {
            if(pathNode.GetFCost() < lowestFCostPathNode.GetFCost())
            {
                lowestFCostPathNode = pathNode;
            }
        }
        return lowestFCostPathNode;
    }

    private List<GridPosition> CalculatePath(PathNode destinationNode)
    {
        List<PathNode> pathNodes = new List<PathNode> { destinationNode };
        PathNode currentNode = destinationNode;

        while(currentNode.GetPreviousPathNode() != null)
        {
            pathNodes.Add(currentNode.GetPreviousPathNode());
            currentNode = currentNode.GetPreviousPathNode();
        }

        pathNodes.Reverse();

        return pathNodes.Select(p => p.GetGridPosition()).ToList();
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();
        GridPosition gridPosition = currentNode.GetGridPosition();

        for (int x = gridPosition.x - 1; x <= gridPosition.x + 1; x++)
        {
            for (int z = gridPosition.z - 1; z <= gridPosition.z + 1; z++)
            {
                if (x == gridPosition.x && z == gridPosition.z) // Skip current node
                {
                    continue;
                }

                neighbourList.Add(x >= 0 && x < gridSystem.GetWidth() && z >= 0 && z < gridSystem.GetHeight() ? GetNode(x, z) : null);
            }
        }

        neighbourList.RemoveAll(node => node == null); // Remove any null nodes

        return neighbourList;
    }

}
