using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMouseInput : MonoBehaviour
{
    private GameObject _selectedObject = null;

    private BattlegroundGridManager _battlegroundGridManager = null;

    // Start is called before the first frame update
    private void Awake()
    {
        _battlegroundGridManager = FindObjectOfType<BattlegroundGridManager>();
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
    
    private void SelectUnit()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 10f))
        {
            BattlegroundGridNode targetNode = _battlegroundGridManager.GetNodeByWorldPosition(hit.transform.position);

            if (hit.transform.GetComponent<Unit>() && _selectedObject == null)
            {
                _selectedObject = hit.transform.gameObject;
                _selectedObject.GetComponent<GridTileHighlight>().ShowMoveTiles(true);
                hit.transform.GetComponent<Unit>().isSelected = true;
            }
            else if (targetNode.isOccupied == false && targetNode.isWalkable && _selectedObject)
            {
                BattlegroundGridNode unitNode = _battlegroundGridManager.GetNodeByWorldPosition(_selectedObject.transform.position);
                unitNode.isOccupied = false;
                _selectedObject.GetComponent<GridTileHighlight>().ShowMoveTiles(false);
                _selectedObject.GetComponent<UnitMovement>().MoveUnitToTarget(_selectedObject.transform.position, hit.transform.position);
                _selectedObject.GetComponent<Unit>().isSelected = false;
                _selectedObject = null;
                Debug.Log($"Unit {hit.transform.name} new move poin is {hit.transform.position}");
            }
        }
    }

    private void UnselectUnit()
    {
        if (_selectedObject != null && _selectedObject.GetComponent<Unit>())
        {
            _selectedObject.GetComponent<GridTileHighlight>().ShowMoveTiles(false);
            _selectedObject.GetComponent<Unit>().isSelected = false;
        }
        _selectedObject = null;
    }
}
