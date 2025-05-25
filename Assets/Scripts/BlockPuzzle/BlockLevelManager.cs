using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine.Rendering.Universal.Internal;
// using DG.Tweening;

public class BlockLevelManager : MonoBehaviour
{
    public float hintTimer;
    float timer = 0;
    public static int pixelsPerUnit = 60;
    public HelpUIManager helpUiManager;
    public BlockHint hintManager;
    public GameObject blockPrefab;
    public Canvas canvas;

    public new Camera camera;

    public GameObject infoPopup;

    Dictionary<int, GameObject> blocks = new Dictionary<int, GameObject>();
    // IMPORTANT: positions will be offset according to blockOffset
    // maps type of block (must use fullName to account for hflip/vflip) to list of all current positions on grid
    // Vector is (x position, y position, ID)
    Dictionary<string, List<Vector3Int>> blockLocations = new Dictionary<string, List<Vector3Int>>();
    // TODO decide what to do w/ this guy
    Dictionary<Vector3Int, string> solution = new Dictionary<Vector3Int, string>();

    bool popupIsOpen = false;

    public static Vector3 blockOffset = new Vector3(3, -2, 0);

    void Start()
    {
        // TODO call from another manager?
        initLevel(1);
    }

    void initLevel(int day)
    {
        infoPopup.SetActive(false);

        blocks.Clear();
        blockLocations.Clear();
        solution.Clear();
        // TODO load new solution

        hintManager.initLevel();

        GameObject[] gos = GameObject.FindGameObjectsWithTag("PuzzleBlock");
        foreach (GameObject go in gos)
            Destroy(go);

        // this.day = day;
        // GameManager.LoadBlockData(day);

        // maxSize = GameManager.blockMaxSize;
        // maxNutrition = GameManager.blockMaxGroupSize;

        // BlockType[] blocksToSpawn = GameManager.blockSpawnList;

        float ycarb = 25f;

        // BLOCK ID MUST BE 1 OR GREATER
        // int id = 1;
        // foreach (BlockType b in blocksToSpawn) {
        //     spawnBlock(id, b, id - 1, ycarb);
        //     id++;
        // }
        spawnBlock(1, BlockType.bigSquare(), 0, ycarb, false, false);
        spawnBlock(2, BlockType.bigSquare(), 1, ycarb, false, false);
        spawnBlock(3, BlockType.smallSquare(), 2, ycarb, false, false);
        spawnBlock(4, BlockType.smallTriangle(), 3, ycarb, false, false);
        spawnBlock(5, BlockType.bigTriangle(), 4, ycarb, false, false);
        spawnBlock(6, BlockType.bigCircle(), 5, ycarb, false, false);
        spawnBlock(7, BlockType.quarterCircle2(), 6, ycarb, false, false);
        spawnBlock(8, BlockType.bigTriangle(), 7, ycarb, true, false);
        spawnBlock(9, BlockType.smallTriangle(), 8, ycarb, true, false);

        updateUI();

        // if (day == 1) {
        //     helpUiManager.startTutorialDay1();
        // } else if (day == 3) {
        //     helpUiManager.startTutorialDay3();
        // }
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
    }

    public void showHint()
    {
        if (timer > 0)
        {
            return;
        }
        Debug.Log("Show a hint");
        timer = hintTimer;
        int id = getFirstMismatch();
        // TODO
        // hintManager.showBlock(id);
    }

    int getFirstMismatch()
    {
        // TODO scan solution and make sure all entries are in player's placement
        // TODO do the same for player placement vs solution
        return -1;
    }

    void spawnBlock(int id, BlockType type, int count, float yoffset, bool hflip, bool vflip)
    {
        int x = count % 4;
        int y = count / 4;

        Vector3 position = new Vector3(-60f + 12f * x, yoffset - 17f * y, 0);
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

        Vector3 hintPos = new Vector3((int)transforms[3], (int)transforms[4]);
        hintManager.initBlock(
            id, type, hintPos, this, canvas
        );
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
        // TODO
        Debug.Log("Check if player has finished the puzzle");
        return false;
    }

    // horiz range: -40 to 440 -> -60 to 420
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
        Debug.Log("Block " + id + " with shape " + type + " placed at " + newPos);
        string key = type.fullName();
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
        Debug.Log("Block " + id + " with shape " + type + " removed from grid");
        string key = type.fullName();
        if (blockLocations.ContainsKey(key))
        {
            for (int i = 0; i < blockLocations[key].Count; ++i)
            {
                if (blockLocations[key][i].z == id)
                {
                    blockLocations[key].RemoveAt(i);
                    return;
                }
            }
        }
    }

    public void toggleInfoPopup()
    {
        infoPopup.SetActive(!infoPopup.activeSelf);
    }
}