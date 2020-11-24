using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] protected GameController m_gameController;
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



    public void Sell(Artifact sellingArtifact)
    {
        m_sellingArtifact = sellingArtifact;
        Sell();
    }

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
}
