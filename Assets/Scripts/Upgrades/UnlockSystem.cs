using System.Collections.Generic;
using UnityEngine;

public class UnlockSystem : MonoBehaviour 
{
    private GameController m_gameController;
    private RequestSystem m_requestSystem;

    //private bool m_isFirstPlaythrough = true;

	[SerializeField] protected List<Blueprint> blueprints;
    public List<Cypher> cyphers;

    public bool[] activeBlueprints; // index = blueprint ID
    public bool[] activeCyphers; // index = cypher ID
    public bool[] unlockedAreas;
    public bool[] boughtUpgrades;

    
    public enum Progress
    {
        SETUP,              // Setting up the game/scene
        START,              // The very beginning of the game
        FIRST_GATHER,
        FIRST_ARTIFACT, 
        FIRST_SELL,
        FIRST_REQUEST,
        FIRST_UPGRADE,
        FIRST_INFUSION,
        UNLOCK_NEW_AREA,
        FIRST_ASSISTANT,
        _
    }
    public enum WaitState
    {
        NOTHING,
        GATHER,
        CRAFT_SWORD,
        SELL_SWORD,
        REQUEST,
        UPGRADE,
        INFUSION,
        GATHER_BEACH,
        SEND_ASSISTANT
    }

    public Progress m_progress;
    private WaitState m_waitState;
    
    
    public void Init(SaveData saveData) {
        
        m_gameController = GetComponent<GameController>();
        m_requestSystem = GetComponent<RequestSystem>();

        // TODO add a check to make sure the blueprints' ID == index in BlueprintDatabase.blueprints
        BlueprintDatabase.SetBlueprints(blueprints);

        // Init the data
        if (saveData == null)
        {
            activeCyphers = new bool[cyphers.Count];
            activeCyphers[0] = true;
            activeCyphers[1] = true;

            boughtUpgrades = new bool[7]; // TODO grab the number of upgrades

            m_progress = Progress.START;
        } 
        else // Read the data from the savefile
        {
            activeBlueprints = saveData.activeBlueprints;
            activeCyphers = saveData.activeCyphers;
            unlockedAreas = saveData.unlockedAreas;
            boughtUpgrades = saveData.boughtUpgrades;

            m_progress = saveData.progress;
        }
    }

    void Start()
    {
        ChangeState(m_progress);
    }


    private void NextState()
    {
        m_progress++;
        ChangeState(m_progress);
    }

    private void ChangeState(Progress progress)
    {
        switch (progress)
        {
            case Progress.START:
                m_gameController.AddDialogue(DialogType.DIALOGUE, "It's a cold morning. You enter the dark shop and light up a candle.");
                m_gameController.AddDialogue(DialogType.DIALOGUE, "It takes you a few minutes to start up the forge, but you are finally successful.");
                m_gameController.AddDialogue(DialogType.DIALOGUE, "Your life as a blacksmith starts today.");

                m_gameController.AddNewline();

                m_gameController.AddDialogue(DialogType.DIALOGUE, "<#FFA100>Welcome to Artifact Infuser</color>");

                NextState();
                break;

            case Progress.FIRST_GATHER:
                m_gameController.AddNewline();
                m_gameController.AddDialogue(DialogType.DIALOGUE, "The warehouse is empty. It's time to go out and Gather some resources.");
                m_gameController.AddDialogue(DialogType.DIALOGUE, "From the <#FFA100>[Gathering]</color> tab on the left, select the <#FFA100>[Woods]</color> and click <#FFA100>[Gather Resources]</color>");
                
                m_waitState = WaitState.GATHER;
                break;

            case Progress.FIRST_ARTIFACT:
                m_gameController.AddNewline();
                m_gameController.AddDialogue(DialogType.DIALOGUE, "Try crafting a <#FFA100>[Simple Sword]</color> from the <#FFA100>[Forge]</color> menu. You should have sufficient resources now.");
                m_gameController.AddDialogue(DialogType.DIALOGUE, "Select the <#FFA100>[Simple Sword Blueprint]</color> from the list on the left. Then click the <#FFA100>[Craft]</color> button.");

                m_waitState = WaitState.CRAFT_SWORD;
                break;

            case Progress.FIRST_SELL:
                m_gameController.AddNewline();
                m_gameController.AddDialogue(DialogType.DIALOGUE, "The newly created artifact has been deposited in the <#FFA100>[Armory]</color>.");
                m_gameController.AddDialogue(DialogType.DIALOGUE, "Select it from there and <#FFA100>[Sell it]</color>.");

                m_waitState = WaitState.SELL_SWORD;
                break;

            case Progress.FIRST_REQUEST:
                m_gameController.AddNewline();
                m_gameController.AddDialogue(DialogType.DIALOGUE, "Your old firend Steve has come to commission a specific Artifact.");
                m_gameController.AddDialogue(DialogType.DIALOGUE, "Requests can be seen from the <#FFA100>[Commissions]</color> panel above.");

                m_requestSystem.AddFirstRequest();
                m_waitState = WaitState.REQUEST;
                break;

            case Progress.FIRST_UPGRADE:
                m_gameController.AddNewline();
                m_gameController.AddDialogue(DialogType.DIALOGUE, "Improve your shop and your crafting skills from the <#FFA100>[Upgrades]</color> menu.");
                m_gameController.AddDialogue(DialogType.DIALOGUE, "Gather enough resources to buy your first Upgrade.");

                m_gameController.UnlockUpgrades();
                m_waitState = WaitState.UPGRADE;
                break;

            case Progress.FIRST_INFUSION:
                m_gameController.AddNewline();
                m_gameController.AddDialogue(DialogType.DIALOGUE, "You just unlocked <#FFA100>[Infusion]</color>!");
                m_gameController.AddDialogue(DialogType.DIALOGUE, "Infusion can add magical properties to an artifact by imbuing it with the power of Runes.");
                m_gameController.AddDialogue(DialogType.DIALOGUE, "Just like blueprints, different abilities require different Cyphers.");

                m_gameController.AddNewline();

                m_gameController.AddDialogue(DialogType.DIALOGUE, "Infuse your first artifact with the <#FFA100>[Higher Power Cypher]</color>.");

                m_gameController.UnlockInfusion();
                m_waitState = WaitState.INFUSION;
                break;

            case Progress.UNLOCK_NEW_AREA:
                m_gameController.AddNewline();
                m_gameController.AddDialogue(DialogType.DIALOGUE, "Steve has come back to thank you for the sword you forged.");
                m_gameController.AddDialogue(DialogType.DIALOGUE, "He tells you his party of adventurers just successfully chased away the mosnters in the Beach south of here (partially) thanks to your smithing skills!");
                m_gameController.AddDialogue(DialogType.DIALOGUE, "You unlocked <#FFA100>[The Beach Area]</color> in the Gather menu. Go and see what you find there");

                UnlockArea(2); // 2 = Beach area ID
                m_waitState = WaitState.GATHER_BEACH;
                break;

            case Progress.FIRST_ASSISTANT:
                m_gameController.AddNewline();
                m_gameController.AddDialogue(DialogType.DIALOGUE, "A young lady enters the shop and asks to be your apprentice.");
                m_gameController.AddDialogue(DialogType.DIALOGUE, "You unlocked your first <#FFA100>[Assistant]</color>. From Gather menu you can send assistants to gather resources in your stead.");

                UnlockAssitant();
                m_waitState = WaitState.SEND_ASSISTANT;
                break;

            case Progress._:
                print("Tutorial Done");
                break;

            default:
                break;
        }
    }



