using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour {
    // Instantiated buttons
    public GameObject CraftBtn;
    public GameObject StopCraftBtn;

    public GameObject InfuseBtn;
    public GameObject StopInfuseBtn;

    // References
    public GameObject GameManager;
    public GameObject Crafter;
    public GameObject Infuser;
    public GameObject Armory;
    public GameObject Upgrades;

    private GameController gameController;
    private Crafter crafterComp; // Calling it crafterComp to not confuse it with the Crafter GO
    private Infuser infuserComp;
    


    void Start() {
    	gameController = GameManager.GetComponent<GameController>();
        crafterComp = Crafter.GetComponent<Crafter>();
        infuserComp = Infuser.GetComponent<Infuser>();
    }

    // Starts the Resource Gathering expedition
    public void OnGatherClick() {
    	gameController.TryGathering();
    } 


    // ---------------------- //

    // Starts crafting the selected blueprint into an item
    public void OnCraftClick() {
        crafterComp.Craft();
    }

    public void OnStopCraftClick() {
        crafterComp.StopCraft();
    }

    
    public void OnSelectBlueprintClick(GameObject caller, int blueprintID) {
        // Forward the caller to remember which button was clicked
        crafterComp.SelectBlueprint(blueprintID, caller);
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

    // Makes the Craft Button clickable
    public void ActivateCraftBtn() {
        Button craftButton = CraftBtn.GetComponent<Button>();
        if (!craftButton.interactable) craftButton.interactable = true;
    }


    // ------------------------ //


    // Starts crafting the selected blueprint into an item
    public void OnInfuseClick() {
        infuserComp.Infuse();
    }

    public void OnStopInfusionClick() {
        infuserComp.StopInfusion();
    }

    public void OnSelectCypherClick(int cypherID) {
        infuserComp.SelectCypher(cypherID);    
        
    }

    // Swap Craft -> Stop Crafting
    public void SawpInfuseWithStop() {
        InfuseBtn.SetActive(false);
        StopInfuseBtn.SetActive(true);
    } 
    
    // Swap Stop Crafting -> Craft
    public void SawpStopWithInfuse() {
        StopInfuseBtn.SetActive(false);
        InfuseBtn.SetActive(true);
    }

    // Makes the Craft Button clickable
    public void ActivateInfuseBtn() {
        InfuseBtn.GetComponent<Button>().interactable = true;
    }

	

}