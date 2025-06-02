using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    //test to see how long my load time is
    [SerializeField] DialogueRunner dialogueRunner;
    [SerializeField] Camera mainCamera;
    [SerializeField] PlayerMovement playerController;
    PlayerInputActions playerInputActions;

    // Possible values: "Incomplete", "Completed"
    Dictionary<string, string> questStatus = new Dictionary<string, string>
    {
        //no longer need to manually enter values, game will fill in automatically
        //{"BabyBear", "Incomplete"},
        //{"ElderlyGardener", "Incomplete"},
        //{"Sapling", "Incomplete"}
    };

    // for completed interactions with environment objects
    HashSet<string> completedInteractions = new HashSet<string>();

    // private NPCFadeAndDisappear npcFadeScript;
    private NPCParticles npcFadeScript;

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.StartDialogue.Enable();
    }

    void Start()
    {
        dialogueRunner.AddCommandHandler("fade_npc", FadeNPC);
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

            if (Physics.Raycast(ray, out objectHit))
            {
                //NPC clicked must have tag hasDialogue
                if (objectHit.collider.gameObject.CompareTag("hasDialogue"))
                {
                    playerController.faceNPC(objectHit.collider.gameObject.transform);
                    string npcName = objectHit.collider.gameObject.name;
                    //Debug.Log("Clicked: " + npcName);
                    //dialogueRunner.StartDialogue($"{npcName}");
                    npcFadeScript = objectHit.collider.gameObject.GetComponent<NPCParticles>();
                    RunQuest(npcName);
                }
                else if (objectHit.collider.gameObject.CompareTag("givesMemory"))
                {
                    string objName = objectHit.collider.gameObject.name;
                    if (!completedInteractions.Contains(objName))
                    {
                        completedInteractions.Add(objName);
                        objectHit.collider.gameObject.transform.parent.GetComponent<IndicatorShow>().MemoryUnavailable();
                        dialogueRunner.StartDialogue($"{objName}");
                    }
                }
            }
        }
    }

    public void RunQuest(string npcName)
    {
        if (!questStatus.ContainsKey(npcName))
        {
            questStatus.Add(npcName, "Incomplete");
            dialogueRunner.StartDialogue($"{npcName}");
        }
        else if (questStatus[npcName] == "Incomplete")
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
        Debug.Log($"{npcName} quest marked complete");
    }

    public void FadeNPC()
    {
        if (npcFadeScript != null)
        {
            npcFadeScript.StartFade();
        }
    }
}