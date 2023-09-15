using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class LoseScreen : Screen
    {
        [SerializeField] private Button _menuBtn;
    
        public event Action onMenu;

        public override void Show()
        {
            base.Show();
            _menuBtn.onClick.AddListener(Menu);
        }

        public override void Hide()
        {
            _menuBtn.onClick.RemoveListener(Menu);
            base.Hide();
        }

        private void Menu()
        {
            onMenu?.Invoke();
        }
    }
}