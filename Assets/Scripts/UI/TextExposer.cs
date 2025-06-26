using TMPro;
using UnityEngine;

public class TextExposer : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;

    public void SetText(string text)
    {
        this.text.text = text;
    }

    public string GetText()
    {
        return text.text;
    }
}
