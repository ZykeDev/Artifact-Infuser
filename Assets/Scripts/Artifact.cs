using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact {

	private int artifactTypeID;
	private string name;
	private Sprite sprite;
	private int rarity;

	private int price;

	// TODO how do I encode abilities and properties? a list of IDs?
	private int[] abilities;
	private int[] properties;



	public Artifact(int artifactTypeID, string name, Sprite sprite, int rarity, int price) {
		this.artifactTypeID = artifactTypeID;
		this.name = name;
		this.sprite = sprite;
		this.rarity = rarity;
		this.price = price;
	}



	public string GetName() {
		return this.name;
	}


	



    
}

