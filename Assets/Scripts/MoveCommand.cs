using UnityEngine;
using DG.Tweening;

public class MoveCommand
{
    private Block block;
    private float maxMoveDistance = 5f;
    private float moveDuration = 0.5f;

    public MoveCommand(Block block)
    {
        this.block = block;
    }

    public void Execute()
    {
        if (block == null || !LevelManager.Instance.canInteract) return;

        LevelManager.Instance.canInteract = false;

        Vector3 startPos = block.transform.position;
        float allowedDistance = block.GetAvailableMoveDistance();
        Vector3 targetPos = startPos + block.moveDirection * allowedDistance;

        if (allowedDistance >= maxMoveDistance)
        {
            Vector3 finalPos = startPos + block.moveDirection * maxMoveDistance;

            block.transform.DOMove(finalPos, moveDuration)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    Collider col = block.GetComponent<Collider>();
                    if (col != null)
                        col.enabled = false;

                    GameObject.Destroy(block.gameObject);
                    LevelManager.Instance.CheckWinCondition();

                    AudioManager.Instance.PlaySFX(0); // 🔊 phát âm thanh khi move thành công

                    DOVirtual.DelayedCall(0.6f, () => LevelManager.Instance.canInteract = true);
                });
        }
        else
        {
            float moveTime = moveDuration * (allowedDistance / maxMoveDistance);
            Block blockedBlock = block.GetBlockedBlock();

            block.transform.DOMove(targetPos, moveTime)
                .SetEase(Ease.InCubic)
                .OnComplete(() =>
                {
                    block.transform.DOMove(startPos, moveTime)
                        .SetEase(Ease.OutCubic);

                    block.Shake();
                    block.FlashMaterial(block.incorrectMaterial);
                    blockedBlock?.Shake();

                    AudioManager.Instance.PlaySFX(0); // 🔊 phát âm thanh khi move thất bại

                    DOVirtual.DelayedCall(0.6f, () => LevelManager.Instance.canInteract = true);
                });
        }
    }
}
