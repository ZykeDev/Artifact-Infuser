using UnityEngine;
using UnityEngine.UI;

public class ButtonGraphic : MonoBehaviour
{
    private Button buttonComponent;
    private ColorBlock buttonColors;
    // Store an initial copy of the colors
    private ColorBlock defaultColors;

    [SerializeField] private Text m_name;
    [SerializeField] private Image m_image;

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
        buttonComponent.colors = buttonColors;
    }

    // Reverts the normal color to the original one
    public void Deselect()
    {
        buttonColors.normalColor = defaultColors.normalColor;
        buttonComponent.colors = buttonColors;
    }

    public void SetName(string name)
    {
        m_name.text = name;
    }

    public void SetImage(Sprite sprite)
    {
        m_image.sprite = sprite;
    }

    public void SetData(string name, Sprite sprite)
    {
        SetName(name);
        SetImage(sprite);
    }
}
