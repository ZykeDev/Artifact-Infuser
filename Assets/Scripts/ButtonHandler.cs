using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    // Instantiated buttons
    public GameObject CraftBtn;
    public GameObject StopCraftBtn;


    public GameObject GameManager;
    private GameController gameController;
    private Crafter crafter;



    void Start() {
    	gameController = GameManager.GetComponent<GameController>();
        crafter = GameObject.Find("Crafter").GetComponent<Crafter>();
    }

    // Starts the Resource Gathering expedition
    public void OnGatherClick() {
    	gameController.TryGathering();
    } 

    // Starts crafting the selected blueprint into an item
    public void OnCraftClick() {
        crafter.Craft();
    }

    public void OnStopCraftClick() {
        crafter.StopCraft();
    }

    
    public void OnSelectBlueprintClick(int blueprintID) {
        crafter.SelectBlueprint(blueprintID);    
        
    }


    // Swap Craft -> Stop Crafting
    public void SawpCraftWithStop() {
        CraftBtn.SetActive(false);
        StopCraftBtn.SetActive(true);
    } 
    
    // Swap Stop Crafting -> Craft
    public void SawpStopWithCraft() {
        StopCraftBtn.SetActive(false);
        CraftBtn.SetActive(true);
    }

	

}
