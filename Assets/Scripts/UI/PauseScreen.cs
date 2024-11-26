using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused = false;
    [SerializeField] Button Resume;
    [SerializeField] Button Quit;
    [SerializeField] Slider VolumeSlider;
    [SerializeField] Slider SensSlider;

    [SerializeField] TMP_Text HealthUI;
    [SerializeField] TMP_Text ManaUI;
    [SerializeField] TMP_Text DamageUI;
    [SerializeField] TMP_Text ManaRegenUI; 
    [SerializeField] TMP_Text SpeedUI;

    public int healthNum = 0;
    public int damageNum = 0;
    public int manaNum = 0;
    public int manaRegenNum = 0;
    public int speedNum = 0;


    private void Awake()
    {
        Resume.onClick.AddListener(ResumeClick);
        Quit.onClick.AddListener(quitClick);

    }
    public void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            HealthUI.text = healthNum.ToString();
            ManaUI.text = manaNum.ToString();
            DamageUI.text = damageNum.ToString();
            ManaRegenUI.text = manaRegenNum.ToString();
            SpeedUI.text = speedNum.ToString();
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
