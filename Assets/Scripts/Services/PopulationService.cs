using Models.Interfaces;
using ScriptableObjects;
using Services.Interfaces;

namespace Services {
    public class PopulationService : IPopulationService {
        private Variables variables;
        private int[] neighborsGrid;
        private int population;

        public PopulationService(Variables variables) {
            this.variables = variables;
            this.neighborsGrid = new int[this.variables.width * this.variables.height];
        }
        
        /// <summary>
        /// Get the next generation of the grid
        /// </summary>
        /// <param name="grid"></param>
        public void NextGeneration(IGrid grid) {
            int requiredSize = this.variables.width * this.variables.height;
            if (this.neighborsGrid == null || this.neighborsGrid.Length != requiredSize) {
                this.neighborsGrid = new int[requiredSize];
            }
            
            CountNeighbors(grid);
            ControlPopulation(grid);
        }

        /// <summary>
        /// Count the neighbors of each cell in the grid
        /// </summary>
        /// <param name="grid"></param>
        private void CountNeighbors(IGrid grid) {
            int[] cells = grid.Cells;
            int width = this.variables.width;
            int height = this.variables.height;

            for (int y = 0; y < height; y++) {
                int yIndex = y * width;
                int yUpIndex = (y + 1) * width;
                int yDownIndex = (y - 1) * width;
                
                bool hasUp = (y + 1) < height;
                bool hasDown = (y - 1) >= 0;

                for (int x = 0; x < width; x++) {
                    int neighbors = 0;
                    int xMinus = x - 1;
                    int xPlus = x + 1;
                    bool hasLeft = xMinus >= 0;
                    bool hasRight = xPlus < width;

                    // Row above
                    if (hasUp) {
                        if (hasLeft) neighbors += cells[yUpIndex + xMinus];
                        neighbors += cells[yUpIndex + x];
                        if (hasRight) neighbors += cells[yUpIndex + xPlus];
                    }

                    // Current row
                    if (hasLeft) neighbors += cells[yIndex + xMinus];
                    if (hasRight) neighbors += cells[yIndex + xPlus];

                    // Row below
                    if (hasDown) {
                        if (hasLeft) neighbors += cells[yDownIndex + xMinus];
                        neighbors += cells[yDownIndex + x];
                        if (hasRight) neighbors += cells[yDownIndex + xPlus];
                    }
                    
                    this.neighborsGrid[yIndex + x] = neighbors;
                }
            }
        }
        
        /// <summary>
        /// Update the population of the grid
        /// based on the neighbors <br />
        /// <list type="bullet">
        /// <item>
        /// Any live cell with fewer than two live neighbors dies, as if by underpopulation.<br />
        /// </item>
        /// <item>
        /// Any live cell with two or three live neighbors lives on to the next generation.<br />
        /// </item>
        /// <item>
        /// Any live cell with more than three live neighbors dies, as if by overpopulation.<br />
        /// </item>
        /// <item>
        /// Any dead cell with exactly three live neighbors becomes a live cell, as if by reproduction.
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="grid"></param>
        private void ControlPopulation(IGrid grid) {
            this.population = 0;
            int[] cells = grid.Cells;
            int length = cells.Length;
            
            for (int i = 0; i < length; i++) {
                int cell = cells[i];
                int neighbors = this.neighborsGrid[i];

                if (cell == 1) {
                    if (neighbors < 2 || neighbors > 3) {
                        cells[i] = 0;
                    } else {
                        this.population++;
                    }
                } else if (neighbors == 3) {
                    cells[i] = 1;
                    this.population++;
                }
            }
        }
    }
}