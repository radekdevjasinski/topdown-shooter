using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArsenalDisplay : MonoBehaviour
{
    public WeaponBase weaponToDisplay;
    Slider slider;
    void Start()
    {
        slider = GetComponentInChildren<Slider>();
    }

    void Update()
    {
        slider.maxValue = weaponToDisplay.Cooldown;
        slider.value = weaponToDisplay.cooldownTimer;
    }
}
