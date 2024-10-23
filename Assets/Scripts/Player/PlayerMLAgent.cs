using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices.WindowsRuntime;

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

    [SerializeField] private RayPerceptionSensorComponent2D raySensorEnemies;
    


    [Header("UI")]
    [SerializeField] private GameObject panelMLAgent;
    [SerializeField] private GameObject panelObserwations;
    [SerializeField] private GameObject panelActions;

    [SerializeField] private GameObject textPrefab;
    private Dictionary<string, GameObject> consoleTexts = new();

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
        iter++;
        ConsoleText(panelMLAgent, "iteration", iter.ToString());
        SetReward(0);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        ConsoleText(panelMLAgent, "reward", GetCumulativeReward().ToString("F2"));
        
        // pozycja gracza vector2
        Vector2 playerPos = Player.Instance.transform.position;
        sensor.AddObservation(playerPos);
        ConsoleText(panelObserwations, "pos", playerPos.x.ToString("F2") + ", " + playerPos.y.ToString("F2"));

        //// czy przeciwnicy w poblizu 0/1
        //sensor.AddObservation(RayDetectedEnemy(raySensorEnemies));
        //ConsoleText(panelObserwations, "enemies-close", RayDetectedEnemy(raySensorEnemies).ToString());


        //skierowany w 0 -> lewo, 1 -> prawo
        sensor.AddObservation(PlayerSide());
        ConsoleText(panelObserwations, "player-side", PlayerSide().ToString());

        // ilosc zycia
        //sensor.AddObservation(Player.Instance.Hp);
        //ConsoleText(panelObserwations, "health", Player.Instance.Hp.ToString("F2"));


    }
    
    int RayDetectedEnemy(RayPerceptionSensorComponent2D rayType)
    {
        RayPerceptionInput input = rayType.GetRayPerceptionInput();
        RayPerceptionOutput output = RayPerceptionSensor.Perceive(input);
        // Sprawdzenie, czy promienie coœ wykry³y
        foreach (var rayOutput in output.RayOutputs)
        {
            if (rayOutput.HitTaggedObject) // Obiekt z tagiem zosta³ wykryty
            {
                return 1;
            }
        }
        return 0;
    }
    int PlayerSide()
    {
        if (animator.GetFloat("WasFacing") > 0) return 1;
        return 0;
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        // x
        if (movementInput.x < 0)
            discreteActions[0] = 0; // -1 ruch
        else if (movementInput.x == 0)
            discreteActions[0] = 1; // brak ruchu
        else
            discreteActions[0] = 2; // +1 ruch

        // y
        if (movementInput.y < 0)
            discreteActions[1] = 0; // -1 ruch
        else if (movementInput.y == 0)
            discreteActions[1] = 1; // brak ruchu
        else
            discreteActions[1] = 2; // +1 ruch
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int moveX = actions.DiscreteActions[0];  // 0 -> -1, 1 -> 0, 2 -> +1
        int moveY = actions.DiscreteActions[1];  // 0 -> -1, 1 -> 0, 2 -> +1

        float mappedMoveX = moveX == 0 ? -1f : (moveX == 1 ? 0f : 1f);
        float mappedMoveY = moveY == 0 ? -1f : (moveY == 1 ? 0f : 1f);

        //znormalizowany wektor, czyli wartoœæ 0.7, gdy porusza siê na skos
        movement = new Vector2(mappedMoveX, mappedMoveY).normalized;
        ConsoleText(panelActions, "move", movement.x.ToString("F2") + ", " + movement.y.ToString("F2"));

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

        //if (RayDetectedEnemy(raySensorEnemies) == 1)
        //{
        //    AddReward(0.1f * Time.deltaTime);
        //}

    }
    private void OnMove(InputValue input)
    {
        movementInput = input.Get<Vector2>();
    }
    public void ResetEpisode()
    {
        EndEpisode();
    }
    void RandomUpgrades()
    {
        if (uIController.levelUpPanelOn)
        {
            WeaponLevel selectedUpgrade = levelUpSceen.activeCards[Random.Range(0, levelUpSceen.activeCards.Length)];
            if (selectedUpgrade != null)
            {
                arsenalController.UpgradeWeaponWithId(selectedUpgrade.weaponId, selectedUpgrade);
            }
            uIController.HideLevelUp();
        }
    }
    private void ConsoleText(GameObject section, string name, string value)
    {
        if (consoleTexts.TryGetValue(name, out GameObject foundObject))
        {
            TMP_Text text = foundObject.GetComponent<TMP_Text>();
            text.text = name + ": " + value;
        }
        else
        {
            GameObject newText = Instantiate(textPrefab, section.transform);
            newText.GetComponent<TMP_Text>().text = name + ": " + value;
            consoleTexts.Add(name, newText);
        }
    }
}
