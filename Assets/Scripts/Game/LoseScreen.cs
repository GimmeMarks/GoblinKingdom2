using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseScreen : MonoBehaviour
{

    [SerializeField] Button StartOver;
    [SerializeField] Button Quit;

    private void Awake()
    {
        StartOver.onClick.AddListener(StartOverClick);
        Quit.onClick.AddListener(quitClick);
    }

    void StartOverClick()
    {
        SceneManager.LoadScene("MainGame");
    }
    void quitClick()
    {
        Application.Quit();
    }
}
