using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmoryHandler : MonoBehaviour {

    [SerializeField]
	private GameObject m_gameManager, m_prefabCell;
	private GameController m_gameController;

    [SerializeField] private ButtonHandler m_buttonHandler;

    [SerializeField]
    private GameObject m_armoryContentAll,
        m_armoryContentWeapons, 
        m_armoryContentArmor, 
        m_armoryContentAccessories, 
        m_armoryContentAbyss;
    
	private int m_cellsPerRow;


	/// <summary>
    /// Struct containing a pool of armory cells
    /// </summary>
	struct Cells {
        public List<GameObject> allCellsList;
		public List<GameObject> weaponCellsList;
		public List<GameObject> armorCellsList;
		public List<GameObject> accessoryCellsList;
		public List<GameObject> abyssCellsList;
	}

	private Cells instantiatedCells;

	public void Awake()
    {
		m_gameController = m_gameManager.GetComponent<GameController>();

        m_cellsPerRow = 7;

        // Store the instantiated cells as lists inside a Cells struct
        instantiatedCells = new Cells
        {
            allCellsList = new List<GameObject>(),
            weaponCellsList = new List<GameObject>(),
            armorCellsList = new List<GameObject>(),
            accessoryCellsList = new List<GameObject>(),
            abyssCellsList = new List<GameObject>()
        };
    }

    void Start() => PopulateGrids();
    

    /// <summary>
    /// Updates the contents of every tab in the Armory.
    /// </summary>
    public void UpdateContents() => PopulateGrids();

    /// <summary>
    /// Updates the tab contents when it is focused on.
    /// </summary>
    public void OnFocus() => PopulateGrids();
    


    /// <summary>
    /// Populates the grids with the Artifacts in the Armory.
    /// </summary>
    private void PopulateGrids()
    {
    	// First, clear the list of currently instantiated cells
    	DestroyAllCells();

        // Display all artifacts in the All tab
        int j = 0;
        foreach (Artifact artifact in m_gameController.armory.GetArtifacts())
        {
            GameObject targetParent = m_armoryContentAll;
            GameObject cell = CreateGridCell(artifact, targetParent, j);

            // Add it to the corresponding pool
            instantiatedCells.allCellsList.Add(cell);

            j++;
        }

        // Display the Artifacts in different tabs depending on their type
        foreach (ArtifactType type in System.Enum.GetValues(typeof(ArtifactType)))
        {
    		List<Artifact> filteredArtifacts = m_gameController.armory.FilterByType(type); 

    		int i = 0;
    		foreach(Artifact artifact in filteredArtifacts)
            {
    			GameObject targetParent = GetParentFromType(type);
    			GameObject cell = CreateGridCell(artifact, targetParent, i);
    			
    			// Add it to the corresponding pool
    			GetCellGroupFromType(instantiatedCells, type).Add(cell);

    			i++;
    		}	
    	}
    }


    /// <summary>
    /// Creates a new cell and instantiates it.
    /// </summary>
    /// <param name="artifact"></param>
    /// <param name="targetParent"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    private GameObject CreateGridCell(Artifact artifact, GameObject targetParent, int index)
    {
        float gap = 50f;
    	float offset = 14f;
    	float cellSize = 128f;

    	GameObject newCell = Instantiate(m_prefabCell, new Vector2(0, 0), Quaternion.identity, targetParent.transform);
	    newCell.name = "Cell_" + index;

        // Assign the corresponding sprite
        ArmoryGridCell agc = newCell.GetComponent<ArmoryGridCell>();
        TooltipTrigger tooltip = newCell.GetComponent<TooltipTrigger>();
        PromptTrigger prompt = newCell.GetComponent<PromptTrigger>();
        Button button = newCell.GetComponent<Button>();


        agc.SetSprite(artifact.GetSprite());
        tooltip.SetTooltipData(artifact.GetName());
        prompt.SetTexts("Sell " + artifact.GetName() + "?");

        button.onClick.AddListener(delegate 
            {
                tooltip.HideTooltip();
                prompt.ShowPrompt();
                m_buttonHandler.OnArmoryCellClick(artifact);
            });

	    // Place the cells in a grid, changing row when needed
		float rowIndex = 0;
		float colIndex = 0;

		if (index != 0) {
			rowIndex = (float)index % m_cellsPerRow;
			colIndex = (float)Mathf.Floor(index / m_cellsPerRow);
		}

    	float x = gap + (cellSize + offset) * rowIndex;
    	float y = -gap - (cellSize + offset) * colIndex;
        
	    newCell.transform.localPosition = new Vector2(x + offset*2 , y + 800f - cellSize/2 - offset);

    	return newCell;
    }


    /// <summary>
    /// Returns the tab parent GameObject depending on the Artifact type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private GameObject GetParentFromType(ArtifactType type)
    {
    	switch(type) {
    		case ArtifactType.WEAPON:
    			return m_armoryContentWeapons;

    		case ArtifactType.ARMOR:
    			return m_armoryContentArmor;

    		case ArtifactType.ACCESSORY:
    			return m_armoryContentAccessories;

    		case ArtifactType.ABYSS:
    			return m_armoryContentAbyss;

    		default:
    			return m_armoryContentAll;    		
    	}
    }

    /// <summary>
    /// Returns the cell group depending on the Artifact type.
    /// </summary>
    /// <param name="cells"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private List<GameObject> GetCellGroupFromType(Cells cells, ArtifactType type)
    {
    	switch(type) {
    		case ArtifactType.WEAPON:
    			return cells.weaponCellsList;

    		case ArtifactType.ARMOR:
    			return cells.armorCellsList;

    		case ArtifactType.ACCESSORY:
    			return cells.accessoryCellsList;

    		case ArtifactType.ABYSS:
    			return cells.abyssCellsList;

    		default:
    			return cells.allCellsList;    		
    	}
    }





    /// <summary>
    /// Destroys every GameObject in instantiatedCells and Clears the list.
    /// </summary>
    private void DestroyAllCells() {
    	DestroyCells(instantiatedCells.weaponCellsList);
    	DestroyCells(instantiatedCells.armorCellsList);
    	DestroyCells(instantiatedCells.accessoryCellsList);
    	DestroyCells(instantiatedCells.abyssCellsList);    	
    }

    private void DestroyCells(List<GameObject> group) {
    	if (group == null) {
    		return;
    	}

    	foreach(GameObject a in group) {
    		Destroy(a);
    	}

		group.Clear();
    }


    /// <summary>
    /// Returns the Content-GameObject that holds the artifacts of the given type.
    /// </summary>
    /// <param name="artifactType"></param>
    /// <returns></returns>
    public GameObject GetContent(ArtifactType artifactType)
    {
        switch (artifactType)
        {
            case ArtifactType.WEAPON:
                return m_armoryContentWeapons;
            case ArtifactType.ARMOR:
                return m_armoryContentArmor;
            case ArtifactType.ACCESSORY:
                return m_armoryContentAccessories;
            case ArtifactType.ABYSS:
                return m_armoryContentAbyss;
            default:
                return m_armoryContentAll;
        }
    }

    public GameObject GetContent() => m_armoryContentAll;
    
}
