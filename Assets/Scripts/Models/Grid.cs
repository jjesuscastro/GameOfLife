using System;
using Models.Interfaces;

namespace Models {
    public class Grid : IGrid {
        private int[] cells;
        public int[] Cells => this.cells;

        private int width;
        
        public Grid(int width, int height) {
            this.cells = new int[width * height];
            this.width = width;
        }

        public void SetCell(int x, int y, int value) {
            this.cells[y * this.width + x] = value;
        }

        public void ClearGrid() {
            Array.Clear(this.cells, 0, this.cells.Length);
        }
    }
}