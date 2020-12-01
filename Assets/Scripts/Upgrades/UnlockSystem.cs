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
    public bool[] boughtUpgrades;

    
    // TODO move to a specific Progress.cs file? or an actual FSM system?
    private enum Progress
    {
        SETUP,              // Setting up the game/scene
        START,              // The very beginning of the game
        FIRST_GATHER,
        FIRST_BLUEPRINT,
        FIRST_ARTIFACT,
        FIRST_REQUEST,
        _
    }
    private enum WaitState
    {
        NOTHING,
        CRAFT_SWORD
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
                m_gameController.AddDialogue(DialogType.DIALOGUE, "You are a bit nervous. It's your first day on the job, after all.");
                m_gameController.AddDialogue(DialogType.DIALOGUE, "It takes you a few minutes to start up the forge, but you are finally successful.");
                m_gameController.AddDialogue(DialogType.DIALOGUE, "Purchasing this old smithy to start your new career as a blacksmith might not have been a bad idea.");

                m_gameController.AddNewline();
                NextState();
                break;

            case Progress.FIRST_GATHER:
                NextState();
                break;

            case Progress.FIRST_BLUEPRINT:
                NextState();
                break;

            case Progress.FIRST_ARTIFACT:
                m_gameController.AddDialogue(DialogType.DIALOGUE, "Try crafting a <#FFA100>[Simple Sword]</color> from the <#FFA100>[Forge]</color> menu. You should have sufficient resources.");
                m_gameController.AddDialogue(DialogType.DIALOGUE, "Select the <#FFA100>[Simple Sword]</color> blueprint from the list on the left. Then click the <#FFA100>[Craft]</color> button.");
                m_waitState = WaitState.CRAFT_SWORD;
                break;

            case Progress.FIRST_REQUEST:
                m_gameController.AddDialogue(DialogType.DIALOGUE, "The newly created artifact has been deposite in the <#FFA100>[Armory]</color>.");
                m_gameController.AddNewline();
                FirstRequest();
                break;

            case Progress._:
                break;

            default:
                break;
        }
    }




    private void FirstRequest()
    {
        m_requestSystem.AddFirstRequest(); 
    }




    public void Notify(Artifact artifact)
    {
        // Unlock if it's waiting for a sword
        if (m_waitState == WaitState.CRAFT_SWORD)
        {
            if (artifact.GetArtifactID() == 0)
            {
                m_waitState = WaitState.NOTHING;
                NextState();
            }
        }
    }


}