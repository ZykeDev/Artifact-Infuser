using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmoryHandler : MonoBehaviour {

	public GameObject CellPrefab;

	public GameObject ArmoryContentWeapons;
	public GameObject ArmoryContentArmor;
	public GameObject ArmoryContentAccessories;
	public GameObject ArmoryContentAbyss;

	private GameController gameController;
	private int cellsPerRow;

	[SerializeField]
	private ArmoryTab currentTab;

	
	// Pool of all cell groups
	struct Cells {
		public List<GameObject> weaponCellsList;
		public List<GameObject> armorCellsList;
		public List<GameObject> accessoryCellsList;
		public List<GameObject> abyssCellsList;
	}

	private Cells instantiatedCells;

	void Awake() {
		gameController = GameObject.Find("GameManager").GetComponent<GameController>();
		cellsPerRow = 7;

		instantiatedCells = new Cells();
		instantiatedCells.weaponCellsList = new List<GameObject>();
		instantiatedCells.armorCellsList = new List<GameObject>();
		instantiatedCells.accessoryCellsList = new List<GameObject>();
		instantiatedCells.abyssCellsList = new List<GameObject>();

	}

    // Start is called before the first frame update
    void Start() {
    	currentTab = ArmoryTab.WEAPONS; // Default is Weapons
    	PopulateGrids();
    }


    public void UpdateContents() {
		PopulateGrids();
    }


    // Changes the current tab
    public void ChangeTab() {

    }


    // Populate the grids with the Artifacts in the Armory
    private void PopulateGrids() {
    	// First, clear the list of currently instantiated cells
    	DestroyAllCells();

    	// Display the Artifacts in different tabs depending on their type
    	foreach(ArtifactType type in System.Enum.GetValues(typeof(ArtifactType))) {
    		List<Artifact> filteredArtifacts = gameController.armory.FilterByType(type); 

    		int i = 0;
    		foreach(Artifact artifact in filteredArtifacts) {
    			GameObject targetParent = GetParentFromType(type);
    			GameObject cell = CreateGridCell(artifact, targetParent, i);
    			
    			// Add it to the corresponding pool
    			GetCellGroupFromType(this.instantiatedCells, type).Add(cell);

    			i++;
    		}	
    	}
    }


    // Creates a new cell and instantiates it
    private GameObject CreateGridCell(Artifact artifact, GameObject targetParent, int index) {
    	float offset = 14f;
    	float cellSize = 72f;

    	GameObject newCell = Instantiate(CellPrefab, new Vector2(0, 0), Quaternion.identity, targetParent.transform) as GameObject;
	    newCell.name = "cell_" + index;
	    // Apply the corresponding sprite
	    newCell.GetComponent<ArmoryGridCell>().cellIcon.GetComponent<Image>().sprite = artifact.GetSprite();
	    
	    // PLace the cells in a grid, changing row when needed
		float rowIndex = 0;
		float colIndex = 0;

		if (index != 0) {
			rowIndex = (float)index % cellsPerRow;
			colIndex = (float)Mathf.Floor(index / cellsPerRow);
		}

    	float x = 50f + (cellSize + offset) * rowIndex;
    	float y = -50f - (cellSize + offset) * colIndex;
    	
	    newCell.transform.localPosition = new Vector2(x, y);


    	return newCell;
    }


    // Returns the tab parent GO depending on the Artifact type
    private GameObject GetParentFromType(ArtifactType type) {
    	switch(type) {
    		case ArtifactType.WEAPON:
    			return ArmoryContentWeapons;

    		case ArtifactType.ARMOR:
    			return ArmoryContentArmor;

    		case ArtifactType.ACCESSORY:
    			return ArmoryContentAccessories;

    		case ArtifactType.ABYSS:
    			return ArmoryContentAbyss;

    		default:
    			return ArmoryContentWeapons;    		
    	}
    }

    // Returns the cell group depending on the Artifact type
    private List<GameObject> GetCellGroupFromType(Cells cells, ArtifactType type) {
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
    			return cells.weaponCellsList;    		
    	}
    }





    // Destroys every gameobject in instantiatedCells and Clears the list
    private void DestroyAllCells() {
    	DestroyCells(instantiatedCells.weaponCellsList);
    	DestroyCells(instantiatedCells.armorCellsList);
    	DestroyCells(instantiatedCells.accessoryCellsList);
    	DestroyCells(instantiatedCells.abyssCellsList);    	
    }

    private void DestroyCells(List<GameObject> group) {
    	foreach(GameObject a in group) {
    		Destroy(a);
    	}

		group.Clear();
    }

}
