using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillCounter : MonoBehaviour
{
    public static KillCounter Instance { get; private set; }
    TMP_Text text;
    int killCount = 0;
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
    }
    void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
        text.text = killCount.ToString();
    }
    public void AddKill()
    {
        killCount++;
        text.text = killCount.ToString();
    }
}
