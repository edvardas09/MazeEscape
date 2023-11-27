using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MazeEsacpe.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private List<CanvasBase> m_canvasBases;

        private void Awake()
        {
            m_canvasBases = FindObjectsOfType<CanvasBase>(true).ToList();
        }

        private void Start()
        {
            foreach (var canvasBase in m_canvasBases)
            {
                canvasBase.Initialize();
            }
        }

        public T ShowCanvas<T>() where T : CanvasBase
        {
            var canvasBase = m_canvasBases.Find(x => x is T);
            canvasBase.Show();
            return canvasBase as T;
        }

        public T HideCanvas<T>() where T : CanvasBase
        {
            var canvasBase = m_canvasBases.Find(x => x is T);
            canvasBase.Hide();
            return canvasBase as T;
        }
    }
}