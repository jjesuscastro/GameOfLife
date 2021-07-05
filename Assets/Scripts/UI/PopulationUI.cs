using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopulationUI : MonoBehaviour
{
    public TMP_Text populationCount;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        gameManager.onPopulationUpdate.AddListener(UpdateGenerationCount);
    }

    private void UpdateGenerationCount()
    {
        populationCount.text = gameManager.GetPopulation().ToString();
    }
}
