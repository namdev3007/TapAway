using DG.Tweening;
using UnityEngine;

public class UIAnimator : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public void ShowUI(float duration = 0.5f)
    {
        canvasGroup.DOFade(1f, duration).SetEase(Ease.OutQuad);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void HideUI(float duration = 0.3f)
    {
        canvasGroup.DOFade(0f, duration).SetEase(Ease.InQuad);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
