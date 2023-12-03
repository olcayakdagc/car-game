using Managers;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class WinPanel : UIPanel
    {
        private void Start()
        {
            GameEvents.onWin += WaitForScreen;
            GameEvents.onLoad += Close;
        }
        private void OnDestroy()
        {
            GameEvents.onWin -= WaitForScreen;
            GameEvents.onLoad -= Close;
        }
        private void WaitForScreen()
        {
            StartCoroutine(WaitFOrScreenEnum());
        }
        IEnumerator WaitFOrScreenEnum()
        {
            yield return new WaitForSeconds(1);
            Open();
        }
    }
}

