using System.Collections.Generic;
using UnityEngine;

public class UnlockSystem : MonoBehaviour {
	
    private GameController m_gameController;
    private RequestSystem m_requestSystem;

    private bool m_isFirstPlaythrough = true;

	[SerializeField] private List<Blueprint> blueprints;
    public List<Cypher> cyphers;
    public bool[] activeBlueprints; // index = blueprint ID
    public bool[] activeCyphers; // index = cypher ID
    public bool[] unlockedAreas;
    public bool[] boughtUpgrades; // unused for now

    
    // TODO move to a specific Progress.cs file? or an actual FSM system?
    private enum Progress
    {
        SETUP,              // Setting up the game/scene
        START,              // The very beginning of the game
        FIRST_GATHER,
        FIRST_BLUEPRINT,
        FIRST_ARTIFACT, 
        FIRST_SELL,
        FIRST_REQUEST,
        _
    }
    public enum WaitState
    {
        NOTHING,
        GATHER,
        CRAFT_SWORD,
        SELL_SWORD,
        REQUEST,
    }

    private Progress m_progress;
    private WaitState m_waitState;
    
    
    void Awake() {
        m_gameController = GetComponent<GameController>();
        m_requestSystem = GetComponent<RequestSystem>();

        // TODO add a check to make sure the blueprints' ID == index in BlueprintDatabase.blueprints
        BlueprintDatabase.SetBlueprints(blueprints);
 
        activeCyphers = new bool[cyphers.Count];
        activeCyphers[0] = true;
        activeCyphers[1] = true;

        m_progress = Progress.SETUP;

    }

    void Start()
    {
        NextState();
    }





    private void NextState()
    {
        m_progress++;

        switch (m_progress)
        {
            case Progress.START:
                //m_gameController.AddDialogue(DialogType.DIALOGUE, "It's a cold autum morning. You enter the dark shop and light up a candle.");
                //m_gameController.AddDialogue(DialogType.DIALOGUE, "You are a bit nervous. It's your first day on the job, after all.");
                //m_gameController.AddDialogue(DialogType.DIALOGUE, "It takes you a few minutes to start up the forge, but you are finally successful.");
                //m_gameController.AddDialogue(DialogType.DIALOGUE, "Purchasing this old smithy to start your new career as a blacksmith might not have been a bad idea.");
                m_gameController.AddDialogue(DialogType.DIALOGUE, "Welcome to Artifact Infuser");


                m_gameController.AddNewline();
                NextState();
                break;

            case Progress.FIRST_GATHER:
                m_gameController.AddDialogue(DialogType.DIALOGUE, "The warehouse is kind of empty. It's time to go out and Gather some resources.");
                m_gameController.AddDialogue(DialogType.DIALOGUE, "From the <#FFA100>[Gathering]</color> tab on the left, select the <#FFA100>[Woods]</color> and click <#FFA100>[Gather Resources]</color>");
                
                m_waitState = WaitState.GATHER;
                break;

            case Progress.FIRST_BLUEPRINT:
                NextState();
                break;

            case Progress.FIRST_ARTIFACT:
                m_gameController.AddDialogue(DialogType.DIALOGUE, "Try crafting a <#FFA100>[Simple Sword]</color> from the <#FFA100>[Forge]</color> menu. You should have sufficient resources.");
                m_gameController.AddDialogue(DialogType.DIALOGUE, "Select the <#FFA100>[Simple Sword]</color> blueprint from the list on the left. Then click the <#FFA100>[Craft]</color> button.");

                m_gameController.AddNewline();

                m_waitState = WaitState.CRAFT_SWORD;
                break;

            case Progress.FIRST_SELL:
                m_gameController.AddDialogue(DialogType.DIALOGUE, "The newly created artifact has been deposited in the <#FFA100>[Armory]</color>.");
                m_gameController.AddDialogue(DialogType.DIALOGUE, "Select it from there and <#FFA100>[Sell it]</color> to your first costumer.");

                m_gameController.AddNewline();

                m_waitState = WaitState.SELL_SWORD;
                break;

            case Progress.FIRST_REQUEST:
                m_gameController.AddDialogue(DialogType.DIALOGUE, "Your old firend Steve has come to commission a specific Artifact.");
                m_gameController.AddDialogue(DialogType.DIALOGUE, "Requests can be seen from the <#FFA100>[Commissions]</color> panel above.");

                m_gameController.AddNewline();

                m_requestSystem.AddFirstRequest();
                m_waitState = WaitState.REQUEST;
                break;

            case Progress._:
                break;

            default:
                break;
        }
    }



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

}