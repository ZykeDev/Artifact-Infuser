using System.Collections.Generic;
using UnityEngine;

public class RequestSystem : MonoBehaviour
{
    [SerializeField] private GameObject m_requestPref;
    private GameController m_gameController;
    private UnlockSystem m_unlockSystem;

    private List<Request> m_activeRequests;
    private List<GameObject> m_requestObjs;



    void Awake()
    {
        m_gameController = GetComponent<GameController>();
        m_unlockSystem = GetComponent<UnlockSystem>();

        m_activeRequests = new List<Request>();
        m_requestObjs = new List<GameObject>();

        DisplayRequests();

        NewRequest();
    }


    /// <summary>
    /// Instantiates a Request object and a RequestSettings for every active request
    /// </summary>
    private void DisplayRequests()
    {
        GameObject[] slots = GameObject.FindGameObjectsWithTag("Request Slot");

        if (m_activeRequests.Count > slots.Length)
        {
            Debug.LogError("Not enough Request slots.");
        }

        int i = 0;
        foreach (Request request in m_activeRequests)
        {
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




    public void NewRequest()
    {
        Request newRequet = new Request(Rarity.COMMON, 1); // TODO

        m_activeRequests.Add(newRequet);

        UpateRequests();
    }


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


}
