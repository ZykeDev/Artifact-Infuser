using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // Currently selected blueprint
    private int selectedBlueprintID;


	// Start is called before the first frame update
	void Start() {
		gameController = GameObject.Find("GameManager").GetComponent<GameController>();
		unlockSystem = GameObject.Find("GameManager").GetComponent<UnlockSystem>();
		buttonHandler = GameObject.Find("MainCanvas").GetComponent<ButtonHandler>();

		blueprintBtns = new List<GameObject>();

		blueprints = unlockSystem.blueprints;
		activeBlueprints = unlockSystem.activeBlueprints;

        // Instantiate the active blueprint btns
		InstantiateBlueprints();
       
        // Udapte the viewer h = #active * btn.h
		UpdateViewportHeight();

    }

    // Update is called once per frame
    void Update() {
        
    }


    public void Notify() {

    }

    // Destroys all blueprint btns and re-instantiates them, checking for new active ones
    public void UpdateActiveBlueprints() {
    	foreach (GameObject bp in blueprintBtns) {
    		Destroy(bp);
    	}

    	//blueprintBtns = new List<GameObject>();
    	blueprintBtns.Clear();

    	InstantiateBlueprints();
    	print(blueprintBtns.Count);
    }




    // Instantiates all active blueprints
    private void InstantiateBlueprints() {
       	
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

	    	// Add a listener to the new Button
	    	newBlueprint.GetComponent<Button>().onClick.AddListener(delegate {buttonHandler.OnSelectBlueprintClick(bp.ID); });
	    	
	    	float width = newBlueprint.GetComponent<RectTransform>().sizeDelta.x;
	    	float height = newBlueprint.GetComponent<RectTransform>().sizeDelta.y;

	    	// Store the btn size locally
	    	if (this.blueprintBtnHeight == 0) this.blueprintBtnHeight = height;
	    	if (this.blueprintBtnWidth  == 0) this.blueprintBtnHeight = width;
	    	
	    	// Correctly position the new button. Use it's ID as a vertical index
	    	newBlueprint.transform.localPosition = new Vector2(width / 2 + 8f, -height/2 - (height * bp.ID) - gap);

	    	blueprintBtns.Add(newBlueprint);
    	}
    }





	private void UpdateViewportHeight() {
		RectTransform bpSelectorContentTransform = blueprintSelectorContent.GetComponent<RectTransform>();
		float blueprintSelectorContentWidth = bpSelectorContentTransform.sizeDelta.x;
		// 4 is how many btns can fit at a time
		if (GetNumberOfActiveBlueprints() < 4) {
			bpSelectorContentTransform.sizeDelta = new Vector2(blueprintSelectorContentWidth, 560f);

		} else {
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
