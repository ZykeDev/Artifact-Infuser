using UnityEngine;
using UnityEngine.UI;

public class ButtonGraphic : MonoBehaviour
{
    private Image m_background;
    private Button m_button;
    private ColorBlock m_buttonColors;
    // Store an initial copy of the colors
    private ColorBlock m_defaultColors;
    private Rarity m_rarity;

    [SerializeField] private Text m_name;
    [SerializeField] private Image m_image;


    void Awake()
    {
        m_button = GetComponent<Button>();
        m_background = GetComponent<Image>();
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
        Color color = m_background.color;

        switch (m_rarity)
        {
            case Rarity.COMMON:
                color = new Color(0.9f, 0.9f, 0.9f);
                break;
            case Rarity.UNCOMMON:
                color = new Color(0.137f, 0.707f, 0.707f);
                break;
            case Rarity.RARE:
                color = new Color(0.09968f, 0.6037f, 0.09968f);
                break;
            case Rarity.UNIQUE:
                color = new Color(0.3718f, 0.058f, 0.585f);
                break;
            case Rarity.LEGENDARY:
                //color = new Color(1f, 0.5f, 0f);
                color = new Color(0.698f, 0.3444f, 0.056f);
                break;
            case Rarity.ABYSSAL:
                color = new Color(1f, 0.2311f, 1f);
                break;
            case Rarity.IMMORTAL:
                color = new Color(0.6698f, 0.1485f, 0.1485f);
                break;
        }

        m_background.color = color;
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
