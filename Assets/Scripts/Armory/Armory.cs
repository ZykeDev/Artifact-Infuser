using System.Collections.Generic;
using UnityEngine;

public class Armory {

	private List<Artifact> m_artifacts;

	/// <summary>
	/// Creates an empty Armory
	/// </summary>
	public Armory()
	{
		m_artifacts = new List<Artifact>();
	}


	public void AddArtifact(Artifact newArtifact)
	{
		if (newArtifact == null) 
		{
#if UNITY_EDITOR
			Debug.LogError("An invalid artifact is being added to the Armory.");
#endif
			return;
		}

		m_artifacts.Add(newArtifact);
	}


	/// <summary>
	/// Removes an artifact from the armory. Returns false if it's not in the list.
	/// </summary>
	/// <param name="artifactToRemove"></param>
	/// <returns></returns>
	public bool RemoveArtifact(Artifact artifactToRemove)
    {
		return m_artifacts.Remove(artifactToRemove);
    }




	#region Getters

	/// <summary>
	/// Returns true if the armory is empty
	/// </summary>
	/// <returns></returns>
	public bool IsEmpty() => m_artifacts.Count == 0;

    /// <summary>
    /// Returns the list of artifacts in the Armory
    /// </summary>
    /// <returns></returns>
    public List<Artifact> GetArtifacts()
	{
		return m_artifacts;
	}

	/// <summary>
	/// Returns the first artifact in the armory with the given ID. Returns null if none are found.
	/// </summary>
	/// <param name="ID"></param>
	/// <returns></returns>
	public Artifact GetArtifact(int ID)
    {
		if (ID < m_artifacts.Count)
        {
			if (m_artifacts[ID].GetArtifactID() == ID)
            {
				return m_artifacts[ID];
            }
        }

		foreach (Artifact artifact in m_artifacts)
        {
			if (artifact.GetArtifactID() == ID)
            {
				return artifact;
            }
        }

		return null;
    }

	/// <summary>
	/// Returns true if an artifact with the given ID exists in the armory.
	/// </summary>
	/// <param name="ID"></param>
	/// <returns></returns>
	public bool HasArtifact(int ID)
    {
		if (ID < m_artifacts.Count)
		{
			if (m_artifacts[ID].GetArtifactID() == ID)
			{
				return true;
			}
		}

		foreach (Artifact artifact in m_artifacts)
		{
			if (artifact.GetArtifactID() == ID)
			{
				return true;
			}
		}

		return false;
	}


	/// <summary>
	/// Returns a list of all artifacts, except the last N ones
	/// </summary>
	/// <param name="exceptN"></param>
	/// <returns></returns>
	public List<Artifact> GetArtifacts(int exceptN)
	{
		List<Artifact> filterdArtifacts = new List<Artifact>();

		int targetAmount = m_artifacts.Count - exceptN;

		if (targetAmount > 0)
		{
			for (int i = 0; i < targetAmount; i++)
			{
				filterdArtifacts.Add(m_artifacts[i]);
			}
		}

		return filterdArtifacts;
	}



	/// <summary>
	/// Returns all Artifacts in the Armory that fit the given type, except for the last n ones
	/// </summary>
	/// <param name="filter"></param>
	/// <param name="n"></param>
	/// <returns></returns>
	public List<Artifact> FilterByType(ArtifactType filter, int n)
	{
		List<Artifact> filteredArtifacts = new List<Artifact>();

		int targetAmount = m_artifacts.Count - n;

		if (targetAmount > 0)
		{
			for (int i = 0; i < targetAmount; i++)
			{
				Artifact art = m_artifacts[i];
				if (art.GetArtifactType() == filter)
				{
					filteredArtifacts.Add(art);
				}
			}
		}

		return filteredArtifacts;
	}

	/// <summary>
	/// Returns all Artifacts in the Armory that fit the given type
	/// </summary>
	/// <param name="filter"></param>
	/// <returns></returns>
	public List<Artifact> FilterByType(ArtifactType filter) => FilterByType(filter, 0);



	/// <summary>
	/// Returns all Artifacts in the Armory that fit the given rarity, except for the last n ones
	/// </summary>
	/// <param name="filter"></param>
	/// <param name="n"></param>
	/// <returns></returns>
	public List<Artifact> FilterByRarity(Rarity filter, int n)
    {
		List<Artifact> filteredArtifacts = new List<Artifact>();

		int targetAmount = m_artifacts.Count - n;

		if (targetAmount > 0)
		{
			for (int i = 0; i < targetAmount ; i++)
			{
				Artifact art = m_artifacts[i];
				if (art.GetRarity() == filter)
				{
					filteredArtifacts.Add(art);
				}
			}
		}

		return filteredArtifacts;
	}

	/// <summary>
	/// Returns all Artifacts in the Armory that fit the given type
	/// </summary>
	/// <param name="filter"></param>
	/// <returns></returns>
	public List<Artifact> FilterByRarity(Rarity filter) => FilterByRarity(filter, 0);

	#endregion

}

