﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    [Header("Main Text")]
    [SerializeField] private Text m_text;

    [Header("Gain Text")]
    [SerializeField] private GameObject m_gainObj;
    [SerializeField] private Transform m_gainDestination;
    [SerializeField] private float m_animDuration = 0.75f;

    private Color m_gainColor;
    private Color m_loseColor;

    private Text m_gain;
    private Coroutine m_gainCoroutine;


    void Awake()
    {
        m_gain = m_gainObj.GetComponent<Text>();
        m_gain.text = "";
        m_gainObj.SetActive(false);
    }


    public void SetColors(Color gain, Color lose)
    {
        m_gainColor = gain;
        m_loseColor = lose;
    }


    /// <summary>
    /// Updates the given resource in the inventory
    /// </summary>
    /// <param name="inv"></param>
    /// <param name="type"></param>
    public void UpdateUI(Inventory inv, ResourceType type)
    {
        m_text.text = inv.GetResourceAmount(type).ToString();
    }


    /// <summary>
    /// Updates the gold
    /// </summary>
    /// <param name="inv"></param>
    public void UpdateUI(Inventory inv)
    {
        m_text.text = inv.GetGold().ToString();
    }



    public void DisplayGain(Inventory inv)
    {
        int value = inv.GetGold();

        DisplayGain(value);
    }

    public void DisplayGain(Inventory inv, ResourceType type)
    {
        int value = (int)inv.GetResourceAmount(type);

        DisplayGain(value);
    }

    public void DisplayGain(int value)
    {
        if (value != 0)
        {
            // Show the "+" if the number is positive
            if (value > 0)
            {
                m_gain.text = "+" + value.ToString();
                m_gain.color = m_gainColor;
            }
            else
            {
                m_gain.text = value.ToString();
                m_gain.color = m_loseColor;
            }

            m_gainObj.SetActive(true);
            m_gainCoroutine = StartCoroutine(AnimateGain());
        }
    }



    private IEnumerator AnimateGain()
    {
        Vector3 startingPos = m_gainObj.transform.position;
        float startingAlpha = 1f;
        float finalAlpha = 0f;

        float time = m_animDuration;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            m_gainObj.transform.position = Vector2.Lerp(startingPos, m_gainDestination.position, EaseOut(elapsedTime / time));
            
            // Lerp the alpha channel of the color
            Color newColor = m_gain.color;
            float newAlpha = Mathf.Lerp(startingAlpha, finalAlpha, EaseOut(elapsedTime / (time*2)));
            newColor.a = newAlpha;
            m_gain.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        m_gainObj.SetActive(false);
        m_gainObj.transform.position = startingPos;
        m_gain.text = "";
        
        m_gainCoroutine = null;
    }


    /// <summary>
    /// Returns the result of an EaseOut (x^4) function at a given value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private float EaseOut(float value)
    {
        if (value < 0 || value > 1)
        {
            return 0f;
        }

        return Flip(Mathf.Pow(Flip(value), 4));
    }

    /// <summary>
    /// Returns 1 - value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private float Flip(float value)
    {
        return 1 - value;
    }


}
