using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabHandler : MonoBehaviour {
    
	private Tab currentTab; 
	
	public GameObject crafterTab;
	public GameObject infuserTab;
	public GameObject armoryTab;
	public GameObject upgradesTab;

	public GameObject crafterTabBtn;
	public GameObject infuserTabBtn;
	public GameObject armoryTabBtn;
	public GameObject upgradesTabBtn;

	private Button crafterBtn;
	private Button infuserBtn;
	private Button armoryBtn;
	private Button upgradesBtn;

    void Awake() {
        crafterBtn = crafterTabBtn.GetComponent<Button>();
        infuserBtn = infuserTabBtn.GetComponent<Button>();
        armoryBtn = armoryTabBtn.GetComponent<Button>();
        upgradesBtn = upgradesTabBtn.GetComponent<Button>();        
    }

	void Start() {		
		SetAllTabsInteractable();
    	CloseAllTabs();

    	currentTab = Tab.CRAFTER; // TODO change this to save-state's last Tab
    	crafterBtn.interactable = false; // TODO make this change depending on the default loaded tab
    	crafterTab.SetActive(true);
	}


    public void SwitchTabToCrafter() {
    	SwitchTab(Tab.CRAFTER);
    }

    public void SwitchTabToInfuser() {
    	SwitchTab(Tab.INFUSER);
    }

    public void SwitchTabToArmory() {
    	SwitchTab(Tab.ARMORY);
    }

    public void SwitchTabToUpgrades() { // TODO update this if "upgrades" changes
    	SwitchTab(Tab.UPGRADES);
    }


    private void SwitchTab(Tab tab) {
    	if (currentTab == tab) {
    		return;
    	}

    	currentTab = tab;
    	SetAllTabsInteractable();
    	CloseAllTabs();

    	switch (tab) {
    		case Tab.CRAFTER:
    			crafterBtn.interactable = false;
    			crafterTab.SetActive(true);
    			break;

    		case Tab.INFUSER:
    			infuserBtn.interactable = false;
    			infuserTab.SetActive(true);
    			break;

    		case Tab.ARMORY:
    			armoryBtn.interactable = false;
    			armoryTab.SetActive(true);
                armoryTab.GetComponent<ArmoryHandler>().OnFocus();
    			break;

    		case Tab.UPGRADES:
    			upgradesBtn.interactable = false;
    			upgradesTab.SetActive(true);
    			break;

    		default:
    			break;    			
    	}    		
    }

    private void CloseAllTabs() {
    	crafterTab.SetActive(false);
    	infuserTab.SetActive(false);
    	armoryTab.SetActive(false);
    	upgradesTab.SetActive(false);
    }

    private void SetAllTabsInteractable() {
	    crafterBtn.interactable = true;
		infuserBtn.interactable = true;
		armoryBtn.interactable = true;
		upgradesBtn.interactable = true;
    }
}
