using System.Collections.Generic;
using UnityEngine;

public class UnlockSystem : MonoBehaviour {
	
    private GameController m_gameController;
    private RequestSystem m_requestSystem;

    private bool m_isFirstPlaythrough = true;

	public List<Blueprint> blueprints;
    public List<Cypher> cyphers;
    public bool[] activeBlueprints; // index = blueprint ID
    public bool[] activeCyphers; // index = cypher ID


    // TODO move to a specific Progress.cs file?
    private enum Progress
    {
        START,              // The very beginning of the game
        FIRST_GATHER,
        FIRST_BLUEPRINT,
        FIRST_ARTIFACT,
        FIRST_REQUEST,
        _
    }

    private Progress m_progress;

    
    
    // Start is called before the first frame update
    void Awake() {
        m_gameController = GetComponent<GameController>();
        m_requestSystem = GetComponent<RequestSystem>();

        BlueprintDatabase.SetBlueprints(blueprints);
        /*
        activeBlueprints = new bool[blueprints.Count];
        activeBlueprints[0] = true;
        activeBlueprints[1] = true;
        activeBlueprints[2] = true;
        */
        activeCyphers = new bool[cyphers.Count];
        activeCyphers[0] = true;
        activeCyphers[1] = true;

        m_progress = Progress.FIRST_REQUEST;

        
    }

    void Start()
    {
        FirstRequest();
    }


    // TODO
    private void NextState()
    {
        m_progress++;

    }



    private void FirstRequest()
    {
        Request firstRequest = m_requestSystem.AddFirstRequest(); 

        m_gameController.AddDialogueRequest(firstRequest);
    }



















    // Notifies the Crafter that its data has been updated (i.e. new blueprint unlock) TODO
    public void NotifyCrafter() {
    	//Crafter crafter = GameObject.Find("Crafter").GetComponent<Crafter>();
        //crafter.Notify();
    }

    /// <summary>
    /// Returns the Blueprint with the given ID
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public Blueprint GetBlueprint(int ID)
    {
        if (blueprints[ID].GetID() == ID)
        {
            return blueprints[ID];
        }
        else
        {
            foreach (Blueprint bp in blueprints)
            {
                if (bp.GetID() == ID)
                {
                    return bp;
                }
            }
        }

        return null;
    }

    
}