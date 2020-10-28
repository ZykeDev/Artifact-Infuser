using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 0.1f)] private float m_tickIncrement = 0.01f;

    [SerializeField]
    [Range(0.1f, 1f)] private float m_minCraftingTime = 0.25f;
    [SerializeField]
    [Range(0.1f, 1f)] private float m_minInfusionTime = 0.25f;

    [SerializeField]
	private GameController m_gameController;
    
    
    private Coroutine m_craftingCoroutine = null;
	private Coroutine m_infusionCoroutine = null;
	private int m_selectedBlueprintID = -1;
	private int m_selectedCypherID = -1;


    public void Craft(int blueprintID, float time)
    {
    	m_selectedBlueprintID = blueprintID;
    	m_craftingCoroutine = StartCoroutine(Crafting(time, FinishCrafting));
    }

    public void StopCraft()
    {
    	StopCoroutine(m_craftingCoroutine);
    }

    public void FinishCrafting()
    {
    	if (m_craftingCoroutine == null) return;

    	// Check for invalid IDs
		if (m_selectedBlueprintID < 0)
        {
#if UNITY_EDITOR
            Debug.LogError("Invalid artifact ID found when crafting.");
#endif
            return;
        }

        StopCoroutine(m_craftingCoroutine);

        m_gameController.FinishCrafting(m_selectedBlueprintID);
    }


    // Crafting coroutine
    private IEnumerator Crafting(float time, Action callback)
    {
        if (time == 0) time = m_minCraftingTime;

    	for (float i = 0f; i < time; i += m_tickIncrement)
        {
    		m_gameController.UpdateCraftingProgress(i/time);

    		yield return new WaitForSeconds(m_tickIncrement);

    		// Callback when done
    		if (i >= time - m_tickIncrement)
            {
    			m_gameController.UpdateCraftingProgress(0);
                callback?.Invoke();
            }	
    	}
    }


    // ------------- //
   
    public void Infuse(int cypherID, float time)
    {
    	m_selectedCypherID = cypherID;
    	m_infusionCoroutine = StartCoroutine(Infusing(time, FinishInfusing));
    }

    public void StopInfusion()
    {
    	StopCoroutine(m_infusionCoroutine);
    }

    public void FinishInfusing()
    {
        if (m_infusionCoroutine == null) return;

    	// Check for invalid IDs
		if (m_selectedCypherID < 0)
        {
#if UNITY_EDITOR
            Debug.LogError("Invalid cypher ID found when infusing.");
#endif
            return;
        }

        StopCoroutine(m_infusionCoroutine);

        m_gameController.FinishInfusing(m_selectedCypherID);
    }


    // Crafting coroutine
    private IEnumerator Infusing(float time, Action callback)
    {
        if (time == 0) time = m_minInfusionTime;

        for (float i = 0f; i < time; i += m_tickIncrement)
        {
    		m_gameController.UpdateInfusionProgress(i/time);

    		yield return new WaitForSeconds(m_tickIncrement);

    		// Callback when done
    		if (i >= time - m_tickIncrement)
            { 
    			m_gameController.UpdateInfusionProgress(0);
                callback?.Invoke();
            }
    			
    	}
    }



}
