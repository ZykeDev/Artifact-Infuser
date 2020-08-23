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


    
}

