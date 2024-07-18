using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //publiczna instancja singletona
    public static Player Instance { get; private set; }
    [Header("Health Points")]
    [SerializeField] private float defaultHp;
    [SerializeField] private float defaultSpeed;

    [SerializeField] private float hp;
    [SerializeField] private float speed;
    
    private SpawnEnemies enemiesSpawner;

    //gettery i settery
    public float DefaultSpeed
    {
        get => defaultSpeed;
        set => defaultSpeed = value;
    }
    public float DefaultHp
    {
        get => defaultHp;
        set => defaultHp = value;
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

        enemiesSpawner = GameObject.Find("EnemiesSpawner").GetComponent<SpawnEnemies>();
    }
    public void ChangeHp(float amount)
    {
        hp += amount;
        if (hp <= 0)
        {
            Die();
        }
        hp = Mathf.Clamp(hp, 0, defaultHp);
    }
    public void Die()
    {
        Player.Instance.gameObject.transform.position = Vector2.zero;
        enemiesSpawner.ClearAllEnemies();
        hp = defaultHp;
    }
    void Update()
    {
        if (speed != defaultSpeed)
        {

        }
    }
}