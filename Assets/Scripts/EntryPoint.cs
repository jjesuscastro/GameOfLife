using Services.Interfaces;
using VContainer.Unity;

namespace DefaultNamespace {
    public class EntryPoint : IStartable {
        private readonly IGridSimulationService gridSimulationService;
        private readonly IInputService inputService;
        
        public EntryPoint(IGridSimulationService gridSimulationService, IInputService inputService) {
            this.gridSimulationService = gridSimulationService;
            this.inputService = inputService;
        }

        public void Start() {
            this.gridSimulationService.Start();
            this.inputService.OnPlayPauseRequested += this.gridSimulationService.ToggleSimulation;
        }
    }
}