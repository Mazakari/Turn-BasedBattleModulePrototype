using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider _healthBarSlider = null;

    private Unit _unit = null;

    private void Awake()
    {
        _healthBarSlider = GetComponent<Slider>();
        _unit = GetComponentInParent<Unit>();

        _healthBarSlider.maxValue = _unit.UnitMaxHealth;
        _healthBarSlider.minValue = 0;
        _healthBarSlider.value = _healthBarSlider.maxValue;
    }

    /// <summary>
    /// Обновляет значение здоровья прогресс-бара юнита
    /// </summary>
    /// <param name="newHealthValue">Новое значение здоровья юнита</param>
    public void UpdateHealhBar(float newHealthValue)
    {
        Mathf.Clamp(newHealthValue, _healthBarSlider.minValue, _healthBarSlider.maxValue);
        _healthBarSlider.value = newHealthValue;
    }
}
