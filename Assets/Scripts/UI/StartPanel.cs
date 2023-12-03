using Managers;
using UnityEngine;

namespace UI
{
    public class StartPanel : UIPanel
    {

        private void Awake()
        {
            GameEvents.onLoad += Open;
        }
        private void OnDestroy()
        {
            GameEvents.onLoad -= Open;
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.instance.gameState != GameStates.Start) return;
                GameManager.instance.GameStart();
                Close();
            }
        }
    }
}
