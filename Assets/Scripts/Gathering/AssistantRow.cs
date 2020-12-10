using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssistantRow : MonoBehaviour
{
    [SerializeField] private TMP_Text m_name;
    [SerializeField] private Image m_avatar;
    [SerializeField] private TMP_Dropdown m_areaChoice;
    [SerializeField] private Button m_sendBtn;
    [SerializeField] private Toggle m_isRepeat;

    [SerializeField] private GameObject m_waitText;


    public void Set(string name, Sprite sprite)
    {
        m_name.text = name;
        m_avatar.sprite = sprite;
    }








}
