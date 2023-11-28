using MazeEscape.Gameplay.Characters;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MazeEscape.UI
{
    public class PlayerDiedCanvas : CanvasBase
    {
        [SerializeField] private Button m_retryButton;

        protected override void Awake()
        {
            base.Awake();
            m_retryButton.onClick.AddListener(OnRetryButtonClicked);
            OnCanvasShowFinished += () => Time.timeScale = 0;
        }

        public override void Initialize()
        {
            var player = FindObjectOfType<Player>();
            if (player == null)
            {
                return;
            }

            player.OnPlayerDied += Show;
        }

        private void OnRetryButtonClicked()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}