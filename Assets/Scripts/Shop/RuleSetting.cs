using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RuleSetting : MonoBehaviour
{
    [SerializeField] protected Toggle m_toggle;
    [SerializeField] protected TMP_Dropdown m_amountType;
    [SerializeField] protected TMP_Dropdown m_sellType;
    [SerializeField] protected TMP_Dropdown m_choiceType;


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
        m_enabled = m_toggle.isOn;
        m_autosellAmount = (AutosellAmount)m_amountType.value;
        m_autosellType = (AutosellType)m_sellType.value;

        // If ANY has been selected, dont show the choice dropdown
        if (m_autosellType == AutosellType.ANY) m_choiceType.gameObject.SetActive(false);
        else                                    m_choiceType.gameObject.SetActive(true);


        switch (m_autosellType)
        {
            case AutosellType.RARITY:
                m_rarity = (Rarity)m_choiceType.value;
                PopulateChoiceDropdown();
                break;

            case AutosellType.TYPE:
                m_artifactType = (ArtifactType)m_choiceType.value;
                PopulateChoiceDropdown();
                break;

            default:
                break;
        }
    }

    private void PopulateDropdowns()
    {
        m_toggle.isOn = m_enabled;

        PopulateAmountDropdown();
        PopulateTypeDropdown();

        PopulateChoiceDropdown();
    }


    /// <summary>
    /// Adds an option for every enum record. Changes depending on the given type.
    /// </summary>
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
                    // Ignore ALL
                    if (artifactType == ArtifactType.ALL) continue;

                    options.Add(artifactType.ToString());
                }

                break;

            default:
                break;
        }

        m_choiceType.ClearOptions();
        m_choiceType.AddOptions(options);
    }

    private void PopulateAmountDropdown()
    {
        List<string> options = new List<string>();

        foreach (AutosellAmount amount in Enum.GetValues(typeof(AutosellAmount)))
        {
            string optionString = GetAmountToString(amount.ToString());
            options.Add(optionString);
        }

        m_amountType.ClearOptions();
        m_amountType.AddOptions(options);
    }

    private void PopulateTypeDropdown()
    {
        List<string> options = new List<string>();

        foreach (AutosellType amount in Enum.GetValues(typeof(AutosellType)))
        {
            options.Add(amount.ToString());
        }

        m_sellType.ClearOptions();
        m_sellType.AddOptions(options);
    }



    public bool GetEnabled() => m_enabled;
    public AutosellAmount GetAutosellAmount() => m_autosellAmount;
    public AutosellType GetAutosellType() => m_autosellType;
    public ArtifactType GetArtifactType() => m_artifactType;
    public Rarity GetRarity() => m_rarity;




    private string GetAmountToString(string input)
    {
        if (input == "ALL")         return "ALL";
        if (input == "ALL_BUT_1")   return "ALL BUT 1";

        return input;
    }
}
