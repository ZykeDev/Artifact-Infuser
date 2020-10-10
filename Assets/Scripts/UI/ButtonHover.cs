using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private ButtonHandler buttonHandler;
    private TooltipData tooltipData;

    private Vector2 bestPosition;
    private float bestWidth, bestHeight;


    void Awake()
    {
        // TODO can Find be averted?
        buttonHandler = GameObject.Find("MainCanvas").GetComponent<ButtonHandler>();
        bestPosition = Vector2.zero;
    }

    public void SetTooltipData(TooltipData tooltipData)
    {
        this.tooltipData = tooltipData;
        bestPosition = GetBestPosition();
        bestWidth = GetBestWidth();
        bestHeight = GetBestHeight();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (bestPosition == Vector2.zero)
        {
            bestPosition = GetBestPosition();
            bestWidth = GetBestWidth();
            bestHeight = GetBestHeight();
        }

        buttonHandler?.ShowTooltip(bestPosition, bestWidth, bestHeight, tooltipData);
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

    private float GetBestHeight()
    {
        return 150f;
    }

    private float GetBestWidth()
    {
        return 300f;
    }
}
