using UnityEngine;

public class Resource {
	
	private ResourceType type;
	public double amount;

	public Resource(ResourceType type, double amount) {
		this.type = type;
		this.amount = amount;
	}


	public void Add(double amountToAdd) {
		// Throw an exception if the amount would overflow
		if (this.amount >= double.MaxValue - amountToAdd) {
			throw new System.InvalidOperationException("Amount of " + this.type + " has reached MaxDouble");
			return;
		}

		if (amountToAdd <= 0) {
			return;
		}

		// Cast the result to int, then back to double to maintain it
		this.amount = (double)Mathf.Round((float)(this.amount + amountToAdd));
	}



	public string ToString() {
		return this.amount.ToString();
	}



	public void Print() {
		MonoBehaviour.print(this.amount);
	}


}
