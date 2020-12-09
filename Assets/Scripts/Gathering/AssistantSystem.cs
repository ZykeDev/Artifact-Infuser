using System.Collections.Generic;
using UnityEngine;


public class AssistantSystem : MonoBehaviour
{
    private int m_assistantsNumber;

    private List<(Assistant, int, bool)> m_assistants;


    public void Init(SaveData saveData)
    {
        //m_assistants = saveData.assistants;

        foreach ((Assistant assistant, int area, bool isWorking) in m_assistants)
        {
            // Display the assistant row
        }

    }


    public void AddAssistant()
    {
        Assistant newAssistant = new Assistant(m_assistantsNumber);
        m_assistants.Add((newAssistant, 0, false));

        m_assistantsNumber = m_assistants.Count;
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

    public Assistant(int id)
    {
        this.id = id;
    }
}