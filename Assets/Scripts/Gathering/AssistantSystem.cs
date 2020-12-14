using System;
using System.Collections.Generic;
using UnityEngine;

public class AssistantSystem : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] protected GameController m_gameController;

    [Header("UI Components")]
    [SerializeField] private GameObject m_button;
    [SerializeField] private GameObject m_rowPrefab;

    [Space(10)]
    [SerializeField] private List<Sprite> m_sprites;

    [Header("Assistant")]
    [SerializeField] private List<GameObject> m_slots;

    private List<AssistantRow> m_rows;
    public List<Assistant> assistants;

    public void Init(SaveData saveData)
    {
        m_rows = new List<AssistantRow>();

        // If the save contains assistants, use those
        if (saveData != null && saveData.assistants != null)
        {
            assistants = saveData.assistants;

            // Reload the sprites
            foreach (Assistant assistant in assistants)
            {
                assistant.UpdateSprites(m_sprites);
            }
        }
        else
        {
            // Otherwise create a new list
            assistants = new List<Assistant>();
        }

        AddRow();
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


    /// <summary>
    /// Adds a new assistant with the given name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Assistant AddAssistant(string name)
    {
        Assistant newAssistant = new Assistant(assistants.Count, name, m_sprites);

        assistants.Add(newAssistant);

        AddRow();

        return newAssistant;
    }


    public void UpdateAssistant(Assistant assistant, int area, bool isRepeat)
    {
        for (int i = 0; i < assistants.Count; i++)
        {
            if (assistants[i] == assistant)
            {
                assistant.area = area;
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

    public void Unlock() 
    {
        m_button.SetActive(true);

        string defaultName = "Lydia Smith";

        // Ignore if the assistant is already there
        bool isDuplicate = false;

        for (int i = 0; i < assistants.Count; i++)
        {
            if (assistants[i].Name == defaultName)
            {
                isDuplicate = true;
                break;
            }
        }    
        
        if (!isDuplicate) AddAssistant(defaultName);
    }

    /// <summary>
    /// Fills all slots with the available assistants
    /// </summary>
    private void AddRow()
    {
        for (int i = 0; i < assistants.Count; i++)
        {
            if (m_slots[i].transform.childCount > 0)
            {
                continue;
            }

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
    [NonSerialized] private List<Sprite> sprites;

    public int area;
    public bool isWorking = false;
    public bool repeat = false;
    // TODO bonuses?

    public Assistant(int id, string name, List<Sprite> spritesList)
    {
        ID = id;
        Name = name;
        sprites = spritesList;

        SpriteID = GetRandomSpriteID();
        sprite = GetSprite();    
    }

    public void UpdateSprites(List<Sprite> updatedSprites)
    {
        sprites = updatedSprites;
        sprite = GetSprite();
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

    private int GetRandomSpriteID() => UnityEngine.Random.Range(0, sprites.Count);
    private Sprite GetSprite() => sprites[SpriteID];
}