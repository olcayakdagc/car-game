using Managers;
using System.Collections;
using UnityEngine;


namespace UI
{
    public class FailPanel : UIPanel
    {

        private void Start()
        {
            GameEvents.onFail += WaitForScreen;
            GameEvents.onLoad += Close;
        }
        private void OnDestroy()
        {
            GameEvents.onFail -= WaitForScreen;
            GameEvents.onLoad -= Close;
        }
        private void WaitForScreen()
        {
            StartCoroutine(WaitFOrScreenEnum());
        }
        IEnumerator WaitFOrScreenEnum()
        {
            yield return new WaitForSeconds(0.5f);
            Open();
        }

    }
}
