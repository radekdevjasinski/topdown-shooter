using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    public int killCount = 0;
    void Start()
    {
        text.text = killCount.ToString();
    }
    public void AddKill()
    {
        killCount++;
        text.text = killCount.ToString();
    }
    public void ResetCounter()
    {
        killCount = 0;
    }
}
