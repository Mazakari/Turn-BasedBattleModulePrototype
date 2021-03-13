// Roman Baranov 07.03.2021
using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    public bool isUnitMoving = false;
    // Родительские объекты для юнитов на сцене 
    [SerializeField] private GameObject _greenUnitsSpawnObject = null;
    [SerializeField] private GameObject _brownUnitsSpawnObject = null;

    private int _greenUnitsAmount = 4;
    public int GreenUnitsAmount { get { return _greenUnitsAmount; } }

    private int _brownUnitsAmount = 4;
    public int BrownUnitsAmount { get { return _brownUnitsAmount; } }

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
        BattlegroundGridNode node = null;

        // Хардкод спавна юнитов слева
        if (isSpawningLeft)
        {
            int zValue = 0;

            for (int i = 0; i < _greenUnitsAmount; i++)
            {
                units.Add(Instantiate(unitPrefab, new Vector3(0, 0, zValue), Quaternion.identity, parentObject.transform));
                units[i].GetComponent<Unit>()._occupiedNode = _battlegroundGridManager.GetNodeByWorldPosition(units[i].transform.position);
                units[i].GetComponent<Unit>().isMovesLeft = true;
                node = _battlegroundGridManager.GetNodeByWorldPosition(units[i].transform.position);
                node.occupiedByUnit = units[i];
                node.isOccupied = true;
                node.isWalkable = false;

                zValue += 2;
            }
        }
        // Хардкод спавна юнитов справа
        else
        {
            int xValue = 7;
            int zValue = 0;

            for (int i = 0; i < _brownUnitsAmount; i++)
            {
                units.Add(Instantiate(unitPrefab, new Vector3(xValue, 0, zValue), Quaternion.identity, parentObject.transform));
                units[i].GetComponent<Unit>()._occupiedNode = _battlegroundGridManager.GetNodeByWorldPosition(units[i].transform.position);
                units[i].GetComponent<Unit>().isMovesLeft = false;
                node = _battlegroundGridManager.GetNodeByWorldPosition(units[i].transform.position);
                node.occupiedByUnit = units[i];
                node.isOccupied = true;
                node.isWalkable = false;

                zValue += 2;
            }
        }
    }
}
