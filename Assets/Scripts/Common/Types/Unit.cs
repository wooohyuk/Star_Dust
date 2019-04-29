namespace Common.Types
{
    public enum TeamType
    {
        Black,
        White,
        Gray,
    }

    public enum UnitType
    {
        Undefined = 0,
        Footman,
        Hero,
        Castle,
    }

    public enum UnitSyncType
    {
        Undefined = 0,
        Hp,
        MoveSpeed,
        Position,
        Direction,
        Dead,
    }
}