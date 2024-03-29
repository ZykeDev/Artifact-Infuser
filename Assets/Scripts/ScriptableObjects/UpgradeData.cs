﻿using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrade")]
public class UpgradeData : ScriptableObject {

    [SerializeField] public Sprite sprite;
    [SerializeField] public int ID;

    [Header("Upgrade costs")]
    [SerializeField] public UpgradeData[] requirements;
    [Header("")]
    [SerializeField] [Min(0)] public int gold; // Gold is public
    [SerializeField] [Min(0)] protected int wood, metal, leather, crystals;
    [SerializeField] [Min(0)] protected int alpha, nova, prisma;

    [Header("Effects")]
    [TextArea] public string dex;
    [SerializeField] protected EffectType effectType;
    [SerializeField] protected EffectBonus effectBonus;
    [SerializeField] protected ResourceType effectResource;
    [SerializeField] protected float modifier;
    [SerializeField] protected UnlockFeature feature;


    [Header("Already Unlocked")]
    public bool unlocked = false;


    private Effect effect;
    [NonSerialized] public RequiredResources requiredResources;
    [NonSerialized] public RequiredRunes requiredRunes;


    public void Init()
    {
        effect = new Effect(effectType, effectBonus, effectResource, modifier, feature);
        requiredResources = new RequiredResources(wood, metal, leather, crystals);
        requiredRunes = new RequiredRunes(alpha, nova, prisma);
    }

    public Effect GetEffect() => effect;
}
