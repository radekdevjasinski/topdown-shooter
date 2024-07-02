using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    [SerializeField] private float hp;
    [SerializeField] private float speed;

    private PlayerMLAgent playerMLAgent;
    private SpawnEnemies enemiesSpawner;



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

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        playerMLAgent = GetComponent<PlayerMLAgent>();
        enemiesSpawner = GameObject.Find("EnemiesSpawner").GetComponent<SpawnEnemies>();
    }
    public void TakeDamage(float amount)
    {
        Hp -= amount ;
        if (Hp <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Player.Instance.gameObject.transform.position = Vector2.zero;
        enemiesSpawner.ClearAllEnemies();
        Hp = 5;
        if (playerMLAgent.enabled)
        {
            playerMLAgent.ResetEpisode();
        }
        
    }
}
