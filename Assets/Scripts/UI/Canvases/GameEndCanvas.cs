using MazeEscape.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MazeEsacpe.UI
{
    public class GameEndCanvas : CanvasBase
    {
        [SerializeField] private Button m_nextLevelButton;

        protected override void Awake()
        {
            m_nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);

            base.Awake();
        }

        public override void Show()
        {
            base.Show();

            Time.timeScale = 0;
        }

        public override void Initialize()
        {
            var gameEndObject = FindObjectOfType<GameEndObject>();
            gameEndObject.OnGameEnd += Show;
        }

        private void OnNextLevelButtonClicked()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}