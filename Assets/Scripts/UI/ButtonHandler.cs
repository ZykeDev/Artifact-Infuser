using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] private GameController m_gameController;
    [SerializeField] private Gathering m_gathering;
    [SerializeField] private Crafter m_crafter;
    [SerializeField] private Infuser m_infuser;
    [SerializeField] private Shop m_shop;

    [Header("Buttons")]
    [SerializeField] private GameObject m_gatherBtn;
    [SerializeField] private GameObject m_stopGatherBtn;
    
    [SerializeField] private GameObject m_craftBtn;
    [SerializeField] private GameObject m_stopCraftBtn;

    [SerializeField] private GameObject m_infuseBtn;
    [SerializeField] private GameObject m_stopInfuseBtn;

    [SerializeField] private GameObject m_autosellBtn;
    [SerializeField] private GameObject m_stopAutosellBtn;


    [Header("Prompt System Reference")]
    [SerializeField] private PromptSystem m_promptSystem;


    private bool m_isWaitingForConfirm = false;



    void Update()
    {
        if (m_isWaitingForConfirm)
        {
            // Make sure the prompt system exists
            if (m_promptSystem == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("PromptSystem reference is missing from the ButtonHandler. Seaching one with Find.");
#endif
                m_promptSystem = FindObjectOfType<PromptSystem>();

                if (m_promptSystem == null)
                {
#if UNITY_EDITOR
                    Debug.LogError("PromptSystem reference could not be found.");
#endif
                    m_isWaitingForConfirm = false;
                    return;
                }
            }

            if (m_promptSystem.isConfirm)
            {
                m_isWaitingForConfirm = false;
                m_promptSystem.ResetConfirm();
                m_gameController.Sell();
            }
        }
    }



    #region Gathering

    /// <summary>
    /// Starts the Resource Gathering expedition
    /// </summary>
    public void OnGatherClick()
    {
        m_gathering.Gather();
    }

    public void OnStopGatherClick()
    {
        m_gathering.StopGather();
    }


    public void EnableGather()
    {
        m_gatherBtn.GetComponent<Button>().interactable = true;
    }

    public void SawpGatherWithStop()
    {
        m_gatherBtn.SetActive(false);
        m_stopGatherBtn.SetActive(true);
    }

    public void SawpStopWithGather()
    {
        m_stopGatherBtn.SetActive(false);
        m_gatherBtn.SetActive(true);
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
    public void EnableCraftBtn() => m_craftBtn.GetComponent<Button>().interactable = true;
    
    public void DisableCraftBtn() => m_craftBtn.GetComponent<Button>().interactable = false;

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
        // Prepare the shop to sell the artifact
        m_shop.PromptSell(artifact);

        // Make the ButtonHandler wait for confirmation
        m_isWaitingForConfirm = true;
    }

    #endregion


    #region Autoselling

    public void EnableAutosell()
    {
        m_shop.EnableAutosell();
        SawpAutosellWithStop();
    }

    public void DisableAutosell()
    {
        m_shop.DisableAutosell();
        SawpStopWithAutosell();
    }




    public void SawpAutosellWithStop()
    {
        m_autosellBtn.SetActive(false);
        m_stopAutosellBtn.SetActive(true);
    }

    public void SawpStopWithAutosell()
    {
        m_stopAutosellBtn.SetActive(false);
        m_autosellBtn.SetActive(true);
    }

    #endregion

}