using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafter : MonoBehaviour {

    private GameController gameController;
    private UnlockSystem unlockSystem;
    private ButtonHandler buttonHandler;

    public GameObject blueprintSelectorContent;
    public GameObject blueprintBtnPref;

    private List<GameObject> blueprintBtns;
    private List<Blueprint> blueprints;
    private bool[] activeBlueprints;

    private float blueprintBtnWidth = 0;
    private float blueprintBtnHeight = 0;


	// Start is called before the first frame update
	void Start() {
		gameController = GameObject.Find("GameManager").GetComponent<GameController>();
		unlockSystem = GameObject.Find("GameManager").GetComponent<UnlockSystem>();
		buttonHandler = GameObject.Find("MainCanvas").GetComponent<ButtonHandler>();

		blueprintBtns = new List<GameObject>();

		blueprints = unlockSystem.blueprints;
		activeBlueprints = unlockSystem.activeBlueprints;

        // Instantiate all btns
		InstantiateBlueprints();
       
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
    private void InstantiateBlueprints() {
       	int i = 0;
    	float gap = 10f;
    	
    	foreach (Blueprint bp in this.blueprints) {
    		// Don't show blueprints that are not active
    		if (activeBlueprints[bp.ID] == false) {
    			continue;
    		}

    		// For some reason Instantiate doesn't accept a 5th param "false" for isWorldMapSpace, so localPos is set later
	    	GameObject newBlueprint = Instantiate(blueprintBtnPref, new Vector2(0, 0), Quaternion.identity, blueprintSelectorContent.transform) as GameObject;
	    	newBlueprint.name = "BPbtn_" + bp.ID;

	    	// Not being used for now. Shouse it be added to the "newBlueprint" gameobj?
	    	BlueprintBtnData bpbtndata = newBlueprint.AddComponent<BlueprintBtnData>();
	    	bpbtndata.SetID(bp.ID);

	    	print(newBlueprint.GetComponent<Button>());
	    	//newBlueprint.GetComponent<Button>().onClick.AddListener(delegate {buttonHandler.OnSelectBlueprintClick(1); });
	    	

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
		RectTransform bpSelectorContentTransform = blueprintSelectorContent.GetComponent<RectTransform>();
		float blueprintSelectorContentWidth = bpSelectorContentTransform.sizeDelta.x;
		// 4 is how many btns can fit at a time
		if (GetNumberOfActiveBlueprints() < 4) {
			bpSelectorContentTransform.sizeDelta = new Vector2(blueprintSelectorContentWidth, 560f);

		} else {
			print(blueprintSelectorContentWidth);
			print(blueprintBtnHeight * (GetNumberOfActiveBlueprints() + 1));
			bpSelectorContentTransform.sizeDelta = new Vector2(blueprintSelectorContentWidth, blueprintBtnHeight * (GetNumberOfActiveBlueprints() + 1));
		}
	}


	// Returns the number of active blueprints
	private int GetNumberOfActiveBlueprints() {
		int n = 0;
		for (int i = 0; i < activeBlueprints.Length; i++) {
			if (activeBlueprints[i]) n++;
		}

		return n;
	}


}
