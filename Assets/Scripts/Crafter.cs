using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafter : MonoBehaviour {

    private GameController gameController;
    private UnlockSystem unlockSystem;
    private ButtonHandler buttonHandler;

    private Slider progressbar;

    public GameObject blueprintSelectorContent;
    public GameObject blueprintBtnPref;


    private List<GameObject> blueprintBtns;
    private List<Blueprint> blueprints;
    public bool[] activeBlueprints;

    private float blueprintBtnWidth = 0;
    private float blueprintBtnHeight = 0;

    // Currently selected blueprint
    private int selectedBlueprintID = -1;

    private bool isCrafting = false;
	private Coroutine craftingCoroutine = null;
	private Inventory refoundedResources = new Inventory();



	// Start is called before the first frame update
	void Start() {
		gameController = GameObject.Find("GameManager").GetComponent<GameController>();
		unlockSystem = GameObject.Find("GameManager").GetComponent<UnlockSystem>();
		buttonHandler = GameObject.Find("MainCanvas").GetComponent<ButtonHandler>();

		// Reference the crafting progressbar
		progressbar = GameObject.Find("CraftProgressbar").GetComponent<Slider>();

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

    // TODO remove
    public void Notify() {

    }

    // Destroys all blueprint btns and re-instantiates them, checking for new active ones
    public void UpdateActiveBlueprints() {
    	foreach (GameObject bp in blueprintBtns) {
    		Destroy(bp);
    	}

    	blueprintBtns.Clear();

    	InstantiateBlueprints();
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


    // Updates the height of the blueprints viewport to accomodate more buttons
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




	// TODO also be able to deselect and leave the crafting area empty
	public void SelectBlueprint(int blueprintID) {
        // Set the blueprint as "selected"
        this.selectedBlueprintID = blueprintID;

		// Render the artifact silhouette in the ArtifactViewer

	}


	// Tries to start crafting an Artifact
	public void Craft() {
		// Check if there is a blueprint selected
		if (selectedBlueprintID < 0) {
			print("No blueprint selected");
			return;
		}

		// Check if there are enough resources

		// Check if there is enough space (unimplemented)
		// ---

		// Start the crafting timer coroutine
		isCrafting = true;
		float craftingDuration = 2f;
		progressbar.value = 0;
		craftingCoroutine = StartCoroutine(Crafting(progressbar, craftingDuration, FinishCrafting));

		// Compute the "refounded resources" if stopped and store them
		
		// Spend the resources

		// Change the Craft btn to the "Stop Crafting" btn
		buttonHandler.SawpCraftWithStop();
	}


	public void StopCraft() {
		isCrafting = false;

		// Stop the timer
    	StopCoroutine(craftingCoroutine);
    	progressbar.value = 0;

    	// Change the Craft btn to the "Craft" btn
		buttonHandler.SawpStopWithCraft();

		// Return the "refounded resources"
		RefoundResources();
	}


	// Upon compleation, show the new Artifact and add it to the armory
	private void FinishCrafting() {
		print("Finished Crafting...");
		isCrafting = false;

		// Stop the timer
    	StopCoroutine(craftingCoroutine);
    	progressbar.value = 0;

    	// Change the Craft btn to the "Craft" btn
		buttonHandler.SawpStopWithCraft();


		// Show confirmation window (if this is the first artifact of its kind)

		// Add the Artifact to the Armory

	}

	// TODO
	private void RefoundResources() {
		print("refounding...");
	}





	private IEnumerator Crafting(Slider progressbar, float time, Action callback) {
    	float increment = 0.01f;

    	for (float i = 0f; i < time; i += increment) {
    		progressbar.value = i/time;
    		yield return new WaitForSeconds(increment);

    		// Callback when done
    		if (i >= time - increment) {
    			progressbar.value = 0;
    			if (callback != null) callback();
    		}
    			
    	}
    }




}
