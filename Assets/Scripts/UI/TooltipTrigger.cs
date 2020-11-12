using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TooltipData m_tooltipData;

    public void SetTooltipData(TooltipData tooltipData) => m_tooltipData = tooltipData;


    public void OnPointerEnter(PointerEventData eventData) => TooltipSystem.Show(m_tooltipData);
    
    public void OnPointerExit(PointerEventData eventData) => TooltipSystem.Hide();
    
    public void HideTooltip() => TooltipSystem.Hide();
    

}
