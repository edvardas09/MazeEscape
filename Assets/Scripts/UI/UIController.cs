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

        public void ShowCanvas<T>() where T : CanvasBase
        {
            var canvasBase = m_canvasBases.Find(x => x is T);
            canvasBase.Show();
        }

        public void HideCanvas<T>() where T : CanvasBase
        {
            var canvasBase = m_canvasBases.Find(x => x is T);
            canvasBase.Hide();
        }
    }
}