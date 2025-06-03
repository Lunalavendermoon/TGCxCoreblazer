using NUnit.Framework;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
    private int islandNumber;

    // Possible values: "Incomplete", "Complete"
    Dictionary<string, string> questStatus = new Dictionary<string, string>
    {
        //no longer need to manually enter values, game will fill in automatically
        //{"BabyBear", "Incomplete"},
        //{"ElderlyGardener", "Incomplete"},
        //{"Sapling", "Incomplete"}
    };

    List<List<string>> islandQuests = new List<List<string>>()
    {
        new List<string> { }, //start island (0)
        new List<string>{"BabyBear", "Adventurer", "LittleGirl"}, // island 1
        new List<string>{"Gardener", "Bird"} // island 2

        /*
         * islands list<island> - contains islandNumber, and list of all islands
         * island - contains list of quests
         * quest - name, status
         */
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
        islandNumber = 1; //set to 0 later
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
        npcName = RemoveWhitespace(npcName);
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
        //names have no extra whitespace
        npcName = RemoveWhitespace(npcName);
        questStatus[npcName] = "Complete";
        Debug.Log($"{npcName} quest marked complete");
        CheckCurrentIslandStatus();
        foreach(var (key, val) in questStatus)
        {
            Debug.Log(key + ": " + val);
        }
    }

    private static readonly Regex sWhitespace = new Regex(@"\s+");
    public static string RemoveWhitespace(string input)
    {
        return sWhitespace.Replace(input, "");
    }

    public void CheckCurrentIslandStatus()
    {
        //current island number is valid
        if(islandNumber < islandQuests.Count)
        {
            Debug.Log("got into islandstatus check");
            List<string> currentQuests = islandQuests[islandNumber];
            if (containsOnlyCompleteQuests(currentQuests))
            {
                //show platforms
                Debug.Log("Island " + islandNumber + " complete");
                islandNumber += 1;
            }
        }
        /*
         * if islandNumber < islandQuests.length
         * if all quests for current island are complete
         * show platforms
         * print island 1 complete
         * add 1 to islandNumber
         */
    }

    public bool containsOnlyCompleteQuests(List<string> quests)
    {
        foreach (string quest in quests)
        {
            if (!questStatus.ContainsKey(quest) || questStatus[quest] != "Complete")
            {
                Debug.Log(quest + " not contained or not complete yet for Island " + islandNumber);
                return false;
            }
        }
        return true;
    }
    public void FadeNPC()
    {
        if (npcFadeScript != null)
        {
            npcFadeScript.StartFade();
        }
    }
}