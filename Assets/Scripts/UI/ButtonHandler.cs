using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour {
    // Instantiated buttons
    [SerializeField]
    private GameObject CraftBtn, StopCraftBtn, InfuseBtn, StopInfuseBtn;

    // References
    [SerializeField]
    private GameObject GameManager, Crafter, Infuser, Armory, Upgrades;

    [SerializeField]
    private GameObject tooltipPrefab;

    private GameController gameController;
    private Crafter crafterComp; // Calling it crafterComp to not confuse it with the Crafter GO
    private Infuser infuserComp;

    private GameObject CurrentlyOpenedTooltip;
    private float screenHeight;

    private void Awake()
    {
        screenHeight = Screen.height;
    }
    void Start() {
    	gameController = GameManager.GetComponent<GameController>();
        crafterComp = Crafter.GetComponent<Crafter>();
        infuserComp = Infuser.GetComponent<Infuser>();
    }


    // --------- Blueprint Buttons --------- //

    public void ShowTooltip(float width, float height, TooltipData tooltipData)
    {
        // TODO this could be optimized by pooling all tooltips on Start (or on first instance)
        // instad of instantiating one every time
        CurrentlyOpenedTooltip = Instantiate(
            tooltipPrefab,
            Vector2.zero, 
            Quaternion.identity,
            this.transform) as GameObject;

        Tooltip tooltip = CurrentlyOpenedTooltip.GetComponent<Tooltip>();
        RectTransform tooltipRT = CurrentlyOpenedTooltip.GetComponent<RectTransform>();

        CurrentlyOpenedTooltip.name = "Tooltip";

        // Set the correct position
        tooltipRT.anchoredPosition = new Vector2(Input.mousePosition.x + 1f, Input.mousePosition.y - screenHeight + 1f);
        
        // Sets the correct size for the tooltip
        tooltipRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        tooltipRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        
        tooltip.SetTexts(tooltipData);
        tooltip.FollowCursor();
    }

    public void HideTooltip()
    {
        // TODO does StopFollowingCursor need to be called?
        Destroy(CurrentlyOpenedTooltip);
        CurrentlyOpenedTooltip = null;
    }


    // ----------- Gather Button ----------- //

    // Starts the Resource Gathering expedition
    public void OnGatherClick() {
    	gameController.TryGathering();
    }


    // ----------- Craft Button ----------- //

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


    // ----------- Infuse Button ----------- //

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