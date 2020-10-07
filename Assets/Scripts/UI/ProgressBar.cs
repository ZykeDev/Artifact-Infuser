using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
	private Slider progressbar;

	private bool isRunning = false;
	private float target = 0;
	private float speedPerFrame = 0.5f;


	void Awake() {
		progressbar = gameObject.GetComponent<Slider>();
	}

    // Start is called before the first frame update
    void Start()
    {
        StartProgress(1f);
    }

    // Update is called once per frame
    void Update() {
    	// TODO this shouldn't always be checking
    	if (isRunning) {
	        if (progressbar.value < target) {
	        	progressbar.value += speedPerFrame * Time.deltaTime;

	        } else { 
	        	isRunning = false;
	        	progressbar.value = 0f;

	        }
    	}
    }


    // Sets the target point and resets the progress
    public void StartProgress(float target) {
    	this.target = Mathf.Clamp(target, 0, 1);
    	progressbar.value = 0;

    	isRunning = true;
    }

    public void StopProgress() {
    	isRunning = false;
    }
}
