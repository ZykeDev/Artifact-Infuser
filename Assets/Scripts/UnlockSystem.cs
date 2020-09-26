using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockSystem : MonoBehaviour {
	
	private GameController gameController;
	private bool isFirstPlaythrough = true;

	public List<Blueprint> blueprints;
    public List<Cypher> cyphers;
    public bool[] activeBlueprints; // index = blueprint ID
    public bool[] activeCyphers; // index = cypher ID
    
    // Start is called before the first frame update
    void Awake() {
        gameController = this.GetComponent<GameController>();
        
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
    
}