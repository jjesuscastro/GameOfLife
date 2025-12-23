using Models.Interfaces;

namespace Services.Interfaces {
    public interface IMeshService {
        void UpdateMatrices(IGrid grid, int width, int height);
        void Render();
    }
}