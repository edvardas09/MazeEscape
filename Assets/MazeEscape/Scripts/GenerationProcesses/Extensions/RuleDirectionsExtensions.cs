namespace MazeEscape.GenerationProcesses
{
    public static class RuleDirectionsExtensions
    {
        public static bool Compare(this RuleDirections directions, RuleDirections other)
        {
            if ((int)directions == -1)
            {
                directions = RuleDirections.West | RuleDirections.South | RuleDirections.East | RuleDirections.North;
            }

            return directions == other;
        }
    }
}