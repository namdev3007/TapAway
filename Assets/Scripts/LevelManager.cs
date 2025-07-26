using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public Transform levelParent;
    public CameraOrbit cameraOrbit;

    private GameObject currentLevel;

    public bool canInteract = true;

    public GameObject[] firework;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadLevel(int levelIndex)
    {
        if (currentLevel != null)
            Destroy(currentLevel);

        string levelPath = $"Levels/Level_{levelIndex}";
        GameObject prefab = Resources.Load<GameObject>(levelPath);

        currentLevel = Instantiate(prefab, levelParent);

        LevelCameraSettings settings = currentLevel.GetComponentInChildren<LevelCameraSettings>();
 
        cameraOrbit.SetLevel(settings);
 
  
    }

    public void CheckWinCondition()
    {
        if (currentLevel == null) return;

        int childCount = currentLevel.transform.childCount;
        Debug.Log($"🔍 Số đối tượng còn lại trong level: {childCount}");

        if (childCount == 1)
        {
            StartCoroutine(ShowWinAfterDelay());

            PlayFireworkEffect();
        }
    }

    private System.Collections.IEnumerator ShowWinAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        UIManager.Instance.ShowWinPanel();
    }


    private void PlayFireworkEffect()
    {
        AudioManager.Instance.PlaySFX(2);
        foreach (GameObject fw in firework)
        {
            if (fw != null)
            {
                fw.SetActive(true); // Bật pháo hoa

                // Phát hiệu ứng nếu có ParticleSystem
                ParticleSystem ps = fw.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    ps.Play();
                }
            }
        }

        StartCoroutine(HideFireworksAfterDelay(0.7f));
    }

    private System.Collections.IEnumerator HideFireworksAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (GameObject fw in firework)
        {
            if (fw != null)
                fw.SetActive(false);
        }
    }


    public GameObject GetCurrentLevel()
    {
        return currentLevel;
    }
}
