using System;
using System.Collections;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    #region Vars

    [SerializeField, Tooltip("Time between each process tick.")]
    [Range(0.01f, 0.1f)] private float m_tickIncrement = 0.01f;

    [SerializeField]
    [Range(0.1f, 1f)] private float m_minGatheringTime = 0.25f;
    [SerializeField]
    [Range(0.1f, 1f)] private float m_minCraftingTime = 0.25f;
    [SerializeField]
    [Range(0.1f, 1f)] private float m_minInfusionTime = 0.25f;

    [SerializeField]
	private GameController m_gameController;
    
    
    private Coroutine m_craftingCoroutine = null;
	private Coroutine m_infusionCoroutine = null;
    private Coroutine m_gatheringCoroutine = null;

    private int m_gatheringTier = 1;
    private int m_selectedBlueprintID = -1;
	private int m_selectedCypherID = -1;


    #endregion

    #region Gather

    public void Gather(int tier, float time)
    {
        m_gatheringCoroutine = StartCoroutine(Gather(time, delegate { FinishGathering(tier); }));
    }

    public void StopGathering() => StopCoroutine(m_gatheringCoroutine);

    private void FinishGathering(int tier)
    {
        StopGathering();
        m_gameController.FinishGathering(tier);
    }

    private IEnumerator Gather(float time, Action callback)
    {
        if (time == 0) time = m_minGatheringTime;

        for (float i = 0f; i < time; i += m_tickIncrement)
        {
            m_gameController.UpdateGatheringProgress(i / time);

            yield return new WaitForSeconds(m_tickIncrement);

            // Callback when done
            if (i >= time - m_tickIncrement)
            {
                m_gameController.UpdateGatheringProgress(0);
                callback?.Invoke();
            }
        }
    }

    #endregion


    #region Craft

    public void Craft(int blueprintID, float time)
    {
    	m_selectedBlueprintID = blueprintID;
    	m_craftingCoroutine = StartCoroutine(Crafting(time, FinishCrafting));
    }

    public void StopCraft() => StopCoroutine(m_craftingCoroutine);
    

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


    #endregion


    #region Infuse

    public void Infuse(int cypherID, Artifact baseArtifact, float time)
    {
        m_selectedCypherID = cypherID;
        m_infusionCoroutine = StartCoroutine(Infusing(time, delegate
        {
            FinishInfusing(baseArtifact);
        }));
    }

    public void StopInfusion()
    {
    	StopCoroutine(m_infusionCoroutine);
    }

    public void FinishInfusing(Artifact baseArtifact)
    {
        //if (m_infusionCoroutine == null) return;
        if (baseArtifact == null)
        {
#if UNITY_EDITOR
            Debug.LogError("After infusing, the base artifact has been found to be null.");
#endif
            return;
        }

    	// Check for invalid IDs
		if (m_selectedCypherID < 0)
        {
#if UNITY_EDITOR
            Debug.LogError("Invalid cypher ID found when infusing.");
#endif
            return;
        }

        StopCoroutine(m_infusionCoroutine);

        m_gameController.FinishInfusing(m_selectedCypherID, baseArtifact);
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

    #endregion


}
