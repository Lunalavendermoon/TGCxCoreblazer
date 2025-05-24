using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class Dialogue : MonoBehaviour
{

    [SerializeField] DialogueRunner dialogueRunner;
    [SerializeField] Camera mainCamera;
    PlayerInputActions playerInputActions;

    // Possible values: "Incomplete", "Completed"
    Dictionary<string, string> questStatus = new Dictionary<string, string>
    {
        {"BabyBear", "Incomplete"},
        {"BlueNPC", "Incomplete"}
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
        //questStatus["RedNPC"] = "new";
        //Debug.Log(questStatus["RedNPC"]);

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
                //dialogueRunner.StartDialogue($"{npcName}");
                RunQuest(npcName);
            }
        }
    }

    public void RunQuest(string npcName)
    {
        if (questStatus[npcName] == "Incomplete")
        {
            dialogueRunner.StartDialogue($"{npcName}");
        }
        else
        {
            Debug.Log($"Quest for {npcName} has been completed");
        }
    }

    public void SetQuestComplete(string npcName)
    {
        questStatus[npcName] = "Complete";
    }

    /*
     * how to:
     * 1. send values to yarnspinner (probably more like retrieve values from c#)
     * 2. run script based on value (variable?)
     */
}