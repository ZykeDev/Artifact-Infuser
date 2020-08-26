using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact {

	private int artifactTypeID;
	private ArtifactType type;
	private string name;
	private Sprite sprite;
	private int rarity;

	private int price;

	// TODO how do I encode abilities and properties? a list of IDs?
	private int[] abilities;
	private int[] properties;



	public Artifact(int artifactTypeID, ArtifactType type, string name, Sprite sprite, int rarity, int price) {
		this.artifactTypeID = artifactTypeID;
		this.type = type;
		this.name = name;
		this.sprite = sprite;
		this.rarity = rarity;
		this.price = price;
	}

	public Artifact(Blueprint bp) : this(bp.ID, bp.type, bp.name, bp.sprite, bp.rarity, bp.price) {
		// Constructor overalod using only a BP
	}



	public ArtifactType GetType() {
		return this.type;
	}

	public string GetName() {
		return this.name;
	}

	public Sprite GetSprite() {
		return this.sprite;
	}

	public int GetRarity() {
		return this.rarity;
	}

	public int GetPrice() {
		return this.price;
	}



	



    
}

