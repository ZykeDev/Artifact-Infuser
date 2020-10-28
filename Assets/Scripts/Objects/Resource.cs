using UnityEngine;

public class Resource {
	
	private ResourceType type;
	public double amount;

	public Resource(ResourceType type, double amount) {
		this.type = type;
		this.amount = amount;
	}


	public void Add(double amountToAdd) {
		if (amountToAdd <= 0) return;

		// Throw an exception if the amount would overflow
		if (amount >= double.MaxValue - amountToAdd) {
#if UNITY_EDITOR
			Debug.LogError("Amount of " + type + " has reached MaxDouble");
#endif
		}

		// Cast the result to int, then back to double to maintain it
		amount = (double)Mathf.Round((float)(amount + amountToAdd));
	}



    public override string ToString() => amount.ToString();

    public void Print() => MonoBehaviour.print(amount);


}
