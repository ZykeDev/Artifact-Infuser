using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    [SerializeField]
    protected BackgroundManager m_backgroundManager;

    public GameObject m_crafter, m_infuser, m_armory, m_upgrades;

    private Crafter m_crafterComp;
    private Infuser m_infuserComp;
    private ArmoryHandler m_armoryHandler; //Called armoryHandler to not confuse it with the armory struct
    //private UpgradesHandler m_upgradesHandler;

    public Inventory inventory;
    public Armory armory;

	private int m_tier;

	[SerializeField]
    [Range(1f, 60f)] private float m_gatheringDuration = 2f;
	private bool m_isGatherning = false; // TODO make this into a state?
    [SerializeField] private Text m_gatherBtnText;
	private Coroutine m_gatheringCoroutine = null;


	void Awake() {
        m_crafterComp = m_crafter.GetComponent<Crafter>();
        m_infuserComp = m_infuser.GetComponent<Infuser>();
        m_armoryHandler = m_armory.GetComponent<ArmoryHandler>();
        //m_upgradesHandler = m_upgrades.GetComponent<Upgrades>();


        //m_gatherBtnText = GameObject.Find("GatherBtnText").GetComponent<Text>();
        m_tier = 0;

        // Init Inventory
        inventory = new Inventory();
        armory = new Armory();
	}

    #region Crafting

    public void Craft(int blueprintID, float time) => m_backgroundManager.Craft(blueprintID, time);
    public void StopCraft() => m_backgroundManager.StopCraft();

    public void UpdateCraftingProgress(float progress)
    {
        // Update only if the crafter is active
        if (m_crafter.activeSelf) m_crafterComp.UpdateCraftingProgress(progress); 
    }
    
    public void FinishCrafting(int selectedBlueprintID)
    {
        // Creat the artifact from the BP data
        Blueprint bp = m_crafterComp.GetBlueprintWithID(selectedBlueprintID);
        Artifact newArtifact = new Artifact(bp);

        // Add the Artifact to the Armory
        AddNewArtifact(newArtifact);

        m_crafterComp.FinishCrafting();
    }

#endregion

    #region Infusion   

    public void Infuse(int cypherID, float time) => Infuse(cypherID, null, time);
    public void Infuse(int cypherID, Artifact baseArtifact, float time) => m_backgroundManager.Infuse(cypherID, baseArtifact, time);
    public void StopInfusion() => m_backgroundManager.StopInfusion();
    
    public void UpdateInfusionProgress(float progress)
    {
        // Update only if the infuser is active
        if (m_infuser.activeSelf) m_infuserComp.UpdateInfusionProgress(progress); 
    }

    public void FinishInfusing(int selectedCypherID, Artifact baseArtifact)
    {
        // Creat the artifact from the BP data
        Cypher cypher = m_infuserComp.GetCypherWithID(selectedCypherID);
        
        Artifact newArtifact = new Artifact(baseArtifact, cypher);

        // Add the Artifact to the Armory
        AddNewArtifact(newArtifact);

        m_infuserComp.FinishInfusing();
    }

    #endregion

    #region Gathering

    /// <summary>
    /// Stops gathering resources and adds them to the inventory
    /// </summary>
    private void FinishGathering()
    {
    	StopGatherResources();
    	Inventory booty = new Inventory();
    	booty.SetRandomResources(m_tier);

    	inventory.CombineWith(booty);
    }


    // TODO change this into 2 different buttons
    public void GatherResources(Button caller) 
    {
        caller.interactable = false;
        Slider progressbar = GameObject.Find("GatherProgressbar").GetComponent<Slider>();

    	// Start a timer coroutine
    	// Show the progress bar
    	m_isGatherning = true;
    	m_gatherBtnText.text = "Gathering...";
    	m_gatheringCoroutine = StartCoroutine(Gathering(progressbar,  m_gatheringDuration, 
            delegate 
            { 
                caller.interactable = true; 
                FinishGathering(); 
            }
        ));

    	// Show remaining time

   
    	// When compleated, create a list of new resources & show a prompt with the gained resources
    	// On click, the resources are added to the inventory
    }

    public void StopGatherResources() 
    {
    	Slider progressbar = GameObject.Find("GatherProgressbar").GetComponent<Slider>(); // TODO tidy up this mess
    	progressbar.value = 0;
    	m_isGatherning = false;
    	StopCoroutine(m_gatheringCoroutine);
    	
    	m_gatherBtnText.text = "Gather Resources";
    }

    // Gathering coroutine. Handles the progressbar.
    private IEnumerator Gathering(Slider progressbar, float time, Action callback)
    {
    	float increment = 0.01f;

    	for (float i = 0f; i < time; i += increment) {
    		progressbar.value = i/time;
    		yield return new WaitForSeconds(increment);

    		// Callback when done
    		if (i >= time - increment) {
    			progressbar.value = 0;
                callback?.Invoke();
            }
    			
    	}
    }

    // Gather or stop gathering upon clicking, depending on the state
    public void TryGathering(Button caller)
    {
    	if (m_isGatherning) {
    		StopGatherResources();

    	} else {
    		GatherResources(caller);
    	}
    }

    #endregion



    // Adds the newly crafted Artifact to the Armory and Updates it
    public void AddNewArtifact(Artifact artifact)
    {
        armory.AddArtifact(artifact);

        if (m_armory.activeSelf) {
            m_armoryHandler.GetComponent<ArmoryHandler>().UpdateContents();
        }
    }

}
