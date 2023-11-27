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

        private void OnNextLevelButtonClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}