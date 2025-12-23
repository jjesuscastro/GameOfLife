using System;
using Models.Interfaces;
using ScriptableObjects;
using Services.Interfaces;
using UnityEngine;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace Services {
    public class GridSimulationService : IGridSimulationService, ITickable, IDisposable {

        private Variables variables;
        private IGrid grid;
        
        private IPopulationService populationService;

        private Matrix4x4[] matrices;
        private Matrix4x4[] batchMatrices = new Matrix4x4[1023];
        private Mesh quadMesh;
        private MaterialPropertyBlock propertyBlock;

        private int generation = 1;
        private float timer;
        private bool isPlaying;
        

        public GridSimulationService(Variables variables, IGrid grid, IPopulationService populationService) {
            this.variables = variables;
            this.grid = grid;
            this.populationService = populationService;
        }
        
        /// <summary>
        /// VContainer Start Method
        /// </summary>
        public void Start() {
            this.propertyBlock = new MaterialPropertyBlock();
            this.matrices = new Matrix4x4[this.variables.width * this.variables.height];
            this.quadMesh = CreateQuadMesh();
            
            FillGrid(true);
            UpdateMatrices();
        }
        
        /// <summary>
        /// Create a quad mesh for drawing
        /// </summary>
        /// <returns>
        /// The new mesh
        /// </returns>
        private Mesh CreateQuadMesh() {
            Mesh mesh = new Mesh {
                vertices = new[] {
                    new Vector3(-0.5f, -0.5f, 0), new Vector3(0.5f, -0.5f, 0),
                    new Vector3(-0.5f, 0.5f, 0), new Vector3(0.5f, 0.5f, 0)
                },
                uv = new[] {
                    new Vector2(0, 0), new Vector2(1, 0),
                    new Vector2(0, 1), new Vector2(1, 1)
                },
                triangles = new[] { 0, 2, 1, 2, 3, 1 }
            };
            mesh.RecalculateNormals();
            return mesh;
        }
        
        private void UpdateMatrices() {
            int width = this.variables.width;
            int height = this.variables.height;
            int total = width * height;

            // Ensure our array matches the current grid size
            if (this.matrices.Length != total) {
                this.matrices = new Matrix4x4[total];
            }

            for (int i = 0; i < total; i++) {
                int x = i % width;
                int y = i / width;
                
                // Get the cell state (1 for alive, 0 for dead)
                int state = this.grid.Cells[i];
                
                // Position the matrix at (x, y, 0)
                this.matrices[i] = Matrix4x4.TRS(
                    new Vector3(x, y, 0), 
                    Quaternion.identity, 
                    new Vector3(state, state, 1f)
                );
            }
        }
        
        /// <summary>
        /// VContainer Tick Method
        /// </summary>
        public void Tick() {
            if (this.isPlaying) {
                if (this.timer >= 1) {
                    this.timer = 0;
                
                    this.populationService.NextGeneration(this.grid);
                    UpdateMatrices();
                
                    this.generation++;
                }
            
                this.timer += Time.deltaTime * this.variables.speed;
            }
            
            Render();
        }
        
        /// <summary>
        /// Draws the grid using instanced rendering
        /// </summary>
        private void Render() {
            if (this.variables.cellMaterial == null || this.quadMesh == null) return;

            int total = this.matrices.Length;
            for (int i = 0; i < total; i += 1023) {
                int count = Mathf.Min(1023, total - i);
                
                // We must use the 'batchMatrices' array we allocated previously
                Array.Copy(this.matrices, i, this.batchMatrices, 0, count);

                Graphics.DrawMeshInstanced(this.quadMesh, 
                    0, this.variables.cellMaterial, this.batchMatrices, 
                    count, this.propertyBlock,
                    UnityEngine.Rendering.ShadowCastingMode.Off,
                    false,
                    0,    // Layer
                    null  // Camera (null = all cameras)
                );
            }
        }

        /// <summary>
        /// Instantiates a Cell object,
        /// Gets a CellView from the object pool,
        /// and binds them together.
        /// </summary>
        private void SpawnCell(int x, int y, bool randomize = false) {
            this.grid.SetCell(x, y, randomize ? Random.Range(0, 2) : 0);
        }

        /// <summary>
        /// Fills the grid with Cell objects and CellViews
        /// </summary>
        private void FillGrid(bool randomize = false) {
            this.isPlaying = false;
            int width = this.variables.width;
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
            UpdateMatrices();
            this.generation = 1;
        }

        public void ClearGrid() {
            this.isPlaying = false;
            this.grid.ClearGrid();
            UpdateMatrices();
            this.generation = 1;
        }

        /// <summary>
        /// VContainer Dispose Method
        /// </summary>
        public void Dispose() {
        }
    }
}