public class GridCell
{
    public GridType Type { get; set; }
    public uint Area { get; set; } = 0;

    public GridCell(GridType type)
    {
        Type = type;
    }

    public GridCell(GridCell gridCell)
    {
        Type = gridCell.Type;
        Area = gridCell.Area;
    }

    public string MakeConsoleString(int size = 2)
    {
        var cellChar = Type.GetChar();

        var output = "";

        for (var i = 0; i < size; i++)
            output += cellChar;

        return output;
    }

    public bool HasAreaData()
    {
        return Area > 0;
    }

    public override string ToString()
    {
        return $"GridCell [Type = {Type}, Area = {Area}]";
    }
}
