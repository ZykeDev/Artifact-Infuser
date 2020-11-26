using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequestSettings : MonoBehaviour
{
    [SerializeField] private TMP_Text m_client;
    [SerializeField] private TMP_Text m_artifact;
    [SerializeField] private Image m_image;

    [SerializeField] private Button m_confirm;
    [SerializeField] private Button m_cancel;

    private RequestSystem m_requestSystem;
    private Request m_request;

    private string m_clientName;
    private string m_artifactName;
    private Sprite m_sprite;


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
        // TODO make the button not interactable while its not done
    }


    public void Confirm()
    {
        m_requestSystem?.FulfilRequest(m_request);
    }

    public void Cancel()
    {
        m_requestSystem?.CancelRequest(m_request);
    }


}
