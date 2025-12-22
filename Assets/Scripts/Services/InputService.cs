using System;
using Services.Interfaces;
using UnityEngine;

namespace Services {
    public class InputService : MonoBehaviour, IInputService{
        public event Action OnClearRequested;
        public event Action<int> OnSizeChangeRequested;
        public event Action OnPlayPauseRequested;
        public event Action OnResetRequested;
        
        private void Update() {
            if(Input.GetKeyDown(KeyCode.Space)) OnPlayPauseRequested?.Invoke();
            if(Input.GetKeyDown(KeyCode.R)) OnResetRequested?.Invoke();
            if(Input.GetKeyDown(KeyCode.C)) OnClearRequested?.Invoke();
        }
    }
}