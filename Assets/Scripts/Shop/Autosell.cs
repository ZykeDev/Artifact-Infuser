using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autosell : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] protected GameController m_gameController;
    [SerializeField] protected BackgroundManager m_backgroundManager;


    [SerializeField]
    private bool m_enabled = false;
    private bool m_running = false;

    [SerializeField, Tooltip("Every how many seconds a rule is executed.")]
    private float m_tickrate = 2f;


    private List<AutosellRule> m_rules;
    private List<Coroutine> m_ruleCoroutines;


    void Awake()
    {
        m_rules = new List<AutosellRule>();
        m_ruleCoroutines = new List<Coroutine>();
    }

    void Start()
    {
        AutosellRule testRule = new AutosellRule(AutosellAmount.ALL, AutosellType.TYPE, ArtifactType.ARMOR);
        m_rules.Add(testRule);

        if (m_enabled)
        {
            foreach (AutosellRule rule in m_rules)
            {
                rule.Activate();
            }

            StartAutosell();
        }
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




    public void StopAutosell()
    {
        m_enabled = false;
        m_running = false;

        foreach (Coroutine ruleCoroutine in m_ruleCoroutines)
        {
            StopCoroutine(ruleCoroutine);
        }

    }


    public void StartAutosell()
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



