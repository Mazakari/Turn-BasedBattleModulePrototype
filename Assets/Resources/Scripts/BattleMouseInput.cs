// Roman Baranov 06.03.2021
using UnityEngine;

public class BattleMouseInput : MonoBehaviour
{
    private GameObject _selectedObject = null;

    private BattlegroundGridManager _battlegroundGridManager = null;

    private UnitsManager _unitsManager = null;

    private UnitPathfinding _unitPathfinding = null;

    // Start is called before the first frame update
    private void Awake()
    {
        _battlegroundGridManager = FindObjectOfType<BattlegroundGridManager>();
        _unitsManager = FindObjectOfType<UnitsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        MouseInput();
    }

    /// <summary>
    /// Контролирует нажатия кнопок мыши и выбора объектов на поле
    /// </summary>
    private void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectUnit();
        }

        if (Input.GetMouseButtonDown(1))// TO DO NullRefefence
        {
            UnselectUnit();
        }
    }

    /// <summary>
    /// Снимает выделение с выбранного в данный момент юнита
    /// </summary>
    public void UnselectUnit()
    {
        if (_selectedObject != null && _selectedObject.GetComponent<Unit>())
        {
            _selectedObject.GetComponent<GridTileHighlight>().ShowMoveTiles(false);
            _selectedObject.GetComponent<Unit>().isSelected = false;
        }
        _selectedObject = null;
    }

    /// <summary>
    /// Автоматически выбирает следующий доступный юнит
    /// </summary>
    /// <param name="selectedObject"></param>
    public void AutoSelectUnit(GameObject selectedObject)
    {
        _selectedObject = selectedObject.transform.gameObject;
        _selectedObject.GetComponent<Unit>().isSelected = true;
        _selectedObject.GetComponent<GridTileHighlight>().ShowMoveTiles(true);
    }

    /// <summary>
    /// Обрабатывает выбор юнита и точку его передвижения
    /// </summary>
    private void SelectUnit()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 10f))
        {
            //Debug.Log($"Mouse clock point {hit.transform.position}");
            BattlegroundGridNode targetNode = _battlegroundGridManager.GetNodeByWorldPosition(hit.transform.position);
            //Debug.Log($"targetNode.isWalkable = {targetNode.isWalkable}");
            // Выбираем юнит
            if (hit.transform.GetComponent<Unit>() && _selectedObject == null && !_unitsManager.isUnitMoving && hit.transform.GetComponent<Unit>().isMovesLeft)
            {
                _selectedObject = hit.transform.gameObject;
                _selectedObject.GetComponent<Unit>().isSelected = true;
                _selectedObject.GetComponent<GridTileHighlight>().ShowMoveTiles(true);
            }
            // Выбираем тайл для перемещения юнита
            else if (!targetNode.isOccupied && targetNode.isWalkable && _selectedObject)
            {
                BattlegroundGridNode unitNode = _battlegroundGridManager.GetNodeByWorldPosition(_selectedObject.transform.position);
                _unitPathfinding = _selectedObject.GetComponent<UnitPathfinding>();

                unitNode.isOccupied = false;
                _selectedObject.GetComponent<Unit>().isSelected = false;
                //Получаем путь с клетками для юнита 
                _unitPathfinding.Pathfinding(unitNode, targetNode);
                _selectedObject.GetComponent<UnitMovement>().MoveUnitToTarget(unitNode, targetNode);
                _selectedObject.GetComponent<GridTileHighlight>().ShowMoveTiles(false);
                _selectedObject = null;
            }
            // Выбираем юнит и атакуем, если он вражеский
            else if (targetNode.isOccupied && targetNode.isWalkable && _selectedObject && hit.transform.gameObject.GetComponent<Unit>())
            {
                Unit enemyUnit = hit.transform.gameObject.GetComponent<Unit>();
                Unit unit = _selectedObject.GetComponent<Unit>();
                Debug.Log($"unit.unitSide = {unit.unitSide}");
                Debug.Log($"enemyUnit.unitSide = {enemyUnit.unitSide}");

                if (unit.unitSide != enemyUnit.unitSide)
                {
                    // TO DO
                    // Ищем путь к ноде вражеского юнита
                    // Двигаемся в ноду с вражеским юнитом
                    // Атакуем вражеский юнит
                    // Возвращаем юнит на предыдущую клетку

                    //_unitPathfinding.Pathfinding(unit.occupiedNode, enemyUnit.occupiedNode);
                    //_selectedObject.GetComponent<UnitMovement>().MoveUnitToTarget(unitNode, targetNode);
                    //_selectedObject.GetComponent<GridTileHighlight>().ShowMoveTiles(false);
                    //_selectedObject = null;
                    Debug.Log($"Attack = {unit.UnitAttack}");
                    Debug.Log($"Enemy Health = {enemyUnit.UnitCurrentHealth}");
                    unit.Attack(enemyUnit);
                    UnselectUnit();
                }
            }
        }
    }
}
