using System.IO;
using UnityEngine;

public class LoadFileDialog : MonoBehaviour
{
    [SerializeField]
    GameObject fileButton;
    [SerializeField]
    GismosHandler gismosHandler;
    [SerializeField]
    Transform content;

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void PopulateFiles()
    {
        ClearFiles();

        string[] files = Directory.GetFiles(Application.persistentDataPath + "/spell_designs");

        foreach (string filePath in files)
        {
            string[] splitFilePath = filePath.Split("/");
            string[] fileParts = splitFilePath[splitFilePath.Length - 1].Split(".");
            string file = fileParts[0];

            // make sure we're reading only json files
            if (fileParts.Length < 2 || fileParts[1] != "json") continue;

            GameObject newButton = Instantiate(fileButton, content);
            newButton.GetComponent<LoadFileButton>().Initialize(file, gismosHandler, gameObject);
        }
    }

    void ClearFiles()
    {
        foreach (Transform child in content)
        {
            Destroy(child);
        }
    }
}
