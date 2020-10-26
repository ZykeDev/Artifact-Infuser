using UnityEngine;

[CreateAssetMenu(fileName = "New Blueprint", menuName = "Blueprint")]
public class Blueprint : ScriptableObject {

	[SerializeField] private int m_ID;
	[SerializeField] private string m_artifactName;
	[SerializeField] private ArtifactType m_type;
	[SerializeField] private Sprite m_blueprintSprite, m_artifactSprite;

	[SerializeField] private float m_craftingTime;
	[SerializeField] private int m_rarity, m_price;

	[SerializeField] private double m_wood, m_metal, m_leather, m_crystals;

	[SerializeField] private string m_tooltipDex;
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
	public float GetCraftingTime() => m_craftingTime;
	public int GetRarity() => m_rarity;
	public int GetPrice() => m_price;

}

public struct TooltipData
{
	public string title, dex;

	public TooltipData(string title, string dex)
    {
		this.title = title;
		this.dex = dex;
    }
}


public struct RequiredResources
{
	public double wood, metal, leather, crystals;

	public RequiredResources(double wood, double metal, double leather, double crystals) {
		this.wood = wood;
		this.metal = metal;
		this.leather = leather;
		this.crystals = crystals;
	}
}