using System;
using UnityEngine;

public class IndicatorShow : MonoBehaviour
{
    public GameObject memoryIndicator;
    private bool memoryAvailable = true;

    void Start()
    {
        memoryIndicator.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && memoryAvailable)
        {
            memoryIndicator.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && memoryAvailable)
        {
            memoryIndicator.SetActive(false);
        }
    }

    public void MemoryUnavailable()
    {
        memoryAvailable = false;
        memoryIndicator.SetActive(false);
    }
}
