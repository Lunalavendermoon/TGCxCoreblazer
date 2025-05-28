using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Rendering.Universal.Internal;
using Unity.Multiplayer.Center.Common;
// using DG.Tweening;

public class BlockLevelManager : MonoBehaviour
{
    public float hintTimer;
    float timer = 0;
    public static int pixelsPerUnit = 60;
    public BlockHint hintManager;
    public GameObject blockPrefab;
    public Canvas canvas;

    public new Camera camera;

    Dictionary<int, GameObject> blocks = new Dictionary<int, GameObject>();
    // IMPORTANT: positions will be offset according to blockOffset
    // maps type of block (must use fullName to account for hflip/vflip) to list of all current positions on grid
    // Vector is (x position, y position, ID)
    Dictionary<string, List<Vector3Int>> blockLocations = new Dictionary<string, List<Vector3Int>>();
    // Same as blockLocations but without a z coordinate
    Dictionary<string, List<Vector2Int>> solution = new Dictionary<string, List<Vector2Int>>();

    bool popupIsOpen = false;

    public static Vector3Int blockOffset = new Vector3Int(3, -2, 0);

    int hintedBlock = -1;

    void Start()
    {
        // TODO call from another manager?

        // format: name of block, position of top left corner (row, col)
        initLevel(new string[] {
            "bigSquareFF,0,0",
            "smallTriangleTF,0,0",
            "smallTriangle2TT,0,0",
            "bigCircleFF,0,0",
            "quarterCircle4FF,0,0"
        });
    }

    void initLevel(string[] solutionArray)
    {
        blocks.Clear();
        blockLocations.Clear();
        hintManager.initLevel();

        GameObject[] gos = GameObject.FindGameObjectsWithTag("PuzzleBlock");
        foreach(GameObject go in gos)
            Destroy(go);


        int id = 0;
        solution.Clear();
        foreach (string blockSoln in solutionArray)
        {
            string[] components = blockSoln.Split(",");
            Vector2Int pos = new Vector2Int(
                -60 + int.Parse(components[2]) * pixelsPerUnit + blockOffset.x,
                240 - (int.Parse(components[1]) * pixelsPerUnit) + blockOffset.y
            );
            if (solution.ContainsKey(components[0]))
            {
                solution[components[0]].Add(pos);
            }
            else
            {
                List<Vector2Int> lst = new List<Vector2Int>()
                {
                    pos
                };
                solution[components[0]] = lst;
            }
            string name = components[0].Substring(0, components[0].Length - 2);

            spawnBlock(id+1, BlockType.stringToBlock(name), id,
                charToBool(components[0][components[0].Length-2]), charToBool(components[0][components[0].Length-1]),
                new Vector3Int(pos.x, pos.y, 0));
            ++id;
        }

        bool charToBool(char b)
        {
            return b == 'T';
        }

        updateUI();
    }

    public void setPopupStatus(bool status)
    {
        if (status == popupIsOpen)
        {
            return;
        }
        popupIsOpen = status;
        deselectBlock();
        foreach (var block in blocks.Values)
        {
            block.GetComponent<Block2>().setEnabled(!status);
        }
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }
        if (hintedBlock != -1)
        {
            hintManager.hideBlock(hintedBlock);
            hintedBlock = -1;
        }
    }

    public void showHint()
    {
        if (timer > 0)
        {
            return;
        }
        timer = hintTimer;
        int id = getFirstMismatch();
        hintedBlock = id;
        hintManager.showBlock(id);
    }

    int getFirstMismatch()
    {
        foreach (KeyValuePair<string, List<Vector3Int>> kv in blockLocations)
        {
            foreach (Vector3Int pos in kv.Value)
            {
                Vector2Int lookup = new Vector2Int(pos.x, pos.y);
                int id = pos.z;

                if (!solution.ContainsKey(kv.Key) || !solution[kv.Key].Contains(lookup))
                {
                    return id;
                }
            }
        }
        return -1;
    }

    void spawnBlock(int id, BlockType type, int count, bool hflip, bool vflip, Vector3Int hintPos)
    {
        int x = count % 4;
        int y = count / 4;

        Vector3 position = new Vector3(-60f + 12f * x, 25f - 17f * y, 0);
        Vector3 jitter = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0);

        GameObject block = Instantiate(blockPrefab, position + jitter, Quaternion.identity, canvas.transform);
        Vector3 blockpos = block.GetComponent<RectTransform>().anchoredPosition3D;
        blockpos.z = 0f;
        block.GetComponent<RectTransform>().anchoredPosition3D = blockpos;

        type.hflipped = hflip;
        type.vflipped = vflip;
        block.GetComponent<Block2>().initBlock(id, type, this, canvas);
        blocks.Add(id, block);

        object[] transforms = new object[] { false, false, 0, 0, 0 };
        // if (GameManager.blockPositionArray.ContainsKey(id)) {
        //     transforms = GameManager.blockPositionArray[id];
        // }

        hintManager.initBlock(
            id, type, hintPos, this, canvas
        );

        addBlock(type, id, new Vector3Int(0,0,id));
    }

    Block2 getBlock(int id)
    {
        GameObject block = blocks[id];
        return block.GetComponent<Block2>();
    }

    void updateUI()
    {
        // TODO
    }

