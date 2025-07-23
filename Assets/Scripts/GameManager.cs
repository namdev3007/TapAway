using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int moveCount = 0;

    private GameObject currentLevel;
    public int currentLevelIndex = 1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        LoadLevel(currentLevelIndex);
    }

    public void IncrementMove()
    {
        moveCount++;
        Debug.Log($"📦 Move Count: {moveCount}");
    }

    public int GetMoveCount() => moveCount;

    public void LoadLevel(int index)
    {
        moveCount = 0;

        if (currentLevel != null)
            Destroy(currentLevel);

        string levelName = $"Levels/Level_{index}";
        GameObject levelPrefab = Resources.Load<GameObject>(levelName);

        if (levelPrefab != null)
        {
            currentLevel = Instantiate(levelPrefab, this.transform);
            Debug.Log($"✅ Loaded level {index}");
        }
        else
        {
            Debug.LogError($"❌ Không tìm thấy prefab: {levelName}");
        }
    }

    public void OnLevelCompleted()
    {
        Debug.Log("🎉 Hoàn thành màn chơi!");
        Invoke(nameof(NextLevel), 1.5f);
    }

    public void NextLevel()
    {
        currentLevelIndex++;
        LoadLevel(currentLevelIndex);
    }

    public void RestartLevel()
    {
        LoadLevel(currentLevelIndex);
    }
}
