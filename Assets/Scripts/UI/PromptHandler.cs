using UnityEngine;

public class PromptHandler : MonoBehaviour {

	public GameObject m_windowPref;

	// TODO
	public void NewWindow() {
		// Default position is the center of the Canvas
		Vector2 pos = new Vector2(transform.position.x, transform.position.y);
		GameObject newWindow = Instantiate(m_windowPref, pos, Quaternion.identity, transform);

	}
    
}
