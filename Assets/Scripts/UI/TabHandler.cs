using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabHandler : MonoBehaviour {
    
	private Tab currentTab; 
	
    [SerializeField]
	private GameObject m_crafterTab, 
                      m_infuserTab, 
                      m_armoryTab, 
                      m_upgradesTab;

	[SerializeField]
    private Button m_crafterBtn,
                   m_infuserBtn,
                   m_armoryBtn,
                   m_upgradesBtn;


	void Start()
    {		
		SetAllTabsInteractable();
    	CloseAllTabs();

    	currentTab = Tab.CRAFTER; // TODO change this to save-state's last Tab
    	m_crafterBtn.interactable = false; // TODO make this change depending on the default loaded tab
    	m_crafterTab.SetActive(true);
	}


    public void SwitchTabToCrafter() => SwitchTab(Tab.CRAFTER);
    public void SwitchTabToInfuser() => SwitchTab(Tab.INFUSER);
    public void SwitchTabToArmory() => SwitchTab(Tab.ARMORY);
    // TODO update this if "upgrades" changes, to signal new available upgrades?
    public void SwitchTabToUpgrades() => SwitchTab(Tab.UPGRADES);
  

    private void SwitchTab(Tab tab)
    {
    	if (currentTab == tab) {
    		return;
    	}

    	currentTab = tab;
    	SetAllTabsInteractable();
    	CloseAllTabs();

    	switch (tab) {
    		case Tab.CRAFTER:
    			m_crafterBtn.interactable = false;
    			m_crafterTab.SetActive(true);
    			break;

    		case Tab.INFUSER:
    			m_infuserBtn.interactable = false;
    			m_infuserTab.SetActive(true);
                m_infuserTab.GetComponent<Infuser>().OnFocus();
                break;

    		case Tab.ARMORY:
    			m_armoryBtn.interactable = false;
    			m_armoryTab.SetActive(true);
                m_armoryTab.GetComponent<ArmoryHandler>().OnFocus();
    			break;

    		case Tab.UPGRADES:
    			m_upgradesBtn.interactable = false;
    			m_upgradesTab.SetActive(true);
    			break;

    		default:
    			break;    			
    	}    		
    }

    private void CloseAllTabs()
    {
    	m_crafterTab.SetActive(false);
    	m_infuserTab.SetActive(false);
    	m_armoryTab.SetActive(false);
    	m_upgradesTab.SetActive(false);
    }

    private void SetAllTabsInteractable()
    {
	    m_crafterBtn.interactable = true;
		m_infuserBtn.interactable = true;
		m_armoryBtn.interactable = true;
		m_upgradesBtn.interactable = true;
    }
}
