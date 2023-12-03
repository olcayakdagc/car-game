using UnityEngine;

namespace UI
{
    public abstract class UIPanel : MonoBehaviour
    {
        [SerializeField] GameObject panel;
        public void Open()
        {
            panel.SetActive(true);
        }
        public void Close()
        {
            panel.SetActive(false);
        }
    }
}

