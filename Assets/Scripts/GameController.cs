using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject Crafter;
    public GameObject Infuser;
    public GameObject Armory;
    public GameObject Upgrades;

    private Crafter crafter;
    //private Infuser infuser;
    private ArmoryHandler armoryHandler;
    //private Upgrades upgrades;

	public Inventory inventory;
    public Armory armory;

	private int tier;

	private float gatheringDuration = 2f;
	private bool isGatherning = false; // TODO make this into a state?
	private Text gatherBtnText; // I dont like it being here, but its convenient
	private Coroutine gatheringCoroutine = null;


	void Awake() {
        crafter = Crafter.GetComponent<Crafter>();
        //infuser = Infuser.GetComponent<Infuser>();
        armoryHandler = Armory.GetComponent<ArmoryHandler>();
        //upgrades = Upgrades.GetComponent<Upgrades>();


		gatherBtnText = GameObject.Find("GatherBtnText").GetComponent<Text>();
        this.tier = 0;

        // Init Inventory
        this.inventory = new Inventory();
        this.armory = new Armory();
	}

    // Start is called before the first frame update
    void Start() {
    	
    }

    // Update is called once per frame
    void Update() {
        
    }



    // Stops gathering resources and adds them to the inventory
    private void FinishGathering() {
    	StopGatherResources();
    	Inventory booty = new Inventory();
    	booty.SetRandomResources(this.tier);

    	this.inventory.CombineWith(booty);
    }


    // TODO change this into 2 different buttons
    public void GatherResources() {
    	Slider progressbar = GameObject.Find("GatherProgressbar").GetComponent<Slider>();

    	// Start a timer coroutine
    	// Show the progress bar
    	isGatherning = true;
    	gatherBtnText.text = "Stop Gathering";
    	gatheringCoroutine = StartCoroutine(Gathering(progressbar, gatheringDuration, FinishGathering));

    	// Show remaining time

   
    	// When compleated, create a list of new resources & show a prompt with the gained resources
    	// On click, the resources are added to the inventory
    }

    public void StopGatherResources() {
    	Slider progressbar = GameObject.Find("GatherProgressbar").GetComponent<Slider>(); // TODO tidy up this mess
    	progressbar.value = 0;
    	isGatherning = false;
    	StopCoroutine(gatheringCoroutine);
    	
    	gatherBtnText.text = "Gather Resources";
    }

    // Gathering coroutine. Handles the progressbar.
    private IEnumerator Gathering(Slider progressbar, float time, Action callback) {
    	float increment = 0.01f;

    	for (float i = 0f; i < time; i += increment) {
    		progressbar.value = i/time;
    		yield return new WaitForSeconds(increment);

    		// Callback when done
    		if (i >= time - increment) {
    			progressbar.value = 0;
    			if (callback != null) callback();
    		}
    			
    	}
    }

    // Gather or stop gathering upon clicking, depending on the state
    public void TryGathering() {
    	if (isGatherning) {
    		StopGatherResources();

    	} else {
    		GatherResources();
    	}
    }


    // Adds the newly crafted Artifact to the Armory and Updates it
    public void AddNewArtifact(Artifact artifact) {
        this.armory.AddArtifact(artifact);
        armoryHandler.GetComponent<ArmoryHandler>().UpdateContents();
    }




   
}
