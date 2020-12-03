using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Upgrades : MonoBehaviour
{
    [SerializeField] private GameController m_gameController;  

    private List<Upgrade> m_upgrades; // List of all upgrades (maybe make this into an upgrade DB?)
    public List<Upgrade> boughtUpgrades;

    private List<UpgradeButton> m_upgradeButtons;

    public void Awake()
    {
        m_upgrades = new List<Upgrade>();
        boughtUpgrades = new List<Upgrade>();

        m_upgradeButtons = FindObjectsOfType<UpgradeButton>().ToList();
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
                upgrade.Buy();
                boughtUpgrades.Add(upgrade);
                m_gameController.UpdateResourceUILoss(upgrade); // TODO remove once the ResUI takes care of this on update


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



}
