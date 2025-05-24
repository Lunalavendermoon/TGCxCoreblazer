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
        {"Community Memory", new Dictionary<string, string>
            {
                {"type", "Community" },
                {"description", "a description for Community memory"}
            }
        },
        {"Joy Memory", new Dictionary<string, string>
            {
                {"type", "Joy" },
                {"description", "a description for joy memory"}
            }
        },
        {"Love Memory", new Dictionary<string, string>
            {
                {"title", "Love" },
                {"description", "another description for love memory"}
            }
        }
    };

    [SerializeField]
    public static List<string> MemoryList = new List<string>();

    public Dictionary<string, string> GetMemoryInfo(string memoryName)
    {
        return MemoryInfo[memoryName];
    }

    //[YarnCommand("take_memory")]
    //public static void TakeMemory(string memoryName)
    //{
    //    MemoryList.Add(memoryName);
    //}

    //[YarnCommand("give_memory")]
    //public static void GiveMemory(string memoryName)
    //{
    //    MemoryList.Remove(memoryName);
    //}
}
