using UnityEngine;
using UnityEngine.UI;

public class ButtonGraphic : MonoBehaviour
{
    private Button m_buttonComponent;
    private ColorBlock m_buttonColors;
    // Store an initial copy of the colors
    private ColorBlock m_defaultColors;

    [SerializeField] private Text m_name;
    [SerializeField] private Image m_image;

    private void Awake()
    {
        m_buttonComponent = GetComponent<Button>();
        m_buttonColors = m_buttonComponent.colors;
        m_defaultColors = m_buttonColors;

        // Deselect call to make sure the button is in its default state
        Deselect();
    }

    /// <summary>
    /// Sets the normal color to "Selected".
    /// </summary>
    public void Select()
    {
        m_buttonColors.normalColor = m_defaultColors.selectedColor;
        m_buttonComponent.colors = m_buttonColors;
    }

    /// <summary>
    /// Reverts the normal color to the original one.
    /// </summary>
    public void Deselect()
    {
        m_buttonColors.normalColor = m_defaultColors.normalColor;
        m_buttonComponent.colors = m_buttonColors;
    }


    public void SetName(string name) => m_name.text = name;
    public void SetImage(Sprite sprite) => m_image.sprite = sprite;

    /// <summary>
    /// Sets both the Name and the Sprite of the button
    /// </summary>
    /// <param name="name"></param>
    /// <param name="sprite"></param>
    public void SetData(string name, Sprite sprite)
    {
        SetName(name);
        SetImage(sprite);
    }
}
