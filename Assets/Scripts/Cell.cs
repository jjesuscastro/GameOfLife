using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    public bool isAlive = false;
    public int numNeighbors = 0;
    SpriteRenderer spriteRenderer;
    GameManager gameManager;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = GameManager.instance;
    }

    public void SetAlive(bool alive)
    {
        isAlive = alive;
        spriteRenderer.enabled = alive;
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        SetAlive(!isAlive);
        gameManager.CountPopulation(true);
    }
}
