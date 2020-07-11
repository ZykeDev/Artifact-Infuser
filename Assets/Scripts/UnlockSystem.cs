using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockSystem : MonoBehaviour {
	
	private GameController gameController;
	private bool isFirstPlaythrough = true;

	public List<Blueprint> blueprints;
    public bool[] activeBlueprints; // index = blueprint ID
    
    // Start is called before the first frame update
    void Start() {
        gameController = this.GetComponent<GameController>();
        
        activeBlueprints = new bool[blueprints.Count];
        activeBlueprints[0] = true;
        activeBlueprints[1] = true;
        activeBlueprints[2] = true;
        activeBlueprints[3] = true;
    }


    // Notifies the Crafter that its data has been updated (i.e. new blueprint unlock)
    public void NotifyCrafter() {
    	GameObject.Find("Crafter").GetComponent<Crafter>().Notify();
    }
}
