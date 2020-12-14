using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gathering : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] protected GameController m_gameController;
    [SerializeField] protected BackgroundManager m_backgroundManager;
    [SerializeField] protected ButtonHandler m_buttonHandler;
    [SerializeField] protected MapHandler m_mapHandler;
    [SerializeField] protected AssistantSystem m_assistantSystem;

    [Header("UI Components")]
    [SerializeField] private Slider m_progressbar;

    [SerializeField]
    [Range(1f, 60f)] private float m_gatheringDuration = 2f;
    
    private GatheringState m_gatheringState;
    private enum GatheringState { IDLE, GATHERING }

    private Coroutine m_gatheringCoroutine = null;

    private int m_tier = 0;
    private MapArea m_selectedArea;

    public void Awake()
    {
        
    }

    public void Init(SaveData saveData)
    {
        m_tier = 0;
        m_assistantSystem.Init(saveData);
    }

    public void SetArea(MapArea area)
    {
        m_selectedArea = area;
        m_buttonHandler.EnableGather();
    }



    #region Gathering


    /// <summary>
    /// Starts the gathering process
    /// </summary>
    public void Gather()
    {
        if (m_gatheringState == GatheringState.GATHERING) return;
        
        m_gatheringState = GatheringState.GATHERING;

        m_buttonHandler.SawpGatherWithStop();

        m_progressbar.value = 0;

        m_gameController.Gather(m_tier, m_selectedArea.GetTime());
    }


    /// <summary>
    /// Updates the progress bar
    /// </summary>
    /// <param name="progress"></param>
    public void UpdateGatheringProgress(float progress)
    {
        m_progressbar.value = progress;
    }

    /// <summary>
    /// Stops the gathering process
    /// </summary>
    public void StopGather()
    {
        m_gatheringState = GatheringState.IDLE;

        m_gameController.StopGather();

        UpdateGatheringProgress(0);

        m_buttonHandler.SawpStopWithGather();
    }


    /// <summary>
    /// Stops gathering resources and adds them to the inventory
    /// </summary>
    public void FinishGathering(int tier)
    {
        m_gatheringState = GatheringState.IDLE;

        UpdateGatheringProgress(0);

        m_buttonHandler.SawpStopWithGather();

        // Add the new resources to the inventory
        Inventory booty = new Inventory();
        booty.SetRandomResources(tier);

        m_gameController.AddResources(booty);
    }

    public List<MapArea> GetAreas() => m_mapHandler.GetAreas();

    #endregion


    public void UpdateAreas()
    {
        m_mapHandler.UpdateAreas();
        m_assistantSystem.UpdateRows();
    }
    
    public void UnlockAssistant()
    {
        // TODO
        //m_assistantSystem.Unlock();
    }


    #region Assistants

    public void SendAssistant(Assistant assistant)
    {
        float time = GetAreas()[assistant.area].GetTime();

        m_backgroundManager.SendAssistant(assistant, time);
    }

    public void AssistantReturn(Assistant assistant)
    {
        // Add the new resources to the inventory
        Inventory booty = new Inventory();
        booty.SetRandomResources(assistant.area);

        m_gameController.AddResources(booty);

        if (!assistant.repeat)
        {
            m_assistantSystem.Return(assistant, assistant.area);
        }
        
    }

    #endregion
}
