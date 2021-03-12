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
    /// Выбирает юнит, на который наведен курсор мыши, либо тайл, в который нужно двигаться выбранному юниту
    /// </summary>
    private void SelectUnit()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 10f))
        {
            Debug.Log($"Mouse clock point {hit.transform.position}");
            BattlegroundGridNode targetNode = _battlegroundGridManager.GetNodeByWorldPosition(hit.transform.position);
            // Выбираем юнит
            if (hit.transform.GetComponent<Unit>() && _selectedObject == null && _unitsManager.isUnitMoving == false && hit.transform.GetComponent<Unit>().isMovesLeft)
            {
                _selectedObject = hit.transform.gameObject;
                hit.transform.GetComponent<Unit>().isSelected = true;
                _selectedObject.GetComponent<GridTileHighlight>().ShowMoveTiles(true);
            }
            // Выбираем тайл для перемещения юнита
            else if (targetNode.isOccupied == false && targetNode.isWalkable && _selectedObject)
            {
                BattlegroundGridNode unitNode = _battlegroundGridManager.GetNodeByWorldPosition(_selectedObject.transform.position);
                targetNode = _battlegroundGridManager.GetNodeByWorldPosition(hit.transform.position);
                int moveDistance = _selectedObject.GetComponent<Unit>().UnitMoveDistance;
                _unitPathfinding = _selectedObject.GetComponent<UnitPathfinding>();

                unitNode.isOccupied = false;
                _selectedObject.GetComponent<Unit>().isSelected = false;
                //Получаем путь с клетками для юнита 
                _unitPathfinding.Pathfinding(unitNode, targetNode, moveDistance);
                Debug.Log($"UnitMovePath = {_unitPathfinding.UnitMovePath}");
                Debug.Log($"UnitMovePath.Count = {_unitPathfinding.UnitMovePath.Count}");
                _selectedObject.GetComponent<UnitMovement>().MoveUnitToTarget(_selectedObject.transform.position, hit.transform.position);
                _selectedObject.GetComponent<GridTileHighlight>().ShowMoveTiles(false);
                _selectedObject = null;
            }
        }
    }
}
