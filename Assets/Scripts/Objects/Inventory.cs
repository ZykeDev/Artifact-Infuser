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
		gold = 0;

		wood = new Resource(ResourceType.WOOD, 0);
		metal = new Resource(ResourceType.METAL, 0);
		leather = new Resource(ResourceType.LEATHER, 0);
		crystals = new Resource(ResourceType.CRYSTALS, 0);

		blueprints = new List<Blueprint>();
	}


	public int GetGold() => gold;


	/// <summary>
	/// Adds the resources from another inventory object
	/// </summary>
	/// <param name="newInv"></param>
	public void CombineWith(Inventory newInv) {
		gold += newInv.gold;

		wood.Add(newInv.wood.amount);
		metal.Add(newInv.metal.amount);
		leather.Add(newInv.leather.amount);
		crystals.Add(newInv.crystals.amount);
	}


	/// <summary>
	/// Retruns the current amount of a given resource type
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public double GetResourceAmount(ResourceType type) {
		switch (type) {
			case ResourceType.WOOD:
				return wood.amount;

			case ResourceType.METAL:
				return metal.amount;

			case ResourceType.LEATHER:
				return leather.amount;

			case ResourceType.CRYSTALS:
				return crystals.amount;
	
			default:
				return 0;
		}
	}




	/// <summary>
	/// Adds random resources to the inventory, based on the given tier
	/// </summary>
	/// <param name="tier"></param>
	public void SetRandomResources(int tier) {
		double multiplier = tierMultiplier[tier];

		wood.Add(multiplier * Random.Range(5f, 12f));
		metal.Add(multiplier * Random.Range(6f, 9f));
		leather.Add((multiplier-1) * Random.Range(4f, 7f));
		crystals.Add((multiplier-4) * Random.Range(1f, 2f));
	}


	/// <summary>
	/// Returns true if there are enough resources in the inventory
	/// </summary>
	/// <param name="requiredResources"></param>
	/// <returns></returns>
	public bool HasEnoughResources(RequiredResources requiredResources)
    {
		if (requiredResources.wood > 0 && wood.amount < requiredResources.wood)
		{
			Debug.Log("Not enough wood. Need " + (requiredResources.wood - wood.amount) + " more.");
			return false;
		}
		if (requiredResources.metal > 0 && metal.amount < requiredResources.metal)
		{
			Debug.Log("Not enough metal.");
			return false;
		}
		if (requiredResources.leather > 0 && leather.amount < requiredResources.leather)
		{
			Debug.Log("Not enough leather.");
			return false;
		}
		if (requiredResources.crystals > 0 && crystals.amount < requiredResources.crystals)
		{
			Debug.Log("Not enough crystals.");
			return false;
		}

		return true;
    }
    

	/// <summary>
	/// Subtracts the given resources from the inventory, if there are enough
	/// </summary>
	/// <param name="requiredResources"></param>
	public bool SpendResources(RequiredResources requiredResources)
    {
		if (!HasEnoughResources(requiredResources))
        {
#if UNITY_EDITOR
			Debug.Log("Not enough resources.");
#endif
			return false;
        }

		wood.Remove(requiredResources.wood);
		metal.Remove(requiredResources.metal);
		leather.Remove(requiredResources.leather);
		crystals.Remove(requiredResources.crystals);

		return true;
	}


	public void AddResources(RequiredResources addedResources)
    {
		wood.Add(addedResources.wood);
		metal.Add(addedResources.metal);
		leather.Add(addedResources.leather);
		crystals.Add(addedResources.crystals);
	}

}

