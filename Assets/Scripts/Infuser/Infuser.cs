using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;

public class Infuser : MonoBehaviour {

    [SerializeField] private GameController m_gameController;
    [SerializeField] private UnlockSystem m_unlockSystem;
	[SerializeField] private ButtonHandler m_buttonHandler;
	[SerializeField] private Slider m_progressbar;

	[SerializeField]
	private GameObject m_cypherSelectorContent, m_artifactSelectorContent, 
		m_cypherBtnPref, m_artifactBtnPrefab;

    private List<GameObject> m_cypherBtns, m_artifactBtns;
    private List<Cypher> m_cyphers;
	private float m_btnHeight = 0;
	private int m_cyphersBtnsPerScreen = 4;
	private int m_selectedCypherID = -1;
	
	private bool[] m_activeCyphers;


	private InfuserState m_infuserState;

	private enum InfuserState 
	{ 
		SELECTING_ARTIFACT, 
		SELECTING_CYPHER, 
		INFUSING
	}


	void Start()
	{
		m_infuserState = InfuserState.SELECTING_ARTIFACT;

		m_cypherBtns = new List<GameObject>();
		m_artifactBtns = new List<GameObject>();

		m_cyphers = m_unlockSystem.cyphers;
		m_activeCyphers = m_unlockSystem.activeCyphers;

        // Instantiate the active cypher btns
		InstantiateCyphers();
       
        // Udapte the viewer h = #active * btn.h
		UpdateViewportHeight();

    }

	internal void OnFocus()
	{
		UpdateActiveCyphers();
		UpdateActiveArtifacts();

		UpdateViewportHeight();
		//UpdateArtifactViewportHeight();
	}

	private void ChangeState(InfuserState nextState)
    {
        switch (m_infuserState)
        {
            case InfuserState.SELECTING_ARTIFACT:
				if (nextState == InfuserState.SELECTING_CYPHER)
                {
					m_artifactSelectorContent.SetActive(false);

					UpdateActiveCyphers();

					m_cypherSelectorContent.SetActive(true);

					m_infuserState = nextState;
                }
                break;
            case InfuserState.SELECTING_CYPHER:
				if (nextState == InfuserState.SELECTING_ARTIFACT)
				{
					m_cypherSelectorContent.SetActive(false);

					UpdateActiveArtifacts();

					m_artifactSelectorContent.SetActive(true);
					
					m_infuserState = nextState;
				}
				break;
            case InfuserState.INFUSING:
                break;
            default:
                break;
        }
    }


	/// <summary>
	/// Instantiate all availabe artifacts as buttons
	/// </summary>
	private void InstantiateArtifacts()
    {
		// Button's offset position from the sides
		float offset = 15f;
		int listIndex = 0;

		foreach (Artifact art in m_gameController.armory.GetArtifacts())
		{
			GameObject newArtifactBtn = Instantiate(m_artifactBtnPrefab,
				new Vector2(0, 0),
				Quaternion.identity,
				m_artifactSelectorContent.transform);

			// Store the btn size locally
			float height = newArtifactBtn.GetComponent<RectTransform>().sizeDelta.y;
			if (m_btnHeight == 0) m_btnHeight = height;

			// Correctly position the new button. Use its ID as a vertical index
			newArtifactBtn.transform.localPosition = new Vector2(offset, -height - offset - (height * listIndex));
			newArtifactBtn.name = "Artifact Btn " + listIndex;

			// Add a listener to the new Button
			newArtifactBtn.GetComponent<Button>().onClick.AddListener(delegate
			{
				ChangeState(InfuserState.SELECTING_CYPHER);
			});

			newArtifactBtn.GetComponent<TooltipTrigger>().SetTooltipData(art.GetTooltipData());

			// Add the text and the sprite to the button
			newArtifactBtn.GetComponent<ButtonGraphic>().SetData(art.GetName(), art.GetSprite());

			// Add the finished button to the list of instantiated buttons
			m_artifactBtns.Add(newArtifactBtn);
			
			listIndex++;
		}
	}



    /// <summary>
    /// Destroys all artifact btns and re-instantiates them, checking for new ones.
    /// </summary>
    public void UpdateActiveArtifacts()
	{
		foreach (GameObject a in m_artifactBtns)
		{
			Destroy(a);
		}

		m_artifactBtns.Clear();

		InstantiateArtifacts();
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
				m_cypherSelectorContent.transform);

	    	// Store the btn size locally
	    	float height = newCypher.GetComponent<RectTransform>().sizeDelta.y;
	    	if (m_btnHeight == 0) m_btnHeight = height;

			// Correctly position the new button. Use its ID as a vertical index
			newCypher.transform.localPosition = new Vector2(offset, -height - offset - (height * currentID));
			newCypher.name = "Cypher Btn " + currentID;

			Button buttonComp = newCypher.GetComponent<Button>();
			TooltipTrigger tooltipComp = newCypher.GetComponent<TooltipTrigger>();
			ButtonGraphic graphicComp = newCypher.GetComponent<ButtonGraphic>();

			// Add a listener to the new Button
			buttonComp.onClick.AddListener(delegate 
			{
				//print("clicked");
				m_buttonHandler.OnSelectCypherClick(currentID);
			});

			// Send the tooptip data to the ButtonHover component
			tooltipComp.SetTooltipData(c.GetTooltipData());

			// Add the text and the sprite to the button
			graphicComp.SetData(c.GetName(), c.GetCypherSprite());


	    	// Add the finished button to the list of instantiated buttons
	    	m_cypherBtns.Add(newCypher);
    	}
    }


    // Updates the height of the cyphers viewport to accomodate more buttons
	private void UpdateViewportHeight()
	{
		RectTransform cSelectorContentTransform = m_cypherSelectorContent.GetComponent<RectTransform>();
		float cypherSelectorContentWidth = cSelectorContentTransform.sizeDelta.x;
		// 4 is how many btns can fit at a time
		if (GetNumberOfActiveCyphers() < m_cyphersBtnsPerScreen) {
			cSelectorContentTransform.sizeDelta = new Vector2(cypherSelectorContentWidth, 560f);

		} else {
			cSelectorContentTransform.sizeDelta = new Vector2(cypherSelectorContentWidth, m_btnHeight * (GetNumberOfActiveCyphers() + 1));
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
