using DG.Tweening;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("Cài đặt di chuyển")]
    public Vector3 moveDirection = Vector3.right;
    public float moveDistance = 3f;
    public float moveDuration = 0.5f;
    public Transform visualBlock; // ✅ Di chuyển phần hiển thị

    [Header("Vật liệu phản hồi")]
    public Material trueMaterial;
    public Material falseMaterial;
    public Material waitForMovesMaterial;

    [Header("Chế độ đặc biệt")]
    public bool waitForMoves = false;
    public int requiredMovesBeforeActive = 0;
    public TextMeshPro[] countdownTexts;

    private bool isMoving = false;
    private bool canClick = true;
    public bool shouldCount = true;

    public Renderer blockRenderer;
    private Material originalMaterial;

    void Start()
    {
        if (blockRenderer != null)
            originalMaterial = blockRenderer.material;

        LevelManager.Instance?.RegisterBlock(this);
        UpdateCountdownDisplay();
    }

    void Update()
    {
        if (waitForMoves)
            UpdateCountdownDisplay();
    }

    public void UpdateCountdownDisplay()
    {
        int remaining = Mathf.Max(0, requiredMovesBeforeActive - GameManager.Instance.GetMoveCount());

        if (remaining > 0)
        {
            foreach (var text in countdownTexts)
            {
                if (text != null)
                {
                    text.gameObject.SetActive(true);
                    text.text = remaining.ToString();
                }
            }

            if (blockRenderer != null && waitForMovesMaterial != null)
                blockRenderer.material = waitForMovesMaterial;
        }
        else
        {
            foreach (var text in countdownTexts)
            {
                if (text != null)
                    text.gameObject.SetActive(false);
            }

            ResetMaterial();
        }
    }

    public bool IsMoving() => isMoving;
    public void SetMoving(bool value) => isMoving = value;

    public bool CanClick() => canClick;

    public void TemporarilyDisableClick(float delay)
    {
        canClick = false;
        Invoke(nameof(EnableClick), delay);
    }

    private void EnableClick()
    {
        canClick = true;
    }

    public void SetMaterialTrue()
    {
        if (blockRenderer != null && trueMaterial != null)
            blockRenderer.material = trueMaterial;
    }

    public void SetMaterialFalseTemporary()
    {
        if (blockRenderer != null && falseMaterial != null)
        {
            blockRenderer.material = falseMaterial;
            Invoke(nameof(ResetMaterial), 0.5f);
        }
    }

    public void ResetMaterial()
    {
        if (blockRenderer != null && originalMaterial != null)
            blockRenderer.material = originalMaterial;
    }

    public void PlayBounceBackEffect()
    {
        if (isMoving || visualBlock == null) return;

        Vector3 worldDirection = transform.TransformDirection(moveDirection.normalized);
        Vector3 originalPos = visualBlock.position;
        Vector3 bumpPos = originalPos + worldDirection * 0.3f;

        SetMaterialFalseTemporary();
        isMoving = true;

        visualBlock.DOMove(bumpPos, 0.15f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            visualBlock.DOMove(originalPos, 0.15f).SetEase(Ease.InQuad).OnComplete(() =>
            {
                isMoving = false;
            });
        });
    }

    public void DominoPush(Vector3 pushDirection, int depth = 0)
    {
        if (depth > 5 || visualBlock == null) return;

        float pushAmount = 0.3f;
        Vector3 targetPosition = visualBlock.position + pushDirection.normalized * pushAmount;
        Vector3 originalPos = visualBlock.position;

        isMoving = true;

        visualBlock.DOMove(targetPosition, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            visualBlock.DOMove(originalPos, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                isMoving = false;

                if (Physics.Raycast(transform.position, pushDirection, out RaycastHit hit, 1f))
                {
                    Block nextBlock = hit.collider.GetComponent<Block>();
                    if (nextBlock != null && !nextBlock.IsMoving())
                    {
                        nextBlock.DominoPush(pushDirection, depth + 1);
                    }
                }
            });
        });
    }

    public void DebugInfo() =>
        Debug.Log($"[BLOCK] {name} - MoveDirection: {moveDirection}, IsMoving: {isMoving}");

    public void LogStartMove() =>
        Debug.Log($"[BLOCK] {name} bắt đầu di chuyển.");

    public void LogBlocked(string colliderName) =>
        Debug.LogWarning($"[BLOCK] {name} bị chặn bởi: {colliderName}");

    public void LogDestroyed() =>
        Debug.Log($"[BLOCK] {name} đã hoàn tất di chuyển và bị xoá.");
}
