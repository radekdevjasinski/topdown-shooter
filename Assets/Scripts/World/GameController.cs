using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Threat")]
    [SerializeField] private float threatValue;
    [SerializeField] private float threatSpeed;
    
    [Header("Level")]
    [SerializeField] private int level;
    [SerializeField] private float experience;
    [SerializeField] private float expToNextLevel;


    private float threatSpikeTimer = 0;
    private static float spikeChangeTime = 5;


    public static GameController Instance;
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
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject.GetComponent<GameController>());
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        level = 1;
        experience = 0;
        expToNextLevel = LevelingFunction();
    }
    void Update()
    {
        threatValue += Time.deltaTime * threatSpeed;
        threatSpikeTimer += Time.deltaTime;
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
        ExpSlider.Instance.UpdateExpSlider();
        if (experience >= expToNextLevel)
        {
            LevelUp();
        }
    }
    void LevelUp()
    {
        experience = 0;
        level++;
        expToNextLevel = LevelingFunction();
        ExpSlider.Instance.UpdateExpSlider();

    }
    float LevelingFunction()
    {
        return Mathf.Pow(2, level);
    }
}