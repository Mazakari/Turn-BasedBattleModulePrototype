// Roman Baranov 07.03.2021
using UnityEngine;

public class GridTileHighlight : MonoBehaviour
{
    private MeshRenderer _meshRenderer = null;// Ссылка на компонент MeshRenderer обекта
    private Color _defaultColor;// Цвет объекта в момент наведения курсора мыши на него
    private Color _mousoverColor;// Цвет объекта при наведения курсора мыши на него

    private Vector3 _unitStartMovePosition;// Начальная точка движения юнита

    private Unit _unit = null;// Ссылка на компонент Unit

    private BattlegroundGridManager _battlegroundGridManager = null;// Ссылка на класс BattlegroundGridManager

    private UnitPathfinding _unitPathfinding = null;// Ссылка на UnitPathfinding

    private void Awake()
    {
        // Обрабатываем ситуацию инициализации юнита
        if (GetComponent<Unit>())
        {
            _unitPathfinding = GetComponent<UnitPathfinding>();
            _unit = GetComponent<Unit>();
            _battlegroundGridManager = FindObjectOfType<BattlegroundGridManager>();

            _unitStartMovePosition = gameObject.transform.position;
        }

        _meshRenderer = GetComponent<MeshRenderer>();

        _defaultColor = _meshRenderer.material.color;
        _mousoverColor = Color.yellow;
    }

    // Меняем цвет объекта при наведении мыши на новый
    void OnMouseEnter()
    {
        _defaultColor = _meshRenderer.material.color;
        _meshRenderer.material.color = _mousoverColor;
    }

    // Меняем цвет объекта на предыдущий, когда курсор мыши больше не находится на нем
    void OnMouseExit()
    {
        _meshRenderer.material.color = _defaultColor;
    }

    /// <summary>
    /// Отображает дистанцию для передвижения юнита
    /// </summary>
    /// <param name="showColoredTiles">Отображать ли доступные ходы юнита</param>
    public void ShowMoveTiles(bool showColoredTiles)
    {
        if (showColoredTiles)
        {
            _unitPathfinding.CheckTiles(_battlegroundGridManager.GetNodeByWorldPosition(gameObject.transform.position), _unit.UnitMoveDistance);
            ColorTiles(true);
        }
        else
        {
            ColorTiles(false);
        }
    }

    /// <summary>
    /// Окрашивает найденные вокруг юнита ноды в соответствующий цвет
    /// </summary>
    private void ColorTiles(bool showColoredTiles)
    {
        BattlegroundGridNode node = null;

        if (showColoredTiles)
        {
            if (_unitPathfinding.MovementTiles != null)
            {
                for (int i = 0; i < _unitPathfinding.MovementTiles.Count; i++)
                {
                    node = _battlegroundGridManager.GetNodeByWorldPosition(_unitPathfinding.MovementTiles[i].transform.position);

                    // Если нода свободна для передвижения и в ней нет юнита
                    if (node.isWalkable && node.isOccupied == false)
                    {
                        _unitPathfinding.MovementTiles[i].GetComponent<MeshRenderer>().material.color = Color.green;
                    }
                    // Если 
                    else if (node.isWalkable == false && node.isOccupied == false && node.occupiedByUnit.GetComponent<Unit>().unitSide == _unit.unitSide)
                    {
                        _unitPathfinding.MovementTiles[i].GetComponent<MeshRenderer>().material.color = Color.white;
                    }
                    // Если нода занята и занята врагом, то красим ее в красный цвет
                    else if (node.isWalkable == false && node.isOccupied && node.occupiedByUnit.GetComponent<Unit>().unitSide != _unit.unitSide)
                    {
                        _unitPathfinding.MovementTiles[i].GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                }
            }
            
        }
        else
        {
            if (_unitPathfinding.MovementTiles != null)
            {
                for (int i = 0; i < _unitPathfinding.MovementTiles.Count; i++)
                {
                    node = _battlegroundGridManager.GetNodeByWorldPosition(_unitPathfinding.MovementTiles[i].transform.position);
                    _unitPathfinding.MovementTiles[i].GetComponent<MeshRenderer>().material.color = Color.white;
                }
                _unitPathfinding.MovementTiles.Clear();
            }
            
        }
    }
}