// TODO: if there's no use for this we can delete it.
    public void selectBlock(int id)
    {
        // selectedBlock = id;
        // blocks[id].GetComponent<Renderer>().sortingOrder = orderCount++;
        // // this will probably never happen but yknow, just in case lol
        // if (orderCount == 30000)
        // {
        //     int minOrder = orderCount + 1, maxOrder = 0;
        //     foreach (var value in blocks.Values)
        //     {
        //         if (value.GetComponent<Renderer>().sortingOrder == 0)
        //         {
        //             continue;
        //         }
        //         minOrder = Math.Min(value.GetComponent<Renderer>().sortingOrder, minOrder);
        //     }
        //     foreach (var value in blocks.Values)
        //     {
        //         value.GetComponent<Renderer>().sortingOrder = minOrder;
        //         maxOrder = Math.Max(value.GetComponent<Renderer>().sortingOrder, maxOrder);
        //     }
        //     orderCount = maxOrder + 1;
        // }
    }

    public void deselectBlock()
    {
        // selectedBlock = -1;
    }

    public bool metRequirements()
    {
        return getFirstMismatch() == -1;
    }

    // horiz range: -60 to 420
    // vert range: -360 to 240
    // cell size: 60 x 60
    public Vector3Int snapToGrid(Vector3 world, BlockType blockType)
    {
        return new Vector3Int(Mathf.RoundToInt(world.x / pixelsPerUnit) * pixelsPerUnit,
                                Mathf.RoundToInt(world.y / pixelsPerUnit) * pixelsPerUnit + blockType.height * pixelsPerUnit);
    }

    public bool checkBlockPosition(Vector3 pos, BlockType blockType)
    {
        Vector3 gridPos = snapToGrid(pos, blockType);
        return gridPos.x >= -60 && gridPos.x + blockType.width * pixelsPerUnit <= 420 &&
                gridPos.y <= 240 && gridPos.y - blockType.height * pixelsPerUnit >= -300;
    }

    public void updateBlock(int id, BlockType type, Vector3Int newPos)
    {
        removeBlock(type, id);
        addBlock(type, id, newPos);
    }

    Vector3Int encodeBlockPos(int id, Vector3Int newPos)
    {
        return new Vector3Int(newPos.x, newPos.y, id);
    }

    public void addBlock(BlockType type, int id, Vector3Int newPos)
    {
        string key = type.fullName();
        // Debug.Log("Block " + id + " with shape " + key + " placed at " + newPos);
        if (blockLocations.ContainsKey(key))
        {
            for (int i = 0; i < blockLocations[key].Count; ++i)
            {
                if (blockLocations[key][i].z == id)
                {
                    blockLocations[key][i] = encodeBlockPos(id, newPos);
                    break;
                }
            }
        }
        else
        {
            List<Vector3Int> lst = new List<Vector3Int>
            {
                encodeBlockPos(id, newPos)
            };
            blockLocations.Add(key, lst);
        }
    }

    public void removeBlock(BlockType type, int id)
    {
        string key = type.fullName();
        // Debug.Log("Block " + id + " with shape " + key + " removed from grid");
        if (blockLocations.ContainsKey(key))
        {
            for (int i = 0; i < blockLocations[key].Count; ++i)
            {
                if (blockLocations[key][i].z == id)
                {
                    blockLocations[key][i] = new Vector3Int(0,0,id);
                    return;
                }
            }
        }
    }
}