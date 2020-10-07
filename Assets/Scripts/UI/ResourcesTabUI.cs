using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesTabUI : MonoBehaviour {

	public GameObject gameManager;
	private GameController gameController;

	public GameObject gold;
	public GameObject wood;
	public GameObject metal;
	public GameObject leather;
	public GameObject crystals;

	private Text goldText, woodText, metalText, leatherText, crystalsText;

	void Awake() {
		gameController = gameManager.GetComponent<GameController>();
	}

	void Start() {
		goldText = gold.GetComponent<Text>();
		woodText = wood.GetComponent<Text>();
		metalText = metal.GetComponent<Text>();
		leatherText = leather.GetComponent<Text>();
		crystalsText = crystals.GetComponent<Text>();
	}


	void Update() {
		// Retrieve the current inventory from the GameController
		Inventory inventory = gameController.inventory;

		goldText.text = inventory.gold.ToString();

		woodText.text = inventory.GetResourceAmount(ResourceType.WOOD).ToString();
		metalText.text = inventory.GetResourceAmount(ResourceType.METAL).ToString();
		leatherText.text = inventory.GetResourceAmount(ResourceType.LEATHER).ToString();
		crystalsText.text = inventory.GetResourceAmount(ResourceType.CRYSTALS).ToString();

	}



}
