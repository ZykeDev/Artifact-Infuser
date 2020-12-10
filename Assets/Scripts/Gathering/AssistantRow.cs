using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssistantRow : MonoBehaviour
{
    [SerializeField] private TMP_Text m_name;
    [SerializeField] private Image m_avatar;
    [SerializeField] private GameObject m_dropdownObj;
    private TMP_Dropdown m_areaChoice;
    [SerializeField] private Button m_sendButton;
    [SerializeField] private TMP_Text m_buttonText;

    [SerializeField] private Toggle m_isRepeat;

    [SerializeField] private GameObject m_waitText;

    private Assistant m_assistant;
    private AssistantSystem m_assistantSystem;

    private bool isGathering = false;

    public void Set(AssistantSystem assistantSystem, Assistant assistant)
    {
        m_assistantSystem = assistantSystem;

        m_assistant = assistant;
        m_name.text = assistant.name;
        m_avatar.sprite = assistant.sprite;
        m_areaChoice = m_dropdownObj.GetComponent<TMP_Dropdown>();

        isGathering = false; // TODO update on save load or after offlineProgress calc
    }

    public void OnValueChange()
    {
        m_assistantSystem?.UpdateAssistant(m_assistant, m_areaChoice.value, m_isRepeat.isOn);
    }


    public void Send()
    {
        isGathering = true;
        UpdateElements();

        m_assistantSystem.Send(m_assistant);
    }


    public void Return()
    {
        isGathering = false;
        UpdateElements();
    }


    /// <summary>
    /// Update the row elements according to the isGathering var
    /// </summary>
    private void UpdateElements()
    {
        // Change the button
        m_sendButton.interactable = !isGathering;
        m_buttonText.text = isGathering ? "Gathering..." : "Send";

        // Hide the dropdown
        m_dropdownObj.SetActive(!isGathering);
        m_waitText.SetActive(isGathering);
    }


}
