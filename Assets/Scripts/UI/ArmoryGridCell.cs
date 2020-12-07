using UnityEngine;
using UnityEngine.UI;

public class ArmoryGridCell : MonoBehaviour {
   
    public Image m_cellIcon;
    public Image m_cellFrame;


    /// <summary>
    /// Sets the sprite of the cell
    /// </summary>
    /// <param name="sprite"></param>
    public void SetSprite(Sprite sprite, Rarity rarity)
    {
        m_cellIcon.sprite = sprite;
        m_cellFrame.color = RarityColor.GetColor(rarity);
    }

}
