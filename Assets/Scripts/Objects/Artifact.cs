using UnityEngine;

public class Artifact {

	private int m_artifactTypeID;
	private readonly ArtifactType m_type;
	private readonly string m_name;
	private readonly Sprite m_sprite;
	private readonly int m_rarity;

	private readonly int m_price;

	// TODO how do I encode abilities and properties? a list of IDs?
	private int[] m_abilities;
	private int[] m_properties;

	private TooltipData m_tooltipData;

	public Artifact(int artifactTypeID, ArtifactType type, string name, Sprite artifactSprite, int rarity, int price) {
		m_artifactTypeID = artifactTypeID;
		m_type = type;
		m_name = name;
		m_sprite = artifactSprite;
		m_rarity = rarity;
		m_price = price;

		m_tooltipData.title = name;
		m_tooltipData.dex = type.ToString();
	}

	public Artifact(Blueprint bp) : this(bp.GetID(), bp.GetArtifactType(), bp.GetName(), bp.GetArtifactSprite(), bp.GetRarity(), bp.GetPrice()) {
		// Constructor overalod using only the BP data
	}



	public ArtifactType GetArtifactType() => m_type;
	public string GetName() => m_name;
	public Sprite GetSprite() => m_sprite;
	public int GetRarity() => m_rarity;
	public int GetPrice() => m_price;
	public TooltipData GetTooltipData() => m_tooltipData;
    
}

