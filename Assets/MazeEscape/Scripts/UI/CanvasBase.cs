using UnityEngine;
using UnityEngine.Events;

namespace MazeEscape.UI
{
    public abstract class CanvasBase : MonoBehaviour
    {
        public UnityAction OnCanvasShowFinished;
        public UnityAction OnCanvasHideFinished;

        [SerializeField] protected bool m_isVisibleOnAwake;

        protected CanvasGroup m_canvasGroup;

        public abstract void Initialize();

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
                OnCanvasShowFinished?.Invoke();
                return;
            }

            m_canvasGroup.alpha = 0f;
            LeanTween.cancel(m_canvasGroup.gameObject);
            LeanTween.alphaCanvas(m_canvasGroup, 1f, 0.5f).setEase(LeanTweenType.easeOutCubic).setOnComplete(() =>
            {
                OnCanvasShowFinished?.Invoke();
            });
        }

        public virtual void Hide()
        {
            if (!m_canvasGroup)
            {
                gameObject.SetActive(false);
                OnCanvasHideFinished?.Invoke();
                return;
            }

            m_canvasGroup.alpha = 1f;
            LeanTween.cancel(m_canvasGroup.gameObject);
            LeanTween.alphaCanvas(m_canvasGroup, 0f, 0.5f).setEase(LeanTweenType.easeOutCubic).setOnComplete(() =>
            {
                OnCanvasHideFinished?.Invoke();
            });
        }
    }
}