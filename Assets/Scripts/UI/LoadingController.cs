using UnityEngine;

public class LoadingController : MonoBehaviour
{
    public ProgressBar progressBar;
    public GameObject loadingUI;
    public float loadDuration = 2f;

    private float timer = 0f;
    private bool loadingDone = false;

    void Start()
    {
        if (progressBar != null)
            progressBar.SetProgress(0f);

        if (loadingUI != null)
            loadingUI.SetActive(true);
    }

    void Update()
    {
        if (loadingDone) return;

        timer += Time.deltaTime;
        float progress = Mathf.Clamp01(timer / loadDuration);

        progressBar.SetProgress(progress);

        if (progress >= 1f)
        {
            loadingDone = true;
            OnLoadingComplete();
        }
    }

    void OnLoadingComplete()
    {
        loadingUI.SetActive(false);

        int levelToLoad = PlayerPrefs.GetInt("CurrentLevel", 1);

        UIManager.Instance.ShowPauseButton();

        AudioManager.Instance.SetBGMVolume(0.1f);
        AudioManager.Instance.PlayBGM(0);

        GameManager.Instance.LoadLevel(levelToLoad);
    }

}
