using System;

[Serializable]
public class SaveData
{

    public bool[] activeBlueprints;
    public bool[] activeCyphers;
    public bool[] unlockedAreas;
    public bool[] boughtUpgrades;

    public Inventory inventory;
    public Armory armory;

    public UnlockSystem.Progress progress;
    
    public SaveData(bool[] activeBlueprints,
                    bool[] activeCyphers, 
                    bool[] unlockedAreas, 
                    bool[] boughtUpgrades, 
                    Inventory inventory, 
                    Armory armory,
                    UnlockSystem.Progress progress)
    
    {
        this.activeBlueprints = activeBlueprints;
        this.activeCyphers = activeCyphers;
        this.unlockedAreas = unlockedAreas;
        this.boughtUpgrades = boughtUpgrades;

        inventory.PrepareForSave();
        this.inventory = inventory;
        this.armory = armory;

        this.progress = progress;

    }
  

}
