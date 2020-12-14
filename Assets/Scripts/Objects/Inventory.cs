using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory {

	public int gold;
	public Resource wood;
	public Resource metal;
	public Resource leather;
	public Resource crystals;

	public Rune alphaRune, novaRune, prismaRune;

	[NonSerialized] public List<Blueprint> blueprints;
	[NonSerialized] public List<Cypher> cyphers;

	public List<int> blueprintIDs;
	public List<int> cypherIDs;

	private double[] tierMultiplier = new double[] {1, 2, 5, 10};


	public Inventory() {
		gold = 0;

		wood = new Resource(ResourceType.WOOD, 0);
		metal = new Resource(ResourceType.METAL, 0);
		leather = new Resource(ResourceType.LEATHER, 0);
		crystals = new Resource(ResourceType.CRYSTALS, 0);

		alphaRune = new Rune(RuneType.ALPHA, 0);
		novaRune = new Rune(RuneType.NOVA, 0);
		prismaRune = new Rune(RuneType.PRISMA, 0);

		blueprints = new List<Blueprint>();
		cyphers = new List<Cypher>();

		blueprintIDs = new List<int>();
		cypherIDs = new List<int>();
	}


	public void PrepareForSave()
    {
		if (blueprints == null) blueprints = new List<Blueprint>();
		if (blueprintIDs == null) blueprintIDs = new List<int>();
		if (cyphers == null)	cyphers = new List<Cypher>();
		if (cypherIDs == null)	cypherIDs = new List<int>();


		// Store the blueprints as IDs
		blueprintIDs.Clear();
		for (int i = 0; i < blueprints.Count; i++)
        {
			blueprintIDs.Add(blueprints[i].GetID());
        }

		// Store the cyphers as IDs
		cypherIDs.Clear();
		for (int i = 0; i < cyphers.Count; i++)
		{
			cypherIDs.Add(cyphers[i].GetID());
		}
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

		wood.Add(multiplier * UnityEngine.Random.Range(5f, 12f));
		metal.Add(multiplier * UnityEngine.Random.Range(6f, 9f));
		leather.Add((multiplier-1) * UnityEngine.Random.Range(4f, 7f));
		crystals.Add((multiplier-4) * UnityEngine.Random.Range(1f, 2f));
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
			Debug.Log("Need " + (requiredResources.wood - wood.amount) + " more wood.");
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
	/// Returns true if there are enough runes in the inventory
	/// </summary>
	/// <param name="requiredRunes"></param>
	/// <returns></returns>
	public bool HasEnoughRunes(RequiredRunes requiredRunes)
    {
		if (requiredRunes.alphaRune > 0 && alphaRune.amount < requiredRunes.alphaRune)
        {
			Debug.Log("Not enough wood. Need " + (requiredRunes.alphaRune - alphaRune.amount) + " more.");
			return false;
		}
		if (requiredRunes.novaRune > 0 && novaRune.amount < requiredRunes.novaRune)
		{
			Debug.Log("Not enough wood. Need " + (requiredRunes.novaRune - novaRune.amount) + " more.");
			return false;
		}
		if (requiredRunes.prismaRune > 0 && prismaRune.amount < requiredRunes.prismaRune)
		{
			Debug.Log("Not enough wood. Need " + (requiredRunes.prismaRune - prismaRune.amount) + " more.");
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

	/// <summary>
	/// Subtracts the given runes from the inventory, if there are enough
	/// </summary>
	/// <param name="requiredRunes"></param>
	/// <returns></returns>
	public bool SpendRunes(RequiredRunes requiredRunes)
    {
		if (!HasEnoughRunes(requiredRunes))
		{
#if UNITY_EDITOR
			Debug.Log("Not enough runes.");
#endif
			return false;
		}

		alphaRune.Remove(requiredRunes.alphaRune);
		novaRune.Remove(requiredRunes.novaRune);
		prismaRune.Remove(requiredRunes.prismaRune);


		return true;
    }


	/// <summary>
	/// Adds the given amount of gold to the inventory.
	/// </summary>
	/// <param name="amount"></param>
	public void AddGold(int amount)
    {
		// TODO add maxint checks
		gold += amount;
    }
	
	public bool HasEnoughGold(int amount)
    {
        if (gold >= amount)
        {
			return true;
        }

		return false;
    }

	/// <summary>
	/// Spend the given amount of gold if there is enough.
	/// </summary>
	/// <param name="amount"></param>
	public bool SpendGold(int amount)
    {
		if (!HasEnoughGold(amount))
        {
#if UNITY_EDITOR
			Debug.Log("Not enough gold.");
#endif
			return false;
		}

		gold -= amount;
		return true;
	}


	/// <summary>
	/// Pays the cost of the given upgrade. Returns true if it did.
	/// </summary>
	/// <param name="upgrade"></param>
	/// <returns></returns>
	public bool PayForUpgrade(Upgrade upgrade)
    {
		int upgradeGold = upgrade.GetGold();
		RequiredResources resources = upgrade.GetRequiredResources();
		RequiredRunes runes = upgrade.GetRequiredRunes();

		bool hasGold = HasEnoughGold(upgradeGold);
		bool hasResources = HasEnoughResources(resources);
		bool hasRunes = HasEnoughRunes(runes);

		if (hasGold && hasResources && hasRunes)
        {
			SpendGold(upgradeGold);
			SpendResources(resources);
			SpendRunes(runes);

			return true;
        }

		return false;
	}




	public void AddResources(RequiredResources addedResources)
    {
		wood.Add(addedResources.wood);
		metal.Add(addedResources.metal);
		leather.Add(addedResources.leather);
		crystals.Add(addedResources.crystals);
	}

	public void AddRunes(RequiredRunes addedRunes)
    {
		alphaRune.Add(addedRunes.alphaRune);
		novaRune.Add(addedRunes.novaRune);
		prismaRune.Add(addedRunes.prismaRune);
	}

	public void Add(RequiredResources addedResources) => AddResources(addedResources);
	public void Add(RequiredRunes addedRunes) => AddRunes(addedRunes);

}

