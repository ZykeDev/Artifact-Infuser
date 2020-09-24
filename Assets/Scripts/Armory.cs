using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armory {

	private List<Artifact> artifacts;


	public Armory() {
		this.artifacts = new List<Artifact>();
	}


	public void AddArtifact(Artifact newArtifact) {
		if (newArtifact == null) {
			Debug.Log("ERROR: Invalid artifact being added to the Armory.");
			return;
		}

		this.artifacts.Add(newArtifact);
	}


	public List<Artifact> GetArtifacts() {
		return this.artifacts;
	}


	// Returns all Artifacts in the Armory that fit the given type
	public List<Artifact> FilterByType(ArtifactType type) {
		List<Artifact> filteredArtifacts = new List<Artifact>();

		foreach(Artifact a in this.artifacts) {
			if (a.GetArtifactType() == type) {
				filteredArtifacts.Add(a);
			}
		}

		return filteredArtifacts;
	}


    
}

