using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.Tilemaps;
using TMPro;

public class PlayerMLAgent : Agent
{
    private float speed;
    private new Rigidbody2D rigidbody;
    private Animator animator;
    private Vector2 movement;

    [SerializeField] private TilemapCollider2D wallCollider;
    [SerializeField] private SpawnEnemies enemySpawner;

    [Header("UI")]
    public Transform textsTransform;
    public TMP_Text[] texts;
    public int iter = 0;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        speed = Player.Instance.Speed;
        //gracz zwrócony w lewo
        animator.SetFloat("WasFacing", -1);
        texts = textsTransform.GetComponentsInChildren<TMP_Text>();
    }
    public override void OnEpisodeBegin()
    {
        Debug.Log("Episode started");
        iter++;
        texts[0].text = "Iteracja: " + iter;
        
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        Vector2 playerPos = transform.position;
        sensor.AddObservation(playerPos);
        texts[1].text = "Nagroda: " + GetCumulativeReward(); ;
        texts[2].text = "Pozycja: " + playerPos;

        GameObject closestEnemy = enemySpawner.getClosestEnemy();
        if (closestEnemy != null)
        {
            Vector2 closestEnemyPos = closestEnemy.transform.position;
            sensor.AddObservation(closestEnemyPos);
            texts[3].text = "Najb. przeciwnik: " + closestEnemyPos;
        }
        else
        {
            // Jeœli nie ma wrogów, dodaj dalek¹ odleg³oœæ
            sensor.AddObservation(new Vector2(100,100));
            texts[3].text = "Najb. przeciwnik: " + new Vector2(100, 100);
        }
        // Oblicz odleg³oœæ miêdzy graczem a najbli¿sz¹ œcian¹
        if (wallCollider != null)
        {
            Vector2 closestPointOnWall = wallCollider.ClosestPoint(playerPos);
            sensor.AddObservation(closestPointOnWall);
            texts[4].text = "Najb. œciana: " + closestPointOnWall;
        }
        else
        {
            // Jeœli nie ma kolidera œciany, dodaj dalek¹ odleg³oœæ
            sensor.AddObservation(new Vector2(100, 100));
            texts[4].text = "Najb. œciana: " + new Vector2(100, 100);
        }


    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];

        movement = new Vector2(moveX, moveY);
        texts[5].text = "RuchX: " + moveX;
        texts[6].text = "RuchY: " + moveY;
    }
    void FixedUpdate()
    {
        rigidbody.velocity = movement * speed * Time.deltaTime;
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        if (movement.x != 0)
        {
            animator.SetFloat("WasFacing", movement.x);
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        //kara dla agenta kiedy bierze udzia³ w kolizji
        AddReward(Time.deltaTime * (-3));
    }
    void Update()
    {
        //nagroda dla agenta w czasie
        AddReward(Time.deltaTime);
        
    }
    public void ResetEpisode()
    {
        SetReward(-1f);
        EndEpisode();
        Debug.Log("Episode ended");
    }
}
