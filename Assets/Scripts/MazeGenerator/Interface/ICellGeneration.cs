using System.Collections.Generic;

namespace MazeEscape.MazeGenerator
{
    public interface ICellGeneration
    {
        public List<Cell> GetCells();
    }
}