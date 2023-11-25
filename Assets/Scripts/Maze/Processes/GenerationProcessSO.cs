using UnityEngine;

namespace MazeEscape.Maze
{
    public abstract class GenerationProcessSO : ScriptableObject
    {
        private MazeGenerator m_mazeGenerator;

        protected abstract void OnGenerate();

        public virtual void OnDrawGizmos()
        {
        }

        public void Generate(MazeGenerator mazeGenerator)
        {
            m_mazeGenerator = mazeGenerator;

            OnGenerate();
        }

        protected T GetProcess<T>() where T : class
        {
            return m_mazeGenerator.GenerationProcesses.Find(process => process is T) as T;
        }
    }
}