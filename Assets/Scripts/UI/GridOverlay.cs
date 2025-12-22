using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Rendering;

public class GridOverlay : MonoBehaviour
{
    Material lineMaterial;

    public Color subColor;
    public Color mainColor;

    public bool showMain = true;
    public bool showSub = false;
    public float smallStep, largeStep;

    [SerializeField]
    private Variables variables;
    int gridSizeX, gridSizeY;
    float startX = -0.5f, startY = -0.5f;

    void Start()
    {
        UpdateValues();
    }

    void UpdateValues() {
        gridSizeX = this.variables.width;
        gridSizeY = this.variables.height;
    }

    void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);

            lineMaterial.hideFlags = HideFlags.HideAndDontSave;

            lineMaterial.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);

            lineMaterial.SetInt("_ZWrite", 0);
            lineMaterial.SetInt("_Cull", (int)CullMode.Off);
        }
    }

    private void OnDisable()
    {
        DestroyImmediate(lineMaterial);
    }

    private void OnPostRender()
    {
        CreateLineMaterial();

        lineMaterial.SetPass(0);
        GL.Begin(GL.LINES);

        if (showSub)
        {
            GL.Color(subColor);
            for (float y = 0; y <= gridSizeY; y += smallStep)
            {
                GL.Vertex3(startX, startY + y, 0);
                GL.Vertex3(startX + gridSizeX, startY + y, 0);
            }

            for (float x = 0; x <= gridSizeX; x += smallStep)
            {
                GL.Vertex3(startX + x, startY, 0);
                GL.Vertex3(startX + x, startY + gridSizeY, 0);
            }
        }

        if (showMain)
        {
            GL.Color(mainColor);
            for (float y = 0; y <= gridSizeY; y += largeStep)
            {
                GL.Vertex3(startX, startY + y, 0);
                GL.Vertex3(startX + gridSizeX, startY + y, 0);
            }

            for (float x = 0; x <= gridSizeX; x += largeStep)
            {
                GL.Vertex3(startX + x, startY, 0);
                GL.Vertex3(startX + x, startY + gridSizeY, 0);
            }
        }

        GL.End();
    }
}
