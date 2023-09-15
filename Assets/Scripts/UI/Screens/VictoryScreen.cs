using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class VictoryScreen : Screen
    {
        [SerializeField] private Button _restartBtn;
        [SerializeField] private Button _menuBtn;
    
        public event Action onRestart;
        public event Action onMenu;

        public override void Show()
        {
            base.Show();
            _restartBtn.onClick.AddListener(Restart);
            _menuBtn.onClick.AddListener(Menu);
        }

        public override void Hide()
        {
            _restartBtn.onClick.RemoveListener(Restart);
            _menuBtn.onClick.RemoveListener(Menu);
            base.Hide();
        }

        private void Restart()
        {
            onRestart?.Invoke();
        }
    
        private void Menu()
        {
            onMenu?.Invoke();
        }
    }
}