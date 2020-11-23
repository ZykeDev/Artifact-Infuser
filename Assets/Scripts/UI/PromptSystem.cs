using System;
using UnityEngine;
using UnityEngine.UI;

public class PromptSystem : MonoBehaviour
{
    private static PromptSystem current;

    public Prompt prompt;

    [NonSerialized] public bool isConfirm = false;


    void Awake()
    {
        current = this;
        current.isConfirm = false;
    }


    /// <summary>
    /// Displays the prompt
    /// </summary>
    /// <param name="text"></param>
    public static void Show(string text)
    {
        // Make this canvas block raycasts
        current.GetComponent<GraphicRaycaster>().enabled = true;

        current.isConfirm = false;
        current.prompt.SetTexts(text);
        current.prompt.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides away the prompt
    /// </summary>
    public static void Hide()
    {
        current.GetComponent<GraphicRaycaster>().enabled = false;
        current.prompt.gameObject.SetActive(false);
    }


    public static void Confirm()
    {
        current.isConfirm = true;
        Hide();
    }

    public void ResetConfirm() => current.isConfirm = false;
}