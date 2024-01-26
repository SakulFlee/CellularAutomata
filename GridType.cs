public enum GridType
{
    Floor,
    Wall,
}

public static class GridTypeImpl
{
    public static char GetChar(this GridType type)
    {
        switch (type)
        {
            case GridType.Floor:
                return ' ';
            case GridType.Wall:
                return '█';
            default:
                return '╳';
        }
    }
}
