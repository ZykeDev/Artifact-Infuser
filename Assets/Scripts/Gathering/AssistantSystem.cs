using System;
using System.Collections.Generic;
using UnityEngine;


public class AssistantSystem : MonoBehaviour
{
    [SerializeField] protected GameController m_gameController;
    [SerializeField] private GameObject m_rowPrefab;
    [SerializeField] private AssistantRow row; // TODO make into a list


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

        Assistant a = AddAssistant();
        row.Set(this, a);
    }


    /// <summary>
    /// Sends the selected assistant to gather. Returns true if it successfuly sends them
    /// </summary>
    /// <param name="assistant"></param>
    public bool Send(Assistant assistant)
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

                    return true;
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log("Can't send assistant.");
#endif
                    return false;
                }
            }
        }

        return false;
    }



    public Assistant AddAssistant()
    {
        Assistant newAssistant = new Assistant(m_assistantsNumber, "Lydia Smith", false);
        m_assistants.Add((newAssistant, 0, false));

        m_assistantsNumber = m_assistants.Count;

        return newAssistant;
    }


    public void UpdateAssistant(Assistant assistant, int area, bool isRepeat)
    {
        for (int i = 0; i < m_assistants.Count; i++)
        {
            if (m_assistants[i].Item1 == assistant)
            {
                assistant.repeat = isRepeat;
                m_assistants[i] = (assistant, area, false);
                break;
            }
        }
    }


    public void Return(Assistant assistant, int area)
    {
        UpdateAssistant(assistant, area, false);
        row.Return();
    }

    public void UpdateRows()
    {
        // TODO foreach row
        row.UpdateDropdown();
    }

}



[Serializable]
public class Assistant
{
    public int id;
    public string name;
    public Sprite sprite;

    public bool repeat;
    // TODO bonuses?

    public Assistant(int id, string name, bool repeat)
    {
        this.id = id;
        this.name = name;
        this.repeat = repeat;

        sprite = RandomSprite();
    }


    // TODO
    private Sprite RandomSprite()
    {
        return null;
    }
}