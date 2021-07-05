using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedUI : MonoBehaviour
{
    public TMP_InputField inputField;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        inputField.text = gameManager.GetSpeed().ToString();
        gameManager.onSpeedChanged.AddListener(UpdateUI);
    }

    public void ValidateInput()
    {
        float value = float.Parse(inputField.text);
        value = Mathf.Clamp(value, gameManager.minSpeed, gameManager.maxSpeed);
        gameManager.SetSpeed(value);
    }

    void UpdateUI()
    {
        inputField.text = gameManager.GetSpeed().ToString();
    }
}
