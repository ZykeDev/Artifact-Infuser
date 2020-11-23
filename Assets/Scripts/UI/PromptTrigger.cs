using UnityEngine;

public class PromptTrigger : MonoBehaviour
{
    private string texts;

    public void SetTexts(string texts) => this.texts = texts;


    public void ShowPrompt() => PromptSystem.Show(texts);
    public void HideTooltip() => PromptSystem.Hide();


}