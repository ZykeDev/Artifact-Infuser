using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] protected GameController m_gameController;
    [SerializeField] protected Autosell m_autosell;
    [SerializeField] protected DialogHandler m_dialog;
    [SerializeField] protected Upgrades m_upgrades;

    private Artifact m_sellingArtifact;


    void Awake()
    {
        if (m_gameController == null)
        {
            m_gameController = FindObjectOfType<GameController>();
        }

        if (m_dialog == null)
        {
            m_dialog = FindObjectOfType<DialogHandler>();
        }
    }


    #region Dialog 

    /// <summary>
    /// Sells all the given artifacts
    /// </summary>
    /// <param name="artifacts"></param>
    public void Sell(List<Artifact> artifacts)
    {
        if (artifacts == null || artifacts.Count == 0) return;

        int numberOfArtifacts = artifacts.Count;
        int total = 0;

        // Using .ToArray() to have a copy of the current artifacts before they are deleated
        foreach (Artifact art in artifacts.ToArray())
        {
            int goldGained = art.GetPrice();

            goldGained = m_upgrades.ApplyBonuses(goldGained);

            total += goldGained;

            m_gameController.RemoveArtifact(art);
            m_gameController.GainGold(goldGained);
        }

        m_dialog.AddLine(DialogType.SELL, "Sold " + numberOfArtifacts + " artifacts for a total of " + total + " gold.");
    }

    /// <summary>
    /// Sells the given artifact
    /// </summary>
    /// <param name="artifact"></param>
    public void Sell(Artifact artifact)
    {
        m_sellingArtifact = artifact;
        Sell();
    }

    public void Sell(Artifact artifact, int reward)
    {
        if (artifact != null)
        {
            string artifactName = artifact.GetName();
            int goldGained = reward;
            
            goldGained = m_upgrades.ApplyBonuses(goldGained);

            m_gameController.RemoveArtifact(artifact);
            m_gameController.GainGold(goldGained);

            m_dialog.Sold(artifactName, goldGained);
        }
    }

    /// <summary>
    /// Sells the artifact selected in the shop
    /// </summary>
    public void Sell()
    {
        if (m_sellingArtifact != null)
        {
            string artifactName = m_sellingArtifact.GetName();
            int goldGained = m_sellingArtifact.GetPrice();
            
            goldGained = m_upgrades.ApplyBonuses(goldGained);
            
            m_gameController.RemoveArtifact(m_sellingArtifact);
            m_gameController.GainGold(goldGained);

            m_dialog.Sold(artifactName, goldGained);
            

            m_sellingArtifact = null;
        }
        
    }

    public void PromptSell(Artifact artifact)
    {
        m_sellingArtifact = artifact;
        print("sellintgArt has been set");
    }

    public void AddNewline() => m_dialog.AddNewline();

    public void NewRequest(Request request) => m_dialog.AddLine(DialogType.REQUEST, request);
    

    public void NewDialogue(DialogType type, string line) => m_dialog.AddLine(type, line);

    #endregion


    #region Autosell

    public void EnableAutosell() => m_autosell.Enable();
    public void DisableAutosell() => m_autosell.Disable();

    #endregion


    #region Getters

    public Artifact GetSellingArtifact() => m_sellingArtifact;

    #endregion

}
