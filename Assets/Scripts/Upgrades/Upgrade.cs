using System.Collections.Generic;
using UnityEngine;

public class Upgrade
{
    private List<UpgradeData> m_requirements;
    private Sprite m_sprite;
    private Effect m_effect;
    [SerializeField, TextArea] private string m_dex;
    private string m_name;

    private bool m_unlocked = false;
    private bool m_bought = false;

    private UpgradeData m_originalData;



    public Upgrade(UpgradeData data)
    {
        m_requirements = data.requirements;
        m_sprite = data.sprite;
        m_unlocked = data.unlocked;
        m_dex = data.dex;
        m_effect = data.effect;
        m_name = data.name;

        m_originalData = data;

        m_bought = false;
    }


    public void Unlock() => m_unlocked = true;
    public void Buy() => m_bought = true;
    public bool GetUnlocked() => m_unlocked;
    public bool GetBought() => m_bought;
    public Sprite GetSprite() => m_sprite;
    public string GetDex() => m_dex;
    public string GetName() => m_name;
    public List<UpgradeData> GetRequirements() => m_requirements;

    public UpgradeData GetOriginal() => m_originalData;
}