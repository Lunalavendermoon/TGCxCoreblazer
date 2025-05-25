using System.Collections.Generic;
using UnityEngine;

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
}
