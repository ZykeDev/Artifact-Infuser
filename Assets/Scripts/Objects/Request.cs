using UnityEngine;

public class Request
{
    private int m_artifactID;
    private int m_reward;

    public string client = "Client Name";
    public string artifactName = "Artifact Name";

    private float m_minRewardFluct = 0.6f;
    private float m_maxRewardFluct = 1.2f;


    public Request(int artifactID, int reward)
    {
        m_artifactID = artifactID;
        m_reward = reward;
    }

    public Request(Rarity rarity, int baseReward)
    {
        (Artifact artifact, int reward) = GetRandomRequirements(rarity, baseReward);

        m_artifactID = artifact.GetArtifactID();
        m_reward = reward;
        artifactName = artifact.GetName();
        client = GetRandomClient();
    }


    private (Artifact, int) GetRandomRequirements(Rarity rarity, int baseReward)
    {
        int multiplier = 1 + (int)rarity;
        int reward = (int)(baseReward * multiplier * Random.Range(m_minRewardFluct, m_maxRewardFluct));

        Blueprint blueprint = BlueprintDatabase.GetBlueprint(rarity);
        Artifact artifact = new Artifact(blueprint);


        return (artifact, reward);
    }

    // TODO
    private string GetRandomClient()
    {
        return "Bob";
    }


    #region Getters

    public int GetArtifactID() => m_artifactID;
    public int GetReward() => m_reward;

    #endregion

}
