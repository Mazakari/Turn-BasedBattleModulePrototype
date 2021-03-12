// Roman Baranov 06.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    // Автоматический переход хода, если все юниты походили 
    // Передавать ход по кнопке
    // Не давать выбирать юниты в процессе перемещения
    // 
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

    /// <summary>
    /// Список с сторонами юнитов
    /// </summary>
   

    private void Awake()
    {
        _unitsManager = FindObjectOfType<UnitsManager>();
        _combatGuiManager = FindObjectOfType<CombatGuiManager>();
        _battleMouseInput = FindObjectOfType<BattleMouseInput>();

        currentTurn = Turn.GreenTurn;// TO DO Можно рандомизировать первый ход
        currentGameState = GameState.GameInProgress;
    }

    //TO DO! Сделать полноценный  метод и добавить метод EndTurn()
    /// <summary>
    /// Обновляет данные юнитов в начале хода
    /// </summary>
    public void EndTurn()
    {
        _battleMouseInput.UnselectUnit();
        _currentTurnCounter++;
        
        SwitchCurrentSide();
        _combatGuiManager.SwitchCurrentSideTurnText();
        _combatGuiManager.UpdateTurnNumberText(); 
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

    // // TO DO Нужно свитчить ходы 
    private void SwitchMovesLeft(Turn turn)
    {
        if (turn == CombatManager.Turn.GreenTurn)
        {
            for (int i = 0; i < _unitsManager.BrownUnits.Count; i++)
            {
                _unitsManager.GreenUnits[i].GetComponent<Unit>().isMovesLeft = true;
                _unitsManager.BrownUnits[i].GetComponent<Unit>().isMovesLeft = false;
            }
        }
        else if (turn == CombatManager.Turn.BrownTurn)
        {
            for (int i = 0; i < _unitsManager.GreenUnits.Count; i++)
            {
                _unitsManager.GreenUnits[i].GetComponent<Unit>().isMovesLeft = false;
                _unitsManager.BrownUnits[i].GetComponent<Unit>().isMovesLeft = true;
            }
        }
    }
}
