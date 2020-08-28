using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmoryTabHandler : MonoBehaviour {
	
	private ArmoryTab currentTab;

	public GameObject contentWeapons;
	public GameObject contentArmor;
	public GameObject contentAccessories;
	public GameObject contentAbyss;

	public GameObject weaponsTabBtn;
	public GameObject armorTabBtn;
	public GameObject accessoriesTabBtn;
	public GameObject abyssTabBtn;

	private Button weaponBtn;
	private Button armorBtn;
	private Button accessoriesBtn;
	private Button abyssBtn;

	[SerializeField]
	private ArmoryTab defaultTab;



	void Awake() {
		weaponBtn = weaponsTabBtn.GetComponent<Button>();
		armorBtn = armorTabBtn.GetComponent<Button>();
		accessoriesBtn = accessoriesTabBtn.GetComponent<Button>();
		abyssBtn = abyssTabBtn.GetComponent<Button>();
	}

    // Start is called before the first frame update
    void Start() {
        SetAllTabBtnsInteractable();
    	CloseAllTabs();
		
		currentTab = ArmoryTab.WEAPONS;
		currentTab = defaultTab; // TODO change this to save-state's last Tab

    	weaponBtn.interactable = false; // TODO make this change depending on the default loaded tab    	
    	contentWeapons.SetActive(true);

    	abyssTabBtn.SetActive(false); // TODO make this tied to the unlockSystem	

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SwitchTabToWeapons() {
    	SwitchTab(ArmoryTab.WEAPONS);
    }

    public void SwitchTabToArmor() {
    	SwitchTab(ArmoryTab.ARMOR);
    }

    public void SwitchTabToAccessories() {
    	SwitchTab(ArmoryTab.ACCESSORIES);
    }

    public void SwitchTabToAbyss() { // TODO update this if "upgrades" changes
    	SwitchTab(ArmoryTab.ABYSS);
    }


    private void SwitchTab(ArmoryTab tab) {
    	if (currentTab == tab) {
    		return;
    	}

    	currentTab = tab;
    	SetAllTabBtnsInteractable();
    	CloseAllTabs();

    	switch (tab) {
    		case ArmoryTab.WEAPONS:
    			weaponBtn.interactable = false;
    			contentWeapons.SetActive(true);
    			break;

    		case ArmoryTab.ARMOR:
    			armorBtn.interactable = false;
    			contentArmor.SetActive(true);
    			break;

    		case ArmoryTab.ACCESSORIES:
    			accessoriesBtn.interactable = false;
    			contentAccessories.SetActive(true);
    			break;

    		case ArmoryTab.ABYSS:
    			abyssBtn.interactable = false;
    			contentAbyss.SetActive(true);
    			break;

    		default:
    			break;    			
    	}    		
    }

    private void CloseAllTabs() {
    	contentWeapons.SetActive(false);
    	contentArmor.SetActive(false);
    	contentAccessories.SetActive(false);
    	contentAbyss.SetActive(false);
    }

    private void SetAllTabBtnsInteractable() {
	    weaponBtn.interactable = true;
		armorBtn.interactable = true;
		accessoriesBtn.interactable = true;
		abyssBtn.interactable = true;
    }





}
