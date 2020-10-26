using UnityEngine;

public class Artifact {

	private int m_artifactTypeID;
	private ArtifactType m_type;
	private string m_name;
	private Sprite m_sprite;
	private int m_rarity;

	private int m_price;

	// TODO how do I encode abilities and properties? a list of IDs?
	private int[] m_abilities;
	private int[] m_properties;



	public Artifact(int artifactTypeID, ArtifactType type, string name, Sprite artifactSprite, int rarity, int price) {
		m_artifactTypeID = artifactTypeID;
		m_type = type;
		m_name = name;
		m_sprite = artifactSprite;
		m_rarity = rarity;
		m_price = price;
	}

	public Artifact(Blueprint bp) : this(bp.GetID(), bp.GetArtifactType(), bp.GetName(), bp.GetArtifactSprite(), bp.GetRarity(), bp.GetPrice()) {
		// Constructor overalod using only a BP
	}



	public ArtifactType GetArtifactType() => m_type;
	public string GetName() => m_name;
	public Sprite GetSprite() => m_sprite;
	public int GetRarity() => m_rarity;
	public int GetPrice() => m_price;

    
}

