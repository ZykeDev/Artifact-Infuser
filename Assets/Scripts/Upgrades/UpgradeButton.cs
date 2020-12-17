using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] protected Image m_image;
    [SerializeField] protected UpgradeData m_upgradeData;

    private Upgrade m_upgrade;
    private Button m_button;

    private Upgrades m_upgrades;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        if (m_upgrade != null) return;

        m_upgrades = FindObjectOfType<Upgrades>();


        // Attach the upgrade
        m_upgrade = new Upgrade(m_upgradeData);
        m_upgrades.AddUpgrade(m_upgrade);
            

        // Setup the button and tooltip    
        m_button = GetComponent<Button>();
        TooltipTrigger m_tooltip = GetComponent<TooltipTrigger>();

        m_image.sprite = m_upgrade.GetSprite();
        m_button.interactable = m_upgrade.GetUnlocked() && !m_upgrade.GetBought();

        TooltipData tooltipData = new TooltipData(m_upgrade.GetName(), m_upgrade.GetDex());
        m_tooltip.SetTooltipData(tooltipData);

        m_button.onClick.AddListener(Buy);
    }

    public void UpdateFromSave(SaveData saveData)
    {
        if (saveData == null) return;

        if (m_upgrade == null) Init();

        // If this upgrades had been bought, update it
        if (saveData.boughtUpgrades[m_upgrade.GetID()])
        {
            m_upgrade.Unlock();
            m_upgrade.Buy();

            UpdateButton();
        }
    }

    public void Buy() => m_upgrades.Buy(m_upgrade);


    public void UpdateButton() => UpdateButton(null);

    public void UpdateButton(Upgrade boughtUpgrade)
    {
        // Remove the bought upgrade from the requirements
        if (boughtUpgrade != null)
        {
            UpgradeData originalData = boughtUpgrade.GetOriginal();
            List<UpgradeData> requirements = m_upgrade.GetRequirements();

            if (requirements.Contains(originalData))
            {
                requirements.Remove(originalData);
            }
        }

        // Check if all requirements have been bought
        List<UpgradeData> updatedRequirements = m_upgrade.GetRequirements();

        if (updatedRequirements.Count == 0)
        {
            m_upgrade.Unlock();
        }

        m_button.interactable = m_upgrade.GetUnlocked() && !m_upgrade.GetBought();

        if (m_upgrade.GetBought())
        {
            m_button.image.color = Color.red;
        }

    }


    public void UpdateRequirements(List<UpgradeButton> otherButtons)
    {
        if (m_upgrade == null) Init();

        if (!m_upgrade.GetBought())
        {
            List<UpgradeData> requirements = m_upgrade.GetRequirements();
            bool isUnlockable = true;

            foreach (UpgradeData req in requirements)
            {
                foreach (UpgradeButton upBtn in otherButtons)
                {
                    bool isBought = upBtn.m_upgrade.GetBought();
                    bool isReq = upBtn.m_upgrade.GetOriginal() == req;

                    if (!isBought && isReq)
                    {
                        isUnlockable = false;
                        break;
                    }
                }
            }

            if (isUnlockable)
            {
                m_upgrade.Unlock();
                m_button.interactable = m_upgrade.GetUnlocked() && !m_upgrade.GetBought();
            }
        }
    }

}
