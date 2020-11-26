using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autosell : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] protected GameController m_gameController;
    [SerializeField] protected BackgroundManager m_backgroundManager;

    [Header("Autosell Rules")]
    [SerializeField, Tooltip("Every how many seconds a rule is executed.")]
    private float m_tickrate = 2f;

    [SerializeField] private GameObject m_ruleSettingPref;

    [SerializeField]
    private bool m_enabled = false;
    private bool m_running = false;

    private List<RuleSetting> m_ruleSettings;
    private List<AutosellRule> m_rules;
    private List<Coroutine> m_ruleCoroutines;


    void Awake()
    {
        m_rules = new List<AutosellRule>();
        m_ruleSettings = new List<RuleSetting>();
        m_ruleCoroutines = new List<Coroutine>();

        // TODO get the initial rules from either the savefile, or a default
        if (true) // if (notSavefile)
        {
            AutosellRule defaultRule = new AutosellRule(AutosellAmount.ALL, AutosellType.TYPE, ArtifactType.ARMOR);
            m_rules.Add(defaultRule);
            
        }

        InitRules();
    }

    void Update()
    {
        if (m_enabled && !m_running)
        {
            StartAutosell();
        }
        else if (!m_enabled && m_running)
        {
            StopAutosell();
        }
    }



    public void Enable() => m_enabled = true;
    public void Disable() => m_enabled = false;


    public void InitRules()
    {
        GameObject[] slots = GameObject.FindGameObjectsWithTag("Rule Slot");

        if (m_rules.Count > slots.Length)
        {
            Debug.LogError("Not enough Autosell Rule slots.");
        }

        int i = 0;
        foreach (AutosellRule rule in m_rules)
        {
            // Using a local var so that, when sending the delegate, it doesn't read the highest value of "i".
            int currentIndex = i;

            GameObject defaultRule = Instantiate(m_ruleSettingPref, slots[currentIndex].transform);
            defaultRule.name = "Rule_" + currentIndex;
            defaultRule.transform.localPosition = new Vector2(0, 0);

            RuleSetting rs = defaultRule.GetComponent<RuleSetting>();

            rs.SetSettings(rule);
            rs.SetOnValueChange(delegate { UpdateRule(currentIndex); });

            m_ruleSettings.Add(rs);

            i++;
        }
    }


    /// <summary>
    /// Updates the rule at the given index, reading from its associated ruleSettings
    /// </summary>
    /// <param name="index"></param>
    private void UpdateRule(int index)
    {
        AutosellRule rule = m_rules[index];
        RuleSetting settings = m_ruleSettings[index];

        settings.UpdateSettings();

        rule.UpdateRule(settings);
    }





    private void StopAutosell()
    {
        m_enabled = false;
        m_running = false;

        // Stop the coroutines
        foreach (Coroutine ruleCoroutine in m_ruleCoroutines)
        {
            if (ruleCoroutine != null)
            {
                StopCoroutine(ruleCoroutine);
            }
        }

    }


    private void StartAutosell()
    {
        int i = 0;
        foreach (AutosellRule rule in m_rules)
        {
            Coroutine ruleCoroutine = StartCoroutine(ApplyRule(rule));

            if (m_ruleCoroutines.Count < i + 1)
            {
                m_ruleCoroutines.Add(ruleCoroutine); 
            }
            else
            {
                m_ruleCoroutines[i] = ruleCoroutine;
            }

            i++;
        }

        if (m_ruleCoroutines.Count > 0)
        {
            m_running = true;
        }
        else
        {
            m_running = false;
        }
    }


    private IEnumerator ApplyRule(AutosellRule rule)
    {
        while (m_enabled)
        {
            List<Artifact> artifacts = rule.Apply(m_gameController.armory);

            m_backgroundManager.Sell(artifacts);

            yield return new WaitForSeconds(m_tickrate);
        }
    }


}



