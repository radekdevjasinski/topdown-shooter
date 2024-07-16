using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHp : MonoBehaviour
{
    private Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();    
        slider.minValue = 0;
        slider.maxValue = Player.Instance.Hp;
    }
    void Update()
    {
        slider.value = Player.Instance.Hp;
        
    }
}
