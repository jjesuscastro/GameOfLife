using Models.Interfaces;
using ScriptableObjects;
using Services.Interfaces;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

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
        /// Counts the neighbors and updates the next generation of the grid
        /// using Burst Compiler
        /// </summary>
        /// <param name="grid">
        /// Reference to the current grid
        /// </param>
        public void NextGeneration(IGrid grid) {
            int length = grid.Cells.Length;
            NativeArray<int> currentCells = new NativeArray<int>(grid.Cells, Allocator.TempJob);
            NativeArray<int> nextCells = new NativeArray<int>(length, Allocator.TempJob);

            SimulationJob job = new SimulationJob {
                currentCells = currentCells,
                nextCells = nextCells,
                width = this.variables.width,
                height = this.variables.height
            };

            JobHandle handle = job.Schedule(length, 64);
            handle.Complete();

            nextCells.CopyTo(grid.Cells);

            currentCells.Dispose();
            nextCells.Dispose();
        }
        
        [BurstCompile]
        private struct SimulationJob : IJobParallelFor {
            [ReadOnly] public NativeArray<int> currentCells;
            public NativeArray<int> nextCells;
            public int width;
            public int height;

            public void Execute(int index) {
                int y = index / this.width;
                int x = index % this.width;
                int neighbors = 0;

                // For loops only work for -1 and 1
                // Ignore i == 0 and j == 0 because that is the current cell
                for (int i = -1; i <= 1; i++) {
                    for (int j = -1; j <= 1; j++) {
                        if (i == 0 && j == 0) continue;

                        int neighborX = x + i;
                        int neighborY = y + j;

                        if (neighborX >= 0 && neighborX < this.width && neighborY >= 0 && neighborY < this.height) {
                            neighbors += this.currentCells[neighborY * this.width + neighborX];
                        }
                    }
                }

                int currentState = this.currentCells[index];
                if (currentState == 1) {
                    this.nextCells[index] = (neighbors == 2 || neighbors == 3) ? 1 : 0;
                } else {
                    this.nextCells[index] = (neighbors == 3) ? 1 : 0;
                }
            }
        }
    }
}