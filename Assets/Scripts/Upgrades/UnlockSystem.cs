using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockSystem : MonoBehaviour {
	
    private GameController m_gameController;
	private bool m_isFirstPlaythrough = true;

	public List<Blueprint> blueprints;
    public List<Cypher> cyphers;
    public bool[] activeBlueprints; // index = blueprint ID
    public bool[] activeCyphers; // index = cypher ID

    
    
    // Start is called before the first frame update
    void Awake() {
        m_gameController = GetComponent<GameController>();
        
        activeBlueprints = new bool[blueprints.Count];
        activeBlueprints[0] = true;
        activeBlueprints[1] = true;
        activeBlueprints[2] = true;

        activeCyphers = new bool[cyphers.Count];
        activeCyphers[0] = true;
        activeCyphers[1] = true;

        
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