// Roman Baranov 07.03.2021
using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    // Родительские объекты для юнитов на сцене 
    [SerializeField] private GameObject _greenUnitsSpawnObject = null;
    [SerializeField] private GameObject _brownUnitsSpawnObject = null;

    // Префабы для юнитов
    private GameObject _greenUnitPrefab = null;
    private GameObject _brownUnitPrefab = null;

    private BattlegroundGridManager _battlegroundGridManager = null;// Ссылка на BattlegroundGridManager

    private List<GameObject> _greenUnits;// Коллекция зеленыех юнитов
    /// <summary>
    /// Возвращает ссылку на коллекцию зеленых юнитов
    /// </summary>
    public List<GameObject> GreenUnits { get { return _greenUnits; } }


    private List<GameObject> _brownUnits;// Коллекция коричневых юнитов
    /// <summary>
    /// Возвращает ссылку на коллекцию коричневых юнитов
    /// </summary>
    public List<GameObject> BrownUnits { get { return _brownUnits; } }

    // Start is called before the first frame update
    private void Awake()
    {
        _battlegroundGridManager = FindObjectOfType<BattlegroundGridManager>();// Получаем ссылку на BattlegroundGridManager

        // Загружаем префабы юнитов
        _greenUnitPrefab = Resources.Load("Prefabs/GreenUnit") as GameObject;
        _brownUnitPrefab = Resources.Load("Prefabs/BrownUnit") as GameObject;

        _greenUnits = new List<GameObject>();
        _brownUnits = new List<GameObject>();
    }

    private void Start()
    {
        SpawnUnits(_greenUnits, _greenUnitPrefab, _greenUnitsSpawnObject.transform,true);
        SpawnUnits(_brownUnits, _brownUnitPrefab, _brownUnitsSpawnObject.transform, false);
    }

    /// <summary>
    /// Спавнит зеленых юнитов слева или коричневых справа и заполняет соответствующую коллекцию
    /// </summary>
    /// <param name="units">Коллекция юнитов для заполнения</param>
    /// <param name="unitPrefab">Префаб юнита для спавна</param>
    /// <param name="parentObject">Родительский объект для заспавненных юнитов</param>
    /// <param name="isSpawningLeft">С какой стороны поля спавнить юниты</param>
    private void SpawnUnits(List<GameObject> units, GameObject unitPrefab, Transform parentObject, bool isSpawningLeft)
    {
        // Хардкод спавна юнитов слева
        if (isSpawningLeft)
        {
            units.Add(Instantiate(unitPrefab, new Vector3(0, 0, 0), Quaternion.identity, parentObject.transform));
            units[0].GetComponent<Unit>()._occupiedNode = _battlegroundGridManager.GetNodeByWorldPosition(units[0].transform.position);

            units.Add(Instantiate(unitPrefab, new Vector3(0, 0, 2), Quaternion.identity, parentObject.transform));
            units[1].GetComponent<Unit>()._occupiedNode = _battlegroundGridManager.GetNodeByWorldPosition(units[1].transform.position);

            units.Add(Instantiate(unitPrefab, new Vector3(0, 0, 4), Quaternion.identity, parentObject.transform));
            units[2].GetComponent<Unit>()._occupiedNode = _battlegroundGridManager.GetNodeByWorldPosition(units[2].transform.position);

            units.Add(Instantiate(unitPrefab, new Vector3(0, 0, 6), Quaternion.identity, parentObject.transform));
            units[3].GetComponent<Unit>()._occupiedNode = _battlegroundGridManager.GetNodeByWorldPosition(units[3].transform.position);
        }
        // Хардкод спавна юнитов справа
        else
        {
            units.Add(Instantiate(unitPrefab, new Vector3(7, 0, 0), Quaternion.identity, parentObject.transform));
            units[0].GetComponent<Unit>()._occupiedNode = _battlegroundGridManager.GetNodeByWorldPosition(units[0].transform.position);

            units.Add(Instantiate(unitPrefab, new Vector3(7, 0, 2), Quaternion.identity, parentObject.transform));
            units[1].GetComponent<Unit>()._occupiedNode = _battlegroundGridManager.GetNodeByWorldPosition(units[1].transform.position);

            units.Add(Instantiate(unitPrefab, new Vector3(7, 0, 4), Quaternion.identity, parentObject.transform));
            units[2].GetComponent<Unit>()._occupiedNode = _battlegroundGridManager.GetNodeByWorldPosition(units[2].transform.position);

            units.Add(Instantiate(unitPrefab, new Vector3(7, 0, 6), Quaternion.identity, parentObject.transform));
            units[3].GetComponent<Unit>()._occupiedNode = _battlegroundGridManager.GetNodeByWorldPosition(units[3].transform.position);
        }
    }
}
