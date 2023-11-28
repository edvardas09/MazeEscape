using System.Collections.Generic;

namespace MazeEscape.MazeGenerator
{
    public interface IStartCell
    {
        public List<Cell> GetStartCells();
    }
}