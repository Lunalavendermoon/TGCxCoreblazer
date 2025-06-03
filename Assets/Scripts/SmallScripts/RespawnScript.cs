using UnityEngine;
using System.Collections.Generic;
public class RespawnScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject respawnsFolder;
    [SerializeField] UIInputHandler uiInputHandlerScript;
    private List<Transform> respawnPositions;
    private int currentCheckPoint;

    Dictionary<string, int> respawnModifierCheckpoints;

    private void Start()
    {
        respawnModifierCheckpoints = new Dictionary<string, int>()
        {
            {"Start", 0},
            {"Island1", 1},
            {"Parkour1", 2},
            {"Island2", 3},
        };

        respawnPositions = new List<Transform>();
        currentCheckPoint = 1; //set to 0 later
        foreach(Transform respawn in respawnsFolder.transform)
        {
            respawnPositions.Add(respawn);
            //Debug.Log(respawn.gameObject.name);
        }
        
        if(respawnModifierCheckpoints.Count != respawnPositions.Count)
        {
            Debug.Log("RespawnScript.cs: Respawn area counts do not match with RespawnModifierCounts\n" +
                "double check that: \n" +
                "  1. All respawn modifiers have been added to respawnModifierCheckpoints dictionary\n" +
                "  2. All checkpoints have been added to the RespawnPositions folder in matching order");
        }
    }

    /*
     * note: index of respawn positions in respawnPositions corresponds to checkpoint number
     */
    private void OnCollisionEnter(Collision collision)
    {
        GameObject player = collision.gameObject;
        player.transform.position = respawnPositions[currentCheckPoint].position;
    }

    public void updateCheckpoint(string gameObjectName)
    {
        if(respawnModifierCheckpoints.ContainsKey(gameObjectName))
        {
            int collidedCheckpointNum = respawnModifierCheckpoints[gameObjectName];
            if (collidedCheckpointNum > currentCheckPoint)
            {
                currentCheckPoint = collidedCheckpointNum;
                uiInputHandlerScript.ShowNewSpawnPointUI();
            }
            //Debug.Log(currentCheckPoint);
        }
        else
        {
            Debug.Log(gameObjectName + "'s checkpoint number not found in respawnModifierCheckpoints");
        }
    }
}
