﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private GameController m_gameController;
    [SerializeField] private Crafter m_crafter;
    [SerializeField] private Infuser m_infuser;

    // Instantiated buttons
    [SerializeField]
    private GameObject m_craftBtn, m_stopCraftBtn,
                       m_infuseBtn, m_stopInfuseBtn,
                       m_gatherBtn;

    [SerializeField]
    private GameObject m_tooltipPrefab;

    private GameObject m_currentlyOpenedTooltip;
    private float m_screenHeight;

    private void Awake()
    {
        m_screenHeight = Screen.height;
    }


    // ----------- Gather Button ----------- //

    /// <summary>
    /// Starts the Resource Gathering expedition
    /// </summary>
    public void OnGatherClick()
    {
        Button gatherBtn = m_gatherBtn.GetComponent<Button>();
        m_gameController.TryGathering(gatherBtn);
    }


    // ----------- Craft Button ----------- //

    /// <summary>
    /// Starts crafting the selected blueprint into an item
    /// </summary>
    public void OnCraftClick()
    {
        m_crafter.Craft();
    }

    public void OnStopCraftClick()
    {
        m_crafter.StopCraft();
    }

    
    public void OnSelectBlueprintClick(GameObject caller, int blueprintID)
    {
        // Forward the caller to remember which button was clicked
        m_crafter.SelectBlueprint(blueprintID, caller);
    }

    // Swap Craft -> Stop Crafting
    public void SawpCraftWithStop()
    {
        m_craftBtn.SetActive(false);
        m_stopCraftBtn.SetActive(true);
    } 
    
    // Swap Stop Crafting -> Craft
    public void SawpStopWithCraft()
    {
        m_stopCraftBtn.SetActive(false);
        m_craftBtn.SetActive(true);
    }

    // Makes the Craft Button clickable
    public void ActivateCraftBtn()
    {
        m_craftBtn.GetComponent<Button>().interactable = true;
    }


    // ----------- Infuse Button ----------- //

    /// <summary>
    /// Starts infusing the selected cypher into an item
    /// </summary>
    public void OnInfuseClick()
    {
        m_infuser.Infuse();
    }

    public void OnStopInfusionClick()
    {
        m_infuser.StopInfusion();
    }

    /// <summary>
    /// Sets the clicked cypher as "Selected"
    /// </summary>
    /// <param name="cypherID"></param>
    public void OnSelectCypherClick(int cypherID)
    {
        m_infuser.SelectCypher(cypherID);    
        
    }

    // Swap Craft -> Stop Crafting
    public void SawpInfuseWithStop() 
    {
        m_infuseBtn.SetActive(false);
        m_stopInfuseBtn.SetActive(true);
    } 
    
    // Swap Stop Crafting -> Craft
    public void SawpStopWithInfuse()
    {
        m_stopInfuseBtn.SetActive(false);
        m_infuseBtn.SetActive(true);
    }

    // Makes the Craft Button clickable
    public void ActivateInfuseBtn()
    {
        m_infuseBtn.GetComponent<Button>().interactable = true;
    }

}