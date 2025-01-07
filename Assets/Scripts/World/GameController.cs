using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Threat")]
    [SerializeField] private float threatValue;
    [SerializeField] public float threatSpeed;
    
    [Header("Level")]
    [SerializeField] private int level;
    [SerializeField] private float experience;
    [SerializeField] private float expToNextLevel;

    [Header("Pointers")]
    [SerializeField] private Player player;
    [SerializeField] private PlayerMLAgent mLAgent;
    [SerializeField] private KillCounter killCounter;
    [SerializeField] private ExpSlider expSlider;
    [SerializeField] private SpawnEnemies enemySpawner;
    [SerializeField] private ArsenalController arsenalController;


    private float threatSpikeTimer = 0;
    private static float spikeChangeTime = 30;
    private UIController uIController;

    public float Threat
    {
        get => threatValue;
        set => threatValue = value;
    }
    public float Experience
    {
        get => experience;
        set => experience = value;
    }
    public float ExpToNextLevel
    {
        get => expToNextLevel;
        set => expToNextLevel = value;
    }
    public void Start()
    {
        level = 1;
        experience = 0;
        expToNextLevel = LevelingFunction();
        uIController = GetComponent<UIController>();
        Time.timeScale = 1;
    }
    public void Update()
    {
        // wartość zagrożenia stale rośnie
        threatValue += Time.deltaTime * threatSpeed;
        threatSpikeTimer += Time.deltaTime;

        // co jakiś czas zmienia się prędkość wzrostu zagrożenia
        if (threatSpikeTimer > spikeChangeTime)
        {
            threatSpeed += 0.1f;
            threatSpikeTimer = 0;
        }
    }
    public bool ThreatCheck(float value)
    {
        if (threatValue - value >= 0)
        {
            return true;
        }
        return false;
    }
    public void AddExp(float value)
    {
        experience += value;
        if(expSlider != null)
        {
            expSlider.UpdateExpSlider();
        }
        if (experience >= expToNextLevel)
        {
            LevelUp();
        }
    }
    public void AddKill(float value)
    {
        if (killCounter != null)
        {
            killCounter.AddKill();
        }
        AddExp(value);
        if (mLAgent != null)
        {
            mLAgent.AddReward(value);
        }
    }
    void LevelUp()
    {
        experience = 0;
        level++;
        expToNextLevel = LevelingFunction();
        uIController.ShowLevelUp();
        if (expSlider != null)
        {
            expSlider.UpdateExpSlider();
        }

    }
    float LevelingFunction()
    {
        return 5  * level;
    }
    public void ResetGame()
    {
        player.ResetPlayer();
        enemySpawner.ClearAllEnemies();
        arsenalController.ResetArsenal();
        if(killCounter != null)
        {
            killCounter.ResetCounter();
        }
        threatValue = 0;
        threatSpeed = 0.3f;
        threatSpikeTimer = 0;
        spikeChangeTime = 30;
        level = 1;
        experience = 0;
        expToNextLevel = LevelingFunction();
    }
}
