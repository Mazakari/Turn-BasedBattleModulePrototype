// Roman Baranov 09.03.2021
using System.Collections.Generic;
using UnityEngine;

public class UnitPathfinding : MonoBehaviour
{
    private List<GameObject> _unitMovePath;// Кратчайший путь от одной ноды к другой
    /// <summary>
    /// Кратчайший путь от одной ноды к другой
    /// </summary>
    public List<GameObject> UnitMovePath { get { return _unitMovePath; } }
    private List<GameObject> _tempPathfindNodes;// Временное хранилище для собранных нод в Pathfinding()

    private List<GameObject> _movementTiles = null;// Коллекция тайлов для отображения радиуса передвижения юнита
    /// <summary>
    /// Коллекция тайлов для отображения радиуса передвижения юнита
    /// </summary>
    public List<GameObject> MovementTiles { get { return _movementTiles; } set { _movementTiles = value; } }

    private BattlegroundGridManager _battlegroundGridManager = null;// Ссылка на BattlegroundGridManager

    private void Awake()
    {
        _battlegroundGridManager = FindObjectOfType<BattlegroundGridManager>();
        _tempPathfindNodes = new List<GameObject>();
        _unitMovePath = new List<GameObject>();
    }

    /// <summary>
    /// Проверяет ноды вокруг позиции юнита на проходимость
    /// </summary>
    /// <param name="startNode">Исходная точка юнита</param>
    /// <param name="moveRadius">Радиус передвижения юнита в клетках</param>
    public void CheckTiles(BattlegroundGridNode startNode, int moveRadius)
    {
        _movementTiles = new List<GameObject>();
        CheckTop(startNode, moveRadius, false);
        CheckBottom(startNode, moveRadius, false);
        CheckLeft(startNode, moveRadius, false);
        CheckRight(startNode, moveRadius, false);
    }

    /// <summary>
    /// Находит кратчайший путь в клетках от одной ноды к другой
    /// </summary>
    /// <param name="startNode">Исходная нода</param>
    /// <param name="targetNode">Целевая нода</param>
    /// <param name="moveRadius">Радиус перемещения юнита</param>
    public void Pathfinding(BattlegroundGridNode startNode, BattlegroundGridNode targetNode, int moveRadius)
    {
        _tempPathfindNodes.Clear();
        _unitMovePath.Clear();
        for (int i = 0; i < moveRadius;)
        {
            CheckTop(startNode, i + 1, true);
            CheckBottom(startNode, i + 1, true);
            CheckLeft(startNode, i + 1, true);
            CheckRight(startNode, i + 1, true);

            _unitMovePath.Add(FindClosestNode(_tempPathfindNodes, targetNode));
            i++;
        }
    }

    /// <summary>
    /// Проверяет ноды сверху от позиции юнита на проходимость
    /// </summary>
    /// <param name="startNode">Исходная точка юнита</param>
    /// <param name="moveRadius">Радиус передвижения юнита в клетках</param>
    private void CheckTop(BattlegroundGridNode startNode, int moveRadius, bool isPathfinding)
    {
        BattlegroundGridNode node = null;

        for (int i = 0; i < moveRadius; i++)
        {
            if (_battlegroundGridManager.GetNodeByWorldPosition(new Vector3(startNode.WorldPosition.x, startNode.WorldPosition.y, startNode.WorldPosition.z + (i + 1))) != null)
            {
                node = _battlegroundGridManager.GetNodeByWorldPosition(new Vector3(startNode.WorldPosition.x, startNode.WorldPosition.y, startNode.WorldPosition.z + (i + 1)));
                if (isPathfinding)
                {
                    if (node.isWalkable && !node.isOccupied)
                    {
                        _tempPathfindNodes.Add(node.NodeTile);
                    }
                }
                else
                {
                    node.isWalkable = true;
                    _movementTiles.Add(node.NodeTile);
                }
            }
        }
    }

    /// <summary>
    /// Проверяет ноды снизу от позиции юнита на проходимость
    /// </summary>
    /// <param name="startNode">Исходная точка юнита</param>
    /// <param name="moveRadius">Радиус передвижения юнита в клетках</param>
    private void CheckBottom(BattlegroundGridNode startNode, int moveRadius, bool isPathfinding)
    {
        BattlegroundGridNode node = null;

        for (int i = 0; i < moveRadius; i++)
        {
            if (_battlegroundGridManager.GetNodeByWorldPosition(new Vector3(startNode.WorldPosition.x, startNode.WorldPosition.y, startNode.WorldPosition.z - (i + 1))) != null)
            {
                node = _battlegroundGridManager.GetNodeByWorldPosition(new Vector3(startNode.WorldPosition.x, startNode.WorldPosition.y, startNode.WorldPosition.z - (i + 1)));
                if (isPathfinding)
                {
                    if (node.isWalkable && !node.isOccupied)
                    {
                        _tempPathfindNodes.Add(node.NodeTile);
                    }
                }
                else
                {
                    node.isWalkable = true;
                    _movementTiles.Add(node.NodeTile);
                }
            }
        }
    }

