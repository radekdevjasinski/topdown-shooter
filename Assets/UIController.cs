using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject levelUpPanel;
    public GameObject displayPanel;
    public bool levelUpPanelOn;
    private float defaultTimeScale;
    private void Start()
    {
        defaultTimeScale = Time.timeScale;
    }
    public void ShowLevelUp()
    {
        levelUpPanel.SetActive(true);
        levelUpPanel.GetComponent<LevelUpScreen>().SetUpLevelScreen();
        Time.timeScale = 0;
        levelUpPanelOn = true;
    }
    public void HideLevelUp()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = defaultTimeScale;
        levelUpPanelOn = false;
        displayPanel.GetComponent<ShowActiveWeaponsUI>().UpdateActiveWeaponsUI();
    }
}
