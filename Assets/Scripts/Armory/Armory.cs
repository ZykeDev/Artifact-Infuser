using System;
using System.Collections;
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
	/// Returns the list of artifacts in the Armory
	/// </summary>
	/// <returns></returns>
	public List<Artifact> GetArtifacts()
	{
		return m_artifacts;
	}


	/// <summary>
	/// Returns all Artifacts in the Armory that fit the given type
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public List<Artifact> FilterByType(ArtifactType filter)
	{
		List<Artifact> filteredArtifacts = new List<Artifact>();

		foreach(Artifact art in m_artifacts)
		{
			if (art.GetArtifactType() == filter)
			{
				filteredArtifacts.Add(art);
			}
		}

		return filteredArtifacts;
	}


    
}

