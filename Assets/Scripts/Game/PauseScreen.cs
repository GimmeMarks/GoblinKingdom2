using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePause : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused = false;
    [SerializeField] Button Resume;
    [SerializeField] Button Quit;
    [SerializeField] Slider VolumeSlider;
    [SerializeField] Slider SensSlider;
    private void Awake()
    {
        Resume.onClick.AddListener(ResumeClick);
        Quit.onClick.AddListener(quitClick);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TogglePause();
        }
    }
 
    void ResumeClick()
    {
        TogglePause();
    }
    void quitClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("TitleScreen");
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            PlayerController.Instance.EndGame();
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            PlayerController.Instance.ResumeGame();
        }
    }

    



}