    /// <summary>
    /// Проверяет ноды слева от позиции юнита на проходимость
    /// </summary>
    /// <param name="startNode">Исходная точка юнита</param>
    /// <param name="moveRadius">Радиус передвижения юнита в клетках</param>
    private void CheckLeft(BattlegroundGridNode startNode, int moveRadius, bool isPathfinding)
    {
        BattlegroundGridNode node = null;

        for (int i = 0; i < moveRadius; i++)
        {
            if (_battlegroundGridManager.GetNodeByWorldPosition(new Vector3(startNode.WorldPosition.x - (i + 1), startNode.WorldPosition.y, startNode.WorldPosition.z)) != null)
            {
                node = _battlegroundGridManager.GetNodeByWorldPosition(new Vector3(startNode.WorldPosition.x - (i + 1), startNode.WorldPosition.y, startNode.WorldPosition.z));
                if (isPathfinding)
                {
                    if (node.isWalkable && !node.isOccupied)
                    {
                        _tempPathfindNodes.Add(node.NodeTile);
                        CheckTop(node, moveRadius, true);
                        CheckBottom(node, moveRadius, true);
                    }
                    else if(node.isWalkable && node.isOccupied)
                    {
                        CheckTop(node, moveRadius, true);
                        CheckBottom(node, moveRadius, true);
                    }
                }
                else
                {
                    node.isWalkable = true;
                    _movementTiles.Add(node.NodeTile);
                    CheckTop(node, moveRadius, false);
                    CheckBottom(node, moveRadius, false);
                }
            }
        }
    }

    /// <summary>
    /// Проверяет ноды справа от позиции юнита на проходимость
    /// </summary>
    /// <param name="startNode">Исходная точка юнита</param>
    /// <param name="moveRadius">Радиус передвижения юнита в клетках</param>
    private void CheckRight(BattlegroundGridNode startNode, int moveRadius, bool isPathfinding)
    {
        BattlegroundGridNode node = null;

        for (int i = 0; i < moveRadius; i++)
        {
            if (_battlegroundGridManager.GetNodeByWorldPosition(new Vector3(startNode.WorldPosition.x + (i + 1), startNode.WorldPosition.y, startNode.WorldPosition.z)) != null)
            {
                node = _battlegroundGridManager.GetNodeByWorldPosition(new Vector3(startNode.WorldPosition.x + (i + 1), startNode.WorldPosition.y, startNode.WorldPosition.z));
                if (isPathfinding)
                {
                    if (node.isWalkable && !node.isOccupied)
                    {
                        _tempPathfindNodes.Add(node.NodeTile);
                        CheckTop(node, moveRadius, true);
                        CheckBottom(node, moveRadius, true);
                    }
                    else if (node.isWalkable && node.isOccupied)
                    {
                        CheckTop(node, moveRadius, true);
                        CheckBottom(node, moveRadius, true);
                    }
                }
                else
                {
                    node.isWalkable = true;
                    _movementTiles.Add(node.NodeTile);
                    CheckTop(node, moveRadius, false);
                    CheckBottom(node, moveRadius, false);
                }
            }
        }
    }

    /// <summary>
    /// Возвращает ноду ближайшую к целевой ноде 
    /// </summary>
    /// <param name="nodesToSearch">Коллекция объектов для выбора ближайшей ноды</param>
    /// <param name="targetNode">Целевая нода относительно которой ищется нода из коллекции</param>
    /// <returns></returns>
    private GameObject FindClosestNode(List<GameObject> nodesToSearch, BattlegroundGridNode targetNode)
    {
        if (nodesToSearch != null)
        {
            int minDistanceIndex = 0;
            float distance = Vector3.Distance(nodesToSearch[0].transform.position, targetNode.WorldPosition);

            for (int i = 0; i < nodesToSearch.Count; i++)
            {
                if (distance > Vector3.Distance(nodesToSearch[i].transform.position, targetNode.WorldPosition))
                {
                    distance = Vector3.Distance(nodesToSearch[i].transform.position, targetNode.WorldPosition);
                    minDistanceIndex = i;
                }
            }

            return nodesToSearch[minDistanceIndex];
        }

        return null;
    }
}
