using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIInputHandler : MonoBehaviour
{

    [SerializeField] GameObject canvas;
    [SerializeField] GameObject memoryMenu;
    GraphicRaycaster UI_raycaster;

    PointerEventData click_data;
    List<RaycastResult> click_results;

    void Awake()
    {
        click_data = new PointerEventData(EventSystem.current);
        UI_raycaster = canvas.GetComponent<GraphicRaycaster>();

        click_results = new List<RaycastResult>();
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

                Debug.Log(UI_element.name);

                // menu on/off button clicked
                if (UI_element.name == "MemoryMenuButton")
                {
                    memoryMenu.SetActive(!memoryMenu.activeSelf);
                }
            }
        }

    }
}
