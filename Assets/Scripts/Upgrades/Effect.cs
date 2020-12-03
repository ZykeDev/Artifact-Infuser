using UnityEngine;

public class Effect
{
    private EffectType m_type;
    private EffectBonus m_bonus;
    private float m_modifier;

    public ResourceType m_resourceType; // todo make private
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
                    GoldEffect(effectBonus, (int)modifier);
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
    public Effect(EffectBonus effectBonus, int modifier) => GoldEffect(effectBonus, modifier);
    public Effect(EffectBonus effectBonus, float modifier) => TimeEffect(effectBonus, modifier);
    public Effect(UnlockFeature feature) => FeatureEffect(feature);

    #endregion


    public void ResourceEffect(ResourceType resourceType, EffectBonus effectBonus, float modifier) 
    {
        m_bonus = effectBonus;
        m_modifier = modifier;
        m_resourceType = resourceType;
    }

    public void GoldEffect(EffectBonus effectBonus, int modifier)
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



    public EffectType GetEffectType() => m_type;



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
                return gold + (int)m_modifier;
            }
            else if (m_bonus == EffectBonus.TIMES)
            {
                return (int)Mathf.Floor(gold * m_modifier);
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
                return Mathf.Floor(time * m_modifier);
            }
        }

        return time;
    }
}