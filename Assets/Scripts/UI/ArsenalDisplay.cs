using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArsenalDisplay : MonoBehaviour
{
    public WeaponBase weaponToDisplay;
    Slider slider;
    TMP_Text text;
    void Start()
    {
        slider = GetComponentInChildren<Slider>();
        text = GetComponentInChildren<TMP_Text>();
    }

    void Update()
    {
        slider.maxValue = weaponToDisplay.Cooldown;
        slider.value = weaponToDisplay.cooldownTimer;
        text.text = RomanNumeralConverter.IntToRoman(weaponToDisplay.Level);
    }
    public static class RomanNumeralConverter
    {
        public static string IntToRoman(int number)
        {
            switch (number)
            {
                case 1: return "I";
                case 2: return "II";
                case 3: return "III";
                case 4: return "IV";
                case 5: return "V";
                default: return "";
            }
        }
    }

}
