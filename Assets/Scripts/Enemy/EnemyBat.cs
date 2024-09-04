using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class EnemyBat : Enemy
{
    private IMovementStrategy movementStrategy;
    private IDamageStrategy damageStrategy;
    private IAttackStrategy attackStrategy;
    protected override void Start()
    {
        base.Start();
        slider.maxValue = hp;
        movementStrategy = gameObject.AddComponent<FlyTowardsStrategy>();
        damageStrategy = gameObject.AddComponent<SimpleDamageStrategy>();
        attackStrategy = gameObject.AddComponent<SimpleAttackStrategy>(); 
    }
    protected override void Move()
    {
        movementStrategy.Move(rigidbody, playerRef, speed);
    }
    public override void Damage(float amount)
    {
        base.Damage(amount);
        damageStrategy.Damage(this, amount);
    }
    public override void Attack()
    {
        attackStrategy.Attack();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Attack();
        UpdateSlider();
    }

}
