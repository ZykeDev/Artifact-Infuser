using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private Text m_titleText, m_dexText;

    [SerializeField]
    private float m_offset = 1f;

    private RectTransform m_rectTransform;
    private float m_screenHeight;
    private bool m_isFollowingCursor;
    private Vector3 m_lastMousePosition = new Vector3();

    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_screenHeight = Screen.height;
        m_isFollowingCursor = false;
    }

    private void Update()
    {
        if (m_isFollowingCursor)
        {
            if (m_rectTransform == null)
            {
                Debug.LogError("No RectTransform component found on the Tooltip.");
            }

            // Only update the position if the cursor has moved
            if (m_lastMousePosition != Input.mousePosition)
            {
                m_lastMousePosition = Input.mousePosition;
                m_rectTransform.anchoredPosition = new Vector2(m_lastMousePosition.x + m_offset, 
                                                               m_lastMousePosition.y - m_screenHeight + m_offset);
            }
            
        }
    }


    public void SetTexts(TooltipData tooltipData)
    {
        m_titleText.text = tooltipData.title;
        m_dexText.text = tooltipData.dex;
    }

    public void FollowCursor(bool follow)
    {
        m_isFollowingCursor = follow;
    }

    public void FollowCursor()
    {
        FollowCursor(true);
    }

    public void StopFollowingCursor()
    {
        FollowCursor(false);
    }
}
