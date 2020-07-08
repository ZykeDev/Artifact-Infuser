using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptHandler : MonoBehaviour {

	public GameObject windowPref;


	void Start() { 
		//NewWindow(); 
		
	}


	public void NewWindow() {
		// Default position is the center of the Canvas
		Vector2 pos = new Vector2(this.transform.position.x, this.transform.position.y);
		GameObject newWindow = Instantiate(windowPref, pos, Quaternion.identity, this.transform) as GameObject;

		

	}
    
}
