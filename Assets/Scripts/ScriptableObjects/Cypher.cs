using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cypher", menuName = "Cypher")]
public class Cypher : ScriptableObject {
	
	[SerializeField] protected int m_ID = 0;
	[SerializeField] protected string m_cypherName = "Cypher Name";
	[SerializeField] protected List<ArtifactType> m_allowedTypes;
	// TODO Make the artifact sprite procedural (hue/sprite overlays depending on resulting item)
	[SerializeField] protected Sprite m_cypherSprite, m_artifactSprite;

	[SerializeField] protected float m_infusionTime;
	[SerializeField] protected Rarity m_rarity;
	[SerializeField] protected int m_price;

	[Header("Runes Required")]
	[SerializeField] protected int m_alphaRune;
	[SerializeField] protected int m_novaRune;
	[SerializeField] protected int m_prismaRune;

	[Header("Tooltip Description")]
	[SerializeField, TextArea] protected string m_tooltipDex;
	private TooltipData m_tooltipData;


	public RequiredRunes GetRequiredRunes() {
		RequiredRunes rr = new RequiredRunes(m_alphaRune, m_novaRune, m_prismaRune);
		return rr;
	}

	// Groups the tooltip data fields into one struct
	public void InitTooltipData()
	{
		m_tooltipData = new TooltipData(m_cypherName, m_tooltipDex);
	}

	public TooltipData GetTooltipData() => m_tooltipData;

	public int GetID() => m_ID;
	public string GetName() => m_cypherName;
	public List<ArtifactType> GetAllowedTypes() => m_allowedTypes;
	public Sprite GetCypherSprite() => m_cypherSprite;
	public Sprite GetArtifactSprite() => m_artifactSprite;
	public float GetInfusionTime() => m_infusionTime;
	public Rarity GetRarity() => m_rarity;
	public int GetPrice() => m_price;
}


public struct RequiredRunes {
	public int alphaRune;
	public int novaRune;
	public int prismaRune;


	public RequiredRunes(int alphaRune, int novaRune, int prismaRune) {
		this.alphaRune = alphaRune;
		this.novaRune = novaRune;
		this.prismaRune = prismaRune;
	}
}
