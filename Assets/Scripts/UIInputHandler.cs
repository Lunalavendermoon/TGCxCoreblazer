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
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject memoryMenu;
    [SerializeField] GameObject incompatibleMessage;
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
        dialogueRunner.AddCommandHandler<string>("prompt_memory_selection", PromptMemorySelection);
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
    public IEnumerator PromptMemorySelection(string targetMemoryName)
    {
        memoryMenu.SetActive(true);

        //Repeat until correct UI element is clicked
        while (true)
        {
            // Wait for left mouse click
            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                click_results.Clear();
                click_data.position = Mouse.current.position.ReadValue();

                UI_raycaster.Raycast(click_data, click_results);

                foreach (RaycastResult result in click_results)
                {
                    GameObject UI_element = result.gameObject;
                    Debug.Log("Clicked on: " + UI_element.name);

                    if (UI_element.name == targetMemoryName)
                    {
                        Debug.Log("Target UI clicked!");
                        memoryMenu.SetActive(false);
                        yield break;
                    }
                    //otherwise, show this memory is not compatible message
                    else if (UI_element.name.Contains("Memory"))
                    {
                        StartCoroutine(ActivateIncompatibleMessage());
                    }
                }
            }
            yield return null; //don't return until next frame
        }
    }

    public IEnumerator ActivateIncompatibleMessage()
    {
        incompatibleMessage.SetActive(true);
        yield return new WaitForSeconds(2f);
        incompatibleMessage.SetActive(false);
    }
}
