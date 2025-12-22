using System;
using Services.Interfaces;
using UnityEngine;

namespace Services {
    public class InputService : MonoBehaviour, IInputService{
        public event Action<bool> OnClearRequested;
        public event Action<int> OnSizeChangeRequested;
        public event Action OnPlayPauseRequested;
        public event Action OnResetRequested;
        
        private void Update() {
            if(Input.GetKeyDown(KeyCode.Space)) OnPlayPauseRequested?.Invoke();
        }
    }
}