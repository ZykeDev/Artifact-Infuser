using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;

    public Tooltip tooltip;

    void Awake()
    {
        current = this;   
    }

    public static void Show(TooltipData tooltipData)
    {
        current.tooltip?.SetTexts(tooltipData);
        current.tooltip?.gameObject.SetActive(true);
    }

    public static void Show(string text)
    {
        current.tooltip?.SetTexts(text);
        current.tooltip?.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        current.tooltip?.gameObject.SetActive(false);
    }
}
