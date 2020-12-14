using System;
using System.Collections.Generic;
using UnityEngine;


public class AssistantSystem : MonoBehaviour
{
    [SerializeField] protected GameController m_gameController;
    [SerializeField] private GameObject m_rowPrefab;

    public List<Assistant> assistants;
    private int m_assistantsNumber;

    [SerializeField] private List<GameObject> m_slots;
    private List<AssistantRow> m_rows;


    public void Init(SaveData saveData)
    {
        m_rows = new List<AssistantRow>();

        // If the save contains assistants, use those
        if (saveData != null && saveData.assistants != null)
        {
            assistants = saveData.assistants;
        }
        else
        {
            // Otherwise create a new list
            assistants = new List<Assistant>();
            AddAssistant();
        }

        // Fill a slot with each assistant row
        for (int i = 0; i < assistants.Count; i++)
        {
            // TODO move this to its own function
            GameObject rowObj = Instantiate(m_rowPrefab, m_slots[i].transform);
            rowObj.name = assistants[i].Name;

            RectTransform rowRT = rowObj.GetComponent<RectTransform>();
            rowRT.SetLeft(0);
            rowRT.SetRight(0);
            rowRT.SetTop(0);
            rowRT.SetBottom(0);

            AssistantRow row = rowObj.GetComponent<AssistantRow>();
            row.Set(this, assistants[i]);
            m_rows.Add(row);
        }
        
    }


    /// <summary>
    /// Sends the selected assistant to gather. Returns true if it successfuly sends them
    /// </summary>
    /// <param name="assistant"></param>
    public bool Send(Assistant assistant)
    {
        bool isArea = m_gameController.IsAreaUnlocked(assistant.area);
        bool isFree = !assistant.isWorking;

        if (isArea && isFree)
        {
            m_gameController.SendAssistant(assistant);

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

    /// <summary>
    /// Sends the given assistant to gather after loading up a save
    /// </summary>
    /// <param name="assistant"></param>
    /// <returns></returns>
    public bool Resume(Assistant assistant)
    {
        if (assistant.isWorking)
        {
            m_gameController.SendAssistant(assistant);
            return true;
        }

        return false;   
    }



    public Assistant AddAssistant()
    {
        // TODO check there are enough slots?
        Assistant newAssistant = new Assistant(m_assistantsNumber, "Lydia Smith");
        assistants.Add(newAssistant);

        m_assistantsNumber = assistants.Count;

        return newAssistant;
    }


    public void UpdateAssistant(Assistant assistant, int area, bool isRepeat)
    { 
        for (int i = 0; i < assistants.Count; i++)
        {
            if (assistants[i] == assistant)
            {
                assistant.repeat = isRepeat;
                assistants[i] = assistant;
                break;
            }
        }
    }


    public void Return(Assistant assistant, int area)
    {
        assistant.isWorking = false;
        UpdateAssistant(assistant, area, false);
        GetRow(assistant)?.Return();
    }

    public void UpdateRows()
    {
        for (int i = 0; i < m_rows.Count; i++)
        {
            m_rows[i].UpdateDropdown();
        }
    }

    /// <summary>
    /// Returns the AssistantRow the given Assistant is in
    /// </summary>
    /// <param name="assistant"></param>
    /// <returns></returns>
    private AssistantRow GetRow(Assistant assistant)
    {
        for (int i = 0; i < m_rows.Count; i++)
        {
            if (m_rows[i].assistant == assistant)
            {
                return m_rows[i];
            }
        }

        return null;
    }

}



[Serializable]
public class Assistant
{
    public int ID { get; private set; }
    public string Name { get; private set; }
    public int SpriteID { get; private set; }
    [NonSerialized] public Sprite sprite;

    public int area;
    public bool isWorking = false;
    public bool repeat = false;
    // TODO bonuses?

    public Assistant(int id, string name)
    {
        ID = id;
        Name = name;

        SpriteID = RandomSprite();
        sprite = GetSprite(SpriteID);
    }

    public bool Send()
    {
        if (isWorking)
        {
            return false;
        }

        isWorking = true;
        return true;
    }


    private int RandomSprite()
    {
        return UnityEngine.Random.Range(0, 10);
    }

    // TODO
    private Sprite GetSprite(int id)
    {
        return null;
    }
}