    private void UnlockArea(int index)
    {
        unlockedAreas[index] = true;
        m_gameController.UpdateAreas();
    }

    private void UnlockAssitant()
    {
        m_gameController.UnlockAssitant();
    }

    private void UnlockBlueprint(int id)
    {
        activeBlueprints[id] = true;
        m_gameController.UpdateActiveBlueprints();
    }


    #region Notifications

    /// <summary>
    /// Notifies that a certain action has been completed. 
    /// If the UnlockSystem was waiting for such action, it advances to the next state.
    /// </summary>
    /// <param name="completedState">The WaitingState corresponding to the completed action</param>
    public void Notify(WaitState completedState)
    {
        if (m_waitState == completedState)
        {
            m_waitState = WaitState.NOTHING;
            NextState();
        }
    }


    /// <summary>
    /// Notify that a certain artifact has been crafted
    /// </summary>
    /// <param name="artifact"></param>
    public void Notify(Artifact artifact)
    {
        if (artifact == null) return;

        switch (artifact.GetArtifactID())
        {
            case 0: // ID of the simple sword
                Notify(WaitState.CRAFT_SWORD);
                break;

            default:
                break;
        }

    }

    /// <summary>
    /// Notifies that a certain artifact has been sold
    /// </summary>
    /// <param name="soldArtifact"></param>
    public void NotifySell(Artifact soldArtifact)
    {
        if (soldArtifact == null) return;

        switch (soldArtifact.GetArtifactID())
        {
            case 0: // ID of the simple sword
                Notify(WaitState.SELL_SWORD);
                break;

            default:
                break;
        }
    }
    
    public void Notify(Upgrade upgrade)
    {
        // Check if the upgrade unlocks new blueprints
        if (upgrade.GetEffect().GetFeature() == UnlockFeature.BLUEPRINTS_1)
        {
            UnlockBlueprint(2);
            UnlockBlueprint(3);
            return;
        }

        // Otherwise unlock whatever else
        switch (upgrade.GetID())
        {
            case 0: // ID of the first upgrade
                Notify(WaitState.UPGRADE);
                break;

            default:
                break;
        }
    }

    public void Notify(Cypher cypher)
    {
        switch (cypher.GetID())
        {
            case 1: // ID of the first cypher
                Notify(WaitState.INFUSION);
                break;

            default:
                break;
        }
    }

    public void Notify(Request request)
    {
        switch (request.GetArtifactID())
        {
            case 0: // ID of the artifact in the first request
                Notify(WaitState.REQUEST);
                break;

            default:
                break;
        }
    }


    #endregion


    #region Getters
    
    public bool IsAreaUnlocked(int areaIndex) => unlockedAreas[areaIndex];
 

    #endregion

}