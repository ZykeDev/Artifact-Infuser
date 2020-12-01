using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private Image m_image;
    [SerializeField] private UpgradeData m_upgradeData;

    private Upgrade m_upgrade;
    private Button m_button;

    private Upgrades m_upgrades;

    void Start()
    {
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


    public void Buy() => m_upgrades.Buy(m_upgrade, this);
    

    public void UpdateButton()
    {
        m_button.interactable = m_upgrade.GetUnlocked() && !m_upgrade.GetBought();
    }

}
