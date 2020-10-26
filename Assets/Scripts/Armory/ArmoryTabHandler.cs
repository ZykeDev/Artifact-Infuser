using UnityEngine;
using UnityEngine.UI;

public class ArmoryTabHandler : MonoBehaviour {
	
    [SerializeField]
	private ArmoryTab m_defaultTab;
	private ArmoryTab m_currentTab;

    [SerializeField]
	private GameObject m_artifactGrid;

	private ScrollRect m_artifactGridScrollRect;
    private GameObject m_contentWeapons,
        m_contentArmor, 
        m_contentAccessories, 
        m_contentAbyss;

    [SerializeField]
	private GameObject m_weaponsTabBtn,
        m_armorTabBtn,
        m_accessoriesTabBtn,
        m_abyssTabBtn;

	private Button m_weaponBtn, 
        m_armorBtn, 
        m_accessoriesBtn,
        m_abyssBtn;


	void Awake()
    {
        // Grab the Content-GameObjects from the ArmoryHandler component
        ArmoryHandler m_armoryHandler = GetComponent<ArmoryHandler>();

        m_contentWeapons = m_armoryHandler.GetContent(ArtifactType.WEAPON);
        m_contentArmor = m_armoryHandler.GetContent(ArtifactType.ARMOR);
        m_contentAccessories = m_armoryHandler.GetContent(ArtifactType.ACCESSORY);
        m_contentAbyss = m_armoryHandler.GetContent(ArtifactType.ABYSS);


        m_artifactGridScrollRect = m_artifactGrid.GetComponent<ScrollRect>(); 

		m_weaponBtn = m_weaponsTabBtn.GetComponent<Button>();
		m_armorBtn = m_armorTabBtn.GetComponent<Button>();
		m_accessoriesBtn = m_accessoriesTabBtn.GetComponent<Button>();
		m_abyssBtn = m_abyssTabBtn.GetComponent<Button>();
	}

    void Start()
    {
        SetAllTabBtnsInteractable();
    	CloseAllTabs();
		
		// Set defaults
		m_currentTab = m_defaultTab; // TODO change this to save-state's last Tab

    	m_weaponBtn.interactable = false; // TODO make this change depending on the default loaded tab    	
    	m_contentWeapons.SetActive(true);

    	m_artifactGridScrollRect.content = m_contentWeapons.GetComponent<RectTransform>();

    	m_abyssTabBtn.SetActive(false); // TODO make this tied to the unlockSystem	
    }


    public void SwitchTabToWeapons()
    { 
        SwitchTab(ArmoryTab.WEAPONS);
    }

    public void SwitchTabToArmor()
    {
    	SwitchTab(ArmoryTab.ARMOR);
    }

    public void SwitchTabToAccessories()
    {
    	SwitchTab(ArmoryTab.ACCESSORIES);
    }

    public void SwitchTabToAbyss()
    { 
    	SwitchTab(ArmoryTab.ABYSS);
    }

    /// <summary>
    /// Switches the active Tab by changing the scrollview content.
    /// </summary>
    /// <param name="tab"></param>
    private void SwitchTab(ArmoryTab tab)
    {
    	if (m_currentTab == tab) {
    		return;
    	}

        m_currentTab = tab;

    	SetAllTabBtnsInteractable();
    	CloseAllTabs();

    	switch (tab) {
    		case ArmoryTab.WEAPONS:
    			m_weaponBtn.interactable = false;
    			m_contentWeapons.SetActive(true);
    			m_artifactGridScrollRect.content = m_contentWeapons.GetComponent<RectTransform>();
    			break;

    		case ArmoryTab.ARMOR:
    			m_armorBtn.interactable = false;
                m_contentArmor.SetActive(true);
    			m_artifactGridScrollRect.content = m_contentArmor.GetComponent<RectTransform>();
    			break;

    		case ArmoryTab.ACCESSORIES:
    			m_accessoriesBtn.interactable = false;
                m_contentAccessories.SetActive(true);
    			m_artifactGridScrollRect.content = m_contentAccessories.GetComponent<RectTransform>();
    			break;

    		case ArmoryTab.ABYSS:
    			m_abyssBtn.interactable = false;
                m_contentAbyss.SetActive(true);
    			m_artifactGridScrollRect.content = m_contentAbyss.GetComponent<RectTransform>();
    			break;

    		default:
    			break;    			
    	}    		
    }

    private void CloseAllTabs() {
    	m_contentWeapons.SetActive(false);
        m_contentArmor.SetActive(false);
        m_contentAccessories.SetActive(false);
        m_contentAbyss.SetActive(false);
    }

    private void SetAllTabBtnsInteractable() {
	    m_weaponBtn.interactable = true;
		m_armorBtn.interactable = true;
		m_accessoriesBtn.interactable = true;
		m_abyssBtn.interactable = true;
    }





}
