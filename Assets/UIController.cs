using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject levelUpPanel;
    public GameObject displayPanel;
    public bool levelUpPanelOn;
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
        Time.timeScale = 1;
        levelUpPanelOn = false;
        displayPanel.GetComponent<ShowActiveWeaponsUI>().UpdateActiveWeaponsUI();
    }
}
