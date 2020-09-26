using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour {

	public GameObject gameManager;
	private GameController gameController;

	private Coroutine craftingCoroutine = null;
	private Coroutine infusionCoroutine = null;
	private int selectedBlueprintID = -1;
	private int selectedCypherID = -1;


	void Awake() {
		gameController = gameManager.GetComponent<GameController>();
	}


    public void Craft(int blueprintID, float time) {
    	this.selectedBlueprintID = blueprintID;
    	craftingCoroutine = StartCoroutine(Crafting(time, FinishCrafting));
    }

    public void StopCraft() {
    	StopCoroutine(craftingCoroutine);
    }

    public void FinishCrafting() {
    	StopCoroutine(craftingCoroutine);

    	// Check for invalid IDs
		if (this.selectedBlueprintID < 0) {
            print("ERROR: Invalid artifact ID");
            return;
        }

        gameController.FinishCrafting(this.selectedBlueprintID);
    }


    // Crafting coroutine
    private IEnumerator Crafting(float time, Action callback) {
    	float increment = 0.01f;

    	for (float i = 0f; i < time; i += increment) {
    		gameController.UpdateCraftingProgress(i/time);

    		yield return new WaitForSeconds(increment);

    		// Callback when done
    		if (i >= time - increment) {
    			gameController.UpdateCraftingProgress(0);
    			if (callback != null) callback();
    		}
    			
    	}
    }


    // ------------- //
   
    public void Infuse(int cypherID, float time) {
    	this.selectedCypherID = cypherID;
    	infusionCoroutine = StartCoroutine(Infusing(time, FinishInfusing));
    }

    public void StopInfusion() {
    	StopCoroutine(infusionCoroutine);
    }

    public void FinishInfusing() {
    	StopCoroutine(infusionCoroutine);

    	// Check for invalid IDs
		if (this.selectedCypherID < 0) {
            print("ERROR: Invalid cypher ID");
            return;
        }

        gameController.FinishCrafting(this.selectedCypherID);
    }


    // Crafting coroutine
    private IEnumerator Infusing(float time, Action callback) {
    	float increment = 0.01f;

    	for (float i = 0f; i < time; i += increment) {
    		gameController.UpdateInfusionProgress(i/time);

    		yield return new WaitForSeconds(increment);

    		// Callback when done
    		if (i >= time - increment) {
    			gameController.UpdateInfusionProgress(0);
    			if (callback != null) callback();
    		}
    			
    	}
    }



}
