using MazeEscape.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MazeEscape.UI
{
    public class GameEndCanvas : CanvasBase
    {
        [SerializeField] private Button m_nextLevelButton;

        protected override void Awake()
        {
            base.Awake();
            m_nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
            OnCanvasShowFinished += () => Time.timeScale = 0;
        }

        public override void Initialize()
        {
            var gameEndObject = FindObjectsOfType<GameEndObject>();
            foreach (var gameEnd in gameEndObject)
            {
                gameEnd.OnGameEnd += Show;
            }
        }

        private void OnNextLevelButtonClicked()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}