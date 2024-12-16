using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHp : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Image headRef;
    private Slider slider;
    private Image headImage;
    void Start()
    {
        slider = GetComponentInChildren<Slider>();
        headImage = headRef.GetComponent<Image>();

        slider.maxValue = player.DefaultHp;
        slider.minValue = 0;
    }
    void Update()
    {
        float maxHp = player.DefaultHp;
        float hp = player.Hp;

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
