﻿using System.Collections.Generic;
using UnityEngine;


public class AssistantSystem : MonoBehaviour
{
    [SerializeField] protected GameController m_gameController;
    [SerializeField] private GameObject m_rowPrefab;


    private List<(Assistant, int, bool)> m_assistants;
    private int m_assistantsNumber;


    public void Init(SaveData saveData)
    {
        m_assistants = new List<(Assistant, int, bool)>();
        //m_assistants = saveData.assistants;

        foreach ((Assistant assistant, int area, bool isWorking) in m_assistants)
        {
            // Display the assistant row
        }

    }

    public void Send()
    {
        Send(AddAssistant());
    }

    /// <summary>
    /// Sends the selected assistant to gather.
    /// </summary>
    /// <param name="assistant"></param>
    public void Send(Assistant assistant)
    {
        for (int i = 0; i < m_assistants.Count; i++)
        {
            if (m_assistants[i].Item1 == assistant)
            {
                bool isArea = m_gameController.IsAreaUnlocked(m_assistants[i].Item2);
                bool isFree = !m_assistants[i].Item3;

                if (isArea && isFree)
                {
                    m_assistants[i] = (assistant, m_assistants[i].Item2, true);
                    m_gameController.SendAssistant(m_assistants[i].Item1);
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log("Can't send assistant.");
#endif
                }
                
                break;
            }
        }

    }



    public Assistant AddAssistant()
    {
        Assistant newAssistant = new Assistant(m_assistantsNumber, "Lydia Smith");
        m_assistants.Add((newAssistant, 0, false));

        m_assistantsNumber = m_assistants.Count;

        return newAssistant;
    }    


    public void OnChange(Assistant assistant, int area, bool isWorking)
    {
        for (int i = 0; i < m_assistants.Count; i++)
        {
            if (m_assistants[i].Item1 == assistant)
            {
                m_assistants[i] = (assistant, area, isWorking);
                break;
            }
        }
    }
}



[System.Serializable]
public class Assistant
{
    public int id;
    public string name;
    public Sprite sprite;
    // TODO bonuses?

    public Assistant(int id, string name)
    {
        this.id = id;
        this.name = name;

        sprite = RandomSprite();
    }


    private Sprite RandomSprite()
    {
        return null;
    }
}