using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    [SerializeField] protected GameController m_gameController;  
    [SerializeField] protected UnlockSystem m_unlockSystem;  

    private List<Upgrade> m_upgrades; // List of all upgrades (maybe make this into an upgrade DB?)
    public List<Upgrade> boughtUpgrades;

    private List<UpgradeButton> m_upgradeButtons;

    public void Init(SaveData saveData)
    {
        m_upgrades = new List<Upgrade>();
        boughtUpgrades = new List<Upgrade>();

        m_upgradeButtons = FindObjectsOfType<UpgradeButton>().ToList();

        foreach (UpgradeButton upBtn in m_upgradeButtons)
        {
            upBtn.Init();
        }


        foreach (UpgradeButton upBtn in m_upgradeButtons)
        {
            upBtn.UpdateFromSave(saveData);
        }

        foreach (UpgradeButton upBtn in m_upgradeButtons)
        {
            upBtn.UpdateRequirements(m_upgradeButtons);
        }
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


    public void Buy(Upgrade upgrade)
    {
        if (boughtUpgrades.Contains(upgrade))
        {
            Debug.LogError("Trying to buy an Upgrade that has already been bought.");
        }
        else
        {
            if (m_gameController.inventory.PayForUpgrade(upgrade))
            {
                m_unlockSystem.Notify(upgrade);
                upgrade.Buy();
                boughtUpgrades.Add(upgrade);
                m_unlockSystem.boughtUpgrades[upgrade.GetID()] = true;
                m_gameController.UpdateResourceUILoss(upgrade);


                foreach (UpgradeButton button in m_upgradeButtons)
                {
                    button.UpdateButton(upgrade);
                }
            }
        }
    }


    public Inventory ApplyBonuses(Inventory inv)
    {
        foreach(Upgrade upgrade in boughtUpgrades)
        {
            inv = upgrade.GetEffect().Apply(inv);
        }

        return inv;
    }

    public float ApplyBonuses(float time)
    {
        foreach (Upgrade upgrade in boughtUpgrades)
        {
            time = upgrade.GetEffect().Apply(time);
        }

        return time;
    }


    public int ApplyBonuses(int gold)
    {
        foreach (Upgrade upgrade in boughtUpgrades)
        {
            gold = upgrade.GetEffect().Apply(gold);
        }

        return gold;
    }


    public Upgrade GetUpgrade(int id) => m_upgrades[id]; // TODO add checks

}
