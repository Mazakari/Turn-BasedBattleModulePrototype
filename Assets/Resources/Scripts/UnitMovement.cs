using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    private float _unitMoveSpeed = 1f;// Скорость перемещения юнита от тайла к тайлу

    private Coroutine _moveCoroutine = null;// Курутина для перемещения юнита

    /// <summary>
    /// Перемещает юнит в доступную точку
    /// </summary>
    /// <param name="startPoint">Начальная точка движения юнита</param>
    /// <param name="endPoint">Конечная точка движения юнита</param>
    public void MoveUnitToTarget(Vector3 startPoint, Vector3 endPoint)
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }

        _moveCoroutine = StartCoroutine(MoveUnitCoroutine(startPoint, endPoint));
    }
    /// <summary>
    /// Курутина для перемещения юнита
    /// </summary>
    /// <param name="startPoint">Начальная точка движения юнита</param>
    /// <param name="endPoint">Конечная точка движения юнита</param>
    /// <returns>IEnumerator</returns>
    private IEnumerator MoveUnitCoroutine(Vector3 startPoint, Vector3 endPoint)
    {
        float lerpAmount = 0f;
        while(Vector3.Distance(gameObject.transform.position, endPoint) > 0.0001f)
        {
            gameObject.transform.position = Vector3.Lerp(startPoint, endPoint, lerpAmount);
            lerpAmount += _unitMoveSpeed * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
