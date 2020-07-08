using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockSystem : MonoBehaviour {
	
	private GameController gameController;
	private bool isFirstPlaythrough = true;

	public List<Blueprint> blueprints;
    
    // Start is called before the first frame update
    void Start() {
        gameController = this.GetComponent<GameController>();
    }



    // Notifies the Crafter that its data has been updated (i.e. new blueprint unlock)
    public void NotifyCrafter() {
    	GameObject.Find("Crafter").GetComponent<Crafter>().Notify();
    }
}
