using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmoryTabHandler : MonoBehaviour {
	
	private ArmoryTab currentTab;

	public GameObject artifactGrid;
	private ScrollRect artifactGridScrollRect;

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
		artifactGridScrollRect = artifactGrid.GetComponent<ScrollRect>(); 

		weaponBtn = weaponsTabBtn.GetComponent<Button>();
		armorBtn = armorTabBtn.GetComponent<Button>();
		accessoriesBtn = accessoriesTabBtn.GetComponent<Button>();
		abyssBtn = abyssTabBtn.GetComponent<Button>();
	}

    // Start is called before the first frame update
    void Start() {
        SetAllTabBtnsInteractable();
    	CloseAllTabs();
		
		// Set defaults
		currentTab = ArmoryTab.WEAPONS;
		currentTab = defaultTab; // TODO change this to save-state's last Tab

    	weaponBtn.interactable = false; // TODO make this change depending on the default loaded tab    	
    	contentWeapons.SetActive(true);

    	artifactGridScrollRect.content = contentWeapons.GetComponent<RectTransform>();

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

    // Switches the active Tab by
    // - Setting the toggle button's interatable to false
    // - Setting the content to true
    // - Changing the scrollview content
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
    			artifactGridScrollRect.content = contentWeapons.GetComponent<RectTransform>();
    			break;

    		case ArmoryTab.ARMOR:
    			armorBtn.interactable = false;
    			contentArmor.SetActive(true);
    			artifactGridScrollRect.content = contentArmor.GetComponent<RectTransform>();
    			break;

    		case ArmoryTab.ACCESSORIES:
    			accessoriesBtn.interactable = false;
    			contentAccessories.SetActive(true);
    			artifactGridScrollRect.content = contentAccessories.GetComponent<RectTransform>();
    			break;

    		case ArmoryTab.ABYSS:
    			abyssBtn.interactable = false;
    			contentAbyss.SetActive(true);
    			artifactGridScrollRect.content = contentAbyss.GetComponent<RectTransform>();
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
