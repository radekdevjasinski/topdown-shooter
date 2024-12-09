using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public GameObject playButton;
    public GameObject cpuButton;
    public GameObject vsCpuButton;
    public GameObject quitButton;
    public GameObject mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        playButton.GetComponent<Button>().Select();
    }

    public void PlayButtonClicked(){
        SceneManager.LoadScene("game");
    }
    public void CpuButtonClicked(){
        SceneManager.LoadScene("mlagents default env");
    }
    public void VsCpuButtonClicked(){
        //SceneManager.LoadScene("vs_cpu");
    }
    public void QuitButtonClicked(){
        Application.Quit();
    }
    public void MainMenuButtonClicked(){
        SceneManager.LoadScene("menu");
    }

}
