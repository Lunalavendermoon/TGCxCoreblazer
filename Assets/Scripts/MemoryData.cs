using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MemoryInventory", menuName = "Scriptable Objects/MemoryInventory")]
public class MemoryData : ScriptableObject
{

    static Dictionary<string, Dictionary<string, string>> MemoryInfo = new Dictionary<string, Dictionary<string, string>>()
    {
        //{"", new Dictionary<string, string>
        //    {
        //        {"type", "" },
        //        {"description", ""}
        //    }
        //},
        {"The Fellowship", new Dictionary<string, string>
            {
                {"type", "Community Memory" },
                {"description", "Through meadows and frost, the little bear bounded ahead. A rabbit, fox, and turtle followed, drawn together by adventure."}
            }
        },
        {"The Grove", new Dictionary<string, string>
            {
                {"type", "Community Memory" },
                {"description", "In the hush of morning light, hands reached across rows of earth, sharing tools, laughter, and the silent rhythm of planting side by side."}
            }
        },
        {"The Bloom", new Dictionary<string, string>
            {
                {"type", "Joy Memory" },
                {"description", "With soil-stained fingers, the gardener watched green push through earth — joy found not in the harvest, but in the act of tending.\r\n"}
            }
        },
    };

    [SerializeField]
    public static List<string> MemoryList = new List<string>();

    public static bool IsValidMemory(string memoryName)
    {
        return MemoryInfo.ContainsKey(memoryName);
    }

    public static Dictionary<string, string> GetMemoryInfo(string memoryName)
    {
        return MemoryInfo[memoryName];
    }

    public static string GetMemoryType(string memoryName)
    {
        return MemoryInfo[memoryName]["type"];
    }
}
