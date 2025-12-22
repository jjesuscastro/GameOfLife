using Models;
using Models.Interfaces;
using ScriptableObjects;
using Services.Interfaces;
using UnityEngine;
using VContainer.Unity;
using Views;

namespace Services {
    public class GridSimulationService : IGridSimulationService, ITickable {

        private Variables variables;
        private IGrid grid;
        
        private IObjectPoolService<CellView> objectPoolService;
        private IPopulationService populationService;

        private int generation = 1;
        private float timer;
        private bool isPlaying;
        
        private CellView[] cellViews;

        public GridSimulationService(Variables variables, IGrid grid, IObjectPoolService<CellView> objectPoolService, IPopulationService populationService) {
            this.variables = variables;
            this.grid = grid;
            this.objectPoolService = objectPoolService;
            this.populationService = populationService;
        }
        
        /// <summary>
        /// VContainer Start Method
        /// </summary>
        public void Start() {
            FillGrid(true);
        }
        
        /// <summary>
        /// VContainer Tick Method
        /// </summary>
        public void Tick() {
            if (!this.isPlaying) {
                return;
            }

            if (this.timer >= 1) {
                this.timer = 0;
                
                this.populationService.NextGeneration(this.grid);
                UpdateCellViews();
                
                this.generation++;
            }
            
            this.timer += Time.deltaTime * this.variables.speed;
        }

        private void UpdateCellViews() {
            int[] cells = this.grid.Cells;
            int width = this.variables.width;
            int height = this.variables.height;
            
            for (int y = 0; y < height; y++) {
                int rowOffset = y * width;
                for (int x = 0; x < width; x++) {
                    int index = rowOffset + x;
                    this.cellViews[index].SetAlive(cells[index]);
                }
            }
        }

        /// <summary>
        /// Instantiates a Cell object,
        /// Gets a CellView from the object pool,
        /// and binds them together.
        /// </summary>
        private void SpawnCell(int x, int y, bool randomize = false) {
            this.grid.SetCell(x, y, randomize ? Random.Range(0, 2) : 0);
            int cell = this.grid.GetCell(x, y);
            CellView cellView = this.objectPoolService.Get(x, y);
            cellView.SetAlive(cell);
            this.cellViews[y * this.grid.Width + x] = cellView;
        }

        /// <summary>
        /// Fills the grid with Cell objects and CellViews
        /// </summary>
        private void FillGrid(bool randomize = false) {
            this.cellViews = new CellView[this.variables.width * this.variables.height];
            
            for (int y = 0; y < this.variables.height; y++) {
                for (int x = 0; x < this.variables.width; x++) {
                    SpawnCell(x, y, randomize);
                }
            }
        }

        public void ToggleSimulation() {
            this.isPlaying = !this.isPlaying;
        }
    }
}