using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] private GameController m_gameController;
    [SerializeField] private Crafter m_crafter;
    [SerializeField] private Infuser m_infuser;
    [SerializeField] private Shop m_shop;

    [Header("Buttons")]
    [SerializeField] private GameObject m_craftBtn;
    [SerializeField] private GameObject m_stopCraftBtn;
    [SerializeField] private GameObject m_infuseBtn;
    [SerializeField] private GameObject m_stopInfuseBtn;
    [SerializeField] private GameObject m_gatherBtn;

    [SerializeField]
    private GameObject m_tooltipPrefab;

    private GameObject m_currentlyOpenedTooltip;
    private float m_screenHeight;

    private void Awake()
    {
        m_screenHeight = Screen.height;
    }

    #region Gathering

    /// <summary>
    /// Starts the Resource Gathering expedition
    /// </summary>
    public void OnGatherClick()
    {
        Button gatherBtn = m_gatherBtn.GetComponent<Button>();
        m_gameController.TryGathering(gatherBtn);
    }

    #endregion


    #region Crafting

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

    #endregion


    #region Infusion

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

    #endregion


    #region Armory

    public void OnArmoryCellClick(Artifact artifact)
    {
        m_shop.PromptSell(artifact);
    }

    #endregion

}