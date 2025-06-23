using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadFileButton : MonoBehaviour
{
    [SerializeField]
    Button loadButton;
    [SerializeField]
    TextMeshProUGUI loadButtonText;
    [SerializeField]
    Button deleteButton;

    public void Initialize(string file, GismosHandler gismosHandler, GameObject loadFileDialog)
    {
        loadButtonText.text = file;
        loadButton.onClick.AddListener(() => loadFileDialog.SetActive(false));
        loadButton.onClick.AddListener(() => gismosHandler.LoadSpellDesign(file));

        deleteButton.onClick.AddListener(() => DeleteSpell(file));
        deleteButton.onClick.AddListener(() => Destroy(gameObject));
    }

    static void DeleteSpell(string file)
    {
        File.Delete(Application.persistentDataPath + "/spell_designs/" + file + ".json");
    }
}
