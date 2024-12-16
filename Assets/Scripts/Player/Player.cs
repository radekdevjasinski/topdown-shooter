using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Health Points")]
    [SerializeField] private float defaultHp;
    [SerializeField] private float hp;

    [Header("Speed")]
    [SerializeField] private float defaultSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float speedChangeRate = 0.1f;
    
    [Header("Pointers")]
    [SerializeField] private SpawnEnemies enemiesSpawner;
    [SerializeField] private PlayerMLAgent mLAgent;
    [SerializeField] private GameController game;

    //gettery i settery
    public float Hp
    {
        get => hp;
        set => hp = value;
    }

    public float Speed
    {
        get => speed;
        set => speed = value;
    }
    public float DefaultHp
    {
        get => defaultHp;
    }
    public void ChangeHp(float amount)
    {
        hp += amount;
        if (hp <= 0)
        {
            Die();
        }
        hp = Mathf.Clamp(hp, 0, defaultHp);

        if (mLAgent!= null && amount < 0)    
        {
            mLAgent.AddReward(amount * 10);
        }
    }
    public void Die()
    {
        if (mLAgent != null && mLAgent.gameObject.activeSelf)
        {
            mLAgent.ResetEpisode();
        }
        else
        {
            game.ResetGame();
        }
        

    }
    void Update()
    {
        if (speed != defaultSpeed)
        {
            float difference = defaultSpeed - speed;
            speed += difference * speedChangeRate * Time.deltaTime;

            if (Mathf.Abs(difference) < 0.1f)
            {
                speed = defaultSpeed;
            }
        }
    }
    public void ResetPlayer()
    {
        transform.position = new Vector3(0, 0, 0);
        Hp = DefaultHp;
        Speed = defaultSpeed;
    }

}
