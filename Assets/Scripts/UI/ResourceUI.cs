using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    [Header("Main Text")]
    [SerializeField] protected Text m_text;

    [Header("Gain Text")]
    [SerializeField] protected GameObject m_gainObj;
    [SerializeField] protected Transform m_gainDestination;
    [SerializeField] protected float m_animDuration = 0.75f;

    private Color m_gainColor;
    private Color m_lossColor;

    private Text m_gain;
    private Coroutine m_gainCoroutine;
    private Vector3 m_startingPos;


    void Awake()
    {
        m_gain = m_gainObj.GetComponent<Text>();
        m_gain.text = "";
        m_startingPos = m_gainObj.transform.position;
        m_gainObj.SetActive(false);
    }


    public void SetColors(Color gain, Color loss)
    {
        m_gainColor = gain;
        m_lossColor = loss;
    }


    /// <summary>
    /// Updates the given resource in the inventory
    /// </summary>
    /// <param name="inv"></param>
    /// <param name="type"></param>
    public void UpdateUI(Inventory inv, ResourceType type)
    {
        if (inv == null)
        {
            Debug.LogWarning("Inventory is null");
            return;
        }
        m_text.text = inv.GetResourceAmount(type).ToString();
    }


    /// <summary>
    /// Updates the gold
    /// </summary>
    /// <param name="inv"></param>
    public void UpdateUI(Inventory inv)
    {
        if (inv == null)
        { 
            Debug.LogWarning("Inventory is null"); 
            return; 
        }
        m_text.text = inv.GetGold().ToString();
    }


    #region Gains

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
                m_gain.color = m_lossColor;
            }

            m_gainObj.SetActive(true);
            if (m_gainCoroutine != null) { StopCoroutine(m_gainCoroutine); }
            m_gainCoroutine = StartCoroutine(AnimateGain());
        }
    }


    #endregion


    #region Losses

    public void DisplayLoss(Inventory inv)
    {
        DisplayLoss(inv.gold);
    }

    public void DisplayLoss(Inventory inv, ResourceType type)
    {
        int value = (int)inv.GetResourceAmount(type);

        DisplayLoss(value);
    }

    public void DisplayLoss(int value) => DisplayGain(-value);

    #endregion




    private IEnumerator AnimateGain()
    {
        m_gainObj.transform.position = m_startingPos;

        float startingAlpha = 1f;
        float finalAlpha = 0f;

        float time = m_animDuration;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            m_gainObj.transform.position = Vector2.Lerp(m_startingPos, m_gainDestination.position, EaseOut(elapsedTime / time));
            
            // Lerp the alpha channel of the color
            Color newColor = m_gain.color;
            float newAlpha = Mathf.Lerp(startingAlpha, finalAlpha, EaseOut(elapsedTime / (time*2)));
            newColor.a = newAlpha;
            m_gain.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        m_gainObj.SetActive(false);
        m_gainObj.transform.position = m_startingPos;
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
