﻿using UnityEngine;

[CreateAssetMenu(fileName = "New Blueprint", menuName = "Blueprint")]
public class Blueprint : ScriptableObject {
	
	public int ID;
	public string name;
	public ArtifactType type;
	public Sprite blueprintSprite;
	public Sprite artifactSprite;

	public float craftingTime;
	public int rarity;
	public int price;

	public double wood;
	public double metal;
	public double glass;
	public double crystals;	

	public RequiredResources GetRequiredResources() {
		RequiredResources rr = new RequiredResources(this.wood, this.metal, this.glass, this.crystals);
		return rr;
	}

}


public struct RequiredResources {
	public double wood;
	public double metal;
	public double glass;
	public double crystals;

	public RequiredResources(double wood, double metal, double glass, double crystals) {
		this.wood = wood;
		this.metal = metal;
		this.glass = glass;
		this.crystals = crystals;
	}
}
