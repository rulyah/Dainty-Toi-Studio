using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ErrorPopup : Screen
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _confirmBtn;

        public Queue<string> errorQueue = new();
        public event Action onConfirm;

        public override void Show()
        {
            base.Show();
            _confirmBtn.onClick.AddListener(OnConfirm);
        }
    
        public void SetErrorMessages(string text)
        {
            _text.text = text;
        }

        public override void Hide()
        {
            _confirmBtn.onClick.RemoveListener(OnConfirm);
            base.Hide();
        }
    
        private void OnConfirm()
        {
            onConfirm?.Invoke();
        }
    }
}