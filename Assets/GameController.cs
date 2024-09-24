using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //zagrozenie
    [Header("Threat")]
    [SerializeField] private float threatValue;
    [SerializeField] private float threatSpeed;
    //poziom trudnosci
    [Header("Difficulty")]
    [SerializeField] private float difficulty;
    [SerializeField] private float difficultySpeed;


    private float threatSpikeTimer = 0;
    private float spikeChangeTime = 5;


    public static GameController instance;
    public float Threat
    {
        get => threatValue;
        set => threatValue = value;
    }
    public float Difficulty
    {
        get => difficulty;
        set => difficulty = value;
    }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject.GetComponent<GameController>());
        }
        else
        {
            instance = this;
        }
        
    }
    void Update()
    {
        difficulty += Time.deltaTime * difficultySpeed;
        threatValue += Time.deltaTime * threatSpeed;
        threatSpikeTimer += Time.deltaTime;
        if (threatSpikeTimer > spikeChangeTime)
        {
            threatSpeed += 0.1f;
            threatSpikeTimer = 0;
            spikeChangeTime += 5;
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
}
