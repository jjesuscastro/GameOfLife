using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUI : MonoBehaviour
{
    public GameObject cellToggleText;
    public GameObject panel;
    public GameObject exit;
    public GameObject hotkeys;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            panel.SetActive(!panel.activeInHierarchy);
            exit.SetActive(!exit.activeInHierarchy);
            cellToggleText.SetActive(false);
            hotkeys.SetActive(false);
        }
    }
}
