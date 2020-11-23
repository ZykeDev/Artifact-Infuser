using UnityEngine;
using UnityEngine.UI;

public class TabHandler : MonoBehaviour {
    
	private Tab currentTab;

    [Header("Tabs")]
    [SerializeField] protected GameObject m_gatheringTab;
    [SerializeField] protected GameObject m_crafterTab;
    [SerializeField] protected GameObject m_infuserTab;
    [SerializeField] protected GameObject m_armoryTab;
    [SerializeField] protected GameObject m_upgradesTab;


    [Header("Buttons")]
    [SerializeField] private Button m_gatheringBtn;
    [SerializeField] private Button m_crafterBtn;
    [SerializeField] private Button m_infuserBtn;
    [SerializeField] private Button m_armoryBtn;
    [SerializeField] private Button m_upgradesBtn;


	void Start()
    {		
		SetAllTabsInteractable();
    	CloseAllTabs();

    	currentTab = Tab.GATHERING; // TODO change this to save-state's last Tab
        m_gatheringBtn.interactable = false;
        m_gatheringTab.SetActive(true);
    }


    public void SwitchTabToGathering() => SwitchTab(Tab.GATHERING);
    public void SwitchTabToCrafter() => SwitchTab(Tab.CRAFTER);
    public void SwitchTabToInfuser() => SwitchTab(Tab.INFUSER);
    public void SwitchTabToArmory() => SwitchTab(Tab.ARMORY);
    // TODO update this if "upgrades" changes, to signal new available upgrades?
    public void SwitchTabToUpgrades() => SwitchTab(Tab.UPGRADES);
  

    private void SwitchTab(Tab tab)
    {
    	if (currentTab == tab) return;

    	currentTab = tab;
    	SetAllTabsInteractable();
    	CloseAllTabs();

    	switch (tab) {
            case Tab.GATHERING:
                m_gatheringBtn.interactable = false;
                m_gatheringTab.SetActive(true);
                break;

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
        m_gatheringTab.SetActive(false);
        m_crafterTab.SetActive(false);
    	m_infuserTab.SetActive(false);
    	m_armoryTab.SetActive(false);
    	m_upgradesTab.SetActive(false);
    }

    private void SetAllTabsInteractable()
    {
        m_gatheringBtn.interactable = true;
        m_crafterBtn.interactable = true;
		m_infuserBtn.interactable = true;
		m_armoryBtn.interactable = true;
		m_upgradesBtn.interactable = true;
    }
}
