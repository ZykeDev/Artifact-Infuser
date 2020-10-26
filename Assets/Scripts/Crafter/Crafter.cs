using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafter : MonoBehaviour {

	[SerializeField] private GameController m_gameController;
	[SerializeField] private UnlockSystem m_unlockSystem;
	[SerializeField] private ButtonHandler m_buttonHandler;
	[SerializeField] private Slider m_progressbar;
	[SerializeField] private Image m_artifactSilhouette;

	[SerializeField]
    private GameObject m_blueprintSelectorContent, m_blueprintBtnPref;

    private List<GameObject> m_blueprintBtns;
    private List<Blueprint> m_blueprints;
    private float m_blueprintBtnHeight = 0f;
	private int m_blueprintBtnsPerScreen = 4;
    private int m_selectedBlueprintID = -1;
	private bool[] m_activeBlueprints;

	private Inventory m_refundedResources = new Inventory();


	void Start()
	{
		// Make sure the silhouette is disabled at the start
		m_artifactSilhouette.enabled = false;

		m_blueprintBtns = new List<GameObject>();

		// Boolean list of which blueprints are active (where index = ID)
		m_blueprints = m_unlockSystem.blueprints;
		m_activeBlueprints = m_unlockSystem.activeBlueprints;

		// Instantiate the active blueprint btns
		InstantiateBlueprints();

		// Udapte the viewer h = #active * btn.h
		UpdateViewportHeight();
    }


	/// <summary>
	/// Destroys all blueprint btns and re-instantiates them, checking for new active ones.
	/// </summary>
	public void UpdateActiveBlueprints()
	{
    	foreach (GameObject bp in m_blueprintBtns)
		{
    		Destroy(bp);
    	}

    	m_blueprintBtns.Clear();

    	InstantiateBlueprints();
    }


	/// <summary>
	/// Instantiates all active blueprints as buttons.
	/// </summary>
	private void InstantiateBlueprints()
	{
		// Button's offset position from the sides
		float offset = 15f;
    	
    	foreach (Blueprint bp in m_blueprints)
		{
			int currentID = bp.GetID();

			// Init the tooltip texts within the blueprint
			bp.InitTooltipData();

			// Don't show blueprints that are not active
			if (!m_activeBlueprints[currentID])
			{
    			continue;
    		}

	    	GameObject newBlueprint = Instantiate(m_blueprintBtnPref,
				new Vector2(0, 0),
				Quaternion.identity,
				m_blueprintSelectorContent.transform) as GameObject;

			// Store the btn height locally
	    	float height = newBlueprint.GetComponent<RectTransform>().sizeDelta.y;
	    	if (m_blueprintBtnHeight == 0) m_blueprintBtnHeight = height;
	    	
	    	// Position the new button. Use its ID as a vertical index
			newBlueprint.transform.localPosition = new Vector2(offset, -height - offset - (height * currentID));
			newBlueprint.name = "BPbtn_" + currentID;

	    	// Add a click listener to the new Button
	    	newBlueprint.GetComponent<Button>().onClick.AddListener(delegate {m_buttonHandler.OnSelectBlueprintClick(newBlueprint, currentID); });
			
			// Send the tooptip data to the ButtonHover component
			newBlueprint.GetComponent<ButtonHover>().SetTooltipData(bp.GetTooltipData());

			// Add the text and the sprite to the button
			newBlueprint.GetComponent<ButtonGraphic>().SetData(bp.GetName(), bp.GetBlueprintSprite());

	    	// Add the finished button to the list of instantiated buttons
	    	m_blueprintBtns.Add(newBlueprint);
    	}
    }


	/// <summary>
	/// Updates the height of the blueprints viewport to accomodate more buttons.
	/// </summary>
	private void UpdateViewportHeight()
	{
		RectTransform bpSelectorContentTransform = m_blueprintSelectorContent.GetComponent<RectTransform>();
		float contentWidth = bpSelectorContentTransform.sizeDelta.x;
		
		if (GetNumberOfActiveBlueprints() < m_blueprintBtnsPerScreen) {
			bpSelectorContentTransform.sizeDelta = new Vector2(contentWidth, 560f);

		} else {
			bpSelectorContentTransform.sizeDelta = new Vector2(contentWidth, m_blueprintBtnHeight * (GetNumberOfActiveBlueprints() + 1));
		}
	}


	/// <summary>
	/// Returns the number of active blueprints
	/// </summary>
	/// <returns></returns>
	private int GetNumberOfActiveBlueprints()
	{
		int n = 0;

		for (int i = 0; i < m_activeBlueprints.Length; i++) {
			if (m_activeBlueprints[i]) n++;
		}

		return n;
	}



	// TODO also be able to deselect and leave the crafting area empty
	/// <summary>
	/// Sets the clicked blueprint as selected and activates the Crafting functionality
	/// </summary>
	/// <param name="blueprintID"></param>
	/// <param name="caller"></param>
	public void SelectBlueprint(int blueprintID, GameObject caller)
	{
        // Set the blueprint as selected
        m_selectedBlueprintID = blueprintID;

		// Deselect all other buttons & select the new one
		foreach (GameObject blueprintBtn in m_blueprintBtns)
        {
			blueprintBtn.GetComponent<ButtonGraphic>().Deselect();
		}
		caller.GetComponent<ButtonGraphic>().Select();

		// Activate the Craft button
		m_buttonHandler.ActivateCraftBtn();

		// Make sure the silhouette's image is enabled
		if (!m_artifactSilhouette.enabled) m_artifactSilhouette.enabled = true;

		// Render the artifact's silhouette in the ArtifactViewer
		Sprite artifactSprite = GetBlueprintWithID(m_selectedBlueprintID).GetArtifactSprite();
		m_artifactSilhouette.sprite = artifactSprite;
	}


	/// <summary>
	/// Starts the crafting process (if there are enough resources)
	/// </summary>
	public void Craft()
	{
		// Check if there is a blueprint selected
		if (m_selectedBlueprintID < 0)
		{
			return;
		}

		// Check if there are enough resources

		// Change the Craft btn to the "Stop Crafting" btn
		m_buttonHandler.SawpCraftWithStop();

		// Start the crafting timer coroutine
		float craftingTime = GetBlueprintWithID(m_selectedBlueprintID).GetCraftingTime();
		m_progressbar.value = 0;
		m_gameController.Craft(m_selectedBlueprintID, craftingTime);

		// Compute the "refounded resources" if stopped and store them (better to do this b4 starting the craft?)
		
		// Actually spend the resources

	}
	
	/// <summary>
	/// Updates the progress bar value
	/// </summary>
	/// <param name="progress"></param>
	public void UpdateCraftingProgress(float progress)
	{
		m_progressbar.value = progress;
	}


	public void StopCraft()
	{
		// Stop the timer
    	m_gameController.StopCraft();

    	// Reset the progress bar
    	UpdateCraftingProgress(0);

    	// Change the Craft btn to the "Craft" btn
		m_buttonHandler.SawpStopWithCraft();

		// Return the "refounded resources"
		RefoundResources();
	}


	// Upon compleation, show the new Artifact and add it to the armory
	public void FinishCrafting()
	{
    	UpdateCraftingProgress(0); // Needed?

    	// Change the Craft btn to the "Craft" btn
		m_buttonHandler.SawpStopWithCraft();

		// Show confirmation window (if this is the first artifact of its kind) TODO
		// -- 

		// AFTER the confirmation window is closed (if it ever gets opened)
		// artifactSilhouetteImage.sprite = null;

	}

	// TODO
	private void RefoundResources()
	{
		
	}

    // Returns the Blueprint with the corresponding ID from the blueprints list. Else returns Null.
    public Blueprint GetBlueprintWithID(int ID)
	{
    	foreach(Blueprint bp in m_blueprints)
		{
    		if (bp.GetID() == ID) return bp;
    	}

    	return null;
    }



}
