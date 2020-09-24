using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour {

	public GameObject gameManager;
	private GameController gameController;

	private Coroutine craftingCoroutine = null;
	private int selectedBlueprintID = -1;


	void Awake() {
		gameController = gameManager.GetComponent<GameController>();
	}


    public void Craft(int blueprintID, float duration) {
    	this.selectedBlueprintID = blueprintID;
    	craftingCoroutine = StartCoroutine(Crafting(duration, FinishCrafting));
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


}
