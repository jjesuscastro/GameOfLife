using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundColorUI : MonoBehaviour
{
    public FlexibleColorPicker flexibleColorPicker;
    public Image image;
    Camera mainCamera;

    bool isOpen = false;

    private void Start()
    {
        mainCamera = Camera.main;
        image.color = mainCamera.backgroundColor;
    }

    public void ToggleUI()
    {
        isOpen = !isOpen;

        if (isOpen)
            OpenFCP();
        else
            CloseFCP();
    }

    void OpenFCP()
    {
        flexibleColorPicker.gameObject.SetActive(true);
        flexibleColorPicker.color = image.color;
    }

    void CloseFCP()
    {
        flexibleColorPicker.gameObject.SetActive(false);
        image.color = flexibleColorPicker.color;
        mainCamera.backgroundColor = flexibleColorPicker.color;
    }
}
