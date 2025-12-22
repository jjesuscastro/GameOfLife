using UnityEngine;

namespace Models.Interfaces {
    public interface IGrid {
        int[] Cells { get; }
        int Width { get; }
        int Height { get; }

        int GetCell(int x, int y);
        void SetCell(int x, int y, int cell);
    }
}