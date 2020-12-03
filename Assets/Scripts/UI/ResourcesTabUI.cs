using UnityEngine;

public class ResourcesTabUI : MonoBehaviour 
{
	[Header("Script References")]
	[SerializeField] protected GameController m_gameController;

	[SerializeField] protected ResourceUI m_gold, m_wood, m_metal, m_leather, m_crystals;

	[SerializeField] protected Color m_gainColor;
	[SerializeField] protected Color m_loseColor;


	void Awake()
    {
		m_gold.SetColors(m_gainColor, m_loseColor);
		m_wood.SetColors(m_gainColor, m_loseColor);
		m_metal.SetColors(m_gainColor, m_loseColor);
		m_leather.SetColors(m_gainColor, m_loseColor);
		m_crystals.SetColors(m_gainColor, m_loseColor);
	}


    void Update() {
		// Retrieve the current inventory from the GameController
		Inventory inventory = m_gameController.inventory;

		m_gold.UpdateUI(inventory);
		m_wood.UpdateUI(inventory, ResourceType.WOOD);
		m_metal.UpdateUI(inventory, ResourceType.METAL);
		m_leather.UpdateUI(inventory, ResourceType.LEATHER);
		m_crystals.UpdateUI(inventory, ResourceType.CRYSTALS);
	}


    #region Gains

    public void DisplayGain(Inventory booty)
    {
		m_gold.DisplayGain(booty);
		m_wood.DisplayGain(booty, ResourceType.WOOD);
		m_metal.DisplayGain(booty, ResourceType.METAL);
		m_leather.DisplayGain(booty, ResourceType.LEATHER);
		m_crystals.DisplayGain(booty, ResourceType.CRYSTALS);
	}

	public void DisplayGain(RequiredResources resources)
	{
		m_wood.DisplayGain(resources.wood);
		m_metal.DisplayGain(resources.metal);
		m_leather.DisplayGain(resources.leather);
		m_crystals.DisplayGain(resources.crystals);
	}

	public void DisplayGain(int gold) => m_gold.DisplayGain(gold);
    

	#endregion


	#region Losses

	public void DisplayLoss(RequiredResources resources)
	{
		m_wood.DisplayGain(-resources.wood);
		m_metal.DisplayGain(-resources.metal);
		m_leather.DisplayGain(-resources.leather);
		m_crystals.DisplayGain(-resources.crystals);
	}

	public void DisplayLoss(Inventory booty)
	{
		m_gold.DisplayLoss(booty.gold);
		m_wood.DisplayLoss(booty, ResourceType.WOOD);
		m_metal.DisplayLoss(booty, ResourceType.METAL);
		m_leather.DisplayLoss(booty, ResourceType.LEATHER);
		m_crystals.DisplayLoss(booty, ResourceType.CRYSTALS);
	}


	public void DisplayLoss(Upgrade upgrade)
	{
		m_gold.DisplayLoss(upgrade.GetGold());
		DisplayLoss(upgrade.GetRequiredResources());
	}

	#endregion
}
