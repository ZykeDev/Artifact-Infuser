using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] protected GameController m_gameController;
    [SerializeField] protected Autosell m_autosell;
    [SerializeField] protected DialogHandler m_dialog;

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


    /// <summary>
    /// Sells all the given artifacts
    /// </summary>
    /// <param name="artifacts"></param>
    public void Sell(List<Artifact> artifacts)
    {
        int numberOfArtifacts = artifacts.Count;

        if (numberOfArtifacts == 0) return;

        int total = 0;

        // Using .ToArray() to have a copy of the current artifacts before they are deleated
        foreach (Artifact art in artifacts.ToArray())
        {
            int goldGained = art.GetPrice();

            // TODO multiplty the gained gold by any available multiplier before selling?

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

    /// <summary>
    /// Sells the artifact selected in the shop
    /// </summary>
    public void Sell()
    {
        if (m_sellingArtifact != null)
        {
            string artifactName = m_sellingArtifact.GetName();
            int goldGained = m_sellingArtifact.GetPrice();

            // TODO multiplty the gained gold by any available multiplier before selling?

            m_gameController.RemoveArtifact(m_sellingArtifact);
            m_gameController.GainGold(goldGained);

            m_dialog.Sold(artifactName, goldGained);
            

            m_sellingArtifact = null;
        }
        
    }

    public void PromptSell(Artifact artifact)
    {
        m_sellingArtifact = artifact;

    }



    public void EnableAutosell() => m_autosell.Enable();
    public void DisableAutosell() => m_autosell.Disable();

}
