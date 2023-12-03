using Managers;

namespace UI
{
    public class LoadingPanel : UIPanel
    {
        private void Awake()
        {
            GameEvents.onLoad += Open;
            GameEvents.onLoadEnd += Close;
        }
        private void OnDestroy()
        {
            GameEvents.onLoad -= Open;
            GameEvents.onLoadEnd -= Close;
        }
    }
}

