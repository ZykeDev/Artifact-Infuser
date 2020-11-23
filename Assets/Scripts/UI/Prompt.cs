using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Prompt : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI m_header, m_content;

    [SerializeField]
    protected GameObject m_buttons;

    [SerializeField]
    protected LayoutElement m_layoutElement;

    [SerializeField]
    private int m_charWrapLimit = 160;

    private RectTransform m_rectTransform;

    void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }


    public void SetTexts(string text)
    {
        m_header.text = text;
        m_content.text = text;

        int titleLength = m_header.text.Length;
        int dexLength = m_content.text.Length;

        m_layoutElement.enabled = (titleLength > m_charWrapLimit || dexLength > m_charWrapLimit);


    }
}
