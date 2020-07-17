using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    
    public GameObject GameManager;
    private GameController gameController;


    void Start() {
    	gameController = GameManager.GetComponent<GameController>();
    }

    // Starts the Resource Gathering expedition
    public void OnGatherClick() {
    	gameController.TryGathering();
    }

    public void OnSelectBlueprintClick(int blueprintID) {
        print(blueprintID);
        
    }

	

}
