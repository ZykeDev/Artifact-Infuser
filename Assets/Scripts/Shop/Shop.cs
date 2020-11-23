using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] protected GameController m_gameController;

    private Artifact m_sellingArtifact;


    void Awake()
    {
        if (m_gameController == null)
        {
            m_gameController = FindObjectOfType<GameController>();
        }
    }

    public void Sell()
    {
        if (m_sellingArtifact != null)
        {
            int goldGained = m_sellingArtifact.GetPrice();

            // TODO multiplty the gained gold by any available multiplier before selling?

            m_gameController.RemoveArtifact(m_sellingArtifact);
            m_gameController.GainGold(goldGained);

            m_sellingArtifact = null;
        }
        
    }




    public void PromptSell(Artifact artifact)
    {
        m_sellingArtifact = artifact;

    }
}
