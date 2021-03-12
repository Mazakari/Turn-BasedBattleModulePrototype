// Roman Baranov 08.03.2021
using System.Collections;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    private float _unitMoveSpeed = 1f;// Скорость перемещения юнита от тайла к тайлу

    private int _currentUnitMoveDistance;

    private Coroutine _moveCoroutine = null;// Курутина для перемещения юнита

    private BattlegroundGridManager _battlegroundGridManager = null;

    private UnitsManager _unitsManager = null;

    private Unit _unit = null;

    private void Awake()
    {
        _unit = GetComponent<Unit>();
        _currentUnitMoveDistance = _unit.UnitMoveDistance;
        _battlegroundGridManager = FindObjectOfType<BattlegroundGridManager>();
        _unitsManager = FindObjectOfType<UnitsManager>();
    }

    /// <summary>
    /// Перемещает юнит в доступную точку
    /// </summary>
    /// <param name="startPoint">Начальная точка движения юнита</param>
    /// <param name="endPoint">Конечная точка движения юнита</param>
    public void MoveUnitToTarget(Vector3 startPoint, Vector3 endPoint)
    {
        if (!_unitsManager.isUnitMoving && _unit.isMovesLeft)
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }

            _moveCoroutine = StartCoroutine(MoveUnitCoroutine(startPoint, endPoint));
        }
    }
    /// <summary>
    /// Курутина для перемещения юнита
    /// </summary>
    /// <param name="startPoint">Начальная точка движения юнита</param>
    /// <param name="endPoint">Конечная точка движения юнита</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator MoveUnitCoroutine(Vector3 startPoint, Vector3 endPoint)
    {
        BattlegroundGridNode targetNode = _battlegroundGridManager.GetNodeByWorldPosition(endPoint);
        _unitsManager.isUnitMoving = true;
        float lerpAmount = 0f;

        while(Vector3.Distance(gameObject.transform.position, endPoint) > 0.0001f)
        {
            gameObject.transform.position = Vector3.Lerp(startPoint, endPoint, lerpAmount);
            lerpAmount += _unitMoveSpeed * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        // Debug
        targetNode.isOccupied = true;
        _currentUnitMoveDistance = 0;
        _unit.isMovesLeft = false;
        _unitsManager.isUnitMoving = false;
        yield return null;
    }
}
