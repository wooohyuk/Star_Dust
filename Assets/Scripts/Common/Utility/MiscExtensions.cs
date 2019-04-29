using Common.Types;

public static class TeamExtensions
{
    public static TeamType GetOpposite(this TeamType team)
    {
        switch (team)
        {
            case TeamType.Black:
                return TeamType.White;
            case TeamType.White:
                return TeamType.Black;
            default:
                return TeamType.Gray;
        }
    }
}