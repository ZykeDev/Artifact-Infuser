using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cypher", menuName = "Cypher")]
public class Cypher : ScriptableObject {
	
	public int ID;
	public string name;
	public List<ArtifactType> allowedTypes;
	public Sprite cypherSprite;
	public Sprite artifactSprite; // Make this procedural (hue/sprite overlays depending on resulting item)

	public float infusionTime;
	public int rarity;
	public int price;

	public double alphaRune;
	public double novaRune;
	public double gradientRune;

	public RequiredRunes GetRequiredResources() {
		RequiredRunes rr = new RequiredRunes(this.alphaRune, this.novaRune, this.gradientRune);
		return rr;
	}

}


public struct RequiredRunes {
	public double alphaRune;
	public double novaRune;
	public double gradientRune;


	public RequiredRunes(double alphaRune, double novaRune, double gradientRune) {
		this.alphaRune = alphaRune;
		this.novaRune = novaRune;
		this.gradientRune = gradientRune;
	}
}
