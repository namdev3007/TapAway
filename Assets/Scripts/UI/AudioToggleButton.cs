using UnityEngine;

public class AudioToggleSwitch : MonoBehaviour
{
    public enum AudioType { BGM, SFX }
    public AudioType type;

    public GameObject tickObject;

    private void Start()
    {
        UpdateVisual();
    }

    public void OnTogglePressed()
    {
        if (type == AudioType.BGM)
            AudioManager.Instance.ToggleBGM();
        else if (type == AudioType.SFX)
            AudioManager.Instance.ToggleSFX();

        UpdateVisual();
    }

    private void UpdateVisual()
    {
        bool isOn = type == AudioType.BGM
            ? AudioManager.Instance.IsBGMOn()
            : AudioManager.Instance.IsSFXOn();

        if (tickObject != null)
            tickObject.SetActive(isOn);
    }
}
