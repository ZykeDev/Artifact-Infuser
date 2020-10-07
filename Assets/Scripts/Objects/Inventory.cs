using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {

	public int gold;
	public Resource wood;
	public Resource metal;
	public Resource leather;
	public Resource crystals;

	public List<Blueprint> blueprints;

	private double[] tierMultiplier = new double[] {1, 2, 5, 10};


	public Inventory() {
		this.gold = 0;

		this.wood = new Resource(ResourceType.WOOD, 0);
		this.metal = new Resource(ResourceType.METAL, 0);
		this.leather = new Resource(ResourceType.LEATHER, 0);
		this.crystals = new Resource(ResourceType.CRYSTALS, 0);

		this.blueprints = new List<Blueprint>();
	}


	// Gains the resources from another inventory object
	public void CombineWith(Inventory newInv) {
		this.gold += newInv.gold;

		this.wood.Add(newInv.wood.amount);
		this.metal.Add(newInv.metal.amount);
		this.leather.Add(newInv.leather.amount);
		this.crystals.Add(newInv.crystals.amount);
	}



	public double GetResourceAmount(ResourceType type) {
		switch (type) {
			case ResourceType.WOOD:
				return this.wood.amount;

			case ResourceType.METAL:
				return this.metal.amount;

			case ResourceType.LEATHER:
				return this.leather.amount;

			case ResourceType.CRYSTALS:
				return this.crystals.amount;
	
			default:
				return 0;
		}
	}


	// Adds random resources to the inventory
	public void SetRandomResources(int tier) {
		double multiplier = tierMultiplier[tier];

		this.wood.Add(multiplier * Random.Range(5f, 12f));
		this.metal.Add(multiplier * Random.Range(6f, 9f));
		this.leather.Add((multiplier-1) * Random.Range(4f, 7f));
		this.crystals.Add((multiplier-4) * Random.Range(1f, 2f));
	}



    
}

