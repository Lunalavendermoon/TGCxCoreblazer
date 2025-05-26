using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Yarn.Unity;

public class UIInputHandler : MonoBehaviour
{
    [SerializeField] DialogueRunner dialogueRunner;
    [SerializeField] DialogueManager dialogueManagerScript;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject memoryMenu;
    [SerializeField] GameObject incompatibleMessage;
    [SerializeField] GameObject secondMemoryMessage;
    [SerializeField] MemoryDisplayManager memoryDisplayManager;
    GraphicRaycaster UI_raycaster;

    PointerEventData click_data;
    List<RaycastResult> click_results;

    void Awake()
    {
        click_data = new PointerEventData(EventSystem.current);
        UI_raycaster = canvas.GetComponent<GraphicRaycaster>();

        click_results = new List<RaycastResult>();
    }

    private void Start()
    {
        dialogueRunner.AddCommandHandler<string, string, string>("prompt_memory_selection", PromptMemorySelection);
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            click_results.Clear();
            click_data.position = Mouse.current.position.ReadValue();

            UI_raycaster.Raycast(click_data, click_results);

            foreach(RaycastResult result in click_results)
            {
                GameObject UI_element = result.gameObject;

                // toggle menu
                if (UI_element.name == "MemoryMenuButton")
                {
                    memoryMenu.SetActive(!memoryMenu.activeSelf);
                }
            }
        }

    }
    public IEnumerator PromptMemorySelection(string npcName, string targetMemoryType1, string targetMemoryType2 = "None")
    {
        memoryMenu.SetActive(true);

        List<string> TypesToSelect = new List<string>();
        bool selectMultipleMemories = false; //used to determine if popup to select a memory should be shown

        TypesToSelect.Add(targetMemoryType1);
        if (targetMemoryType2 != "None")
        {
            TypesToSelect.Add(targetMemoryType2);
            selectMultipleMemories = true;
        }

        //Repeat until correct UI element is clicked
        while (true)
        {
            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                click_results.Clear();
                click_data.position = Mouse.current.position.ReadValue();

                UI_raycaster.Raycast(click_data, click_results);

                foreach (RaycastResult result in click_results)
                {
                    GameObject UI_element = result.gameObject;

                    if (MemoryData.IsValidMemory(UI_element.name))
                    {
                        Debug.Log($"\n MemoryType: {MemoryData.GetMemoryType(UI_element.name)}");

                        string memoryType = MemoryData.GetMemoryType(UI_element.name);

                        if (TypesToSelect.Contains(memoryType))
                        {
                            TypesToSelect.Remove(memoryType);
                            memoryDisplayManager.GiveMemory(npcName, UI_element.name);

                            //success message after selecting 1
                            if (selectMultipleMemories = true && TypesToSelect.Count == 1) StartCoroutine(ActivateMessage(secondMemoryMessage));
                            else if (TypesToSelect.Count == 0)
                            {
                                memoryMenu.SetActive(false);
                                dialogueManagerScript.SetQuestComplete(npcName); //mark completed
                                yield break;
                            }
                        }
                        else //otherwise, show this memory is not compatible message
                        {
                            StartCoroutine(ActivateMessage(incompatibleMessage));
                        }
                    }
                }
            }
            yield return null; //don't return until next frame
        }
    }

    public IEnumerator ActivateMessage(GameObject messageWindow)
    {
        messageWindow.SetActive(true);
        yield return new WaitForSeconds(2f);
        messageWindow.SetActive(false);
    }
}


