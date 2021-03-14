// Roman Baranov 06.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public enum Turn
    {
        GreenTurn,
        BrownTurn
    }
    public Turn currentTurn;// Текущий ход сторон в бою

    /// <summary>
    /// Список для хранения состояний игры
    /// </summary>
    public enum GameState
    {
        GameInProgress,
        GreeWwon,
        BrownWon

    }
    public GameState currentGameState;// Текущее состояние игры

    private int _currentTurnCounter = 1;
    public int CurrentTurnNumber { get { return _currentTurnCounter; } set { _currentTurnCounter = value; } }

    private UnitsManager _unitsManager = null;

    private CombatGuiManager _combatGuiManager = null;

    private BattleMouseInput _battleMouseInput = null;

    private BattlegroundGridManager _battlegroundGridManager = null;

    private void Awake()
    {
        _unitsManager = FindObjectOfType<UnitsManager>();
        _combatGuiManager = FindObjectOfType<CombatGuiManager>();
        _battleMouseInput = FindObjectOfType<BattleMouseInput>();
        _battlegroundGridManager = FindObjectOfType<BattlegroundGridManager>();

        currentTurn = Turn.GreenTurn;// TO DO Можно рандомизировать первый ход
        currentGameState = GameState.GameInProgress;
    }

    /// <summary>
    /// Обновляет данные юнитов в начале хода
    /// </summary>
    public void EndTurn()
    {
        _battleMouseInput.UnselectUnit();
        _currentTurnCounter++;
        

        SwitchCurrentSide();
        _battlegroundGridManager.ResetWalkableNodes();// Костылик для фикса атаки юнита вне диапазона передвижения

        _combatGuiManager.SwitchCurrentSideTurnText();
        _combatGuiManager.UpdateTurnNumberText(); 
    }

    /// <summary>
    /// Проверяет все ли юниты указанной стороны походили и автоматически завершает ее ход, если да
    /// </summary>
    /// <param name="unitSide">Юнит текущей ходящей стороны</param>
    public void CheckEndTurn(Unit.UnitSide unitSide)
    {
        if (currentTurn == CombatManager.Turn.GreenTurn && unitSide == Unit.UnitSide.Green)
        {
            for (int i = 0; i < _unitsManager.GreenUnits.Count; i++)
            {
                if (_unitsManager.GreenUnits[i].GetComponent<Unit>().isMovesLeft)
                {
                    // Выбирать следующий юнит
                    _battleMouseInput.AutoSelectUnit(_unitsManager.GreenUnits[i]);
                    return;
                }
            }

            EndTurn();
        }

        if (currentTurn == CombatManager.Turn.BrownTurn && unitSide == Unit.UnitSide.Brown)
        {
            for (int i = 0; i < _unitsManager.BrownUnits.Count; i++)
            {
                if (_unitsManager.BrownUnits[i].GetComponent<Unit>().isMovesLeft)
                {
                    // Выбирать следующий юнит
                    _battleMouseInput.AutoSelectUnit(_unitsManager.BrownUnits[i]);
                    return;
                }
            }

            EndTurn();
        }
    }

    /// <summary>
    /// Меняет текущий ход стороны конфликта
    /// </summary>
    private void SwitchCurrentSide()
    {
        if (currentTurn == CombatManager.Turn.GreenTurn)
        {
            currentTurn = CombatManager.Turn.BrownTurn;
        }
        else if(currentTurn == CombatManager.Turn.BrownTurn)
        {
            currentTurn = CombatManager.Turn.GreenTurn;
        }

        SwitchMovesLeft(currentTurn);
    }

    /// <summary>
    /// Активирует ходы для противоположной стороны
    /// </summary>
    /// <param name="currentTurn">Текущий ход</param>
    private void SwitchMovesLeft(Turn currentTurn)
    {
        if (currentTurn == CombatManager.Turn.GreenTurn)
        {
            for (int i = 0; i < _unitsManager.BrownUnits.Count; i++)
            {
                _unitsManager.GreenUnits[i].GetComponent<Unit>().isMovesLeft = true;
                _unitsManager.BrownUnits[i].GetComponent<Unit>().isMovesLeft = false;
            }
        }
        else if (currentTurn == CombatManager.Turn.BrownTurn)
        {
            for (int i = 0; i < _unitsManager.GreenUnits.Count; i++)
            {
                _unitsManager.GreenUnits[i].GetComponent<Unit>().isMovesLeft = false;
                _unitsManager.BrownUnits[i].GetComponent<Unit>().isMovesLeft = true;
            }
        }
    }
}
