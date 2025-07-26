using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    [Header("UI Elements")]
    public GameObject pauseButton;
    public GameObject pausePanel;

    public GameObject winPanel;

    private void Start()
    {
        pausePanel?.SetActive(false);
        winPanel?.SetActive(false);
        pauseButton?.SetActive(false);
    }

    public void ShowPausePanel()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        AudioManager.Instance.PlaySFX(0);
    }

    public void ShowPauseButton()
    {
        pauseButton.SetActive(true);
        AudioManager.Instance.PlaySFX(0);
    }

    public void HidePausePanel()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        AudioManager.Instance.PlaySFX(0);
    }

    public void ShowWinPanel()
    {
        winPanel.SetActive(true);
    }

    public void OnTapToContinue()
    {
        winPanel.SetActive(false);
        Time.timeScale = 1f;
        GameManager.Instance.NextLevel(); // gọi load level tiếp theo
        Debug.Log("con chim");
        AudioManager.Instance.PlaySFX(0);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        GameManager.Instance.RestartLevel();
        AudioManager.Instance.PlaySFX(0);
        pausePanel.SetActive(false);
    }
}
