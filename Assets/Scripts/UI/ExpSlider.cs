using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpSlider : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private GameController game;

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = game.Experience;
        slider.maxValue = game.ExpToNextLevel;
    }
    public void UpdateExpSlider()
    {
        slider.value = game.Experience;
        slider.maxValue = game.ExpToNextLevel;
    }
}
