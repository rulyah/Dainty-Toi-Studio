using UnityEngine;

namespace UI
{
    public abstract class Screen : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvas;
        [SerializeField] private ScreenTypes _type;
        public ScreenTypes type => _type;
    
        public virtual void Show()
        {
            _canvas.alpha = 1;
            _canvas.interactable = true;
            _canvas.blocksRaycasts = true;
        }
    
        public virtual void Hide()
        {
            _canvas.alpha = 0;
            _canvas.interactable = false;
            _canvas.blocksRaycasts = false;
        }
    }
}
