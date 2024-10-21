using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerMLAgent : Agent
{
    private float speed;
    private new Rigidbody2D rigidbody;
    private Animator animator;
    private Vector2 movement;
    public Vector2 movementInput;

    [SerializeField] private UIController uIController;
    [SerializeField] private SpawnEnemies enemySpawner;
    [SerializeField] private LevelUpScreen levelUpSceen;
    [SerializeField] private ArsenalController arsenalController;
    public static PlayerMLAgent instance;

    [Header("UI")]
    public TMP_Text[] texts;
    public int iter = 0;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    void Start()
    {
        rigidbody = Player.Instance.GetComponent<Rigidbody2D>();
        animator = Player.Instance.GetComponent<Animator>();
        speed = Player.Instance.Speed;
        //gracz zwrócony w lewo
        animator.SetFloat("WasFacing", -1);
        Player.Instance.GetComponent<PlayerMovement>().enabled = false;
        Player.Instance.GetComponent<PlayerInput>().enabled = false;

    }
    public override void OnEpisodeBegin()
    {
        GameController.Instance.ResetGame();

        Debug.Log("Episode started");
        iter++;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // Pozycja gracza
        Vector2 playerPos = Player.Instance.transform.position;
        sensor.AddObservation(playerPos);

        texts[0].text = "iteration " + iter;
        texts[1].text = "reward " + GetCumulativeReward().ToString("F2");

    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = movementInput.x;
        continuousActions[1] = movementInput.y;

    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];

        movement = new Vector2(moveX, moveY);
        texts[2].text = "move " + moveX.ToString("F2") + ", " + moveY.ToString("F2");
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
    void Update()
    {
        //nagroda dla agenta w czasie
        //AddReward(Time.deltaTime);

        //wybierz losowe ulepszenia
        RandomUpgrades();

    }
    private void OnMove(InputValue input)
    {
        movementInput = input.Get<Vector2>();
    }
    public void ResetEpisode()
    {
        //SetReward(-1f);
        Debug.Log("Episode ended");
        EndEpisode();
    }
    void RandomUpgrades()
    {
        if (uIController.levelUpPanelOn)
        {
            WeaponLevel selectedUpgrade = levelUpSceen.activeCards[Random.Range(0, levelUpSceen.activeCards.Length)];
            if (selectedUpgrade != null)
            {
                Debug.Log("Wybrana bron: " + selectedUpgrade.weaponId + " " + selectedUpgrade.name);
                arsenalController.UpgradeWeaponWithId(selectedUpgrade.weaponId, selectedUpgrade);
            }
            uIController.HideLevelUp();
        }
    }
}
