using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmoryHandler : MonoBehaviour {

	public GameObject CellPrefab;

	public GameObject ArmoryContentWeapons;
	public GameObject ArmoryContentArmor;
	public GameObject ArmoryContentAccessories;
	public GameObject ArmoryContentAbyss;

	private GameController gameController;
	private int cellsPerRow;


	void Awake() {
		gameController = GameObject.Find("GameManager").GetComponent<GameController>();
		cellsPerRow = 13;

	}

    // Start is called before the first frame update
    void Start() {
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
    	// Display the Artifacts in different tabs depending on their type
    	foreach(ArtifactType type in System.Enum.GetValues(typeof(ArtifactType))) {
    		List<Artifact> filteredArtifacts = gameController.armory.FilterByType(type); 

    		int i = 0;
    		foreach(Artifact a in filteredArtifacts) {
    			GameObject targetParent = GetParentFromType(type);
    			GameObject cell = CreateGridCell(targetParent, i);
    			i++;

    		}	
    	}
    }


    // Creates a new cell and instantiates it
    private GameObject CreateGridCell(GameObject targetParent, int index) {
    	float offset = 4f;
    	float x = ((float)index * 72f) + offset;
    	float y = offset; // TODO

    	GameObject newCell = Instantiate(CellPrefab, new Vector2(x, y), Quaternion.identity, targetParent.transform) as GameObject;
	    //newCell.name = "cell_" + "0";


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

}
