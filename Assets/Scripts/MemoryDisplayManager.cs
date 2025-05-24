using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class MemoryDisplayManager : MonoBehaviour
{
    [SerializeField] MemoryData memoryData;
    [SerializeField] GameObject memoryObjectPrefab; //Memory object UI prefab
    [SerializeField] Transform contentPanel; //parent UI to hold memory objects

    List<string> memoryDataList;
    List<GameObject> displayedMemoryUI;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        memoryDataList = memoryData.MemoryList;
        displayedMemoryUI = new List<GameObject>();
        RefreshUI();
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
            GameObject memoryUI = Instantiate(memoryObjectPrefab, contentPanel);

            //update TMP title text, description text
            TextMeshProUGUI nameText = memoryUI.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI descriptionText = memoryUI.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>();

            nameText.text = "test name";
            descriptionText.text = "test description";

            //add to list of displayedUI to make updating easier later
            displayedMemoryUI.Add(memoryUI);
            Debug.Log($"{memory} added to memory menu display");
        }
    }
}
