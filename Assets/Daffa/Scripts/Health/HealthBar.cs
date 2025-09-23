using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Slider _healthSlider;

    void Start()
    {
        _healthSlider = GetComponent<Slider>();
    }

    public void SetMaxHealth(int MaxHealth)
    {
        _healthSlider.maxValue = MaxHealth;
        _healthSlider.value = MaxHealth;
    }

    public void SetHealth(int health)
    {
        _healthSlider.value = health;
    }
}
