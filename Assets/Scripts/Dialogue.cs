using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class Dialogue : MonoBehaviour
{

    [SerializeField] DialogueRunner dialogueRunner;
    [SerializeField] YarnProject yarnProject;
    [SerializeField] Camera mainCamera;
    PlayerInputActions playerInputActions;

    Dictionary<string, string> questStatus = new Dictionary<string, string>
    {
        {"RedNPC", "NotStarted"},
        {"YellowNPC", "NotStarted"}
    };

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.StartDialogue.Enable();
    }

    public void OnDisable()
    {
        playerInputActions.Player.StartDialogue.Disable();
    }

    public void Update()
    {
        // disable player movement while dialogue is running
    }

    /*
     * Runs dialogue for NPC clicked, disables movement
    */
    void OnStartDialogue()
    {
        Debug.Log(questStatus["RedNPC"]);

        if (!dialogueRunner.IsDialogueRunning)
        {
            //create ray to mouse position
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit objectHit;

            //NPC clicked must have tag hasDialogue
            if (Physics.Raycast(ray, out objectHit) && objectHit.collider.gameObject.CompareTag("hasDialogue"))
            {
                string npcName = objectHit.collider.gameObject.name;
                //Debug.Log("Clicked: " + npcName);
                dialogueRunner.StartDialogue($"{npcName}");
            }
        }
    }
}