using System.Collections.Generic;
using UnityEngine;

public class AutosellRule
{    
    private bool m_isActive = false;
    private AutosellAmount m_amount;
    private AutosellType m_type;

    private Rarity m_rarity = Rarity.COMMON;
    private ArtifactType m_artifactType = ArtifactType.WEAPON;


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




    public void ChangeRule(AutosellAmount amount, AutosellType type)
    {
        m_amount = amount;
        m_type = type;
    }

    public void SetArtifactType(ArtifactType artifactType) => m_artifactType = artifactType;
    public void SetArtifactRarity(Rarity rarity) => m_rarity = rarity;



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
                filteredArmory = armory.FilterByRarity(m_rarity);
               
                break;

            case AutosellType.TYPE:
                filteredArmory = armory.FilterByType(m_artifactType);

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

}
