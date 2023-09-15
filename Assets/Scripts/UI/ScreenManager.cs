using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class ScreenManager : MonoBehaviour
    {
        private ErrorPopup _errorPopup;
        private List<Screen> _screens = new();
        private Stack<Screen> _screenStack = new();
        //private Queue<string> _errorQueue = new();
    
        private void Awake()
        {
            var popupPrefab = Resources.Load<ErrorPopup>("Popups/Popup");
            _errorPopup = Instantiate(popupPrefab, transform);
        
            var screens = Resources.LoadAll<Screen>("Screens");
            Debug.Log("Loaded screens: " + screens.Length);
    
            foreach (var screen in screens)
            {
                var instance = Instantiate(screen, transform);
                _screens.Add(instance);
            }
        }

        public void OpenScreen(ScreenTypes type)
        {
            var screen = _screens.Find(n => n.type == type);
            screen.Show();
            _screenStack.Push(screen);
        }

        public void CloseLastScreen()
        {
            if (_screenStack.Count <= 0) return;
            _screenStack.Pop().Hide();
        }

        public void ShowPopup(string text)
        {
            _errorPopup.errorQueue.Enqueue(text);

            if (_errorPopup.errorQueue.Count == 1)
            {
                _errorPopup.Show();
                _errorPopup.SetErrorMessages(text);
                _errorPopup.onConfirm += HideErrorPopup;
            }
        }
    
        public void HideErrorPopup()
        {
            _errorPopup.onConfirm -= HideErrorPopup;
            _errorPopup.errorQueue.Dequeue();
            _errorPopup.Hide();

        
            if (_errorPopup.errorQueue.Count > 0)
            {
                _errorPopup.Show();
                _errorPopup.SetErrorMessages(_errorPopup.errorQueue.Peek());
                _errorPopup.onConfirm += HideErrorPopup;
            }
        }
    }
}