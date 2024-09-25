using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHp : MonoBehaviour
{
    private Slider slider;
    private Image headImage;
    void Start()
    {
        slider = GetComponentInChildren<Slider>();
        headImage = GameObject.Find("Head").GetComponent<Image>();

        slider.maxValue = Player.Instance.DefaultHp;
        slider.minValue = 0;
    }
    void Update()
    {
        float maxHp = Player.Instance.DefaultHp;
        float hp = Player.Instance.Hp;

        slider.value = maxHp - (maxHp - hp);
        if (hp <= (maxHp / 2))
        {
            float alpha = Mathf.InverseLerp(0, maxHp / 2, hp);
            Color headColor = headImage.color;
            headColor.a = alpha;
            headImage.color = headColor;
        }
        else
        {
            headImage.color = Color.white;
        }
    }
    
}
