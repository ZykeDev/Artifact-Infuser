using UnityEngine;

[CreateAssetMenu(fileName = "New Blueprint", menuName = "Blueprint")]
public class Blueprint : ScriptableObject {

	[SerializeField] private int m_ID;
	[SerializeField] private string m_artifactName;
	[SerializeField] private ArtifactType m_type;
	[SerializeField] private Sprite m_blueprintSprite, m_artifactSprite, m_reverseSprite;

	[SerializeField] [Min(0)] private float m_craftingTime;
	[SerializeField] private Rarity m_rarity;
	[SerializeField] private int m_price;

	[Header("Resources Required")]
	[SerializeField] [Min(0)] private int m_wood;
	[SerializeField] [Min(0)] private int m_metal;
	[SerializeField] [Min(0)] private int m_leather;
	[SerializeField] [Min(0)] private int m_crystals;

	[Header("Tooltip Description")]
	[SerializeField, TextArea] private string m_tooltipDex;
	private TooltipData m_tooltipData;


	// Groups the tooltip data fields into one struct
	public void InitTooltipData()
	{
		m_tooltipData = new TooltipData(m_artifactName, m_tooltipDex);
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


