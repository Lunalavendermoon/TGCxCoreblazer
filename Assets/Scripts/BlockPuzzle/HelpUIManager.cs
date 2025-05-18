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

    public void startTutorialDay1() {
        initializeStuff();
        setStatus(true);
        closeButton.SetActive(false);
        startTutorialFromButton(1);
    }

    public void startTutorialDay3() {
        initializeStuff();
        setStatus(true);
        closeButton.SetActive(false);
        startTutorialFromButton(3);
    }

    public void startTutorialFromButton(int day) {
        tutorialContent.SetActive(true);
        blockLevelManager.setPopupStatus(true);
        helpButton.GetComponent<HelpButton>().ButtonClickable(false);

        if (day == 1 || day == 3) {
            // dialogueManager.StartDialogueFromDay(day);
        } else {
            // dialogueManager.StartDialogueFromTutorial();
        }
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