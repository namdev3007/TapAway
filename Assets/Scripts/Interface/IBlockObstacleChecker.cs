public interface IBlockObstacleChecker
{
    float GetDistanceUntilBlocked(Block block);
    Block GetFirstBlockInDirection(Block block); // mới thêm
}
