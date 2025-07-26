using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Level Info")]
    public int currentLevelIndex = 1;

    private const string LevelKey = "CurrentLevel";

    private void Awake()
    {
        Instance = this;

        // Lấy level từ PlayerPrefs, nếu không có thì mặc định là 1
        currentLevelIndex = PlayerPrefs.GetInt(LevelKey, 1);
    }

    public void LoadLevel(int index)
    {
        currentLevelIndex = index;
        PlayerPrefs.SetInt(LevelKey, currentLevelIndex);
        PlayerPrefs.Save();

        LevelManager.Instance.LoadLevel(index);
    }

    public void NextLevel()
    {
        LoadLevel(++currentLevelIndex);
        AudioManager.Instance.PlaySFX(1);
    }

    public void RestartLevel()
    {
        LoadLevel(currentLevelIndex);
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey(LevelKey);
        currentLevelIndex = 1;
        LoadLevel(currentLevelIndex);
    }
}
