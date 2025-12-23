using System;
using Models.Interfaces;
using ScriptableObjects;
using Services.Interfaces;
using UnityEngine;

namespace Services {
    public class MeshService : IMeshService {
        private Material cellMaterial;
        
        private Matrix4x4[] matrices;
        private Matrix4x4[] batchMatrices = new Matrix4x4[1023];
        private Mesh quadMesh;
        private MaterialPropertyBlock propertyBlock;
        
        public MeshService(Variables variables) {
            this.cellMaterial = variables.cellMaterial;
            this.propertyBlock = new MaterialPropertyBlock();
            this.matrices = new Matrix4x4[variables.width * variables.height];
            this.quadMesh = CreateQuadMesh();
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
        
        /// <summary>
        /// Update the matrices
        /// Alive cells are drawn with 1x scale, dead cells are drawn with 0x scale
        /// </summary>
        /// <param name="grid">
        /// The grid to draw
        /// </param>
        /// <param name="width">
        /// The width of the grid
        /// </param>
        /// <param name="height">
        /// The height of the grid
        /// </param>
        public void UpdateMatrices(IGrid grid, int width, int height) {
            int total = width * height;

            // Ensure our array matches the current grid size
            if (this.matrices.Length != total) {
                this.matrices = new Matrix4x4[total];
            }

            for (int i = 0; i < total; i++) {
                int x = i % width;
                int y = i / width;
                
                // Get the cell state (1 for alive, 0 for dead)
                int state = grid.Cells[i];
                
                // Position the matrix at (x, y, 0)
                this.matrices[i] = Matrix4x4.TRS(
                    new Vector3(x, y, 0), 
                    Quaternion.identity, 
                    new Vector3(state, state, 1f)
                );
            }
        }

        /// <summary>
        /// Draws the grid using instanced rendering
        /// </summary>
        public void Render() {
            if (this.cellMaterial == null || this.quadMesh == null) return;

            int total = this.matrices.Length;
            for (int i = 0; i < total; i += 1023) {
                int count = Mathf.Min(1023, total - i);
                
                // We must use the 'batchMatrices' array we allocated previously
                Array.Copy(this.matrices, i, this.batchMatrices, 0, count);

                Graphics.DrawMeshInstanced(this.quadMesh, 
                    0, cellMaterial, this.batchMatrices, 
                    count, this.propertyBlock,
                    UnityEngine.Rendering.ShadowCastingMode.Off,
                    false,
                    0,    // Layer
                    null  // Camera (null = all cameras)
                );
            }
        }
    }
}