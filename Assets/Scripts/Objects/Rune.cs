using UnityEngine;

[System.Serializable]
public class Rune
{
    private readonly RuneType type;
	public int amount;
  
    public Rune(RuneType type, int amount)
    {
        this.type = type;
        this.amount = amount;
	}


	/// <summary>
	/// Adds the given amount of a rune
	/// </summary>
	/// <param name="amountToAdd"></param>
	public void Add(int amountToAdd)
	{
		if (amountToAdd <= 0) return;

		// Throw an exception if the amount would overflow
		if (amount >= int.MaxValue - amountToAdd)
		{
#if UNITY_EDITOR
			Debug.LogError("Amount of " + type + " has reached MaxInt");
#endif
		}

		amount += amountToAdd;
	}


	/// <summary>
	/// Removes the given amount of a rune
	/// </summary>
	/// <param name="amountToRemove"></param>
	public void Remove(int amountToRemove)
	{
		if (amountToRemove <= 0) return;

		if (amountToRemove >= int.MaxValue)
		{
#if UNITY_EDITOR
			Debug.LogError("Trying to remove MaxInt of " + type);
#endif
		}

		amount -= amountToRemove;
	}



	public override string ToString() => amount.ToString();

	public void Print() => MonoBehaviour.print(ToString());

}
