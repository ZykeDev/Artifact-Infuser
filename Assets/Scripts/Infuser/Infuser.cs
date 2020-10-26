using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Infuser : MonoBehaviour {

    [SerializeField] private GameController m_gameController;
    [SerializeField] private UnlockSystem m_unlockSystem;
	[SerializeField] private ButtonHandler m_buttonHandler;
	[SerializeField] private Slider m_progressbar;

	[SerializeField]
	private GameObject m_cypherSelectorContent, m_cypherBtnPref;

    private List<GameObject> m_cypherBtns;
    private List<Cypher> m_cyphers;
    private float m_cypherBtnWidth = 0;
	private float m_cypherBtnHeight = 0;
	private int m_cyphersBtnsPerScreen = 4;
	private int m_selectedCypherID = -1;
	
	private bool[] m_activeCyphers;


	void Start()
	{
		m_cypherBtns = new List<GameObject>();

		m_cyphers = m_unlockSystem.cyphers;
		m_activeCyphers = m_unlockSystem.activeCyphers;

        // Instantiate the active cypher btns
		InstantiateCyphers();
       
        // Udapte the viewer h = #active * btn.h
		UpdateViewportHeight();

    }


	/// <summary>
	/// Destroys all cypher btns and re-instantiates them, checking for new active ones.
	/// </summary>
	public void UpdateActiveCyphers()
	{
    	foreach (GameObject c in m_cypherBtns) {
    		Destroy(c);
    	}

    	m_cypherBtns.Clear();

    	InstantiateCyphers();
    }


	/// <summary>
	/// Instantiates all active cyphers.
	/// </summary>
	private void InstantiateCyphers()
	{
		// Button's offset position from the sides
		float offset = 15f;
    	
    	foreach (Cypher c in m_cyphers)
		{
			int currentID = c.GetID();

			// Init the tooltip texts within the cypher
			c.InitTooltipData();

			// Don't show cyphers that are not active
			if (!m_activeCyphers[currentID])
			{
    			continue;
    		}

	    	GameObject newCypher = Instantiate(m_cypherBtnPref,
				new Vector2(0, 0),
				Quaternion.identity,
				m_cypherSelectorContent.transform) as GameObject;

	    	// Store the btn size locally
	    	float height = newCypher.GetComponent<RectTransform>().sizeDelta.y;
	    	if (m_cypherBtnHeight == 0) m_cypherBtnHeight = height;

			// Correctly position the new button. Use its ID as a vertical index
			newCypher.transform.localPosition = new Vector2(offset, -height - offset - (height * currentID));
			newCypher.name = "Cbtn_" + currentID;

			// Add a listener to the new Button
	    	newCypher.GetComponent<Button>().onClick.AddListener(delegate {m_buttonHandler.OnSelectCypherClick(currentID); });

			// Send the tooptip data to the ButtonHover component
			newCypher.GetComponent<ButtonHover>().SetTooltipData(c.GetTooltipData());
			
			// Add the text and the sprite to the button
			newCypher.GetComponent<ButtonGraphic>().SetData(c.GetName(), c.GetCypherSprite());

	    	// Add the text and the sprite to the button TODO move this to the cypher's script
	    	foreach (Transform child in newCypher.transform) {
	    		if (child.name == "CypherName") {
	    			child.GetComponent<Text>().text = c.name;
	    			continue;
	    		}
	    		if (child.name == "CypherSprite") {
	    			child.GetComponent<Image>().sprite = c.GetCypherSprite();
	    			continue;
	    		}

	    	}

	    	// Add the finished button to the list of instantiated buttons
	    	m_cypherBtns.Add(newCypher);
    	}
    }


    // Updates the height of the cyphers viewport to accomodate more buttons
	private void UpdateViewportHeight() {
		RectTransform cSelectorContentTransform = m_cypherSelectorContent.GetComponent<RectTransform>();
		float cypherSelectorContentWidth = cSelectorContentTransform.sizeDelta.x;
		// 4 is how many btns can fit at a time
		if (GetNumberOfActiveCyphers() < m_cyphersBtnsPerScreen) {
			cSelectorContentTransform.sizeDelta = new Vector2(cypherSelectorContentWidth, 560f);

		} else {
			cSelectorContentTransform.sizeDelta = new Vector2(cypherSelectorContentWidth, m_cypherBtnHeight * (GetNumberOfActiveCyphers() + 1));
		}
	}


	// Returns the number of active cyphers
	private int GetNumberOfActiveCyphers() {
		int n = 0;
		for (int i = 0; i < m_activeCyphers.Length; i++) {
			if (m_activeCyphers[i]) n++;
		}

		return n;
	}




	// TODO also be able to deselect and leave the crafting area empty
	public void SelectCypher(int cypherID) {
        // Set the cypher as "selected"
        m_selectedCypherID = cypherID;

        // Activate the Craft button
        m_buttonHandler.ActivateInfuseBtn();

		// Render the artifact's on the rune table
        // TODO

	}


	// Tries to start infusing an Artifact
	public void Infuse() {
		// Check if there is a cypher selected
		if (m_selectedCypherID < 0) {
			return;
		}

		// Check if there are enough resources

		// Check if there is enough space (unimplemented)

		// Change the Infuse btn to the "Stop Infusing" btn
		m_buttonHandler.SawpInfuseWithStop();

		// Start the infusing timer coroutine
		float infusionTime = GetCypherWithID(m_selectedCypherID).GetInfusionTime();
		m_progressbar.value = 0;

		m_gameController.Infuse(m_selectedCypherID, infusionTime);

		// Compute the "refounded resources" if stopped and store them (better to do this b4 starting the infusion?)
		
		// Spend the resources

	}

	public void UpdateInfusionProgress(float progress) {
		m_progressbar.value = progress;
	}


	public void StopInfusion() {
		// Stop the timer
    	m_gameController.StopInfusion();

    	// Reset the progress bar
    	UpdateInfusionProgress(0);

    	// Change the Infuse btn to the "Infuse" btn
		m_buttonHandler.SawpStopWithInfuse();
	}


	// Upon compleation, show the new Artifact and add it to the armory
	public void FinishInfusing() {
    	UpdateInfusionProgress(0); // Needed?

    	// Change the Infuse btn to the "Infuse" btn
		m_buttonHandler.SawpStopWithInfuse();

		// Show confirmation window (if this is the first artifact of its kind) TODO
		// -- 
	}

    // Returns the cypher with the corresponding ID from the cyphers list. Else returns Null.
    public Cypher GetCypherWithID(int ID) {
    	foreach(Cypher c in m_cyphers) {
    		if (c.GetID() == ID) {
    			return c;
    		}
    	}

    	return null;
    }



}
