using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrade")]
public class UpgradeData : ScriptableObject {

    [SerializeField] public Sprite sprite;
    [SerializeField] public int ID;

    [Header("Upgrade costs")]
    [SerializeField] public UpgradeData[] requirements;
    [Header("")]
    [SerializeField] [Min(0)] public int gold; // Gold is public
    [SerializeField] [Min(0)] private int wood, metal, leather, crystals;
    [SerializeField] [Min(0)] private int alpha, nova, prisma;

    [Header("Effects")]
    [TextArea] public string dex;
    [SerializeField] private EffectType effectType;
    [SerializeField] private EffectBonus effectBonus;
    [SerializeField] private ResourceType effectResource;
    [SerializeField] private float modifier;
    [SerializeField] private UnlockFeature feature;


    [Header("Already Unlocked")]
    public bool unlocked = false;


    private Effect effect;
    public RequiredResources requiredResources;
    public RequiredRunes requiredRunes;


    public void Init()
    {
        effect = new Effect(effectType, effectBonus, effectResource, modifier, feature);
        requiredResources = new RequiredResources(wood, metal, leather, crystals);
        requiredRunes = new RequiredRunes(alpha, nova, prisma);
    }

    public Effect GetEffect() => effect;
}
