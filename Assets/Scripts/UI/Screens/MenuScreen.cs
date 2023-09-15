using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class MenuScreen : Screen
    {
        [SerializeField] private Button _playBtn;
        [SerializeField] private Button _exitBtn;
    
        public event Action onPlay;
        public event Action onExit;

        public override void Show()
        {
            base.Show();
            _playBtn.onClick.AddListener(Play);
            _exitBtn.onClick.AddListener(Exit);
        }

        public override void Hide()
        {
            _playBtn.onClick.RemoveListener(Play);
            _exitBtn.onClick.RemoveListener(Exit);
            base.Hide();
        }

        private void Play()
        {
            onPlay?.Invoke();
        }
    
        private void Exit()
        {
            onExit?.Invoke();
        }
    
    }
}