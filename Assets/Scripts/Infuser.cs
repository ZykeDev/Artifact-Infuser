using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Infuser : MonoBehaviour {

	public GameObject GameManager;
	public GameObject MainCanvas;

    private GameController gameController;
    private UnlockSystem unlockSystem;
    private ButtonHandler buttonHandler;

    private Slider progressbar;

    public GameObject cypherSelectorContent;
    public GameObject cypherBtnPref;


    private List<GameObject> cypherBtns;
    private List<Cypher> cyphers;
    public bool[] activeCyphers;

    private float cypherBtnWidth = 0;
    private float cypherBtnHeight = 0;

    // Currently selected cypher
    private int selectedCypherID = -1;


	// Start is called before the first frame update
	void Start() {
		gameController = GameManager.GetComponent<GameController>();
		unlockSystem = GameManager.GetComponent<UnlockSystem>();
		buttonHandler = MainCanvas.GetComponent<ButtonHandler>();

		// Reference the infusing progressbar
		progressbar = GameObject.Find("InfusionProgressbar").GetComponent<Slider>();

		cypherBtns = new List<GameObject>();

		cyphers = unlockSystem.cyphers;
		activeCyphers = unlockSystem.activeCyphers;

        // Instantiate the active cypher btns
		InstantiateCyphers();
       
        // Udapte the viewer h = #active * btn.h
		UpdateViewportHeight();

    }


    // Destroys all cypher btns and re-instantiates them, checking for new active ones
    public void UpdateActiveCyphers() {
    	foreach (GameObject c in cypherBtns) {
    		Destroy(c);
    	}

    	cypherBtns.Clear();

    	InstantiateCyphers();
    }


    // Instantiates all active cyphers
    private void InstantiateCyphers() {

    	float gap = 10f;
    	
    	foreach (Cypher c in this.cyphers) {
    		// Don't show cyphers that are not active
    		if (activeCyphers[c.ID] == false) {
    			continue;
    		}

    		// For some reason Instantiate doesn't accept a 5th param "false" for isWorldMapSpace, so localPos is set later
	    	GameObject newCypher = Instantiate(cypherBtnPref, new Vector2(0, 0), Quaternion.identity, cypherSelectorContent.transform) as GameObject;
	    	newCypher.name = "Cbtn_" + c.ID;

	    	// Not being used for now. Shouse it be added to the "newCypher" gameobj?
	    	CypherBtnData cdata = newCypher.AddComponent<CypherBtnData>();
	    	cdata.SetID(c.ID);

	    	// Add a listener to the new Button
	    	newCypher.GetComponent<Button>().onClick.AddListener(delegate {buttonHandler.OnSelectCypherClick(c.ID); });
	    	
	    	float width = newCypher.GetComponent<RectTransform>().sizeDelta.x;
	    	float height = newCypher.GetComponent<RectTransform>().sizeDelta.y;

	    	// Store the btn size locally
	    	if (this.cypherBtnHeight == 0) this.cypherBtnHeight = height;
	    	if (this.cypherBtnWidth  == 0) this.cypherBtnWidth = width;
	    	
	    	// Correctly position the new button. Use its ID as a vertical index
	    	newCypher.transform.localPosition = new Vector2(width / 2 + 8f, -height/2 - (height * c.ID) - gap);

	    	// Fix the collider to match the button size 
	    	newCypher.GetComponent<BoxCollider2D>().size = new Vector2(width, height);


	    	// Add the text and the sprite to the button TODO move this to the cypher's script
	    	foreach (Transform child in newCypher.transform) {
	    		if (child.name == "CypherName") {
	    			child.GetComponent<Text>().text = c.name;
	    			continue;
	    		}
	    		if (child.name == "CypherSprite") {
	    			child.GetComponent<Image>().sprite = c.cypherSprite;
	    			continue;
	    		}

	    	}

	    	// Add the finished button to the list of instantiated buttons
	    	cypherBtns.Add(newCypher);
    	}
    }


    // Updates the height of the cyphers viewport to accomodate more buttons
	private void UpdateViewportHeight() {
		RectTransform cSelectorContentTransform = cypherSelectorContent.GetComponent<RectTransform>();
		float cypherSelectorContentWidth = cSelectorContentTransform.sizeDelta.x;
		// 4 is how many btns can fit at a time
		if (GetNumberOfActiveCyphers() < 4) {
			cSelectorContentTransform.sizeDelta = new Vector2(cypherSelectorContentWidth, 560f);

		} else {
			cSelectorContentTransform.sizeDelta = new Vector2(cypherSelectorContentWidth, cypherBtnHeight * (GetNumberOfActiveCyphers() + 1));
		}
	}


	// Returns the number of active cyphers
	private int GetNumberOfActiveCyphers() {
		int n = 0;
		for (int i = 0; i < activeCyphers.Length; i++) {
			if (activeCyphers[i]) n++;
		}

		return n;
	}




	// TODO also be able to deselect and leave the crafting area empty
	public void SelectCypher(int cypherID) {
        // Set the cypher as "selected"
        this.selectedCypherID = cypherID;

        // Activate the Craft button
        buttonHandler.ActivateInfuseBtn();

		// Render the artifact's on the rune table
        // TODO

	}


	// Tries to start infusing an Artifact
	public void Infuse() {
		// Check if there is a cypher selected
		if (selectedCypherID < 0) {
			print("No cypher selected");
			return;
		}

		// Check if there are enough resources

		// Check if there is enough space (unimplemented)

		// Change the Infuse btn to the "Stop Infusing" btn
		buttonHandler.SawpInfuseWithStop();

		// Start the infusing timer coroutine
		float infusionTime = GetCypherWithID(this.selectedCypherID).infusionTime;
		progressbar.value = 0;

		gameController.Infuse(this.selectedCypherID, infusionTime);

		// Compute the "refounded resources" if stopped and store them (better to do this b4 starting the infusion?)
		
		// Spend the resources

	}

	public void UpdateInfusionProgress(float progress) {
		progressbar.value = progress;
	}


	public void StopInfusion() {
		// Stop the timer
    	gameController.StopInfusion();

    	// Reset the progress bar
    	UpdateInfusionProgress(0);

    	// Change the Infuse btn to the "Infuse" btn
		buttonHandler.SawpStopWithInfuse();
	}


	// Upon compleation, show the new Artifact and add it to the armory
	public void FinishInfusing() {
    	UpdateInfusionProgress(0); // Needed?

    	// Change the Infuse btn to the "Infuse" btn
		buttonHandler.SawpStopWithInfuse();

		// Show confirmation window (if this is the first artifact of its kind) TODO
		// -- 
	}

    // Returns the cypher with the corresponding ID from the cyphers list. Else returns Null.
    public Cypher GetCypherWithID(int ID) {
    	foreach(Cypher c in this.cyphers) {
    		if (c.ID == ID) {
    			return c;
    		}
    	}

    	return null;
    }



}
