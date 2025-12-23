using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Rendering;
using VContainer;

public class GridOverlay : MonoBehaviour
{
    private static readonly string INTERNAL_SHADER_TAG = "Hidden/Internal-Colored";
    private static readonly int SRC_BLEND = Shader.PropertyToID("_SrcBlend");
    private static readonly int DST_BLEND = Shader.PropertyToID("_DstBlend");
    private static readonly int Z_WRITE = Shader.PropertyToID("_ZWrite");
    private static readonly int CULL = Shader.PropertyToID("_Cull");
    private Material lineMaterial;

    private Color subColor;
    private Color mainColor;

    private bool showMain = true;
    private bool showSub = false;
    private float smallStep, largeStep;

    private Variables variables;
    private int gridSizeX, gridSizeY;
    private float startX = -0.5f, startY = -0.5f;

    [Inject]
    public void Configure(Variables variables) {
        this.variables = variables;
        UpdateValues();
    }
    
    void UpdateValues() {
        this.gridSizeX = this.variables.width;
        this.gridSizeY = this.variables.height;
        this.subColor = this.variables.subColor;
        this.mainColor = this.variables.mainColor;
        this.smallStep = this.variables.smallStep;
        this.largeStep = this.variables.largeStep;
        this.showMain = this.variables.showMainGrid;
        this.showSub = this.variables.showSubGrid;
    }

    void CreateLineMaterial()
    {
        if (!this.lineMaterial)
        {
            Shader shader = Shader.Find(INTERNAL_SHADER_TAG);
            this.lineMaterial = new Material(shader) {
                hideFlags = HideFlags.HideAndDontSave
            };

            this.lineMaterial.SetInt(SRC_BLEND, (int)BlendMode.SrcAlpha);
            this.lineMaterial.SetInt(DST_BLEND, (int)BlendMode.OneMinusSrcAlpha);

            this.lineMaterial.SetInt(Z_WRITE, 0);
            this.lineMaterial.SetInt(CULL, (int)CullMode.Off);
        }
    }

    private void OnDisable()
    {
        DestroyImmediate(this.lineMaterial);
    }

    private void OnPostRender()
    {
        CreateLineMaterial();

        this.lineMaterial.SetPass(0);
        GL.Begin(GL.LINES);

        if (this.showSub)
        {
            GL.Color(this.subColor);
            for (float y = 0; y <= this.gridSizeY; y += this.smallStep)
            {
                GL.Vertex3(this.startX, this.startY + y, 0);
                GL.Vertex3(this.startX + this.gridSizeX, this.startY + y, 0);
            }

            for (float x = 0; x <= this.gridSizeX; x += this.smallStep)
            {
                GL.Vertex3(this.startX + x, this.startY, 0);
                GL.Vertex3(this.startX + x, this.startY + this.gridSizeY, 0);
            }
        }

        if (this.showMain)
        {
            GL.Color(this.mainColor);
            for (float y = 0; y <= this.gridSizeY; y += this.largeStep)
            {
                GL.Vertex3(this.startX, this.startY + y, 0);
                GL.Vertex3(this.startX + this.gridSizeX, this.startY + y, 0);
            }

            for (float x = 0; x <= this.gridSizeX; x += this.largeStep)
            {
                GL.Vertex3(this.startX + x, this.startY, 0);
                GL.Vertex3(this.startX + x, this.startY + this.gridSizeY, 0);
            }
        }

        GL.End();
    }
}
