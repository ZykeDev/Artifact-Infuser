using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public Inventory inventory;

	private int tier;


	private float gatheringDuration = 2f;
	private bool isGatherning = false; // TODO make this into a state?
	private Text gatherBtnText; // I dont like it being here, but its convenient
	private Coroutine gatheringCoroutine = null;


	void Awake() {
		gatherBtnText = GameObject.Find("GatherBtnText").GetComponent<Text>();
	
	}

    // Start is called before the first frame update
    void Start() {
    	this.tier = 0;

    	// Init Inventory
    	this.inventory = new Inventory();
    	
    }

    // Update is called once per frame
    void Update() {
        
    }


    private void FinishGathering() {
    	StopGatherResources();
    	Inventory booty = new Inventory();
    	booty.SetRandomResources(this.tier);

    	this.inventory.CombineWith(booty);
    }



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

    // Upon clicking, gather or stop gathering depending on the state
    public void TryGathering() {
    	if (isGatherning) {
    		StopGatherResources();

    	} else {
    		GatherResources();
    	}
    }








   
}
