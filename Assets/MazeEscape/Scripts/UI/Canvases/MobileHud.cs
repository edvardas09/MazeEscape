using UnityEngine;

namespace MazeEscape.UI
{
    public class MobileHud : CanvasBase
    {
        public override void Initialize()
        {
            var isMobileApplication = Application.isMobilePlatform;
            m_isVisibleOnAwake = isMobileApplication;
            gameObject.SetActive(isMobileApplication);
        }
    }
}