using Models.Interfaces;

namespace Services.Interfaces {
    public interface IPopulationService {
        void NextGeneration(IGrid grid);
    }
}