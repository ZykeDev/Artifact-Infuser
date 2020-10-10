using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Blueprint", menuName = "Blueprint")]
public class Blueprint : ScriptableObject {
	
	[SerializeField]
	private int ID;
	public string artifactName;
	public ArtifactType type;
	public Sprite blueprintSprite, artifactSprite;

	public float craftingTime;
	public int rarity;
	public int price;

	public double wood, metal, leather, crystals;

	[SerializeField]
	private string tooltipDex;

	private TooltipData tooltipData;


    // Groups the tooltip data fields into one struct
    public void InitTooltipData()
    {
		tooltipData = new TooltipData(artifactName, tooltipDex);
    }

	public int GetID()
    {
		return this.ID;
    }

	public RequiredResources GetRequiredResources()
	{
		RequiredResources rr = new RequiredResources(this.wood, this.metal, this.leather, this.crystals);
		return rr;
	}
	
	public TooltipData GetTooltipData()
    {
		return this.tooltipData;
    }

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