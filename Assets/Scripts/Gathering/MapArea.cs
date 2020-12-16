using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("UI Options")]
    [SerializeField, Tooltip("Transparency of the map overlays")]
    protected float m_highlightAlpha = 0.5f;

    [SerializeField, Min(0)] public float m_gatherTime = 0f;
    [SerializeField] protected GameObject m_lock;
    [SerializeField] protected GameObject m_go;

    [SerializeField] protected bool m_isUnlocked = false;
    [SerializeField] protected string m_name = "";


    public int index { get; private set; }
    private bool m_isSelected = false;
    private Image m_image;
    


    void Awake()
    {
        m_image = GetComponent<Image>();

        UpdateLock();
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!m_isSelected) Highlight();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!m_isSelected) Unhighlight();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_isUnlocked && !m_isSelected)
        {
            m_isSelected = true;
            FindObjectOfType<MapOverlay>().Select(index);
        }
    }


    public void SetLock(int index, bool state)
    {
        this.index = index;
        m_isUnlocked = state;
        UpdateLock();
    }

    public void Unlock()
    {
        m_isUnlocked = true;
        UpdateLock();
    }

    public void SetSelected(bool selected)
    {
        m_isSelected = selected;
        if (m_isSelected == false)
        {
            Unhighlight();
        }
    }



    private void UpdateLock()
    {
        m_lock.SetActive(!m_isUnlocked);
        m_go.SetActive(m_isUnlocked);
    }

    private void Highlight() => SetAlpha(m_highlightAlpha);
    private void Unhighlight() => SetAlpha(0f);

    private void SetAlpha(float value)
    {
        Color baseColor = m_image.color;
        baseColor.a = value;
        m_image.color = baseColor;
    }


    public bool IsUnlocked() => m_isUnlocked;
    public string GetName() => m_name;
    public float GetTime() => m_gatherTime;
}
