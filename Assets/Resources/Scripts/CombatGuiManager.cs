// Roman Baranov 08.03.2021
using UnityEngine;
using UnityEngine.UI;

public class CombatGuiManager : MonoBehaviour
{
    public Text _currentSideTurnText = null;// Ссылка на текст с отображением текущего хода юнитов
    public Text _turnsCounterText = null;// Ссылка на текст с отображением номера текущего хода 

    private CombatManager _combatManager = null;

    private string _greenTurn = "Greens Turn";// Строка для отображения текста хода для зеленых юнитов
    /// <summary>
    /// Свойство для отображения текста хода для зеленых юнитов
    /// </summary>
    public string GreenTurn { get { return _greenTurn; } }
    
    private string _brownTurn = "Browns Turn";// Строка для отображения текста хода для коричневых юнитов
    /// <summary>
    /// Свойство для отображения текста хода для коричневых юнитов
    /// </summary>
    public string BrownTurn { get { return _brownTurn; } }

    private void Awake()
    {
        _combatManager = FindObjectOfType<CombatManager>();
    }

    private void Start()
    {

        SwitchCurrentSideTurnText();
        UpdateTurnNumberText();
    }

    /// <summary>
    /// Меняет текст текущего хода юнитов на противоположный
    /// </summary>
    public void SwitchCurrentSideTurnText()
    {
        if (_combatManager.currentTurn == CombatManager.Turn.GreenTurn)
        {
            _currentSideTurnText.text = _greenTurn;
        }
        else if (_combatManager.currentTurn == CombatManager.Turn.BrownTurn)
        {
            _currentSideTurnText.text = _brownTurn;
        }
    }

    public void UpdateTurnNumberText()
    {
        _turnsCounterText.text = _combatManager.CurrentTurnNumber.ToString();
    }

}
