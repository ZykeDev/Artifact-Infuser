using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    [SerializeField] private MapOverlay m_mapOverlay;

    void Awake()
    {
        if (m_mapOverlay == null)
        {
            m_mapOverlay = FindObjectOfType<MapOverlay>();
        }
    }

    public void UpdateAreas() => m_mapOverlay.UpdateAreas();
    public List<MapArea> GetAreas() => m_mapOverlay.GetAreas();
}
