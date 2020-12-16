using UnityEngine;

[CreateAssetMenu(fileName = "New Blueprint", menuName = "Blueprint")]
public class Blueprint : ScriptableObject {

	[SerializeField] protected int m_ID;
	[SerializeField] protected string m_artifactName;
	[SerializeField] protected ArtifactType m_type;
	[SerializeField] protected Sprite m_blueprintSprite, m_artifactSprite, m_reverseSprite;

	[SerializeField] [Min(0)] protected float m_craftingTime;
	[SerializeField] protected Rarity m_rarity;
	[SerializeField] protected int m_price;

	[Header("Resources Required")]
	[SerializeField] [Min(0)] protected int m_wood;
	[SerializeField] [Min(0)] protected int m_metal;
	[SerializeField] [Min(0)] protected int m_leather;
	[SerializeField] [Min(0)] protected int m_crystals;

	[Header("Tooltip Description")]
	[SerializeField, TextArea] protected string m_tooltipDex;
	private TooltipData m_tooltipData;


	// Groups the tooltip data fields into one struct
	public void InitTooltipData()
	{
		m_tooltipData = new TooltipData(m_artifactName, FormatDex(m_tooltipDex));
	}

	public RequiredResources GetRequiredResources()
	{
		RequiredResources rr = new RequiredResources(m_wood, m_metal, m_leather, m_crystals);
		return rr;
	}

	public TooltipData GetTooltipData()
	{
		return m_tooltipData;
	}


	private string FormatDex(string dex)
	{
		dex += "\n";

		if (m_wood > 0) dex += "\n[Wood: " + m_wood + "]";
		if (m_metal > 0) dex += "\n[Metal: " + m_metal + "]";
		if (m_leather > 0) dex += "\n[Leather: " + m_leather + "]";
		if (m_crystals > 0) dex += "\n[Crystals: " + m_crystals + "]";

		return dex;
	}



	public int GetID() => m_ID;
	public string GetName() => m_artifactName;
	public ArtifactType GetArtifactType() => m_type;
	public Sprite GetBlueprintSprite() => m_blueprintSprite;
	public Sprite GetArtifactSprite() => m_artifactSprite;
	public Sprite GetReverseSprite() => m_reverseSprite;
	public float GetCraftingTime() => m_craftingTime;
	public Rarity GetRarity() => m_rarity;
	public int GetPrice() => m_price;

}

[System.Serializable]
public struct TooltipData
{
	public string title, dex;

	public TooltipData(string title, string dex)
	{
		if (title == null)
		{
			this.title = "";
		}
		else
		{
			this.title = title;
		}

		if (dex == null)
		{
			this.dex = "";
		}
		else
		{
			this.dex = dex;
		}
	}

	public bool IsEmpty()
    {
		return (title == null && dex == null) || (title == "" && dex == "");
    }
}


[System.Serializable]
public struct RequiredResources
{
	public int wood, metal, leather, crystals;

	public RequiredResources(int wood, int metal, int leather, int crystals) {
		this.wood = wood;
		this.metal = metal;
		this.leather = leather;
		this.crystals = crystals;
	}
}


