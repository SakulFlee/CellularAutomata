public interface IGridRoom
{
    public (uint, uint) GetPosition();

    public (uint, uint) GetMaxSize();

    public GridRoomType GetType();

    public bool IsInsideRoom((uint, uint) position)
    => position.Item1 >= GetPosition().Item1 &&
            position.Item2 >= GetPosition().Item2 &&
            position.Item1 < GetPosition().Item1 + GetMaxSize().Item1 &&
            position.Item2 < GetPosition().Item2 + GetMaxSize().Item2;

    public GridCell[,] Apply(GridCell[,] grid, bool findDoorways = true)
    {
        for (uint x = 0; x < GetMaxSize().Item1; x++)
            for (uint y = 0; y < GetMaxSize().Item2; y++)
            {
                var actualX = GetPosition().Item1 + x;
                var actualY = GetPosition().Item2 + y;

                if (IsInsideRoom((actualX, actualY)))
                {
                    // Is on an edge?
                    if (x == 0 || y == 0 || x == GetMaxSize().Item1 - 1 || y == GetMaxSize().Item2 - 1)
                    {
                        // If we are looking for doorways and the current tile is a floor
                        var cell = GridGenerator.GetCell(grid, (actualX, actualY), ((uint)grid.GetUpperBound(0), (uint)grid.GetUpperBound(1)));
                        if (findDoorways && cell != null && cell.Type == GridType.Floor)
                        {
                            // grid[actualX, actualY].Type = GridType.PossibleDoor;
                            if (x == 0)
                            {
                                var otherCell = GridGenerator.GetCell(grid, (actualX - 1, actualY), ((uint)grid.GetUpperBound(0), (uint)grid.GetUpperBound(1)));
                                if (otherCell != null && otherCell.Type == GridType.Floor)
                                {
                                    grid[actualX, actualY].Type = GridType.PossibleDoor;
                                }
                            }
                            else if (y == 0)
                            {
                                var otherCell = GridGenerator.GetCell(grid, (actualX, actualY - 1), ((uint)grid.GetUpperBound(0), (uint)grid.GetUpperBound(1)));
                                if (otherCell != null && otherCell.Type == GridType.Floor)
                                {
                                    grid[actualX, actualY].Type = GridType.PossibleDoor;
                                }
                            }
                            else if (x == GetMaxSize().Item1 - 1)
                            {
                                var otherCell = GridGenerator.GetCell(grid, (actualX + 1, actualY), ((uint)grid.GetUpperBound(0), (uint)grid.GetUpperBound(1)));
                                if (otherCell != null && otherCell.Type == GridType.Floor)
                                {
                                    grid[actualX, actualY].Type = GridType.PossibleDoor;
                                }
                            }
                            else if (y == GetMaxSize().Item2 - 1)
                            {
                                var otherCell = GridGenerator.GetCell(grid, (actualX, actualY + 1), ((uint)grid.GetUpperBound(0), (uint)grid.GetUpperBound(1)));
                                if (otherCell != null && otherCell.Type == GridType.Floor)
                                {
                                    grid[actualX, actualY].Type = GridType.PossibleDoor;
                                }
                            }
                        }
                        else
                        {
                            // ... Wall otherwise ...
                            grid[actualX, actualY].Type = GridType.Wall;
                        }
                    }
                    else
                    {
                        // Not on an edge, so floor
                        grid[actualX, actualY].Type = GridType.Floor;
                    }
                }
            }

        return grid;
    }
}