public enum GridType
{
    Floor,
    Wall,
    PossibleDoor
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
            case GridType.PossibleDoor:
                return '░';
            default:
                return '╳';
        }
    }
}
