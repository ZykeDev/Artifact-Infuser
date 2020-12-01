using System.Collections.Generic;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    private List<Upgrade> m_upgrades; // List of all upgrades (maybe make this into an upgrade DB?)
    public List<Upgrade> boughtUpgrades;


    void Awake()
    {
        m_upgrades = new List<Upgrade>();
        boughtUpgrades = new List<Upgrade>();
    }

     
    /// <summary>
    /// Adds the given upgrade to the list of all available upgrades
    /// </summary>
    /// <param name="upgrade"></param>
    public void AddUpgrade(Upgrade upgrade)
    {
        if (m_upgrades.Contains(upgrade))
        {
            Debug.LogError("Trying to add an Upgrade that has already been added.");
        }
        else
        {
            m_upgrades.Add(upgrade);
        }
    }



    public void Buy(Upgrade upgrade, UpgradeButton caller)
    {
        if (boughtUpgrades.Contains(upgrade))
        {
            Debug.LogError("Trying to buy an Upgrade that has already been bought.");
        }
        else
        {
            boughtUpgrades.Add(upgrade);
            upgrade.Buy();
            caller.UpdateButton();
        }
        
    }



}
