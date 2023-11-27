using MazeEscape.Gameplay.Characters;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MazeEsacpe.UI
{
    public class PlayerDiedCanvas : CanvasBase
    {
        [SerializeField] private Button m_retryButton;

        protected override void Awake()
        {
            m_retryButton.onClick.AddListener(OnRetryButtonClicked);

            base.Awake();
        }

        public override void Show()
        {
            base.Show();

            Time.timeScale = 0;
        }

        public override void Initialize()
        {
            var player = FindObjectOfType<Player>();
            player.OnPlayerDied += Show;
        }

        private void OnRetryButtonClicked()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}