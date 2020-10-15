using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private Text titleText, dexText;

    private RectTransform rectTransform;
    private float screenHeight;
    private bool isFollowingCursor;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        screenHeight = Screen.height;
        isFollowingCursor = false;
    }

    private void Update()
    {
        if (isFollowingCursor)
        {
            if (rectTransform == null)
            {
                Debug.LogError("No RectTransform component found on the Tooltip.");
            }
            
            rectTransform.anchoredPosition = new Vector2(Input.mousePosition.x + 1f, Input.mousePosition.y - screenHeight + 1f);
        }
    }

    public void SetTexts(TooltipData tooltipData)
    {
        titleText.text = tooltipData.title;
        dexText.text = tooltipData.dex;
    }

    public void FollowCursor()
    {
        isFollowingCursor = true;
    }

    public void StopFollowingCursor()
    {
        isFollowingCursor = false;
    }
}
