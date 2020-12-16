using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequestSettings : MonoBehaviour
{
    [SerializeField] protected TMP_Text m_client;
    [SerializeField] protected TMP_Text m_artifact;
    [SerializeField] protected Image m_image;

    [SerializeField] protected Button m_confirm;
    [SerializeField] protected Button m_cancel;

    private RequestSystem m_requestSystem;
    private Request m_request;

    private string m_clientName;
    private string m_artifactName;
    private Sprite m_sprite;

    [SerializeField] protected bool m_isFulfilled = false;


    void Update()
    {
        m_confirm.interactable = m_isFulfilled;
    }


    public void SetData(RequestSystem requestSystem, Request request, Sprite sprite)
    {
        // Send a reference to the handler
        m_requestSystem = requestSystem;

        m_request = request;

        m_clientName = request.client;
        m_artifactName = request.artifactName;
        m_sprite = sprite;        
        
        // Update the text fields and image
        m_client.text = m_clientName;
        m_artifact.text = m_artifactName;
        m_image.sprite = m_sprite;

        // Add the onClick listeners to the buttons
        m_confirm.onClick.AddListener(Confirm);
        m_cancel.onClick.AddListener(Cancel);

        m_confirm.GetComponentInChildren<TMP_Text>().text = "Confirm (" + request.GetReward() + ")";
        m_confirm.interactable = m_isFulfilled;
    }



    public void Confirm()
    {
        if (m_isFulfilled)
        {
            m_requestSystem?.FulfilRequest(m_request);
        }
    }

    public void Cancel()
    {
        m_requestSystem?.CancelRequest(m_request);
    }




    public void SetFulfilled(bool isFulfilled)
    {
        m_isFulfilled = isFulfilled;
    }

}
