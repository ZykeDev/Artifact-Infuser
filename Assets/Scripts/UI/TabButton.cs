﻿using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
    [SerializeField] private Text m_text;
    [SerializeField] private GameObject m_lock;
    [SerializeField] private Button m_button;

    [Header("Tab String")]
    [SerializeField] private string tabString = "";

    private bool isLocked = false;
    private bool isSelected = false;


    void Awake()
    {
        UpdateLock();
    }


    public void UpdateLock(bool newValue)
    {
        isLocked = newValue;
        UpdateLock();
    }


    public void UpdateLock()
    {
        m_text.text = isLocked ? "" : tabString;
        m_lock.SetActive(isLocked);
        m_button.interactable = !isLocked;
    }

    public void Select(bool isSelected)
    {
        this.isSelected = isSelected;
        UpdateSelected();
    }

    public void UpdateSelected()
    {
        m_button.interactable = !isLocked && !isSelected;
    }

}
