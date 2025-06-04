using NUnit.Framework;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] DialogueRunner dialogueRunner;
    [SerializeField] Camera mainCamera;
    [SerializeField] PlayerMovement playerController;
    //[SerializeField] Movement2 playerController;
    [SerializeField] IslandManager islandManagerScript;
    PlayerInputActions playerInputActions;

    //wau using hashset smart, anne very big brain
    public HashSet<string> completedQuests = new HashSet<string>();

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

    void OnStartDialogue()
    {
        if (!dialogueRunner.IsDialogueRunning)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit objectHit;

            int ignoreLayerNumber = LayerMask.NameToLayer("IgnoreForDialogue");
            int ignoreLayerMask = ~(1 << ignoreLayerNumber);

            if (Physics.Raycast(ray, out objectHit, Mathf.Infinity, ignoreLayerMask))
            {
                if (objectHit.collider.gameObject.CompareTag("hasDialogue"))
                {
                    //special case
                    if (objectHit.collider.gameObject.name == "LittleGirl")
                    {
                        if(completedQuests.Contains("BabyBear") && completedQuests.Contains("Adventurer"))
                        {
                            playerController.faceNPC(objectHit.collider.gameObject.transform);
                            string npcName = objectHit.collider.gameObject.name;
                            npcFadeScript = objectHit.collider.gameObject.GetComponent<NPCParticles>();
                            RunQuest(npcName);
                        }
                    }
                    else //regular - always triggers dialogue
                    {
                        playerController.faceNPC(objectHit.collider.gameObject.transform);
                        string npcName = objectHit.collider.gameObject.name;
                        npcFadeScript = objectHit.collider.gameObject.GetComponent<NPCParticles>();
                        RunQuest(npcName);
                    }
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
        npcName = RemoveWhitespace(npcName);
        if(completedQuests.Contains(npcName))
        {
            Debug.Log($"Quest for {npcName} has been completed");
        }
        else
        {
            dialogueRunner.StartDialogue($"{npcName}");
        }
    }

    public void SetQuestComplete(string npcName)
    {
        npcName = RemoveWhitespace(npcName);
        completedQuests.Add(npcName);
        Debug.Log($"{npcName} quest marked complete");

        islandManagerScript.CheckCurrentIslandStatus();
    }

    private static readonly Regex sWhitespace = new Regex(@"\s+");
    public static string RemoveWhitespace(string input)
    {
        return sWhitespace.Replace(input, "");
    }

    public void FadeNPC()
    {
        if (npcFadeScript != null)
        {
            npcFadeScript.StartFade();
        }
    }
}