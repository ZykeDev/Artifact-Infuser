using System;
using System.Collections.Generic;
using UnityEngine;


public class AssistantSystem : MonoBehaviour
{
    [SerializeField] protected GameController m_gameController;
    [SerializeField] private GameObject m_rowPrefab;

    public List<(Assistant, int, bool)> assistants;
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
            // Otherwise create a new list and add the default assistant
            //assistants = new List<(Assistant, int, bool)>();
            //Assistant a = AddAssistant();
        }

        // Fill a slot with each assistant row
        for (int i = 0; i < assistants.Count; i++)
        {
            // TODO move this to its own function
            GameObject rowObj = Instantiate(m_rowPrefab, m_slots[i].transform);
            rowObj.name = assistants[i].Item1.name;

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
        for (int i = 0; i < assistants.Count; i++)
        {
            if (assistants[i].Item1 == assistant)
            {
                bool isArea = m_gameController.IsAreaUnlocked(assistants[i].Item2);
                bool isFree = !assistants[i].Item3;

                if (isArea && isFree)
                {
                    assistants[i] = (assistant, assistants[i].Item2, true);
                    m_gameController.SendAssistant(assistants[i].Item1);

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
        // TODO check there are enough slots?
        Assistant newAssistant = new Assistant(m_assistantsNumber, "Lydia Smith", false);
        assistants.Add((newAssistant, 0, false));

        m_assistantsNumber = assistants.Count;

        return newAssistant;
    }


    public void UpdateAssistant(Assistant assistant, int area, bool isRepeat)
    {
        for (int i = 0; i < assistants.Count; i++)
        {
            if (assistants[i].Item1 == assistant)
            {
                assistant.repeat = isRepeat;
                assistants[i] = (assistant, area, false);
                break;
            }
        }
    }


    public void Return(Assistant assistant, int area)
    {
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
    public int id;
    public string name;
    public int spriteID;
    [NonSerialized] public Sprite sprite;

    public bool repeat;
    // TODO bonuses?

    public Assistant(int id, string name, bool repeat)
    {
        this.id = id;
        this.name = name;
        this.repeat = repeat;

        spriteID = RandomSprite();
        this.sprite = GetSprite(spriteID);
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