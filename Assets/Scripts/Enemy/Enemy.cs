using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public float speed;
    [SerializeField] public float hp;
    [SerializeField] public float attack;

    [Header("Threat")]
    [SerializeField] public float threatCost;


    private float flashDuriaton = 0.2f;

    protected GameObject playerRef;
    protected new Rigidbody2D rigidbody;
    protected SpriteRenderer sprite;
    protected Slider slider;

    protected virtual void Start()
    {
        playerRef = Player.Instance.gameObject;
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        slider = GetComponentInChildren<Slider>();
        slider.minValue = 0;
        slider.maxValue = hp;
    }

    protected abstract void Move();
    public virtual void Damage(float amount)
    {
        FlashOnDamage();
    }
    public abstract void Attack();

    protected virtual void FlashOnDamage()
    {
        sprite.color = Color.red;
        StartCoroutine(endFlash());
    }

    IEnumerator endFlash()
    {
        yield return new WaitForSeconds(flashDuriaton);
        sprite.color = Color.white;
    }

    protected virtual void FixedUpdate()
    {
        Move();
    }
    protected virtual void UpdateSlider()
    {
        slider.value = hp;
    }
}
