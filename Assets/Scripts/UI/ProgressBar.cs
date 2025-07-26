using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image fillImage; // Gán ảnh Fill trong Inspector
    [Range(0f, 1f)]
    public float fillAmount = 1f;

    // Gọi hàm này để cập nhật thanh
    public void SetProgress(float value)
    {
        fillAmount = Mathf.Clamp01(value); // Đảm bảo từ 0 đến 1
        fillImage.fillAmount = fillAmount;
    }
}
