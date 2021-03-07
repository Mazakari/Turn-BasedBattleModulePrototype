using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMouseInput : MonoBehaviour
{
    private GameObject _selectedObject = null;

    private Vector3 _movePosition;

    // Start is called before the first frame update
    private void Awake()
    {
        _movePosition = new Vector3(0,0,0);
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
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 10f))
            {
                if (hit.transform.GetComponent<Unit>() && _selectedObject == null)
                {
                    _selectedObject = hit.transform.gameObject;
                    hit.transform.GetComponent<Unit>().isSelected = true;
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (_selectedObject.GetComponent<Unit>())
            {
                _selectedObject.GetComponent<Unit>().isSelected = false;
            }
            _selectedObject = null;
        }

    }    

    // TO DO
    public Vector3 GetMovePosition()
    {
        return _movePosition;
    }
}
