using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Upgrade
{
    private Sprite m_sprite;
    private Effect m_effect;
    [SerializeField, TextArea] private string m_dex;
    private string m_name;

    private bool m_unlocked = false;
    private bool m_bought = false;

    private List<UpgradeData> m_requirements;
    private int m_gold;
    private RequiredResources m_requiredResources;
    private RequiredRunes m_requiredRunes;

    private UpgradeData m_originalData;



    public Upgrade(UpgradeData data)
    {
        data.Init();
        m_originalData = data;

        m_requirements = data.requirements.ToList();
        m_sprite = data.sprite;
        m_unlocked = data.unlocked;
        m_dex = data.dex;
        m_effect = data.GetEffect();
        m_name = data.name;

        m_gold = data.gold;
        m_requiredResources = data.requiredResources;
        m_requiredRunes = data.requiredRunes;


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
    public int GetGold() => m_gold;
    public UpgradeData GetOriginal() => m_originalData;
    public Effect GetEffect() => m_effect;
    public RequiredResources GetRequiredResources() => m_requiredResources;
    public RequiredRunes GetRequiredRunes() => m_requiredRunes;
}