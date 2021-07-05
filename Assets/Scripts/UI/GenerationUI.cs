using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GenerationUI : MonoBehaviour
{
    public TMP_Text generationCount;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        gameManager.onPopulationUpdate.AddListener(UpdateGenerationCount);
    }

    private void UpdateGenerationCount()
    {
        generationCount.text = gameManager.GetGeneration().ToString();
    }
}
