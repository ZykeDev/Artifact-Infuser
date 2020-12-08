using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Artifact {

	private int m_artifactID;
	private int m_ID;
	private readonly ArtifactType m_type;
	private readonly string m_name;
	[System.NonSerialized] private Sprite m_sprite;
	private readonly Rarity m_rarity;

	private readonly int m_price;
	
	private bool m_isInfused;
	// TODO how do I encode abilities and properties? a list of IDs?
	private List<int> m_properties;
	private List<int> m_abilities;

	private TooltipData m_tooltipData;

	public Artifact(int artifactID, ArtifactType type, string name, Sprite artifactSprite, Rarity rarity, int price) {
		m_artifactID = artifactID;
		m_ID = m_artifactID;
		m_type = type;
		m_name = name;
		m_sprite = artifactSprite;
		m_rarity = rarity;
		m_price = price;
		m_isInfused = false;

		m_tooltipData.title = name;
		m_tooltipData.dex = type.ToString();
	}

	/// <summary>
	/// Constructor overalod using only the Blueprint data
	/// </summary>
	/// <param name="bp"></param>
	public Artifact(Blueprint bp) : this(bp.GetID(), bp.GetArtifactType(), bp.GetName(), bp.GetArtifactSprite(), bp.GetRarity(), bp.GetPrice()) { }


	/// <summary>
	/// Creates an Artifact by infusing the given base with a cypher
	/// </summary>
	/// <param name="baseArtifact"></param>
	/// <param name="cypher"></param>
	public Artifact(Artifact baseArtifact, Cypher cypher) {
		m_artifactID = baseArtifact.m_artifactID;

		// The item's ID becomes A00C
		// where A is the artifact ID
		// where C is the cypher ID
		m_ID = (m_artifactID * 1000) + cypher.GetID();

		m_type = baseArtifact.m_type;
		m_name = NewInfusedName(baseArtifact.m_name, cypher);

		if (m_abilities == null || m_abilities.Count == 0)
        {
			m_abilities = new List<int>();
        }

		m_abilities.Add(cypher.GetID());

		m_sprite = baseArtifact.m_sprite;
		m_rarity = baseArtifact.m_rarity;
		m_price = NewPrice(baseArtifact.m_price, cypher);

		m_tooltipData.title = m_name;
		m_tooltipData.dex = m_type.ToString();

		m_isInfused = true;
	}


	public void UpdateSprite() => m_sprite = BlueprintDatabase.GetBlueprint(m_artifactID).GetArtifactSprite();
  

	public int GetArtifactID() => m_artifactID;
	public int GetID() => m_ID;
	public ArtifactType GetArtifactType() => m_type;
	public string GetName() => m_name;
	public Sprite GetSprite() => m_sprite;
	public Rarity GetRarity() => m_rarity;
	public int GetPrice() => m_price;
	public TooltipData GetTooltipData() => m_tooltipData;

	public bool IsInfused() => m_isInfused;
    

	/// <summary>
	/// Returns a new Artifact name with random adjectives based on the cypher
	/// </summary>
	/// <param name="baseName"></param>
	/// <param name="cypher"></param>
	/// <returns></returns>
	private string NewInfusedName(string baseName, Cypher cypher)
    {
		// TODO
		return baseName + " " + cypher.GetName();
    }

	/// <summary>
	/// Returns a new price for the Artifact. The increase in price depends on the rarity of the cypher.
	/// </summary>
	/// <param name="basePrice"></param>
	/// <param name="cypher"></param>
	/// <returns></returns>
	private int NewPrice(int basePrice, Cypher cypher)
    {
		// TODO add random element
		return basePrice * ((int)cypher.GetRarity() + 2);
    }

}

