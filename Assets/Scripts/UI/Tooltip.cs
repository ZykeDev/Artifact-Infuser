using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private Text titleText, dexText;
    

    public void SetTexts(TooltipData tooltipData)
    {
        titleText.text = tooltipData.title;
        dexText.text = tooltipData.dex;
    }

}
