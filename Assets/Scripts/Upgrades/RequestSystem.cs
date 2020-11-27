using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestSystem : MonoBehaviour
{
    [SerializeField] private GameObject m_requestPref;
    private GameController m_gameController;
    private UnlockSystem m_unlockSystem;

    [SerializeField, Tooltip("Enable to make a new commission can randomly appear every so often.")]
    private bool m_enableRandomRequests = false;
    [SerializeField, Tooltip("How often a new commission randomly appear (seconds)."), Min(0.1f)]
    private float m_requestInterval = 5f;
    [SerializeField, Range(0f, 1f)]
    private float m_requestSpawnChance = 0.5f;

    private bool m_isSlotAvailable = true;
    private bool m_running = false;
    private Coroutine m_requestCoroutine;


    private List<Request> m_activeRequests;
    private List<GameObject> m_requestObjs;

    private Request m_firstRequest;

    private int tier = 1;


    void Awake()
    {
        m_gameController = GetComponent<GameController>();
        m_unlockSystem = GetComponent<UnlockSystem>();

        m_activeRequests = new List<Request>();
        m_requestObjs = new List<GameObject>();

        m_firstRequest = new Request(0, 50);


        DisplayRequests();
    }


    void Update()
    {
        if (m_enableRandomRequests && !m_running && m_isSlotAvailable)
        {
            StartRandomRequests();
        }
        else if (!m_enableRandomRequests && m_running)
        {
            StopRandomRequests();
        }
        
        
        // TODO don't find the slots on update
        if (GameObject.FindGameObjectsWithTag("Request Slot").Length > m_activeRequests.Count)
        {
            m_isSlotAvailable = true;
        }
        else
        {
            m_isSlotAvailable = false;
        }
    }




    /// <summary>
    /// Instantiates a Request object and a RequestSettings for every active request
    /// </summary>
    private void DisplayRequests()
    {
        GameObject[] slots = GameObject.FindGameObjectsWithTag("Request Slot");

        if (m_activeRequests.Count > slots.Length)
        {
            Debug.LogWarning("Not enough Request slots.");
        }

        int i = 0;
        foreach (Request request in m_activeRequests)
        {
            // Exit if there are no more slots TODO change this to something... better
            if (i == slots.Length)
            {
                break;
            }

            // Using a local var so that, when sending the delegate, it doesn't read the highest value of "i".
            int currentIndex = i;
            
            GameObject newRequest = Instantiate(m_requestPref, slots[currentIndex].transform);
            newRequest.name = "Request_" + currentIndex;
            newRequest.transform.localPosition = new Vector2(0, 0);

            RequestSettings rs = newRequest.GetComponent<RequestSettings>();
            Sprite sprite = m_unlockSystem.GetBlueprint(request.GetArtifactID()).GetBlueprintSprite();

            rs.SetData(this, request, sprite);

            m_requestObjs.Add(newRequest);
            
            i++;
        }
    }


    /// <summary>
    /// Clears all the Request gameobjects in the scene and re-instantiates them
    /// </summary>
    private void UpateRequests()
    {
        foreach (GameObject request in m_requestObjs)
        {
            Destroy(request);
        }
        m_requestObjs.Clear();

        DisplayRequests();
    }



    /// <summary>
    /// Adds the given request to the list of active requests and displays it
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public Request AddRequest(Request request)
    {
        m_activeRequests.Add(request);

        UpateRequests();

        return request;
    }

    /// <summary>
    /// Creates a new random request and adds it 
    /// </summary>
    /// <returns></returns>
    public Request AddRequest()
    {
        Rarity rarity = (Rarity)UnityEngine.Random.Range(0, tier);
        int baseReward = 10;

        Request request = new Request(rarity, baseReward);

        return AddRequest(request);
    }
    public Request AddFirstRequest() => AddRequest(m_firstRequest);


    /// <summary>
    /// Awards the prize and removes the request from the list of active requests.
    /// </summary>
    /// <param name="request"></param>
    public void FulfilRequest(Request request)
    {
        int rewardGold = request.GetReward();
        m_gameController.GainGold(rewardGold);

        RemoveRequest(request);
    }

    /// <summary>
    /// Cancels a request and removes it from the list of active requests
    /// </summary>
    /// <param name="request"></param>
    public void CancelRequest(Request request)
    {
        RemoveRequest(request);
    }


    private void RemoveRequest(Request request)
    {
        // Check the rule is in the list
        if (!m_activeRequests.Contains(request))
        {
#if UNITY_EDITOR
            Debug.LogError("Fulfilled request does not exist in the Active Requests list.");
#endif
            return;
        }

        m_activeRequests.Remove(request);

        UpateRequests();
    }





    private void StartRandomRequests()
    {
        m_running = true;
        m_requestCoroutine = StartCoroutine(CheckRandomRequest());
    }

    private void StopRandomRequests()
    {
        m_running = false;
        StopCoroutine(m_requestCoroutine);
        m_requestCoroutine = null;
    }


    private IEnumerator CheckRandomRequest()
    {
        bool ignoreFirst = true;
        
        while (m_enableRandomRequests)
        {
            if (ignoreFirst)
            {
                ignoreFirst = false;
            }
            else
            {
                float chance = Random.Range(0f, 1f);

                if (chance <= m_requestSpawnChance && m_isSlotAvailable)
                {
                    AddRequest();
                }
            }
            

            yield return new WaitForSeconds(m_requestInterval);
        }
    }
}
