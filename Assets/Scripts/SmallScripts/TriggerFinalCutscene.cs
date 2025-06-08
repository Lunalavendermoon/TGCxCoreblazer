using UnityEngine;

public class TriggerFinalCutscene : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger touched");
        if(other.gameObject.name == "Player")
        {
            Debug.Log("play cutscene");
        }
    }
}
