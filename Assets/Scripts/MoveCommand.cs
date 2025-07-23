using DG.Tweening;
using UnityEngine;

public class MoveCommand
{
    private Block block;

    public MoveCommand(Block block)
    {
        this.block = block;
    }

    public void Execute()
    {
        if (block == null || block.IsMoving() || !block.CanClick()) return;

        // 🔒 Kiểm tra lượt di chuyển yêu cầu
        if (block.waitForMoves && GameManager.Instance.GetMoveCount() < block.requiredMovesBeforeActive)
        {
            Debug.LogWarning($"⛔ {block.name} chưa thể di chuyển! Cần ít nhất {block.requiredMovesBeforeActive} lượt.");
            block.PlayBounceBackEffect();
            block.TemporarilyDisableClick(1f); // ⏳ Chặn click lại trong 1s
            return;
        }

        Vector3 worldDirection = block.transform.TransformDirection(block.moveDirection.normalized);

        // ✅ Không có vật cản
        if (!Physics.Raycast(block.transform.position, worldDirection, out RaycastHit hit, 1f))
        {
            block.LogStartMove();
            block.SetMaterialTrue();
            block.SetMoving(true);

            Vector3 targetPos = block.visualBlock.position + worldDirection * block.moveDistance;

            block.visualBlock.DOMove(targetPos, block.moveDuration)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    if (block == null) return;

                    block.SetMoving(false);
                    block.LogDestroyed();

                    GameManager.Instance?.IncrementMove();
                    LevelManager.Instance?.UnregisterBlock(block);

                    Object.Destroy(block.gameObject);
                });
        }
        else
        {
            // 🚫 Có vật cản
            block.LogBlocked(hit.collider.name);
            block.PlayBounceBackEffect();
            block.TemporarilyDisableClick(1f); // ⏳ Chặn click lại trong 1s

            if (hit.collider.TryGetComponent<Block>(out Block hitBlock))
            {
                hitBlock.DominoPush(worldDirection);
            }
        }

        block.DebugInfo();
    }
}
