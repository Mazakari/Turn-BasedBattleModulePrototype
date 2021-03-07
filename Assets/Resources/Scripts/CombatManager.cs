// Roman Baranov 06.03.2021
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    // Автоматический переход хода, если все юниты походили
    // Передавать ход по кнопке
    // 
    private enum Turn
    {
        GreenTurn,
        BrownTurn
    }
    private Turn _currentTurn;

    private enum GameState 
    { 
        GameInProgress,
        GreenWon,
        BrownWon

    }
    private GameState _currentGameState;

    private UnitsManager _unitsManager = null;

    private void Awake()
    {
        
    }
}
