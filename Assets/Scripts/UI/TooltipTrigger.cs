using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TooltipData m_tooltipData;

    [SerializeField] private string m_header;

    public void SetTooltipData(TooltipData tooltipData) => m_tooltipData = tooltipData;
    public void SetTooltipData(string header) => m_tooltipData = new TooltipData(header, null);


    public void OnPointerEnter(PointerEventData eventData)
    {
        print("called");
        if (m_header != null && m_header != "")
        {
            TooltipSystem.Show(m_header);
        }
        else
        {
            TooltipSystem.Show(m_tooltipData);
        }
    }
    
    public void OnPointerExit(PointerEventData eventData) => TooltipSystem.Hide();
    
    public void HideTooltip() => TooltipSystem.Hide();
    

}
