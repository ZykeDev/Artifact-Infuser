using System.Collections.Generic;
using UnityEngine;

public class AutosellRule
{
    #region Vars

    private bool m_isActive = false;
    private AutosellAmount m_amount;
    private AutosellType m_type;

    private Rarity m_rarity = Rarity.COMMON;
    private ArtifactType m_artifactType = ArtifactType.WEAPON;

    #endregion


    #region Constructors

    public AutosellRule(AutosellAmount amount, AutosellType type)
    {
        m_amount = amount;
        m_type = type;
    }

    public AutosellRule(AutosellAmount amount, AutosellType type, ArtifactType artifactType)
    {
        if (type != AutosellType.TYPE)
        {
            Debug.LogError("Using the wrong AutosellRule constructor.");
            return;
        }

        m_amount = amount;
        m_type = type;
        m_artifactType = artifactType;
    }

    public AutosellRule(AutosellAmount amount, AutosellType type, Rarity rarity)
    {
        if (type != AutosellType.RARITY)
        {
            Debug.LogError("Using the wrong AutosellRule constructor.");
            return;
        }

        m_amount = amount;
        m_type = type;
        m_rarity = rarity;
    }

    #endregion



    public void SetArtifactType(ArtifactType artifactType) => m_artifactType = artifactType;
    public void SetArtifactRarity(Rarity rarity) => m_rarity = rarity;

    public void UpdateRule(AutosellAmount amount, AutosellType type)
    {
        m_amount = amount;
        m_type = type;
    }


    public void UpdateRule(RuleSetting settings)
    {
        m_isActive = settings.GetEnabled();
        m_amount = settings.GetAutosellAmount();
        m_type = settings.GetAutosellType();

        m_artifactType = settings.GetArtifactType();
        m_rarity = settings.GetRarity();
    } 





    /// <summary>
    /// Applies the current rule to the given armory
    /// </summary>
    /// <returns></returns>
    public List<Artifact> Apply(Armory armory)
    {
        // Exit if the rules is not active
        if (!m_isActive) return null;

        List<Artifact> filteredArmory = new List<Artifact>();

        switch (m_type)
        {
            case AutosellType.RARITY:
                if (m_amount == AutosellAmount.ALL_BUT_1)
                {
                    filteredArmory = armory.FilterByRarity(m_rarity, 1);
                }
                else
                {
                    filteredArmory = armory.FilterByRarity(m_rarity);
                }
               
                break;

            case AutosellType.TYPE:
                if (m_amount == AutosellAmount.ALL_BUT_1)
                {
                    filteredArmory = armory.FilterByType(m_artifactType, 1);
                }
                else
                {
                    filteredArmory = armory.FilterByType(m_artifactType);
                }

                break;

            case AutosellType.ANY:
                if (m_amount == AutosellAmount.ALL_BUT_1)
                {
                    filteredArmory = armory.GetArtifacts(1);
                }
                else
                {
                    filteredArmory = armory.GetArtifacts();
                }

                // TODO if amount is set to ALL this will sell every single artifact.
                // A prompt to alert the user might be helpul.

                break;

            default:
                break;
        }

        if (filteredArmory.Count != 0)
        {
            return filteredArmory;
        }

        return null;

    }

    /// <summary>
    /// Sets the rule as active
    /// </summary>
    public void Activate() 
    {
        if (m_isActive)
        {
            Debug.Log("Rule is already active.");
        }
        m_isActive = true;
    }
    
    /// <summary>
    /// Deactivates the rule
    /// </summary>
    public void Deactivate()
    {
        m_isActive = false;
    }



    #region Getters

    public bool GetIsActive() => m_isActive;
    public AutosellAmount GetAutosellAmount() => m_amount;
    public AutosellType GetAutosellType() => m_type;
    public ArtifactType GetArtifactType() => m_artifactType;
    public Rarity GetRarity() => m_rarity;

    #endregion
}
