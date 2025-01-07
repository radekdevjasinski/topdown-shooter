using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSecondScene : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("cpu_pov", LoadSceneMode.Additive);
    }
}
