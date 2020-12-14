using System.Collections.Generic;
using UnityEngine;

public class MapOverlay : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] private UnlockSystem m_unlockSystem;
    [SerializeField] private Gathering m_gathering;

    [Header("Areas")]
    [SerializeField] protected GameObject m_areaWoodsObj;
    [SerializeField] protected GameObject m_areaRiverObj;
    [SerializeField] protected GameObject m_areaBeachObj;
    [SerializeField] protected GameObject m_areaVillageObj;
    [SerializeField] protected GameObject m_areaMineObj;

    private List<MapArea> m_areas;
  
    void Awake()
    {
        SetupAreas();
        UpdateAreas();
    }


    /// <summary>
    /// Adds the areas to the m_areas list
    /// </summary>
    private void SetupAreas()
    {
        m_areas = new List<MapArea>();

        // Areas are added in sequence to keep their order
        m_areas.Add(m_areaWoodsObj.GetComponent<MapArea>());
        m_areas.Add(m_areaRiverObj.GetComponent<MapArea>());
        m_areas.Add(m_areaBeachObj.GetComponent<MapArea>());
        m_areas.Add(m_areaVillageObj.GetComponent<MapArea>());
        m_areas.Add(m_areaMineObj.GetComponent<MapArea>());
    }


    /// <summary>
    /// Updates the lock state of all areas
    /// </summary>
    public void UpdateAreas()
    {
        // Make sure the areas have been setup
        if (m_areas == null || m_areas.Count == 0)
        {
            SetupAreas();
        } 

        for (int i = 0; i < m_areas.Count; i++)
        {
            if (i < m_unlockSystem.unlockedAreas.Length)
            {
                m_areas[i].SetLock(i, m_unlockSystem.unlockedAreas[i]);
            }
        }
    }
    


    public void Select(int index)
    {
        // Deselect every other area
        for (int i = 0; i < m_areas.Count; i++)
        {
            if (i != index)
            {
                m_areas[i].SetSelected(false);
            }
        }

        // Set the gathering area
        m_gathering.SetArea(m_areas[index]);
    }

    public List<MapArea> GetActiveAreas()
    {
        List<MapArea> activeAreas = new List<MapArea>();

        foreach (MapArea area in m_areas)
        {
            if (area.IsUnlocked())
            {
                activeAreas.Add(area);
            }
        }

        return activeAreas;
    }

    public List<MapArea> GetAreas() => m_areas;

}
