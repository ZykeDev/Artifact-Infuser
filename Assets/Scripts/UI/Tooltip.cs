using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI m_titleText, m_dexText;

    [SerializeField]
    protected LayoutElement m_layoutElement;

    [SerializeField]
    private int m_charWrapLimit = 80;

    [SerializeField]
    private bool m_dynamicPosition = false;

    private RectTransform m_rectTransform;

    void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        if (m_dynamicPosition)
        {
            float pivotX = mousePos.x / Screen.width;
            float pivotY = mousePos.y / Screen.height;

            m_rectTransform.pivot = new Vector2(pivotX - 0.2f, pivotY);
            transform.position = mousePos;
        }
        else
        {
            m_rectTransform.pivot = new Vector2(0.1f, -0.1f);
            transform.position = mousePos;
        }
        
    }


    public void SetTexts(TooltipData tooltipData)
    {
        if (tooltipData.IsEmpty())
        {
            m_titleText.text = "";
            m_dexText.text = "";
        }
        else
        {
            m_titleText.text = tooltipData.title;
            m_dexText.text = tooltipData.dex;
        }


        int titleLength = m_titleText.text.Length;
        int dexLength = m_dexText.text.Length;

        m_layoutElement.enabled = (titleLength > m_charWrapLimit || dexLength > m_charWrapLimit);
    }

    public void SetTexts(string text)
    {
        TooltipData tooltipData = new TooltipData(text, "");
        SetTexts(tooltipData);
    }
}
