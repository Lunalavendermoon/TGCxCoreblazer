using UnityEngine;

public class HelpButton : MonoBehaviour
{
    public GameObject levelManager;
    private bool click = true;


    void OnMouseDown() {
        if (click) {
            AudioSFXManager.Instance.PlayAudio("ding");
            levelManager.GetComponent<BlockLevelManager>().showHint();
        }
    }

    public void ButtonClickable (bool clickable) {
        click = clickable;
    }
}