public class Request
{
    private int m_artifactID;
    private int m_reward;

    public string client = "Bob";
    public string artifactName = "Simple Sword";


    public Request(int artifactID, int reward)
    {
        m_artifactID = artifactID;
        m_reward = reward;
    }

    public Request(Rarity rarity, int baseReward)
    {
        (int artifactID, int reward) = GetRandomRequest(rarity, baseReward);

        m_artifactID = artifactID;
        m_reward = reward;
    }


    private (int, int) GetRandomRequest(Rarity rarity, int baseReward)
    {
        // TODO

        return (0, 0);
    }


    #region Getters

    public int GetArtifactID() => m_artifactID;
    public int GetReward() => m_reward;

    #endregion

}
