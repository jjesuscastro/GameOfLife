using System;
using Models.Interfaces;
using ScriptableObjects;
using Services.Interfaces;
using UnityEngine;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace Services {
    public class GridSimulationService : IGridSimulationService, ITickable {

        private Variables variables;
        private IGrid grid;
        
        private IPopulationService populationService;
        private IMeshService meshService;

        private int generation = 1;
        private float timer;
        private bool isPlaying;

        public GridSimulationService(Variables variables, IGrid grid, IPopulationService populationService, IMeshService meshService) {
            this.variables = variables;
            this.grid = grid;
            this.populationService = populationService;
            this.meshService = meshService;
        }
        
        /// <summary>
        /// VContainer Start Method
        /// </summary>
        public void Start() {
            FillGrid(true);
            this.meshService.UpdateMatrices(this.grid, this.variables.width, this.variables.height);
        }
        
        /// <summary>
        /// VContainer Tick Method
        /// </summary>
        public void Tick() {
            if (this.isPlaying) {
                if (this.timer >= 1) {
                    this.timer = 0;
                
                    this.populationService.NextGeneration(this.grid);
                    this.meshService.UpdateMatrices(this.grid, this.variables.width, this.variables.height);
                
                    this.generation++;
                }
            
                this.timer += Time.deltaTime * this.variables.speed;
            }
            
            this.meshService.Render();
        }

        /// <summary>
        /// Instantiates a Cell object,
        /// </summary>
        private void SpawnCell(int x, int y, bool randomize = false) {
            this.grid.SetCell(x, y, randomize ? Random.Range(0, 2) : 0);
        }

        /// <summary>
        /// Fills the grid with Cell objects
        /// </summary>
        private void FillGrid(bool randomize = false) {
            this.isPlaying = false;
            for (int y = 0; y < this.variables.height; y++) {
                for (int x = 0; x < this.variables.width; x++) {
                    SpawnCell(x, y, randomize);
                }
            }
        }

        /// <summary>
        /// Toggle the simulation on and off.
        /// </summary>
        public void ToggleSimulation() {
            this.isPlaying = !this.isPlaying;
        }

        public void RandomizeGrid() {
            FillGrid(true);
            this.meshService.UpdateMatrices(this.grid, this.variables.width, this.variables.height);
            this.generation = 1;
        }

        public void ClearGrid() {
            this.isPlaying = false;
            this.grid.ClearGrid();
            this.meshService.UpdateMatrices(this.grid, this.variables.width, this.variables.height);
            this.generation = 1;
        }
    }
}