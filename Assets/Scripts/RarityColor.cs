using System.Collections.Generic;
using UnityEngine;

public class RarityColor : MonoBehaviour
{
    private static RarityColor Instance;
    private Dictionary<Rarity, Color> m_rarityColor;


    void Awake()
    {
        Instance = this;

        m_rarityColor = new Dictionary<Rarity, Color>
        {
            { Rarity.COMMON, new Color(0.9f, 0.9f, 0.9f) },
            { Rarity.UNCOMMON, new Color(0.137f, 0.707f, 0.707f) },
            { Rarity.RARE, new Color(0.09968f, 0.6037f, 0.09968f) },
            { Rarity.UNIQUE, new Color(0.3718f, 0.058f, 0.585f) },
            { Rarity.LEGENDARY, new Color(0.698f, 0.3444f, 0.056f) },
            { Rarity.ABYSSAL, new Color(1f, 0.2311f, 1f) },
            { Rarity.IMMORTAL, new Color(0.6698f, 0.1485f, 0.1485f) },
        };
    }

    public static Color GetColor(Rarity rarity)
    {
        // TODO fix this mess
        if (!Instance)
        {
            Dictionary<Rarity, Color> rarityColor = new Dictionary<Rarity, Color>
            {
                { Rarity.COMMON, new Color(0.9f, 0.9f, 0.9f) },
                { Rarity.UNCOMMON, new Color(0.137f, 0.707f, 0.707f) },
                { Rarity.RARE, new Color(0.09968f, 0.6037f, 0.09968f) },
                { Rarity.UNIQUE, new Color(0.3718f, 0.058f, 0.585f) },
                { Rarity.LEGENDARY, new Color(0.698f, 0.3444f, 0.056f) },
                { Rarity.ABYSSAL, new Color(1f, 0.2311f, 1f) },
                { Rarity.IMMORTAL, new Color(0.6698f, 0.1485f, 0.1485f) },
            };

            return rarityColor[rarity];
        }

        return Instance.m_rarityColor[rarity];
    }
}