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
    [SerializeField] private TabButton m_gatheringBtn;
    [SerializeField] private TabButton m_crafterBtn;
    [SerializeField] private TabButton m_infuserBtn;
    [SerializeField] private TabButton m_armoryBtn;
    [SerializeField] private TabButton m_upgradesBtn;

    public bool isInfusionLocked = true;
    public bool isUpgradesLocked = true;

	void Start()
    {		
		SetAllTabsInteractable();
    	CloseAllTabs();

    	currentTab = Tab.GATHERING;
        m_gatheringBtn.Select(true);
        m_gatheringTab.SetActive(true);

        UpdateTabInteractability();
    }

    public void UpdateLocks(SaveData saveData)
    {
        if (saveData != null)
        {
            isInfusionLocked = saveData.isInfusionUnlocked;
            isUpgradesLocked = saveData.isUpgradesUnlocked;
        }

        UpdateTabInteractability();
    }

    public void UpdateTabInteractability()
    {
        m_infuserBtn.UpdateLock(isInfusionLocked);
        m_upgradesBtn.UpdateLock(isUpgradesLocked);
    }


    public void SwitchTabToGathering() => SwitchTab(Tab.GATHERING);
    public void SwitchTabToCrafter() => SwitchTab(Tab.CRAFTER);
    public void SwitchTabToInfuser() => SwitchTab(Tab.INFUSER);
    public void SwitchTabToArmory() => SwitchTab(Tab.ARMORY);
    // TODO update this if "upgrades" changes, to signal new available upgrades? like with a "!" next to it
    public void SwitchTabToUpgrades() => SwitchTab(Tab.UPGRADES);
  

    private void SwitchTab(Tab tab)
    {
    	if (currentTab == tab) return;

    	currentTab = tab;
    	SetAllTabsInteractable();
    	CloseAllTabs();

    	switch (tab) {
            case Tab.GATHERING:
                m_gatheringBtn.Select(true);
                m_gatheringTab.SetActive(true);
                break;

            case Tab.CRAFTER:
                m_crafterBtn.Select(true);
                m_crafterTab.SetActive(true);
    			break;

    		case Tab.INFUSER:
    			m_infuserBtn.Select(true);
                m_infuserTab.SetActive(true);
                m_infuserTab.GetComponent<Infuser>().OnFocus();
                break;

    		case Tab.ARMORY:
    			m_armoryBtn.Select(true);
                m_armoryTab.SetActive(true);
                m_armoryTab.GetComponent<ArmoryHandler>().OnFocus();
    			break;

    		case Tab.UPGRADES:
    			m_upgradesBtn.Select(true);
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
        m_gatheringBtn.Select(false);
        m_crafterBtn.Select(false);
        m_infuserBtn.Select(false);
        m_armoryBtn.Select(false);
        m_upgradesBtn.Select(false);
    }




    #region Unlocks

    public void UnlockUpgrades()
    {
        isUpgradesLocked = false;
        UpdateTabInteractability();
    }

    public void UnlockInfusion()
    {
        isInfusionLocked = false;
        UpdateTabInteractability();
    }


    #endregion

}
