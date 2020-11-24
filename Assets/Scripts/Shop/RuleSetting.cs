﻿using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RuleSetting : MonoBehaviour
{
    [SerializeField] private Toggle m_toggle;
    [SerializeField] private TMP_Dropdown m_amountType;
    [SerializeField] private TMP_Dropdown m_sellType;
    [SerializeField] private TMP_Dropdown m_choiceType;


    private bool m_enabled;
    private AutosellAmount m_autosellAmount;
    private AutosellType m_autosellType;

    private ArtifactType m_artifactType;
    private Rarity m_rarity;



    void Awake()
    {
        UpdateSettings();
    }


    public void SetOnValueChange(Action callback)
    {
        m_toggle.onValueChanged.AddListener(delegate { callback?.Invoke(); });
        m_amountType.onValueChanged.AddListener(delegate { callback?.Invoke(); });
        m_sellType.onValueChanged.AddListener(delegate { callback?.Invoke(); });
        m_choiceType.onValueChanged.AddListener(delegate { callback?.Invoke(); });
    }

    public void SetSettings(AutosellRule rule)
    {
        m_enabled = rule.GetIsActive();
        m_autosellAmount = rule.GetAutosellAmount();
        m_autosellType = rule.GetAutosellType();
        m_artifactType = rule.GetArtifactType();
        m_rarity = rule.GetRarity();

        PopulateDropdowns();
    }

    public void UpdateSettings()
    {
        m_enabled = m_toggle.enabled;
        m_autosellAmount = (AutosellAmount)m_amountType.value;
        m_autosellType = (AutosellType)m_sellType.value;

        // If ANY has been selected, dont show the choice dropdown
        if (m_autosellType == AutosellType.ANY)
        {
            m_choiceType.gameObject.SetActive(false);
        }

        switch (m_autosellType)
        {
            case AutosellType.RARITY:
                m_rarity = (Rarity)m_choiceType.value;
                break;

            case AutosellType.TYPE:
                m_artifactType = (ArtifactType)m_choiceType.value;
                break;

            default:
                break;
        }


        PopulateDropdowns();

    }


    /// <summary>
    /// Adds an option for every enum record. Changes depending on the given type.
    /// </summary>
    /// <param name="autosellType"></param>
    private void PopulateChoiceDropdown() 
    {
        List<string> options = new List<string>();

        switch (m_autosellType)
        {
            case AutosellType.RARITY:
                foreach (Rarity rarity in Enum.GetValues(typeof(Rarity)))
                {
                    options.Add(rarity.ToString());
                }

                break;

            case AutosellType.TYPE:
                foreach (ArtifactType artifactType in Enum.GetValues(typeof(ArtifactType)))
                {
                    options.Add(artifactType.ToString());
                }

                break;

            default:
                break;
        }

        m_choiceType.AddOptions(options);
    }


    private void PopulateDropdowns()
    {
        m_toggle.enabled = m_enabled;

        PopulateAmountDropdown();
        PopulateTypeDropdown();

        PopulateChoiceDropdown();
    }


    private void PopulateAmountDropdown()
    {
        List<string> options = new List<string>();

        foreach (AutosellAmount amount in Enum.GetValues(typeof(AutosellAmount)))
        {
            options.Add(amount.ToString());
        }

        m_amountType.AddOptions(options);
    }

    private void PopulateTypeDropdown()
    {
        List<string> options = new List<string>();

        foreach (AutosellType amount in Enum.GetValues(typeof(AutosellType)))
        {
            options.Add(amount.ToString());
        }

        m_sellType.AddOptions(options);
    }


}