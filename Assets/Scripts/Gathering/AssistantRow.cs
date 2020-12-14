﻿using TMPro;
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

    public Assistant assistant { get; private set; }
    private AssistantSystem m_assistantSystem;
    private MapOverlay m_map;

    void Awake()
    {
        m_map = FindObjectOfType<MapOverlay>(); // TODO dont use find
    }

    public void Set(AssistantSystem assistantSystem, Assistant assistant)
    {
        m_assistantSystem = assistantSystem;

        this.assistant = assistant;
        m_name.text = assistant.Name;
        m_avatar.sprite = assistant.sprite;
        m_areaChoice = m_dropdownObj.GetComponent<TMP_Dropdown>();
        m_isRepeat.isOn = assistant.repeat;

        if (m_map == null) m_map = FindObjectOfType<MapOverlay>(); // TODO dont use find

        UpdateDropdown();
        SelectDropdown(assistant.area);
        UpdateElements();

        if (assistant.isWorking)
        {
            m_assistantSystem.Resume(assistant);
        }
    }

    public void OnValueChange()
    {
        assistant.area = m_areaChoice.value;
        assistant.repeat = m_isRepeat.isOn;
    }


    /// <summary>
    /// Called (from the Send button) to send the assistant
    /// </summary>
    public void Send()
    {
        assistant.repeat = m_isRepeat.isOn;
        if (m_assistantSystem.Send(assistant))
        {
            assistant.isWorking = true;
            UpdateElements();
        }
    }


    /// <summary>
    /// Called when the assistant comes back
    /// </summary>
    public void Return()
    {
        assistant.isWorking = false;
        UpdateElements();
    }


    /// <summary>
    /// Update the row elements according to the isGathering var
    /// </summary>
    private void UpdateElements()
    {
        // Change the button
        m_sendButton.interactable = !assistant.isWorking;
        m_buttonText.text = assistant.isWorking ? "Gathering..." : "Send";

        // Hide the dropdown
        m_dropdownObj.SetActive(!assistant.isWorking);
        m_waitText.SetActive(assistant.isWorking);
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

    public void SelectDropdown(int area)
    {
        m_areaChoice.value = area;
    }
}
