using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.SideChannels;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerMLAgent : Agent
{
    private float speed;
    private new Rigidbody2D rigidbody;
    private Animator animator;
    private Vector2 movement;
    public Vector2 movementInput;

    [SerializeField] private Player player;
    [SerializeField] private GameController game;
    [SerializeField] private UIController uIController;
    [SerializeField] private LevelUpScreen levelUpSceen;
    [SerializeField] private ArsenalController arsenalController;

    [SerializeField] private RayPerceptionSensorComponent2D raySensorEnemies;
    
    [Header("UI")]
    [SerializeField] private GameObject panelMLAgent;
    [SerializeField] private GameObject panelObserwations;
    [SerializeField] private GameObject panelActions;
    [SerializeField] private KillCounter killCounter;

    [Header("Stats")]
    [SerializeField] private GameObject textPrefab;
    private Dictionary<string, GameObject> consoleTexts = new();
    private float distanceCovered;
    private Vector2 lastPosition;
    private StatsRecorder statsRecorder;


    public int iter = 0;
    void Start()
    {
        rigidbody = player.GetComponent<Rigidbody2D>();
        animator = player.GetComponent<Animator>();
        speed = player.Speed;
        //gracz zwr�cony w lewo
        animator.SetFloat("WasFacing", -1);
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<PlayerInput>().enabled = false;

        statsRecorder = Academy.Instance.StatsRecorder;

    }
    public override void OnEpisodeBegin()
    {
        game.ResetGame();
        distanceCovered = 0;
        iter++;
        ConsoleText(panelMLAgent, "iteration", iter.ToString());
        SetReward(0);

    }
    public override void CollectObservations(VectorSensor sensor)
    {
        ConsoleText(panelMLAgent, "reward", GetCumulativeReward().ToString("F2"));
        
        // pozycja gracza vector2
        Vector2 playerPos = player.transform.position;
        sensor.AddObservation(playerPos);
        ConsoleText(panelObserwations, "pos", playerPos.x.ToString("F2") + ", " + playerPos.y.ToString("F2"));

        //// czy przeciwnicy w poblizu 0/1
        //sensor.AddObservation(RayDetectedEnemy(raySensorEnemies));
        //ConsoleText(panelObserwations, "enemies-close", RayDetectedEnemy(raySensorEnemies).ToString());


        ////skierowany w 0 -> lewo, 1 -> prawo
        //sensor.AddObservation(PlayerSide());
        //ConsoleText(panelObserwations, "player-side", PlayerSide().ToString());

        // ilosc zycia
        //sensor.AddObservation(player.Hp);
        //ConsoleText(panelObserwations, "health", player.Hp.ToString("F2"));

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
// Kiedy następuje moment wykonania akcji
    public override void OnActionReceived(ActionBuffers actions)
    {
        // Dwie akcje które wykonuje agent, ruch w poziomie i pionie

        int moveX = actions.DiscreteActions[0];  // 0 -> -1, 1 -> 0, 2 -> +1
        int moveY = actions.DiscreteActions[1];  // 0 -> -1, 1 -> 0, 2 -> +1

        float mappedMoveX = moveX == 0 ? -1f : (moveX == 1 ? 0f : 1f);
        float mappedMoveY = moveY == 0 ? -1f : (moveY == 1 ? 0f : 1f);

        // Znormalizowany wektor, czyli wartość 0.7, gdy porusza się na skos
        movement = new Vector2(mappedMoveX, mappedMoveY).normalized;
        ConsoleText(panelActions, "move", movement.x.ToString("F2") + ", " + movement.y.ToString("F2"));

        // Oblicz przemieszczenie od ostatniej pozycji
        Vector2 currentPosition = player.transform.position;
        distanceCovered += Vector2.Distance(lastPosition, currentPosition);
        lastPosition = currentPosition;
        ConsoleText(panelMLAgent, "distance", distanceCovered.ToString("F2"));
        if(killCounter != null)
        {
            ConsoleText(panelMLAgent, "killed", killCounter.killCount.ToString());
        }

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
        //wybierz losowe ulepszenia
        RandomUpgrades();
    }
    private void OnMove(InputValue input)
    {
        movementInput = input.Get<Vector2>();
    }
    public void ResetEpisode()
    {
        if (gameObject.activeSelf == false)
        {
            return;
        }
        if (killCounter!= null)
        {
            statsRecorder.Add("Enemies/Enemies Killed", killCounter.killCount);
        }
        statsRecorder.Add("Distance/Distance Covered", distanceCovered);
        EndEpisode();
    }
    public void LoseEpisode()
    {
        AddReward(-10f);
        if (killCounter!= null)
        {
            statsRecorder.Add("Enemies/Enemies Killed", killCounter.killCount);
        }
        statsRecorder.Add("Distance/Distance Covered", distanceCovered);
        EndEpisode();
    }
    
    void RandomUpgrades()
    {
        if (uIController == null) {return;}
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
