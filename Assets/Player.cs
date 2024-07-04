using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //publiczna instancja singletona
    public static Player Instance { get; private set; }
    [SerializeField] private float maxHp;
    [SerializeField] private float hp;
    [SerializeField] private float speed;

    private PlayerMLAgent playerMLAgent;
    private SpawnEnemies enemiesSpawner;
    //gettery i settery
    public float MaxHp
    {
        get => maxHp;
        set => maxHp = value;
    }
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
        //stworzenie instancji
        if (Instance == null)
        {
            Instance = this;
        }
        //usuniecie kopii
        else
        {
            Destroy(gameObject);
        }

        playerMLAgent = GetComponent<PlayerMLAgent>();
        enemiesSpawner = GameObject.Find("EnemiesSpawner").GetComponent<SpawnEnemies>();
    }
    public void TakeDamage(float amount)
    {
        hp -= amount ;
        if (hp <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Player.Instance.gameObject.transform.position = Vector2.zero;
        enemiesSpawner.ClearAllEnemies();
        hp = maxHp;
        if (playerMLAgent.enabled)
        {
            playerMLAgent.ResetEpisode();
        }
        
    }
}
