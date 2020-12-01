using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrade")]
public class UpgradeData : ScriptableObject {

    [SerializeField] public UpgradeData[] requirements;
    public Sprite sprite;
    public Effect effect;
    [TextArea] public string dex;
    
    [Header("Effects")]
    [SerializeField] private EffectType effectType;
    [SerializeField] private EffectBonus effectBonus;
    [SerializeField] private ResourceType effectResource;
    [SerializeField] private float modifier;
    [SerializeField] private UnlockFeature feature;


    [Header("Already Unlocked")]
    public bool unlocked = false;



    void Awake()
    {
        effect = new Effect(effectType, effectBonus, effectResource, modifier, feature);
    }
}
