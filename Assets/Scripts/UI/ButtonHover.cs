using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ButtonHandler buttonHandler;
    private TooltipData m_tooltipData;

    private float m_bestWidth, m_bestHeight;


    void Awake()
    {
        // TODO can Find be averted?
        buttonHandler = GameObject.Find("MainCanvas").GetComponent<ButtonHandler>();
        m_bestWidth = 0f;
    }

    public void SetTooltipData(TooltipData tooltipData)
    {
        m_tooltipData = tooltipData;
        m_bestWidth = GetBestWidth();
        m_bestHeight = GetBestHeight();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_bestWidth == 0f)
        {
            m_bestWidth = GetBestWidth();
            m_bestHeight = GetBestHeight();
        }

        buttonHandler?.ShowTooltip(m_bestWidth, m_bestHeight, m_tooltipData);
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        buttonHandler?.HideTooltip();
    }


    /// <summary>
    /// Get the best Height according to the contents
    /// </summary>
    /// <returns></returns>
    private float GetBestHeight()
    {
        // TODO make the hight actually change depending on the content
        return 150f;
    }

    /// <summary>
    /// Get the best Width according to the contents
    /// </summary>
    /// <returns></returns>
    private float GetBestWidth()
    {
        return 280f;
    }
}
