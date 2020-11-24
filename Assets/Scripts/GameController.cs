using UnityEngine;

public class GameController : MonoBehaviour {

    #region Vars

    [SerializeField]
    protected ResourcesTabUI m_resourcesTab;

    [SerializeField]
    protected BackgroundManager m_backgroundManager;

    [SerializeField]
    protected GameObject m_gather, m_crafter, m_infuser, m_armory, m_upgrades;

    private Gathering m_gathering;
    private Crafter m_crafterComp;
    private Infuser m_infuserComp;
    private ArmoryHandler m_armoryHandler; //Called armoryHandler to not confuse it with the armory struct
    //private UpgradesHandler m_upgradesHandler;

    public Inventory inventory;
    public Armory armory;

	

    #endregion


    void Awake() {
        m_gathering = m_gather.GetComponent<Gathering>();
        m_crafterComp = m_crafter.GetComponent<Crafter>();
        m_infuserComp = m_infuser.GetComponent<Infuser>();
        m_armoryHandler = m_armory.GetComponent<ArmoryHandler>();
        //m_upgradesHandler = m_upgrades.GetComponent<Upgrades>();

        // Init Inventory
        inventory = new Inventory();
        armory = new Armory();
	}


    #region Gathering

    public void Gather(int tier, float time) => m_backgroundManager.Gather(tier, time);
    public void StopGather() => m_backgroundManager.StopGathering();

    public void UpdateGatheringProgress(float progress)
    {
        if (m_gather.activeSelf) m_gathering.UpdateGatheringProgress(progress);
    }

    public void FinishGathering(int tier)
    {
        m_gathering.FinishGathering(tier);
    }

    #endregion


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


    


    #region Inventory Management

    /// <summary>
    /// Adds the newly crafted Artifact to the Armory and Updates it.
    /// </summary>
    /// <param name="artifact"></param>
    public void AddNewArtifact(Artifact artifact)
    {
        armory.AddArtifact(artifact);

        if (m_armory.activeSelf) {
            m_armoryHandler.GetComponent<ArmoryHandler>().UpdateContents();
        }
    }

    /// <summary>
    /// Removes the artifact from ther armory, if present
    /// </summary>
    /// <param name="artifact"></param>
    public void RemoveArtifact(Artifact artifact)
    {
        armory.RemoveArtifact(artifact);

        if (m_armory.activeSelf)
        {
            m_armoryHandler.GetComponent<ArmoryHandler>().UpdateContents();
        }
    }

    /// <summary>
    /// Adds the given amount of gold to the inventory
    /// </summary>
    /// <param name="gold"></param>
    public void GainGold(int gold)
    {
        inventory.AddGold(gold);
    }

    #endregion

}
