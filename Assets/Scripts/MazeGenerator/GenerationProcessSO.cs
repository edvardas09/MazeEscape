using UnityEngine;

namespace MazeEscape.MazeGenerator
{
    public abstract class GenerationProcessSO : ScriptableObject
    {
        protected MazeGenerator m_mazeGenerator;

        // returns true if the process was successful
        protected abstract bool OnGenerate();

        public virtual void OnDrawGizmos()
        {
        }

        public bool Generate(MazeGenerator mazeGenerator)
        {
            m_mazeGenerator = mazeGenerator;

            return OnGenerate();
        }

        protected bool TryGetProcess<T>(out T process) where T : class
        {
            return m_mazeGenerator.TryGetProcess(out process);
        }
    }
}