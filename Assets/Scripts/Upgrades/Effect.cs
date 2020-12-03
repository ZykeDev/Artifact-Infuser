using UnityEngine;

public class Effect
{
    private EffectType m_type;
    private EffectBonus m_bonus;
    private float m_modifier;

    private ResourceType m_resourceType;
    private UnlockFeature m_feature;


    public Effect(EffectType effectType, EffectBonus effectBonus, ResourceType effectResource, float modifier, UnlockFeature feature)
    {
        m_type = effectType;

        switch (effectType)
            {
                case EffectType.RESOURCES:
                    ResourceEffect(effectResource, effectBonus, modifier);
                    break;

                case EffectType.GOLD:
                    GoldEffect(effectType, effectBonus, modifier);
                    break;

                case EffectType.TIME:
                    TimeEffect(effectBonus, modifier);
                    break;

                case EffectType.UNLOCK:
                    FeatureEffect(feature);
                    break;

                default:
                    break;
            }
    }

    public Effect(EffectType effectType, EffectBonus effectBonus, ResourceType effectResource, int modifier, UnlockFeature feature)
    {
        m_type = effectType;

        switch (effectType)
        {
            case EffectType.RESOURCES:
                ResourceEffect(effectResource, effectBonus, modifier);
                break;

            case EffectType.GOLD:
                GoldEffect(effectType, effectBonus, modifier);
                break;

            case EffectType.TIME:
                TimeEffect(effectBonus, modifier);
                break;

            case EffectType.UNLOCK:
                FeatureEffect(feature);
                break;

            default:
                break;
        }
    }


    #region Overloads

    public Effect(ResourceType resourceType, EffectBonus effectBonus, float modifier) => ResourceEffect(resourceType, effectBonus, modifier);
    public Effect(EffectType effectType, EffectBonus effectBonus, int modifier) => GoldEffect(effectType, effectBonus, modifier);
    public Effect(EffectType effectType, EffectBonus effectBonus, float modifier) => GoldEffect(effectType, effectBonus, modifier);
    public Effect(EffectBonus effectBonus, float modifier) => TimeEffect(effectBonus, modifier);
    public Effect(UnlockFeature feature) => FeatureEffect(feature);

    #endregion


    #region Constructor Helpers

    public void ResourceEffect(ResourceType resourceType, EffectBonus effectBonus, float modifier) 
    {
        m_bonus = effectBonus;
        m_modifier = modifier;
        m_resourceType = resourceType;
    }

    public void GoldEffect(EffectType effectType, EffectBonus effectBonus, int modifier)
    {
        m_bonus = effectBonus;
        m_modifier = modifier;
    }
    public void GoldEffect(EffectType effectType, EffectBonus effectBonus, float modifier)
    {
        m_bonus = effectBonus;
        m_modifier = modifier;
    }

    public void TimeEffect(EffectBonus effectBonus, float modifier)
    {
        m_bonus = effectBonus;
        m_modifier = modifier;
    }

    public void FeatureEffect(UnlockFeature feature)
    {
        m_feature = feature;
    }

    #endregion


    #region Applications

    /// <summary>
    /// Applies the effect to every resource in the inventory
    /// </summary>
    /// <param name="inv"></param>
    /// <returns></returns>
    public Inventory Apply(Inventory inv)
    {
        Apply(inv.wood);
        Apply(inv.metal);
        Apply(inv.leather);
        Apply(inv.crystals);
        
        return inv;
    }


    /// <summary>
    /// (Gold) Applies the effect to the given gold value and returns it
    /// </summary>
    /// <param name="gold"></param>
    /// <returns></returns>
    public int Apply(int gold)
    {        
        if (m_type == EffectType.GOLD)
        {
            if (m_bonus == EffectBonus.PLUS)
            {
                gold += (int)m_modifier;
            }
            else if (m_bonus == EffectBonus.TIMES)
            {
                gold = (int)Mathf.Round(gold * m_modifier);
            }
        }

        return gold;
    }
    

    /// <summary>
    /// (Resource) Applies the effect to the given resource and returns it
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    public Resource Apply(Resource resource) 
    {
        if (m_type == EffectType.RESOURCES)
        {
            if (resource.GetResourceType() == m_resourceType)
            {
                if (m_bonus == EffectBonus.PLUS)
                {
                    resource.Add(resource.amount + m_modifier);
                }
                else if (m_bonus == EffectBonus.TIMES)
                {
                    resource.Add(resource.amount * m_modifier);
                }
            }
        }

        return resource;
    }


    /// <summary>
    /// (Time) Applies the effect to the given value
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public float Apply(float time)
    {
        if (m_type == EffectType.TIME)
        {
            if (m_bonus == EffectBonus.PLUS)
            {
                return time + m_modifier;
            }
            else if (m_bonus == EffectBonus.TIMES)
            {
                return time * m_modifier;
            }
        }

        return time;
    }

    #endregion


    #region Getters

    public EffectType GetEffectType() => m_type;

    #endregion
}