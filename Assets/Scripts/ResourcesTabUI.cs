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
	public GameObject glass;
	public GameObject crystals;

	private Text goldText, woodText, metalText, glassText, crystalsText;

	void Awake() {
		gameController = gameManager.GetComponent<GameController>();
	}

	void Start() {
		goldText = gold.GetComponent<Text>();
		woodText = wood.GetComponent<Text>();
		metalText = metal.GetComponent<Text>();
		glassText = glass.GetComponent<Text>();
		crystalsText = crystals.GetComponent<Text>();
	}


	void Update() {
		// Retrieve the current inventory from the GameController
		Inventory inventory = gameController.inventory;

		goldText.text = inventory.gold.ToString();

		woodText.text = inventory.GetResourceAmount(ResourceType.WOOD).ToString();
		metalText.text = inventory.GetResourceAmount(ResourceType.METAL).ToString();
		glassText.text = inventory.GetResourceAmount(ResourceType.GLASS).ToString();
		crystalsText.text = inventory.GetResourceAmount(ResourceType.CRYSTALS).ToString();

	}



}
