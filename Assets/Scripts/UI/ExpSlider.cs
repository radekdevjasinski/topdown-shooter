using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpSlider : MonoBehaviour
{
    private Slider slider;
    public static ExpSlider Instance;
    private void Awake()
    {
        if (Instance!=null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = GameController.Instance.Experience;
        slider.maxValue = GameController.Instance.ExpToNextLevel;
    }
    public void UpdateExpSlider()
    {
        slider.value = GameController.Instance.Experience;
        slider.maxValue = GameController.Instance.ExpToNextLevel;
    }
}
