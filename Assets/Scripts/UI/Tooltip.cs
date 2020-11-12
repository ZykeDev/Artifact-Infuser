using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_titleText, m_dexText;

    [SerializeField]
    private LayoutElement m_layoutElement;

    [SerializeField]
    private int m_charWrapLimit = 80;

    private RectTransform m_rectTransform;

    void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        float pivotX = mousePos.x / Screen.width;
        float pivotY = mousePos.y / Screen.height;

        m_rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = mousePos;
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
}
