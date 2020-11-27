using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogHandler : MonoBehaviour
{
    #region Vars

    [SerializeField] private TextMeshProUGUI m_dialog;

    [Header("Dialog Overflow")]
    [SerializeField] private int m_dialogPadding = 12;
    [SerializeField] private int m_visibleRows = 8;
    [SerializeField, Range(8, 24)] private int m_maxRows = 16;
    [SerializeField] private int m_charsPerRow = 50;

    [SerializeField, Range(1, 30)] private int m_lineHeight = 8;

    // A list of tulpes containing the current dialog lines
    private List<(DialogType, string)> m_dialogLines;

    #endregion


    void Awake()
    {
        m_dialogLines = new List<(DialogType, string)>();
    }




    public void AddLine(DialogType type, string newLine)
    {
        m_dialogLines.Add((type, newLine));

        UpdateViewer();

    }

    public void AddLine(DialogType type, Request request)
    {
        string line = "An adventurer enters your shop.\n";
        line += "NEW COMMISSION: Craft a " + request.artifactName + " for " + request.client + ".\n";
        line += "Reward: " + request.GetReward() + " gold.";

        AddLine(type, line);
    }


    public void Sold(string name, int gold)
    {
        string line = "Sold " + name + " for " + gold + " gold.";

        AddLine(DialogType.SELL, line);
    }




    /// <summary>
    /// Updates the dialog viewer to fix the max number of rows allowed, removing the oldest ones if needed.
    /// </summary>
    private void UpdateViewer()
    {
        int numberOfRows = GetNumberOfRows();

        // Update the viewer
        if (numberOfRows > m_visibleRows && numberOfRows < m_maxRows)
        {
            RectTransform rt = m_dialog.GetComponent<RectTransform>();

            float newOffsetY = (m_dialogPadding * 2) + (numberOfRows * m_lineHeight);

            rt.offsetMax = new Vector2(rt.offsetMax.x, newOffsetY);
        }
        

        // Reset the text     
        m_dialog.text = "";

        // Remove the overflowing lines, starting from the oldest
        if (numberOfRows > m_maxRows)
        {
            for (int j = 0; j < (numberOfRows - m_maxRows); j++)
            {
                m_dialogLines.RemoveAt(0);
            }
        }

        // Fill the text with the updated lines
        for (int i = 0; i < m_dialogLines.Count; i++)
        {
            // Items2 is the second item in the tuple
            string line = m_dialogLines[i].Item2;
            bool ignoreFirstNewline = i == 0;

            if (ignoreFirstNewline)
            {
                m_dialog.text += line;
            }
            else
            {
                m_dialog.text += "\n" + line;
            }
        }
    }


    /// <summary>
    /// Returns the effective number of rows being displayed in the dialog
    /// </summary>
    /// <returns></returns>
    private int GetNumberOfRows()
    {
        int counter = 0;

        foreach ((DialogType type, string line) in m_dialogLines)
        {
            if (type == DialogType.REQUEST)
            {
                counter += 2;
            }

            counter++;
        }

        return counter;
    }
}
