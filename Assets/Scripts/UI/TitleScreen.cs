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
        Menu.onClick.AddListener(titleScreenClick);
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
        Debug.Log("Trying to quit");
        SceneManager.LoadScene("CreditsScreen");
    }
    void titleScreenClick()
    {
        Debug.Log("Trying to quit");
        SceneManager.LoadScene("TitleScreen");
    }
    public void quitClick()
    {
        Debug.Log("Trying to quit");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        Debug.Log("It was processed");
    }


}