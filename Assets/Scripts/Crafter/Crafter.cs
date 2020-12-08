using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafter : MonoBehaviour {

    #region Vars

	[Header("Script References")]
    [SerializeField] private GameController m_gameController;
	[SerializeField] private UnlockSystem m_unlockSystem;
	[SerializeField] private ButtonHandler m_buttonHandler;

	[Header("UI Elements")]
	[SerializeField] private Slider m_progressbar;
	[SerializeField] private Image m_artifact;
	[SerializeField] private Image m_reverse;
	[SerializeField] private Image m_backdrop;
	[SerializeField] private GameObject m_notice;

	[SerializeField] private GameObject m_blueprintSelectorContent;

	[Header("Prefabs")]
	[SerializeField] private GameObject m_blueprintBtnPref;

    private List<GameObject> m_blueprintBtns;
    private float m_blueprintBtnHeight = 0f;
	private int m_blueprintBtnsPerScreen = 4;
    private int m_selectedBlueprintID = -1;
	private bool[] m_activeBlueprints;

	private bool m_isCrafting = false;

	private RequiredResources m_refund = new RequiredResources();

    #endregion

    #region Awake Start Update

    public void Awake()
    {
        // Does nothing for now
    }

    void Start()
	{
		// Make sure the silhouette is disabled at the start
		m_artifact.enabled = false;
		m_reverse.enabled = false;
		m_backdrop.enabled = false;
		m_notice?.SetActive(false);

		m_blueprintBtns = new List<GameObject>();

		// Boolean list of which blueprints are active (where index = ID)
		m_activeBlueprints = m_unlockSystem.activeBlueprints;

		// Instantiate the active blueprint btns
		InstantiateBlueprints();

		// Udapte the viewer h = #active * btn.h
		UpdateViewportHeight();
    }

    #endregion

    #region Blueprint Selector

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

		int selectorIndex = 0;
    	foreach (Blueprint bp in BlueprintDatabase.blueprintsList)
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
			newBlueprint.transform.localPosition = new Vector2(offset, -height - offset - (height * selectorIndex));
			newBlueprint.name = "BPbtn_" + currentID;

	    	// Add a click listener to the new Button
	    	newBlueprint.GetComponent<Button>().onClick.AddListener(delegate {m_buttonHandler.OnSelectBlueprintClick(newBlueprint, currentID); });
			
			// Send the tooptip data to the ButtonHover component
			newBlueprint.GetComponent<TooltipTrigger>().SetTooltipData(bp.GetTooltipData());

			// Add the text and the sprite to the button
			newBlueprint.GetComponent<ButtonGraphic>().SetData(bp.GetName(), bp.GetBlueprintSprite(), bp.GetRarity());

	    	// Add the finished button to the list of instantiated buttons
	    	m_blueprintBtns.Add(newBlueprint);

			selectorIndex++;
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

	/// <summary>
	/// Sets the clicked blueprint as selected and activates the Crafting functionality
	/// </summary>
	/// <param name="blueprintID"></param>
	/// <param name="caller"></param>
	public void SelectBlueprint(int blueprintID, GameObject caller)
	{
		// Don't allow clicks mid-crafting
		if (m_isCrafting) return;

        // Set the blueprint as selected
        m_selectedBlueprintID = blueprintID;

		// Deselect all other buttons & select the new one
		foreach (GameObject blueprintBtn in m_blueprintBtns)
        {
			blueprintBtn.GetComponent<ButtonGraphic>().Deselect();
		}
		caller.GetComponent<ButtonGraphic>().Select();

		// Activate the Craft button
		m_buttonHandler.EnableCraftBtn();

		// Make sure the silhouette's image is enabled
		m_artifact.enabled = true;
		m_reverse.enabled = true;
		m_backdrop.enabled = true;

		// Render the artifact's silhouette in the ArtifactViewer
		Blueprint bp = GetBlueprintWithID(m_selectedBlueprintID);
		Sprite artifactSprite = bp.GetArtifactSprite();
		Sprite reverseSprite = bp.GetReverseSprite();
		
		m_artifact.sprite = artifactSprite;
		m_reverse.sprite = reverseSprite;
		m_backdrop.transform.localScale = Vector3.one;
	}

	public void ClearSelection()
    {
		if (m_isCrafting)
        {
			return;
        }

		// Remove the silhouette
		m_artifact.enabled = false;
		m_reverse.enabled = false;
		m_backdrop.enabled = false;

		// Deselect all buttons
		foreach (GameObject bpb in m_blueprintBtns)
        {
			bpb.GetComponent<ButtonGraphic>().Deselect();
        }

		// Reset the selected blueprint
		m_selectedBlueprintID = -1;

		// Make the Craft button not interactable
		m_buttonHandler.DisableCraftBtn();
	}


    #endregion

    #region Crafting System

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

		Blueprint blueprint = GetBlueprintWithID(m_selectedBlueprintID);
		
		RequiredResources requiredResources = blueprint.GetRequiredResources();

		// Check there are enough resources and spend them if there are
		bool canCraft = m_gameController.inventory.SpendResources(requiredResources);
		if (!canCraft)
		{
			m_notice?.SetActive(true);
			Delay.DelayAction(delegate { m_notice?.SetActive(false); });
			return;
		}

		m_gameController.UpdateResourceUILoss(requiredResources);

		// Save the refund amount
		m_refund = requiredResources;


		// Change the Craft btn to the "Stop Crafting" btn
		m_buttonHandler.SawpCraftWithStop();

		// Start the crafting timer coroutine
		float craftingTime = blueprint.GetCraftingTime();
		m_progressbar.value = 0;
		m_gameController.Craft(m_selectedBlueprintID, craftingTime);

		m_isCrafting = true;
	}
	
	/// <summary>
	/// Updates the progress bar
	/// </summary>
	/// <param name="progress"></param>
	public void UpdateCraftingProgress(float progress)
	{
		m_progressbar.value = progress;

		m_backdrop.transform.localScale = new Vector3(1f, (1 - progress), 1f);
	}


	/// <summary>
	/// Stops crafting and refunds the spent resources
	/// </summary>
	public void StopCraft()
	{
		// Stop the timer
    	m_gameController.StopCraft();

    	// Reset the progress bar
    	UpdateCraftingProgress(0);

    	// Change the "Stop Craft" btn to the "Craft" btn
		m_buttonHandler.SawpStopWithCraft();

		RefundResources();

		m_isCrafting = false;
	}

	/// <summary>
	/// Refunds the spent resources
	/// </summary>
	public void RefundResources()
    {
		// Exit if the refund var is empty
		if (m_refund.Equals(default(RequiredRunes)))
        {
			m_gameController.inventory.Add(m_refund);
        }

		// Reset the refund var
		m_refund = new RequiredResources();
	}


	public void FinishCrafting()
	{
    	UpdateCraftingProgress(0); // Needed?

    	// Change the Craft btn to the "Craft" btn
		m_buttonHandler.SawpStopWithCraft();

		m_isCrafting = false;
	}


	/// <summary>
	/// Returns the Blueprint with the corresponding ID from the blueprints list. Else returns Null.
	/// </summary>
	/// <param name="ID"></param>
	/// <returns></returns>
	public Blueprint GetBlueprintWithID(int ID)
	{
    	foreach(Blueprint bp in BlueprintDatabase.blueprintsList)
		{
    		if (bp.GetID() == ID) return bp;
    	}

    	return null;
    }


	#endregion

}
