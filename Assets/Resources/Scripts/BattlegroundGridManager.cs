// Roman Baranov 06.03.2021
using UnityEngine;

public class BattlegroundGridManager : MonoBehaviour
{
    // Размер сетки
    [SerializeField] private int _gridHeigh = 8;
    [SerializeField] private int _gridWidth = 8;

    [SerializeField] private GameObject _gridParentObject = null;// Родительский объект для создания нод сетки

    private GameObject _nodeTilePrefab = null;// Префаб тайла ноды для отображения ее статуса

    private BattlegroundGridNode[,] _grid; // Контейнер для хранения нод сетки

    private void Awake()
    {
        _nodeTilePrefab = Resources.Load("Prefabs/NodeTile") as GameObject;
        CreateGrid();
    }

    /// <summary>
    /// Ищет ноду по мировым координатам и возвращает ее
    /// </summary>
    /// <param name="targetWorldPosition">Координаты точки для поиска ноды</param>
    /// <returns>BattlegroundGridNode</returns>
    public BattlegroundGridNode GetNodeByWorldPosition(Vector3 targetWorldPosition)
    {
        for (int i = 0; i < _gridWidth; i++)
        {
            for (int j = 0; j < _gridHeigh; j++)
            {
                if (_grid[i, j].WorldPosition == targetWorldPosition)
                {
                    return _grid[i, j];
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Создает сетку с нодами в указанном родительском объекте
    /// </summary>
    private void CreateGrid()
    {
        _grid = new BattlegroundGridNode[_gridWidth, _gridHeigh];

        if (_nodeTilePrefab && _gridParentObject)
        {
            for (int i = 0; i < _gridWidth; i++)
            {
                for (int j = 0; j < _gridHeigh; j++)
                {
                    GameObject tile = Instantiate(_nodeTilePrefab, new Vector3(i, 0, j), Quaternion.Euler(90, 0, 0), _gridParentObject.transform);
                    _grid[i, j] = new BattlegroundGridNode(i, 0, j, true, false, tile);
                }
            }
        }
        else
        {
            Debug.Log($"{gameObject.name} - NodeTilePrefab and gridParentObject are missing!");
        }
       
    }

}
