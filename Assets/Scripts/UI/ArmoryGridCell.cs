using UnityEngine;
using UnityEngine.UI;

public class ArmoryGridCell : MonoBehaviour {
   
    public GameObject m_cellIcon;


    /// <summary>
    /// Sets the sprite of the cell
    /// </summary>
    /// <param name="sprite"></param>
    public void SetSprite(Sprite sprite)
    {
        Image image = m_cellIcon.GetComponent<Image>();

        if (image != null)
        {
            m_cellIcon.GetComponent<Image>().sprite = sprite;
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogError("Missing Image component in Armory Cell Object.");
#endif
        }
        
    }

}
