using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDamageStrategy : MonoBehaviour, IDamageStrategy
{
    public void Damage(Enemy enemy, float amount)
    {
        enemy.hp -= amount;
        if (enemy.hp <= 0)
        {
            Destroy(enemy.gameObject);
            KillCounter.Instance.AddKill();
            GameController.Instance.AddExp(enemy.threatCost);
            if (PlayerMLAgent.instance != null)
            {
                PlayerMLAgent.instance.AddReward(enemy.threatCost * 5);
            }
        }
    }
}
