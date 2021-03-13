// Roman Baranov 08.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    private float _unitMoveSpeed = 3f;// Скорость перемещения юнита от тайла к тайлу

    private Coroutine _moveCoroutine = null;// Курутина для перемещения юнита

    private BattlegroundGridManager _battlegroundGridManager = null;

    private UnitsManager _unitsManager = null;

    private UnitPathfinding _unitPathfinding = null;

    private Unit _unit = null;

    private void Awake()
    {
        _unit = GetComponent<Unit>();
        _battlegroundGridManager = FindObjectOfType<BattlegroundGridManager>();
        _unitsManager = FindObjectOfType<UnitsManager>();
    }

    /// <summary>
    /// Перемещает юнит в доступную точку
    /// </summary>
    /// <param name="startPoint">Начальная точка движения юнита</param>
    /// <param name="endPoint">Конечная точка движения юнита</param>
    public void MoveUnitToTarget(BattlegroundGridNode startNode, BattlegroundGridNode endNode)
    {
        if (!_unitsManager.isUnitMoving && _unit.isMovesLeft)
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }

            _moveCoroutine = StartCoroutine(MoveUnitCoroutine(startNode, endNode));
        }
    }
    /// <summary>
    /// Курутина для перемещения юнита
    /// </summary>
    /// <param name="startPoint">Начальная точка движения юнита</param>
    /// <param name="endPoint">Конечная точка движения юнита</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator MoveUnitCoroutine(BattlegroundGridNode startNode, BattlegroundGridNode endNode)
    {
        _unitPathfinding = GetComponent<UnitPathfinding>();
        List<GameObject> unitMovePath = _unitPathfinding.UnitMovePath;
        BattlegroundGridNode nextNode;

        for (int i = 0; i < unitMovePath.Count; )
        {
            nextNode = _battlegroundGridManager.GetNodeByWorldPosition(unitMovePath[i].transform.position);
            _unitsManager.isUnitMoving = true;
            float lerpAmount = 0f;

            while (Vector3.Distance(gameObject.transform.position, nextNode.WorldPosition) > 0.0001f)
            {
                gameObject.transform.position = Vector3.Lerp(startNode.WorldPosition, nextNode.WorldPosition, lerpAmount);
                lerpAmount += _unitMoveSpeed * Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
            startNode = nextNode;
            i++;
        }

        // Debug
        endNode.isOccupied = true;
        endNode.isWalkable = false;
        endNode.NodeTile.GetComponent<MeshRenderer>().material.color = Color.white;// Костылик
        _unit.isMovesLeft = false;
        _unitsManager.isUnitMoving = false;
        yield return null;
    }
}
