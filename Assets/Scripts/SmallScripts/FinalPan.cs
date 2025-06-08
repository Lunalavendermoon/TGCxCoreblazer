using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class FinalPan : MonoBehaviour
{
    [SerializeField] GameObject finalPanParent;
    [SerializeField] GameObject playerCamera;
    [SerializeField] GameObject player;
    private bool finalPanFinished;

    private void Start()
    {
        //startPan();
        finalPanFinished = false;
    }
    public void startPan()
    {
        StartCoroutine(playPanningAnimation());
    }
    private IEnumerator playPanningAnimation()
    {
        player.transform.position = new Vector3(54.39f, -6.66f, -51.05f);
        playerCamera.SetActive(false);

        GameObject prevCamera = null;
        foreach (Transform camera in finalPanParent.transform)
        {
            GameObject cameraObject = camera.gameObject;
            //Debug.Log(cameraObject.name + " activated");

            cameraObject.SetActive(true);
            if(prevCamera)
            {
                prevCamera.SetActive(false);
            }
            prevCamera = cameraObject;
            yield return new WaitForSeconds(2f);
        }

        finalPanFinished = true;
    }

    public bool getPanFinished()
    {
        return finalPanFinished;
    }
}
