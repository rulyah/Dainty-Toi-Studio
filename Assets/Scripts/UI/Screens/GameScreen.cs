using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class GameScreen : Screen
    {
        [SerializeField] private Button _menuBtn;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _count;
        
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
        
        public void SetTaskCount(int count)
        {
            _count.text = count.ToString();
        }

        public void SetTaskImg(Sprite sprite)
        {
            _image.sprite = sprite;
        }
        
        private void Menu()
        {
            onMenu?.Invoke();
        }
    }
}