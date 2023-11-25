using System.Collections.Generic;

namespace MazeEscape.Maze
{
    public interface ICellGeneration
    {
        public List<Cell> GetCells();
    }
}