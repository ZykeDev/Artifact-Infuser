using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafter : MonoBehaviour {

    private GameController gameController;
    private UnlockSystem unlockSystem;


    public GameObject blueprintSelectorContent;
    public GameObject blueprintBtnPref;

    private List<GameObject> blueprintBtns;
    private List<int> availableBlueprints;

    private float blueprintBtnWidth = 0;
    private float blueprintBtnHeight = 0;


	// Start is called before the first frame update
	void Start() {
		gameController = GameObject.Find("GameManager").GetComponent<GameController>();
		unlockSystem = GameObject.Find("GameManager").GetComponent<UnlockSystem>();

		blueprintBtns = new List<GameObject>();
		availableBlueprints = new List<int>();

        // Instantiate all btns
		InstantiateBlueprints(unlockSystem.blueprints);
       
        // Udapte the viewer h = #active * btn.h
		UpdateViewportHeight();

        // Get active BPs from the UnlockSystem

    }

    // Update is called once per frame
    void Update() {
        
    }


    public void Notify() {

    }


    // Instantiates ALL game blueprints, activating only the available ones
    private void InstantiateBlueprints(List<Blueprint> bps) {
    	int i = 0;
    	float gap = 10f;
    	
    	foreach (Blueprint bp in bps) {
    		// For some reason Instantiate doesn't accept a 5th param "false" for isWorldMapSpace, so localPos is set later
	    	GameObject newBlueprint = Instantiate(blueprintBtnPref, new Vector2(0, 0), Quaternion.identity, blueprintSelectorContent.transform) as GameObject;
	    	newBlueprint.name = "BPbtn_" + i;

	    	float width = newBlueprint.GetComponent<RectTransform>().sizeDelta.x;
	    	float height = newBlueprint.GetComponent<RectTransform>().sizeDelta.y;

	    	// Store the btn size locally
	    	if (this.blueprintBtnHeight == 0) this.blueprintBtnHeight = height;
	    	if (this.blueprintBtnWidth  == 0) this.blueprintBtnHeight = width;
	    	
	    	newBlueprint.transform.localPosition = new Vector2(width/2 + 8f, -height/2 - height*i - gap);

	    	blueprintBtns.Add(newBlueprint);

	    	i++;
    	}
    }


	private void UpdateViewportHeight() {
		// 4 is how many btns can fit at a time
		if (availableBlueprints.Count < 4) {
			blueprintSelectorContent.GetComponent<RectTransform>().sizeDelta.y = 560f;

		} else {
			//blueprintSelectorContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, blueprintBtnHeight * (availableBlueprints.Count + 1));

		}
	}


}
