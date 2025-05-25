using UnityEngine;

public class HelpUIManager : MonoBehaviour
{
    public BlockLevelManager blockLevelManager;
    // public GameObject tutorialDialogueManager;
    public GameObject tutorialContent;
    public GameObject helpBg;
    public GameObject helpButton;
    public GameObject closeButton;

    // TutorialDialogueManager dialogueManager;

    void initializeStuff() {
        // if (dialogueManager == null) {
        //     dialogueManager = tutorialDialogueManager.GetComponent<TutorialDialogueManager>();
        // }
    }

    void Start() {
        initializeStuff();
        // if (GameManager.day == 2) {
        //     hideHelp();
        // }
    }

    public void hideHelpFromTutorial() {
        closeButton.SetActive(true);
        hideHelp();
    }

    public void showHelp() {
        setStatus(true);
        tutorialContent.SetActive(false);
    }

    public void hideHelp() {
        setStatus(false);
    }

    void setStatus(bool status) {
        helpBg.SetActive(status);
        blockLevelManager.setPopupStatus(status);
    }
}