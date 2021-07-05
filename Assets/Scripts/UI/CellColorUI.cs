using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellColorUI : MonoBehaviour
{
    public FlexibleColorPicker flexibleColorPicker;
    public Image image;
    public Material material;

    bool isOpen = false;

    private void Start()
    {
        image.color = material.color;
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
        material.color = flexibleColorPicker.color;
    }
}
