public class GridRoomRectangular : IGridRoom
{
    public (uint, uint) Position { get; private set; }
    public (uint, uint) MaxSize { get; private set; }

    public GridRoomRectangular((uint, uint) position, (uint, uint) maxSize)
    {
        Position = position;
        MaxSize = maxSize;
    }

    GridRoomType IGridRoom.GetType() => GridRoomType.Rectangular;

    public (uint, uint) GetPosition() => Position;

    public (uint, uint) GetMaxSize() => MaxSize;
}
