using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ButtonHandler buttonHandler;
    private TooltipData tooltipData;



    void Awake()
    {
        // TODO can Find be averted?
        buttonHandler = GameObject.Find("MainCanvas").GetComponent<ButtonHandler>();
    }

    public void SetTooltipData(TooltipData tooltipData)
    {
        this.tooltipData = tooltipData;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonHandler?.ShowTooltip(GetBestPosition(), GetBestWidth(), GetBestHeight(), tooltipData);
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        buttonHandler?.HideTooltip();
    }

    // Returns the best position coordinates where to display the
    // tooltip with respects to the origin position
    private Vector2 GetBestPosition()
    {
        float Xoffset = 40f;
        float Yoffset = 0f;

        float x = this.transform.position.x + Xoffset;
        float y = this.transform.position.y + Yoffset;

        return new Vector2(x, y);
    }
    // TODO maybe make these computed inside SetTooltipData and saved locally
    private float GetBestHeight()
    {
        return 100f;
    }

    private float GetBestWidth()
    {
        return 200f;
    }
}
