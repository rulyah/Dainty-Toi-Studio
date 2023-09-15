using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class ScreenManager : MonoBehaviour
    {
        public static ScreenManager instance { get; private set; }
        //private Popup _popup;
        private List<Screen> _screens = new();
        private Stack<Screen> _screenStack = new();
        //private Queue<string> _errorQueue = new();
    
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            
            var screens = Resources.LoadAll<Screen>("Screens");
    
            foreach (var screen in screens)
            {
                var instance = Instantiate(screen, transform);
                _screens.Add(instance);
            }
            
            var popupPrefab = Resources.Load<Popup>("Popups/Popup");
            var popup = Instantiate(popupPrefab, transform);
            _screens.Add(popup);
        }

        public Screen OpenScreen(ScreenTypes type)
        {
            var screen = _screens.Find(n => n.type == type);
            screen.Show();
            _screenStack.Push(screen);
            return screen;
        }
        
        public Screen GetCurrentScreen()
        {
            return _screenStack.Peek();
        }

        public void CloseLastScreen()
        {
            if (_screenStack.Count <= 0) return;
            _screenStack.Pop().Hide();
        }

        /*public void ShowPopup(string text)
        {
            _popup.errorQueue.Enqueue(text);

            if (_popup.errorQueue.Count == 1)
            {
                _popup.Show();
                _popup.SetErrorMessages(text);
                _popup.onConfirm += HidePopup;
            }
        }
    
        public void HidePopup()
        {
            _popup.onConfirm -= HidePopup;
            _popup.errorQueue.Dequeue();
            _popup.Hide();

        
            if (_popup.errorQueue.Count > 0)
            {
                _popup.Show();
                _popup.SetErrorMessages(_popup.errorQueue.Peek());
                _popup.onConfirm += HidePopup;
            }
        }*/
    }
}