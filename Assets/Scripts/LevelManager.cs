using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    private int blockCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterBlock(Block block)
    {
        if (block.shouldCount)
        {
            blockCount++;
            Debug.Log($"🧱 Đăng ký Block: Tổng cộng = {blockCount}");
        }
    }

    public void UnregisterBlock(Block block)
    {
        if (block.shouldCount)
        {
            blockCount--;
            Debug.Log($"🧱 Block còn lại: {blockCount}");

            if (blockCount <= 0)
                StartCoroutine(DelayLevelComplete());
        }
    }

    private IEnumerator DelayLevelComplete()
    {
        yield return null; // chờ 1 frame
        GameManager.Instance?.OnLevelCompleted();
    }
}
