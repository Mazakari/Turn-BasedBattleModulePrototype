using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPathfinding : MonoBehaviour
{
    private Vector3 _unitStartMovePosition;// Начальная точка движения юнита
    private Vector3 _unitEndMovePosition;// Конечная точка движения юнита

    private List<GameObject> _movementTiles = null;// Коллекция тайлов для отображения радиуса передвижения юнита
    /// <summary>
    /// Коллекция тайлов для отображения радиуса передвижения юнита
    /// </summary>
    public List<GameObject> MovementTiles { get { return _movementTiles; } }

    private BattlegroundGridManager _battlegroundGridManager = null;// Ссылка на BattlegroundGridManager

    private void Awake()
    {
        _movementTiles = new List<GameObject>();
        _battlegroundGridManager = FindObjectOfType<BattlegroundGridManager>();
    }

    /// <summary>
    /// Проверяет ноды вокруг позиции юнита на проходимость
    /// </summary>
    /// <param name="startNode">Исходная точка юнита</param>
    /// <param name="moveRadius">Радиус передвижения юнита в клетках</param>
    public void CheckTiles(BattlegroundGridNode startNode, int moveRadius)
    {
        CheckTop(startNode, moveRadius);
        CheckBottom(startNode, moveRadius);
        CheckLeft(startNode, moveRadius);
        CheckRight(startNode, moveRadius);
    }

    /// <summary>
    /// Проверяет ноды сверху от позиции юнита на проходимость
    /// </summary>
    /// <param name="startNode">Исходная точка юнита</param>
    /// <param name="moveRadius">Радиус передвижения юнита в клетках</param>
    private void CheckTop(BattlegroundGridNode startNode, int moveRadius)
    {
        BattlegroundGridNode node = null;

        for (int i = 0; i < moveRadius; i++)
        {
            if (_battlegroundGridManager.GetNodeByWorldPosition(new Vector3(startNode.WorldPosition.x, startNode.WorldPosition.y, startNode.WorldPosition.z + (i + 1))) != null)
            {
                node = _battlegroundGridManager.GetNodeByWorldPosition(new Vector3(startNode.WorldPosition.x, startNode.WorldPosition.y, startNode.WorldPosition.z + (i + 1)));
                node.isWalkable = true;
                _movementTiles.Add(node.NodeTile);
            }
        }
    }

    /// <summary>
    /// Проверяет ноды снизу от позиции юнита на проходимость
    /// </summary>
    /// <param name="startNode">Исходная точка юнита</param>
    /// <param name="moveRadius">Радиус передвижения юнита в клетках</param>
    private void CheckBottom(BattlegroundGridNode startNode, int moveRadius)
    {
        BattlegroundGridNode node = null;

        for (int i = 0; i < moveRadius; i++)
        {
            if (_battlegroundGridManager.GetNodeByWorldPosition(new Vector3(startNode.WorldPosition.x, startNode.WorldPosition.y, startNode.WorldPosition.z - (i + 1))) != null)
            {
                node = _battlegroundGridManager.GetNodeByWorldPosition(new Vector3(startNode.WorldPosition.x, startNode.WorldPosition.y, startNode.WorldPosition.z - (i + 1)));
                node.isWalkable = true;
                _movementTiles.Add(node.NodeTile);
            }
        }
    }

    /// <summary>
    /// Проверяет ноды слева от позиции юнита на проходимость
    /// </summary>
    /// <param name="startNode">Исходная точка юнита</param>
    /// <param name="moveRadius">Радиус передвижения юнита в клетках</param>
    private void CheckLeft(BattlegroundGridNode startNode, int moveRadius)
    {
        BattlegroundGridNode node = null;

        for (int i = 0; i < moveRadius; i++)
        {
            if (_battlegroundGridManager.GetNodeByWorldPosition(new Vector3(startNode.WorldPosition.x - (i + 1), startNode.WorldPosition.y, startNode.WorldPosition.z)) != null)
            {
                node = _battlegroundGridManager.GetNodeByWorldPosition(new Vector3(startNode.WorldPosition.x - (i + 1), startNode.WorldPosition.y, startNode.WorldPosition.z));
                node.isWalkable = true;
                _movementTiles.Add(node.NodeTile);
                CheckTop(node, moveRadius);
                CheckBottom(node, moveRadius);
            }
        }
    }

    /// <summary>
    /// Проверяет ноды справа от позиции юнита на проходимость
    /// </summary>
    /// <param name="startNode">Исходная точка юнита</param>
    /// <param name="moveRadius">Радиус передвижения юнита в клетках</param>
    private void CheckRight(BattlegroundGridNode startNode, int moveRadius)
    {
        BattlegroundGridNode node = null;

        for (int i = 0; i < moveRadius; i++)
        {
            if (_battlegroundGridManager.GetNodeByWorldPosition(new Vector3(startNode.WorldPosition.x + (i + 1), startNode.WorldPosition.y, startNode.WorldPosition.z)) != null)
            {
                node = _battlegroundGridManager.GetNodeByWorldPosition(new Vector3(startNode.WorldPosition.x + (i + 1), startNode.WorldPosition.y, startNode.WorldPosition.z));
                node.isWalkable = true;
                _movementTiles.Add(node.NodeTile);
                CheckTop(node, moveRadius);
                CheckBottom(node, moveRadius);
            }
        }
    }
}
