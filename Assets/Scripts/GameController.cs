using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    #region Vars

    [Header("Script References")]
    [SerializeField] protected BackgroundManager m_backgroundManager;
    [SerializeField] protected TabHandler m_tabHandler;
    [SerializeField] protected ResourcesTabUI m_resourcesTab;
    [SerializeField] protected Shop m_shop;


    [SerializeField]
    protected GameObject m_gather, m_crafter, m_infuser, m_armory, m_upgrades;

    private Gathering m_gathering;
    private Crafter m_crafterComp;
    private Infuser m_infuserComp;
    private ArmoryHandler m_armoryHandler;
    private Upgrades m_upgradesHandler;

    public Inventory inventory;
    public Armory armory;

    public UnlockSystem m_unlockSystem;

    private SaveData m_saveData;

    #endregion


    void Awake() {
        m_unlockSystem = GetComponent<UnlockSystem>();

        m_gathering = m_gather.GetComponent<Gathering>();
        m_crafterComp = m_crafter.GetComponent<Crafter>();
        m_infuserComp = m_infuser.GetComponent<Infuser>();
        m_armoryHandler = m_armory.GetComponent<ArmoryHandler>();
        m_upgradesHandler = m_upgrades.GetComponent<Upgrades>();

        // Load the save file
        SaveData saveData = Load();

        inventory = new Inventory();
        armory = new Armory();

        // Apply the save data if there is
        if (saveData != null)
        {
            inventory = saveData.inventory;
            armory = saveData.armory;
        }

        m_unlockSystem.Init(saveData);
        m_gathering.Awake();
        m_infuserComp.Awake();
        m_armoryHandler.Awake();
        m_upgradesHandler.Awake();

        //armory.UpdateSprites();

        m_saveData = saveData;
    }


    void Start()
    {
        m_tabHandler.UpdateLocks(m_saveData);
        armory.UpdateSprites();
        Save();
    }


    public void SaveGame() => Save();
    public SaveData Save()
    {
        SaveData saveData = new SaveData(
            m_unlockSystem.activeBlueprints,
            m_unlockSystem.activeCyphers,
            m_unlockSystem.unlockedAreas,
            m_unlockSystem.boughtUpgrades,
            inventory,
            armory,
            m_unlockSystem.m_progress,
            m_tabHandler.isInfusionUnlocked,
            m_tabHandler.isUpgradesUnlocked);

        SaveSystem.Save(saveData);

        return saveData;
    }

    /// <summary>
    /// Loads the default savefile. Returns null if none are found.
    /// </summary>
    private SaveData Load()
    { 
        SaveData data = SaveSystem.Load();

        return data;
    }



    #region Gathering

    public void Gather(int tier, float time)
    {
        // Applies bonuses to the time
        time = m_upgradesHandler.ApplyBonuses(time);

        m_backgroundManager.Gather(tier, time);
    }
    public void StopGather() => m_backgroundManager.StopGathering();

    public void UpdateGatheringProgress(float progress)
    {
        if (m_gather.activeSelf) m_gathering.UpdateGatheringProgress(progress);
    }

    public void FinishGathering(int tier)
    {
        m_unlockSystem.Notify(UnlockSystem.WaitState.GATHER);

        m_gathering.FinishGathering(tier);
    }

    #endregion


    #region Crafting

    public void Craft(int blueprintID, float time)
    {
        // Applies bonuses to the time
        time = m_upgradesHandler.ApplyBonuses(time);

        m_backgroundManager.Craft(blueprintID, time);
    }
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

        // Notify the UnlockSystem in case it's waiting for an artifact to be crafted
        m_unlockSystem.Notify(newArtifact);

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

        m_unlockSystem.Notify(cypher);

        m_infuserComp.FinishInfusing();
    }

    #endregion


    #region Sell

    public void Sell(List<Artifact> artifacts)
    {
        if (artifacts == null || artifacts.Count == 0) return;
        
        m_shop.Sell(artifacts);
    }

   public void Sell(Artifact artifact, int reward) => m_shop.Sell(artifact, reward);
   public void Sell() => m_shop.Sell();
    

    #endregion


    #region Inventory Management

    public void AddResources(Inventory booty)
    {
        booty = m_upgradesHandler.ApplyBonuses(booty);
        m_resourcesTab.DisplayGain(booty);
        inventory.CombineWith(booty);
    }

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
        m_resourcesTab.DisplayGain(gold);
        inventory.AddGold(gold);
    }


    public void UpdateResourceUILoss(RequiredResources resources) => m_resourcesTab.DisplayLoss(resources);
    public void UpdateResourceUILoss(Upgrade upgrade) => m_resourcesTab.DisplayLoss(upgrade);
    public void UpdateResourceUIGain(RequiredResources resources) => m_resourcesTab.DisplayGain(resources);


    #endregion


    #region Dialog

    public void AddNewline() => m_shop.AddNewline();

    public void AddDialogue(DialogType type, string line) => m_shop.NewDialogue(type, line);
    public void AddDialogueRequest(Request request) => m_shop.NewRequest(request);



    #endregion


    #region Unlocks

    public void UnlockUpgrades() => m_tabHandler.UnlockUpgrades();
    public void UnlockInfusion() => m_tabHandler.UnlockInfusion();
    public void UpdateAreas() => m_gathering.UpdateAreas(); 

    #endregion
}
