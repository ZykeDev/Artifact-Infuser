using UnityEngine;
using UnityEngine.UI;

public class ButtonGraphic : MonoBehaviour
{
    [SerializeField] protected Image m_background;
    [SerializeField] protected Button m_button;
    private ColorBlock m_buttonColors;
    // Store an initial copy of the colors
    private ColorBlock m_defaultColors;
    private Rarity m_rarity;

    [SerializeField] protected Text m_name;
    [SerializeField] protected Image m_image;


    void Awake()
    {
        if (!m_button) m_button = GetComponent<Button>();
        if (!m_background) m_background = GetComponent<Image>();
        
        m_buttonColors = m_button.colors;
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
        m_button.colors = m_buttonColors;
    }

    /// <summary>
    /// Reverts the normal color to the original one.
    /// </summary>
    public void Deselect()
    {
        m_buttonColors.normalColor = m_defaultColors.normalColor;
        m_button.colors = m_buttonColors;
    }


    private void UpdateGraphic()
    {
        if (m_background) m_background.color = RarityColor.GetColor(m_rarity);
    }


    #region Setters

    public void SetName(string name) => m_name.text = name;
    public void SetImage(Sprite sprite) => m_image.sprite = sprite;
    public void SetColor(Rarity rarity) { m_rarity = rarity; UpdateGraphic(); }

    /// <summary>
    /// Sets both the Name and the Sprite of the button
    /// </summary>
    /// <param name="name"></param>
    /// <param name="sprite"></param>
    public void SetData(string name, Sprite sprite, Rarity rarity)
    {
        SetName(name);
        SetImage(sprite);
        SetColor(rarity);
    }

    #endregion

}
