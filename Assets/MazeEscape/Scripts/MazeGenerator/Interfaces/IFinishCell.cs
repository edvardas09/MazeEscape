using System.Collections.Generic;

namespace MazeEscape.MazeGenerator
{
    public interface IFinishCell
    {
        public List<Cell> GetFinishCells();
    }
}