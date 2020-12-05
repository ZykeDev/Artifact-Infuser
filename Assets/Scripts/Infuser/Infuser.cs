using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Infuser : MonoBehaviour {

    #region Vars

    [Header("Script References")]
    [SerializeField] private GameController m_gameController;
    [SerializeField] private UnlockSystem m_unlockSystem;
	[SerializeField] private ButtonHandler m_buttonHandler;

	[Header("UI Components")]
	[SerializeField] private GameObject m_artifactSelector;
	[SerializeField] private GameObject m_cypherSelector;

	[SerializeField] private Slider m_progressbar;

	[SerializeField]
	private GameObject m_cypherSelectorContent, m_artifactSelectorContent;

	[Header("Prefabs")]
	[SerializeField] private GameObject m_cypherBtnPref, m_artifactBtnPrefab;

    private List<GameObject> m_cypherBtns, m_artifactBtns;
    private List<Cypher> m_cyphers;
	private float m_btnHeight = 0;
	private int m_cyphersBtnsPerScreen = 4;

	private Artifact m_selectedBaseArtifact = null;
	private int m_selectedCypherID = -1;
	private bool[] m_activeCyphers;

	private RequiredRunes m_refund = new RequiredRunes();

	
	private InfuserState m_infuserState;

	private enum InfuserState 
	{ 
		SELECTING_ARTIFACT, 
		SELECTING_CYPHER, 
		INFUSING
	}

	#endregion

	#region Awake Start Update

	public void Awake()
    {
		m_cypherBtns = new List<GameObject>();
		m_artifactBtns = new List<GameObject>();

		m_infuserState = InfuserState.SELECTING_ARTIFACT;

		m_cyphers = m_unlockSystem.cyphers;
		m_activeCyphers = m_unlockSystem.activeCyphers;

		// Instantiate the active artifact and cypher btns
		//InstantiateArtifacts();
		//InstantiateCyphers();

		//UpdateViewportHeight();
	}

    private void Start()
    {
		// Instantiate the active artifact and cypher btns
		InstantiateArtifacts();
		InstantiateCyphers();

		UpdateViewportHeight();
	}

    #endregion


    internal void OnFocus()
	{
		UpdateActiveArtifacts();
		UpdateActiveCyphers();
		
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
					m_artifactSelector.SetActive(false);

					UpdateActiveCyphers();

					m_cypherSelector.SetActive(true);

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

		List<int> visibleArtifactIDs = new List<int>();

		foreach (Artifact art in m_gameController.armory.GetArtifacts())
		{
			// Ignore duplicates
			if (visibleArtifactIDs.Contains(art.GetArtifactID())) continue;

			visibleArtifactIDs.Add(art.GetArtifactID());

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

			Button buttonComp = newArtifactBtn.GetComponent<Button>();
			TooltipTrigger tooltipComp = newArtifactBtn.GetComponent<TooltipTrigger>();
			ButtonGraphic graphicComp = newArtifactBtn.GetComponent<ButtonGraphic>();

			// Add a listener to the new Button
			buttonComp.onClick.AddListener(delegate
			{
				tooltipComp.HideTooltip();
				m_selectedBaseArtifact = art;
				ChangeState(InfuserState.SELECTING_CYPHER);
			});

			tooltipComp.SetTooltipData(art.GetTooltipData());

			// Add the text and the sprite to the button
			graphicComp.SetData(art.GetName(), art.GetSprite());

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
		if (m_artifactBtns == null) return;

		if (m_artifactBtns.Count != 0)
        {
			foreach (GameObject art in m_artifactBtns)
			{
				Destroy(art);
			}

			m_artifactBtns.Clear();
        }

		InstantiateArtifacts();
	}


	/// <summary>
	/// Destroys all cypher btns and re-instantiates them, checking for new active ones.
	/// </summary>
	public void UpdateActiveCyphers()
	{
		if (m_cypherBtns == null) return;

        if (m_cypherBtns.Count == 0)
        {
			foreach (GameObject cypher in m_cypherBtns)
			{
				Destroy(cypher);
			}

			m_cypherBtns.Clear();
		}

    	InstantiateCyphers();
    }


	/// <summary>
	/// Instantiates all active cyphers.
	/// </summary>
	private void InstantiateCyphers()
	{
		if (m_cyphers == null) return;

		// Button's offset position from the sides
		float offset = 15f;

    	foreach (Cypher c in m_cyphers)
		{
			int currentID = c.GetID();

			// Init the tooltip texts within the cypher
			c.InitTooltipData();

			// Don't show cyphers that are not active
			if (currentID >= m_activeCyphers.Length || !m_activeCyphers[currentID])
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
				tooltipComp.HideTooltip();
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
		if (m_selectedCypherID < 0) return;


		Cypher cypher = GetCypherWithID(m_selectedCypherID);

		// Check if there are enough resources
		RequiredRunes requiredRunes = cypher.GetRequiredRunes();

		// Check there are enough resources and spend them if there are
		bool canInfuse = m_gameController.inventory.SpendRunes(requiredRunes);
		if (!canInfuse) return;

		m_refund = requiredRunes;

		// Remove the consumed artifact
		m_gameController.RemoveArtifact(m_selectedBaseArtifact);


		// Change the Infuse btn to the "Stop Infusing" btn
		m_buttonHandler.SawpInfuseWithStop();

		// Start the infusing timer coroutine
		float infusionTime = cypher.GetInfusionTime();
		m_progressbar.value = 0;

		m_gameController.Infuse(m_selectedCypherID, m_selectedBaseArtifact, infusionTime);
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
		
		RefundResources();
	}


	// Upon compleation, show the new Artifact and add it to the armory
	public void FinishInfusing() {
    	UpdateInfusionProgress(0); // Needed?

    	// Change the Infuse btn to the "Infuse" btn
		m_buttonHandler.SawpStopWithInfuse();

	}


	private void RefundResources()
    {
		m_gameController.AddNewArtifact(m_selectedBaseArtifact);

		// Exit if the refund var is empty
		if (!m_refund.Equals(default(RequiredResources)))
        {
			m_gameController.inventory.Add(m_refund);
        }

		// Reset the refund var
		m_refund = new RequiredRunes();
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
