using UnityEngine;

namespace MazeEscape.MazeGenerator
{
    public abstract class GenerationProcessSO : ScriptableObject
    {
        protected MazeGenerator m_mazeGenerator;

        public abstract bool IsCompleted();
        public abstract void Reset();

        // returns true if the process was successful
        protected abstract bool OnGenerate();

        public virtual void OnDrawGizmos()
        {
        }

        public bool Generate(MazeGenerator mazeGenerator)
        {
            m_mazeGenerator = mazeGenerator;

            var result = OnGenerate();

#if UNITY_EDITOR

            if (result)
            {
                UnityEditor.EditorUtility.SetDirty(mazeGenerator);
                UnityEditor.AssetDatabase.SaveAssets();
            }            

#endif

            return result;
        }
    }
}