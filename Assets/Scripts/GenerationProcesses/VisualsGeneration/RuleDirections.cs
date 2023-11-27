using System;

namespace MazeEscape.GenerationProcesses
{
    [Flags]
    public enum RuleDirections
    {
        None = 0,
        North = 1,
        East = 2,
        South = 4,
        West = 8
    }
}