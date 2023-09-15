using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Popup : Screen
    {
        //[SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _confirmBtn;
        [SerializeField] private Button _cancelBtn;
        
        //public Queue<string> errorQueue = new();
        public event Action onConfirm;
        public event Action onCancel;

        public override void Show()
        {
            base.Show();
            _confirmBtn.onClick.AddListener(OnConfirm);
            _cancelBtn.onClick.AddListener(OnCancel);
        }
    
        /*public void SetErrorMessages(string text)
        {
            _text.text = text;
        }*/

        public override void Hide()
        {
            _confirmBtn.onClick.RemoveListener(OnConfirm);
            _cancelBtn.onClick.RemoveListener(OnCancel);
            base.Hide();
        }
    
        private void OnConfirm()
        {
            onConfirm?.Invoke();
        }
        
        private void OnCancel()
        {
            onCancel?.Invoke();
        }
    }
}