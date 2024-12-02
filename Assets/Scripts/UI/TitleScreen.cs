using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{

    [SerializeField] Button Play;
    [SerializeField] Button Tutorial;
    [SerializeField] Button Credits;
    [SerializeField] Button Menu;
    [SerializeField] Button Quit;

    private void Awake()
    {
        Play.onClick.AddListener(playClick);
        Tutorial.onClick.AddListener(tutorialClick);
        Credits.onClick.AddListener(creditsClick);
        Menu.onClick.AddListener(titleClick);
        Quit.onClick.AddListener(quitClick);
    }

    void playClick()
    {
        SceneManager.LoadScene("MainGame");
    }
    void tutorialClick()
    {
        SceneManager.LoadScene("Tutorial");
    }
    void creditsClick()
    {
        SceneManager.LoadScene("CreditsScreen");
    }
    void titleClick()
    {
        SceneManager.LoadScene("TitleScreen");
    }
    void quitClick()
    {
        Application.Quit();
    }

}