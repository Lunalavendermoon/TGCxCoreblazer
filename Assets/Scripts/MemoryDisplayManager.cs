using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Unity;

public class MemoryDisplayManager : MonoBehaviour
{

    [SerializeField] DialogueRunner dialogueRunner;
    [SerializeField] MemoryData memoryData; //memory inventory ScriptableObject
    [SerializeField] GameObject memoryObjectPrefab; //Memory object UI prefab
    [SerializeField] Transform contentPanel; //parent UI to hold memory objects
    [SerializeField] GameObject memoryMenu;

    List<string> memoryDataList;
    List<GameObject> displayedMemoryUI;

    private void Start()
    {
        memoryDataList = MemoryData.MemoryList;
        displayedMemoryUI = new List<GameObject>();

        dialogueRunner.AddCommandHandler<string>("take_memory", TakeMemory);
        dialogueRunner.AddCommandHandler<string>("give_memory", GiveMemory);
        dialogueRunner.AddFunction<string, bool>("check_has_memory", CheckHasMemory); //param: string, return: boolean
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

            //update TMP title text, description text
            Dictionary<string, string> memoryInfo = memoryData.GetMemoryInfo(memory);
            memoryUIObject.name = memory; //GameObject name - used to see if memory exists?
            nameText.text = memory;
            descriptionText.text = memoryInfo["description"];

            //add to list of displayedUI to make updating easier later
            displayedMemoryUI.Add(memoryUIObject);
        }
    }

    public void TakeMemory(string memoryName)
    {
        memoryDataList.Add(memoryName);
        RefreshUI();
    }

    public void GiveMemory(string memoryName)
    {
        memoryDataList.Remove(memoryName);
        RefreshUI();
    }

    public bool CheckHasMemory(string memoryName)
    {
        return memoryDataList.Contains(memoryName);
    }
}
