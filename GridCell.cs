public class GridCell
{
    public bool IsFloor { get; set; }
    public uint Area { get; set; } = 0;
    public bool CanBeDoor { get; set; } = false;

    // TODO: Add room code

    public GridCell(bool isFloor)
    {
        IsFloor = isFloor;
    }

    public string GetCellString()
    {
        if (!IsFloor)
            if (CanBeDoor)
                return "▒▒";
            else
                return "██";

        if (Area < 10)
            return $"0{Area}";

        return $"{Area}";
    }

    public bool HasAreaData()
    {
        return Area > 0;
    }

    public override string ToString()
    {
        return $"GridCell [is Floor = {IsFloor}, Area = {Area}, can be Door = {CanBeDoor}]";
    }
}
