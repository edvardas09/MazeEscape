namespace MazeEsacpe.UI
{
    public class GameEndCanvas : CanvasBase
    {
        public override void Hide()
        {
            gameObject.SetActive(false);
        }

        public override void Show()
        {
            gameObject.SetActive(true);
        }
    }
}