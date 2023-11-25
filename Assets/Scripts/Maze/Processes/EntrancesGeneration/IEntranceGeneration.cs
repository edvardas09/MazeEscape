using System.Collections.Generic;

namespace MazeEscape.Maze
{
    public interface IEntranceGeneration
    {
        public List<Passage> GetPassages();
    }
}