using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafter : MonoBehaviour {

	[SerializeField]
	private GameObject GameManager;
	[SerializeField]
	private GameObject MainCanvas;
	[SerializeField]
	private GameObject ArtifactSilhouette;

    private GameController gameController;
    private UnlockSystem unlockSystem;
    private ButtonHandler buttonHandler;

	private Image artifactSilhouetteImage;
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

	private Inventory refoundedResources = new Inventory();



	// Start is called before the first frame update
	void Start() {
		gameController = GameManager.GetComponent<GameController>();
		unlockSystem = GameManager.GetComponent<UnlockSystem>();
		buttonHandler = MainCanvas.GetComponent<ButtonHandler>();

		// Reference the silhouette image component
		artifactSilhouetteImage = ArtifactSilhouette.GetComponent<Image>();
		artifactSilhouetteImage.enabled = false;

		// Reference the crafting progressbar
		progressbar = GameObject.Find("CraftProgressbar").GetComponent<Slider>();

		blueprintBtns = new List<GameObject>();

		// Boolean list of which blueprints are active (index = ID)
		activeBlueprints = unlockSystem.activeBlueprints;
		blueprints = unlockSystem.blueprints;

		// Instantiate the active blueprint btns
		InstantiateBlueprints();
       
        // Udapte the viewer h = #active * btn.h
		UpdateViewportHeight();

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
	    	newBlueprint.GetComponent<Button>().onClick.AddListener(delegate {buttonHandler.OnSelectBlueprintClick(newBlueprint, bp.ID); });
	    	
	    	float width = newBlueprint.GetComponent<RectTransform>().sizeDelta.x;
	    	float height = newBlueprint.GetComponent<RectTransform>().sizeDelta.y;

	    	// Store the btn size locally
	    	if (this.blueprintBtnHeight == 0) this.blueprintBtnHeight = height;
	    	if (this.blueprintBtnWidth  == 0) this.blueprintBtnWidth = width;
	    	
	    	// Correctly position the new button. Use its ID as a vertical index
	    	newBlueprint.transform.localPosition = new Vector2(width / 2 + 8f, -height/2 - (height * bp.ID) - gap);

	    	// Fix the collider to match the button size 
	    	newBlueprint.GetComponent<BoxCollider2D>().size = new Vector2(width, height);


	    	// Add the text and the sprite to the button
	    	foreach (Transform child in newBlueprint.transform) {
	    		if (child.name == "BlueprintName") {
	    			child.GetComponent<Text>().text = bp.name;
	    			continue;
	    		}
	    		if (child.name == "BlueprintSprite") {
	    			child.GetComponent<Image>().sprite = bp.blueprintSprite;
	    			continue;
	    		}

	    	}

	    	// Add the finished button to the list of instantiated buttons
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
	public void SelectBlueprint(int blueprintID, GameObject caller) {
        // Set the blueprint as "selected"
        this.selectedBlueprintID = blueprintID;

		// Deselect all other buttons & select the new one
		foreach (GameObject blueprintBtn in blueprintBtns)
        {
			blueprintBtn.GetComponent<ButtonGraphic>().Deselect();
		}
		caller.GetComponent<ButtonGraphic>().Select();

		// Activate the Craft button
		buttonHandler.ActivateCraftBtn();

		// Make sure the silhouette's image is enabled
		if (!artifactSilhouetteImage.enabled) artifactSilhouetteImage.enabled = true;
		// Render the artifact's silhouette in the ArtifactViewer
		Sprite artifactSprite = GetBlueprintWithID(this.selectedBlueprintID).artifactSprite;
		artifactSilhouetteImage.sprite = artifactSprite;
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

		// Change the Craft btn to the "Stop Crafting" btn
		buttonHandler.SawpCraftWithStop();

		// Start the crafting timer coroutine
		float craftingTime = GetBlueprintWithID(this.selectedBlueprintID).craftingTime;
		progressbar.value = 0;

		gameController.Craft(this.selectedBlueprintID, craftingTime);

		// Compute the "refounded resources" if stopped and store them (better to do this b4 starting the craft?)
		
		// Actually spend the resources

	}

	public void UpdateCraftingProgress(float progress) {
		progressbar.value = progress;
	}


	public void StopCraft() {
		// Stop the timer
    	gameController.StopCraft();

    	// Reset the progress bar
    	UpdateCraftingProgress(0);

    	// Change the Craft btn to the "Craft" btn
		buttonHandler.SawpStopWithCraft();

		// Return the "refounded resources"
		RefoundResources();
	}


	// Upon compleation, show the new Artifact and add it to the armory
	public void FinishCrafting() {
    	UpdateCraftingProgress(0); // Needed?

    	// Change the Craft btn to the "Craft" btn
		buttonHandler.SawpStopWithCraft();

		// Show confirmation window (if this is the first artifact of its kind) TODO
		// -- 

		// AFTER the confirmation window is closed (if it ever gets opened)
		// artifactSilhouetteImage.sprite = null;

	}

	// TODO
	private void RefoundResources() {
		print("TODO Refounding...");
	}

    // Returns the Blueprint with the corresponding ID from the blueprints list. Else returns Null.
    public Blueprint GetBlueprintWithID(int ID) {
    	foreach(Blueprint bp in this.blueprints) {
    		if (bp.ID == ID) {
    			return bp;
    		}
    	}

    	return null;
    }



}
