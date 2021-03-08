// Roman Baranov 06.03.2021
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool isSelected;// Выбран ли в данный момент юнит
    public bool isMovesLeft;// Может ли юнит еще передвигаться
    public bool isUnitDead;// Погиб ли юнит

    public BattlegroundGridNode _occupiedNode = null;// Нода, на которой находится юнит

    /// <summary>
    /// Сторона, к которой принадлежит юнит
    /// </summary>
    public enum UnitSide
    {
        Green,
        Brown
    }

    public UnitSide _unitSide;// Сторона конфликта юнита

    private int _unitMoveDistance = 2;// Максимальное количество ходов юнита в клетках
    /// <summary>
    /// Максимальное количество ходов юнита в клетках
    /// </summary>
    public int UnitMoveDistance { get { return _unitMoveDistance; } }

    private float _unitMaxHealth = 100f;// Максимльное здоровье юнита
    private float _unitCurrentHealth;// Текущее здоровье юнита
    
    private float _unitAttack = 25f;// Урон юнита

    // TO DO! урон от атаки на 33% слабее, если бъет по диагонали
    /// <summary>
    /// Атакует выбранный вражеский юнит
    /// </summary>
    /// <param name="enemyUnit"></param>
    public void Attack(Unit enemyUnit)
    {
        if (_unitSide != enemyUnit._unitSide)
        {
            if (enemyUnit._unitCurrentHealth - _unitAttack > 0.0001f)
            {
                enemyUnit._unitCurrentHealth -= _unitAttack;
                Debug.Log($"{gameObject.name} damage {enemyUnit.gameObject.name} on {_unitAttack}!");
            }
            else
            {
                enemyUnit._unitCurrentHealth = 0;
                Debug.Log($"{gameObject.name} delivers a deadly blow to the {enemyUnit.gameObject.name}!");
                enemyUnit.UnitDead();
            }
            
        }
    }
    // TO DO!
    /// <summary>
    /// Переворачивает юнита после смерти
    /// </summary>
    private void UnitDead()
    {
        Debug.Log($"{gameObject.name} is dead!");
        gameObject.SetActive(false);
        // Отключаем юнита и переворачиваем его
    }

}
