using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class MemoryDisplayManager : MonoBehaviour
{

    [SerializeField] DialogueRunner dialogueRunner;
    [SerializeField] MemoryData memoryData; //memory inventory ScriptableObject
    [SerializeField] GameObject memoryObjectPrefab; //Memory object UI prefab
    [SerializeField] Transform contentPanel; //parent UI to hold memory objects
    [SerializeField] GameObject memoryMenu;
    [SerializeField] DialogueManager dialogueManagerScript;

    List<string> memoryDataList;
    List<GameObject> displayedMemoryUI;

    private void Start()
    {
        memoryDataList = MemoryData.MemoryList;
        displayedMemoryUI = new List<GameObject>();

        dialogueRunner.AddCommandHandler<string, string>("take_memory", TakeMemory);
        dialogueRunner.AddFunction<string, bool>("check_has_memory_type", CheckHasMemoryType); //param: string, return: boolean
    }

    public void RefreshUI()
    {
        //clear old UI
        foreach(GameObject memoryUI in displayedMemoryUI)
        {
            //destroy every corresponding gameObject
            Destroy(memoryUI);
        }
        //clear displayedMemoryUI list
        displayedMemoryUI.Clear();

        //update displayedUI - instantiate memoryPrefab, assign parent to panel
        foreach(string memory in memoryDataList)
        {
            GameObject memoryUIObject = Instantiate(memoryObjectPrefab, contentPanel);

            TextMeshProUGUI nameText = memoryUIObject.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI descriptionText = memoryUIObject.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
            Image memoryImage = memoryUIObject.transform.Find("MemoryImage").GetComponent<Image>();

            //update TMP title text, description text
            Dictionary<string, string> memoryInfo = MemoryData.GetMemoryInfo(memory);
            memoryUIObject.name = memory; //GameObject name - used to see if memory exists?
            memoryImage.sprite = Resources.Load<Sprite>("CursedScreenshot");
            nameText.text = memory;
            descriptionText.text = memoryInfo["description"];

            //add to list of displayedUI to make updating easier later
            displayedMemoryUI.Add(memoryUIObject);
        }
    }

    private static readonly Regex sWhitespace = new Regex(@"\s+");
    public static string RemoveWhitespace(string input)
    {
        return sWhitespace.Replace(input, "");
    }

    public void TakeMemory(string npcName, string memoryName)
    {
        npcName = RemoveWhitespace(npcName);
        if(MemoryData.IsValidMemory(memoryName))
        {
            memoryDataList.Add(memoryName);
            Debug.Log($"{memoryName} added to inventory");
            dialogueManagerScript.SetQuestComplete(npcName);
            RefreshUI();
        }
        else
        {
            Debug.Log($"MEMORY NOT SUCCESSFULLY ADDED TO INVENTORY - Attemped to add memory that " +
                $"doesn't exist: {memoryName}\n" +
                $"Double check that the memory name exists in MemoryInfo dictionary " +
                $"(MemoryData.cs), and that the spelling in your YarnSpinner script matches " +
                $"it.");
        }
    }

    public void GiveMemory(string npcName, string memoryName)
    {
        npcName = RemoveWhitespace(npcName);
        memoryDataList.Remove(memoryName);
        Debug.Log($"{memoryName} removed from inventory");
        //dialogueManagerScript.SetQuestComplete(npcName);
        //don't mark complete until both memories given
        RefreshUI();
    }

    public bool CheckHasMemoryType(string memoryType)
    {
        foreach(string memoryName in memoryDataList)
        {
            if (MemoryData.GetMemoryType(memoryName) == memoryType)
            {
                return true;
            }
        }
        return false;
    }
}
