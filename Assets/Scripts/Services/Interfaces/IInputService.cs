using System;

namespace Services.Interfaces {
    public interface IInputService {
        event Action OnClearRequested;
        event Action<int> OnSizeChangeRequested;
        event Action OnPlayPauseRequested;
        event Action OnResetRequested;
    }
}