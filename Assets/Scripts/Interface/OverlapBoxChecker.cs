using UnityEngine;

public class OverlapBoxChecker : IBlockObstacleChecker
{
    private float blockSize = 2f;
    private int maxSteps = 4;

    public float GetDistanceUntilBlocked(Block block)
    {
        Vector3 start = block.transform.position;
        Vector3 dir = block.moveDirection;
        Vector3 halfExtents = block.transform.localScale / 2f * 0.9f;

        for (int i = 1; i <= maxSteps; i++)
        {
            Vector3 checkPos = start + dir * blockSize * i;
            Collider[] hits = Physics.OverlapBox(checkPos, halfExtents);
            foreach (var hit in hits)
            {
                if (hit.gameObject != block.gameObject && hit.GetComponent<Block>())
                {
                    return blockSize * (i - 1);
                }
            }
        }

        return blockSize * maxSteps;
    }

    public Block GetFirstBlockInDirection(Block block)
    {
        Vector3 start = block.transform.position;
        Vector3 dir = block.moveDirection;
        Vector3 halfExtents = block.transform.localScale / 2f * 0.9f;

        for (int i = 1; i <= maxSteps; i++)
        {
            Vector3 checkPos = start + dir * blockSize * i;
            Collider[] hits = Physics.OverlapBox(checkPos, halfExtents);
            foreach (var hit in hits)
            {
                Block other = hit.GetComponent<Block>();
                if (other != null && other != block)
                    return other;
            }
        }

        return null;
    }
}
