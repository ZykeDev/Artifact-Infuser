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

        UpdateRules();
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


    public void UpdateRules()
    {
        GameObject[] slots = GameObject.FindGameObjectsWithTag("Rule Slot");

        if (m_rules.Count > slots.Length)
        {
            Debug.LogError("Missing Autosell Rule slot.");
        }

        int i = 0;
        foreach (AutosellRule rule in m_rules)
        {
            GameObject defaultRule = Instantiate(m_ruleSettingPref, slots[i].transform);
            defaultRule.name = "Rule_" + i;
            defaultRule.transform.localPosition = new Vector2(0, 0);

            RuleSetting rs = defaultRule.GetComponent<RuleSetting>();

            rs.SetSettings(rule);
            rs.SetOnValueChange(delegate { UpdateRules(); });

            m_ruleSettings.Add(rs);

            i++;
        }
    }





    private void StopAutosell()
    {
        m_enabled = false;
        m_running = false;

        // Stop the coroutines
        foreach (Coroutine ruleCoroutine in m_ruleCoroutines)
        {
            StopCoroutine(ruleCoroutine);
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



