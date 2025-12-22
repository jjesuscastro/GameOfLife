using Models.Interfaces;
using UnityEngine;

namespace Models {
    public class Grid : IGrid {
        private int[] cells;
        public int[] Cells => this.cells;

        private int width;
        public int Width => this.width;
        private int height;
        public int Height => this.height;
        
        public Grid(int width, int height) {
            this.cells = new int[width * height];
            this.width = width;
            this.height = height;
        }

        public int GetCell(int x, int y) {
            return this.cells[y * this.width + x];
        }

        public void SetCell(int x, int y, int value) {
            this.cells[y * this.width + x] = value;
        }
    }
}