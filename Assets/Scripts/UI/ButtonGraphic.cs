using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGraphic : MonoBehaviour
{
    private Button buttonComponent;
    private ColorBlock buttonColors;
    // Store an initial copy of the colors
    private ColorBlock defaultColors;

    private void Awake()
    {
        buttonComponent = GetComponent<Button>();
        buttonColors = buttonComponent.colors;
        defaultColors = buttonColors;

        // Deselect call to make sure the button is in its default state
        Deselect();
    }

    // Sets the normal color to "selected"
    public void Select()
    {
        buttonColors.normalColor = defaultColors.selectedColor;
        this.GetComponent<Button>().colors = buttonColors;
    }

    // Reverts the normal color to the original one
    public void Deselect()
    {
        buttonColors.normalColor = defaultColors.normalColor;
        this.GetComponent<Button>().colors = buttonColors;
    }
}
