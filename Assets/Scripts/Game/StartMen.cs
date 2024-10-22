using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMen : MonoBehaviour
{

    [SerializeField] Button Play;
    [SerializeField] Button Quit;

    private void Awake()
    {
        Play.onClick.AddListener(playClick);
        Quit.onClick.AddListener(quitClick);
    }

    void playClick()
    {
        SceneManager.LoadScene("MainGame");
    }
    void quitClick()
    {
        Application.Quit();
    }

}
