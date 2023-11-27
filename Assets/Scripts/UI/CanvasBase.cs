using UnityEngine;

namespace MazeEsacpe.UI
{
    public abstract class CanvasBase : MonoBehaviour
    {
        [SerializeField] protected bool m_isVisibleOnAwake;

        protected CanvasGroup m_canvasGroup;

        protected virtual void Awake()
        {
            m_canvasGroup = GetComponent<CanvasGroup>();

            if (m_isVisibleOnAwake)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        public virtual void Show()
        {
            if (!m_canvasGroup)
            {
                gameObject.SetActive(true);
                return;
            }

            LeanTween.alphaCanvas(m_canvasGroup, 1f, 0.5f).setEase(LeanTweenType.easeOutCubic);
        }

        public virtual void Hide()
        {
            if (!m_canvasGroup)
            {
                gameObject.SetActive(false);
                return;
            }

            LeanTween.alphaCanvas(m_canvasGroup, 0f, 0.5f).setEase(LeanTweenType.easeOutCubic);
        }
    }
}