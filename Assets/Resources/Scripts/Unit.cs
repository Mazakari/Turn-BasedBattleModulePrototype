// Roman Baranov 06.03.2021
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool isSelected;// Выбран ли в данный момент юнит
    public bool isMovesLeft;// Может ли юнит еще передвигаться
    public bool isUnitDead;// Погиб ли юнит

    public BattlegroundGridNode occupiedNode = null;// Нода, на которой находится юнит

    /// <summary>
    /// Сторона, к которой принадлежит юнит
    /// </summary>
    public enum UnitSide
    {
        Green,
        Brown
    }

    public UnitSide unitSide;// Сторона конфликта юнита

    private int _unitMoveDistance = 2;// Максимальное количество ходов юнита в клетках
    /// <summary>
    /// Максимальное количество ходов юнита в клетках
    /// </summary>
    public int UnitMoveDistance { get { return _unitMoveDistance; } }

    private float _unitMaxHealth = 100f;// Максимльное здоровье юнита
    public float UnitMaxHealth { get { return _unitMaxHealth; } }

    private float _unitCurrentHealth;// Текущее здоровье юнита
    public float UnitCurrentHealth { get { return _unitCurrentHealth; } }

    private float _unitAttack = 25f;// Урон юнита
    public float UnitAttack { get { return _unitAttack; } }

    private CombatManager _combatManager = null;

    private GridTileHighlight _gridTileHighlight = null;


    private void Awake()
    {
        _unitCurrentHealth = _unitMaxHealth;
        _combatManager = FindObjectOfType<CombatManager>();
        _gridTileHighlight = GetComponent<GridTileHighlight>();
    }

    // TO DO! урон от атаки на 33% слабее, если бъет по диагонали
    /// <summary>
    /// Атакует выбранный вражеский юнит
    /// </summary>
    /// <param name="enemyUnit"></param>
    public void Attack(Unit enemyUnit)
    {
        if (unitSide != enemyUnit.unitSide)
        {
            if (enemyUnit._unitCurrentHealth - _unitAttack > 0.0001f)
            {
                enemyUnit._unitCurrentHealth -= _unitAttack;

                enemyUnit.gameObject.GetComponentInChildren<HealthBar>().UpdateHealhBar(enemyUnit._unitCurrentHealth);
                Debug.Log($"{gameObject.name} damage {enemyUnit.gameObject.name} on {_unitAttack}!");
            }
            else
            {
                enemyUnit._unitCurrentHealth = 0;
                enemyUnit.gameObject.GetComponentInChildren<HealthBar>().UpdateHealhBar(enemyUnit._unitCurrentHealth);
                Debug.Log($"{gameObject.name} delivers a deadly blow to the {enemyUnit.gameObject.name}!");
                enemyUnit.UnitDead(enemyUnit);
            }

            isMovesLeft = false;
            _gridTileHighlight.ShowMoveTiles(false);
            _combatManager.CheckEndTurn(unitSide);
        }
    }
    // TO DO!
    /// <summary>
    /// Переворачивает юнита после смерти
    /// </summary>
    private void UnitDead(Unit enemyUnit)
    {
        Debug.Log($"{gameObject.name} is dead!");

        enemyUnit.occupiedNode.isOccupied = false;
        enemyUnit.isUnitDead = true;
        enemyUnit.isMovesLeft = false;
        enemyUnit.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
        enemyUnit.gameObject.GetComponent<BoxCollider>().enabled = false;
        // Node.isOcupied = false
        // Переворачиваем юнит
    }

}
