using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAttackStrategy : MonoBehaviour, IAttackStrategy
{
    bool inContactwithPlayer = false;
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inContactwithPlayer = true;
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inContactwithPlayer = false;
        }
    }

    public void Attack()
    {
        if (inContactwithPlayer)
        {
            float damage = gameObject.GetComponent<Enemy>().attack * Time.fixedDeltaTime;
            gameObject.GetComponent<Enemy>().playerRef.GetComponent<Player>().ChangeHp(-damage);
        }
        
    }
}
