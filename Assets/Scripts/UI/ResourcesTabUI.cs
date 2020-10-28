using UnityEngine;
using UnityEngine.UI;

public class ResourcesTabUI : MonoBehaviour {

	[SerializeField] private GameController m_gameController;

	[SerializeField] private Text m_goldText, 
								  m_woodText, 
								  m_metalText, 
								  m_leatherText, 
								  m_crystalsText;

	// TODO should this be moved to something like FixedUpdate or manual update?
	void Update() {
		// Retrieve the current inventory from the GameController
		Inventory inventory = m_gameController.inventory;

		m_goldText.text = inventory.GetGold().ToString();

		m_woodText.text = inventory.GetResourceAmount(ResourceType.WOOD).ToString();
		m_metalText.text = inventory.GetResourceAmount(ResourceType.METAL).ToString();
		m_leatherText.text = inventory.GetResourceAmount(ResourceType.LEATHER).ToString();
		m_crystalsText.text = inventory.GetResourceAmount(ResourceType.CRYSTALS).ToString();
	}

}
