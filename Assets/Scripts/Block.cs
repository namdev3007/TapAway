using DG.Tweening;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Vector3 moveDirection;
    public Transform visualBlock;

    public Material correctMaterial;
    public Material incorrectMaterial;

    private IBlockObstacleChecker obstacleChecker;

    public Renderer visualRenderer;
    private Material originalMaterial;

    private void Awake()
    {
        obstacleChecker = new OverlapBoxChecker();
    }

    private void Start()
    {
        originalMaterial = visualRenderer.sharedMaterial;
        AppearEffect(5f, 0.5f);
    }

    public void Shake()
    {
        if (visualBlock != null)
        {
            visualBlock.DOShakePosition(0.3f, 0.2f, 10, 90f).SetEase(Ease.OutQuad);
        }
    }

    public void FlashMaterial(Material tempMaterial, float duration = 1f)
    {
        if (visualRenderer == null || tempMaterial == null || originalMaterial == null)
            return;

        visualRenderer.material = tempMaterial;

        DOVirtual.DelayedCall(duration, () =>
        {
            visualRenderer.material = originalMaterial;
        });
    }

    public void AppearEffect(float flyDistance = 5f, float duration = 0.5f)
    {
        if (visualBlock == null) return;

        Vector3 randomOffset = Random.onUnitSphere;
        randomOffset.y = Mathf.Abs(randomOffset.y);
        randomOffset.Normalize();
        randomOffset *= flyDistance;

        Vector3 targetPos = visualBlock.localPosition;

        visualBlock.localPosition = targetPos + randomOffset;
        visualBlock.localScale = Vector3.zero;

        visualBlock.DOLocalMove(targetPos, duration).SetEase(Ease.OutBack);
        visualBlock.DOScale(Vector3.one, duration).SetEase(Ease.OutBack);
    }

    public float GetAvailableMoveDistance()
    {
        return obstacleChecker.GetDistanceUntilBlocked(this);
    }

    public Block GetBlockedBlock()
    {
        return obstacleChecker.GetFirstBlockInDirection(this);
    }

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        Gizmos.color = Color.yellow;

        if (moveDirection == Vector3.zero) return;

        float step = 2f;
        int steps = 4; // Số bước kiểm tra
        Vector3 halfExtents = transform.localScale / 2f * 0.9f;

        for (int i = 1; i <= steps; i++)
        {
            Vector3 center = transform.position + moveDirection.normalized * step * i;
            Gizmos.DrawWireCube(center, halfExtents * 2);
        }
#endif
    }

}
