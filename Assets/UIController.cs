using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject levelUpPanel;
    public GameObject displayPanel;
    public GameObject pausePanel;
    public GameObject killCounter;
    public bool levelUpPanelOn;
    public bool pauseOn;
    private float defaultTimeScale;
    private void Start()
    {
        defaultTimeScale = Time.timeScale;
        levelUpPanel?.SetActive(false);
        pausePanel?.SetActive(false);
    }
    public void ShowLevelUp()
    {
        levelUpPanel?.SetActive(true);
        levelUpPanel?.GetComponent<LevelUpScreen>().SetUpLevelScreen();
        Time.timeScale = 0;
        levelUpPanelOn = true;
    }
    public void HideLevelUp()
    {
        Time.timeScale = defaultTimeScale;
        levelUpPanelOn = false;
        levelUpPanel?.SetActive(false);
        displayPanel?.GetComponent<ShowActiveWeaponsUI>().UpdateActiveWeaponsUI();
    }
    public void OnPause()
    {
        if(levelUpPanelOn)
        {
            return;
        }
        if(pauseOn)
        {
            pausePanel?.SetActive(false);
            Time.timeScale = 1;
            pauseOn = false;
        }
        else
        {
            pausePanel?.SetActive(true);
            Time.timeScale = 0;
            pauseOn = true;
        }
        
       
        
    }
}
