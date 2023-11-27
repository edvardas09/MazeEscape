using System.Collections.Generic;

namespace MazeEscape.MazeGenerator
{
    public interface IEntranceGeneration
    {
        public List<Passage> GetPassages();
    }
}