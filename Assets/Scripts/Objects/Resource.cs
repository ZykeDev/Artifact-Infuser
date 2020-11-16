using UnityEngine;

public class Resource {
	
	private ResourceType type;
	public double amount;

	public Resource(ResourceType type, double amount) {
		this.type = type;
		this.amount = amount;
	}

	/// <summary>
	/// Adds the given amount of a resouce
	/// </summary>
	/// <param name="amountToAdd"></param>
	public void Add(double amountToAdd) {
		if (amountToAdd <= 0) return;

		// Throw an exception if the amount would overflow
		if (amount >= double.MaxValue - amountToAdd) {
#if UNITY_EDITOR
			Debug.LogError("Amount of " + type + " has reached MaxDouble");
#endif
		}

		amount = Mathf.Round((float)(amount + amountToAdd));
	}


	/// <summary>
	/// Removes the given amount of a resource
	/// </summary>
	/// <param name="amountToRemove"></param>
	public void Remove(double amountToRemove)
    {
		if (amountToRemove <= 0) return;

		if (amountToRemove >= double.MaxValue)
        {
#if UNITY_EDITOR
			Debug.LogError("Trying to remove MaxDouble of " + type);
#endif
		}

		amount -= amountToRemove;
    }



    public override string ToString() => amount.ToString();

    public void Print() => MonoBehaviour.print(amount);


}
