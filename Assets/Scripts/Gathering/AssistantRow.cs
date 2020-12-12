using TMPro;
using System.Collections.Generic;
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
    private MapOverlay m_map;

    private bool isGathering = false;

    void Awake()
    {
        m_map = FindObjectOfType<MapOverlay>(); // TODO dont use find
    }

    public void Set(AssistantSystem assistantSystem, Assistant assistant)
    {
        m_assistantSystem = assistantSystem;

        m_assistant = assistant;
        m_name.text = assistant.name;
        m_avatar.sprite = assistant.sprite;
        m_areaChoice = m_dropdownObj.GetComponent<TMP_Dropdown>();
        m_isRepeat.isOn = false;

        isGathering = false; // TODO update on save load or after offlineProgress calc

        if (m_map == null) m_map = FindObjectOfType<MapOverlay>(); // TODO dont use find

        UpdateDropdown();
    }

    public void OnValueChange()
    {
        m_assistantSystem?.UpdateAssistant(m_assistant, m_areaChoice.value, m_isRepeat.isOn);
    }


    /// <summary>
    /// Called (from the Send button) to send the assistant
    /// </summary>
    public void Send()
    {
        m_assistant.repeat = m_isRepeat.isOn;
        if (m_assistantSystem.Send(m_assistant))
        {
            isGathering = true;
            UpdateElements();
        }
    }


    /// <summary>
    /// Called when the assistant comes back
    /// </summary>
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



    /// <summary>
    /// Updates the list of selectable areas
    /// </summary>
    public void UpdateDropdown()
    {
        List<string> options = new List<string>();

        foreach (MapArea area in m_map.GetActiveAreas())
        {
            options.Add(area.GetName());
        }

        m_areaChoice.ClearOptions();
        m_areaChoice.AddOptions(options);
    }
}
