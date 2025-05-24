using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

[CreateAssetMenu(fileName = "MemoryInventory", menuName = "Scriptable Objects/MemoryInventory")]
public class MemoryData : ScriptableObject
{

    Dictionary<string, Dictionary<string, string>> MemoryInfo = new Dictionary<string, Dictionary<string, string>>()
    {
        {"Joy", new Dictionary<string, string>
            {
                {"title", "Joy" },
                {"description", "a description for joy memory"}
            }
        },
        {"Love", new Dictionary<string, string>
            {
                {"type", "Love" },
                {"description", "another description for love memory"}
            }
        }
    };

    [SerializeField]
    public List<string> MemoryList = new List<string>();

    public Dictionary<string, string> GetMemoryInfo(string memoryName)
    {
        return MemoryInfo[memoryName];
    }

    [YarnCommand("add_memory")]
    public void AddMemory(string memoryName)
    {
        MemoryList.Add(memoryName);
        Debug.Log($"{memoryName} added");
        Debug.Log("Current MemoryList: ");
        foreach(string memory in MemoryList)
        {
            Debug.Log(memory);
        }
    }
}
