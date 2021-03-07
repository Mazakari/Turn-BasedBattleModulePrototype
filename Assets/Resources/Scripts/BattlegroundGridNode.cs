// Roman Baranov 06.03.2021
using UnityEngine;

public class BattlegroundGridNode
{
    public bool isWalkable;// Возможно ли передвижение по ноде
    public bool isOccupied;// Занята ли нода юнитом

    // Координаты ноды на сетке
    private int _x;
    private int _y;
    private int _z;

    private Vector3 _worldPosition;// Мировые координаты ноды
    public Vector3 WorldPosition { get { return _worldPosition; } }

    private GameObject _nodeTile;// Префаб тайла ноды для отображения ее статуса

    // Конструктор класса ноды
    /// <summary>
    /// Создает экземпляр ноды с заданными параметрами
    /// </summary>
    /// <param name="x">Координата ноды х на сетке</param>
    /// <param name="y">Координата ноды y на сетке</param>
    /// <param name="z">Координата ноды z на сетке</param>
    /// <param name="isWalkable">Возможно ли передвижение по ноде</param>
    /// <param name="isOccupied">Занята ли нода юнитом</param>
    /// /// <param name="nodeTilePrefab">Префаб тайла ноды для отображения ее статуса</param>
    public BattlegroundGridNode (int x, int y, int z, bool isWalkable, bool isOccupied, GameObject nodeTilePrefab)
    {
        _x = x;
        _y = y;
        _z = z;

        this.isWalkable = isWalkable;
        this.isOccupied = isOccupied;

        _worldPosition = new Vector3(_x, _y, _z);

        _nodeTile = nodeTilePrefab;
    }
}
