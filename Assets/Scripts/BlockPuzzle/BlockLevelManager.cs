using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;
// using DG.Tweening;

public class BlockLevelManager : MonoBehaviour
{
    int day;
    public HelpUIManager helpUiManager;
    public BlockHint hintManager;
    public GameObject blockPrefab;
    public Canvas canvas;

    public BlockGrid grid;

    public new Camera camera;


    public GameObject levelWarning;
    public float warningTimer;
    float timer;

    Dictionary<int, GameObject> blocks = new Dictionary<int, GameObject>();

    int selectedBlock = -1;
    int orderCount = 1;

    int size = 0;
    int maxSize;

    int[] nutrition = { 0, 0, 0 };
    int[] maxNutrition;

    bool popupIsOpen = false;

    void Start()
    {
        // TODO call from another manager?
        initLevel(1);
    }

    void initLevel(int day)
    {
        this.day = day;
        // GameManager.LoadBlockData(day);

        // maxSize = GameManager.blockMaxSize;
        // maxNutrition = GameManager.blockMaxGroupSize;

        // BlockType[] blocksToSpawn = GameManager.blockSpawnList;

        float ycarb = 25f;

        grid.initGrid(
            new Dictionary<int, Vector2>(), 8, 7, 0, 5, day
        );

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

    public void showHint()
    {
        int id = grid.getFirstMismatch();
        hintManager.showBlock(id);
    }

    void spawnBlock(int id, BlockType type, int count, float yoffset, bool hflip, bool vflip)
    {
        int x = count % 5;
        int y = count / 5;
        Vector3 position = new Vector3(-70f + 12f * x, yoffset - 17f * y, 0);
        Vector3 jitter = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), 0);
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

        Vector3 hintPos = grid.arrayToWorld((int)transforms[3], (int)transforms[4]);
        hintManager.initBlock(
            id, type, hintPos, (bool)transforms[0], (bool)transforms[1], (int)transforms[2], this, grid
        );
    }

    Block2 getBlock(int id)
    {
        GameObject block = blocks[id];
        return block.GetComponent<Block2>();
    }

    public void playerAddBlock(int id)
    {
        Block2 block = getBlock(id);
        // TODO update stuff - check if player solution is correct, etc
        updateUI();
    }

    public void playerRemoveBlock(int id)
    {
        Block2 block = getBlock(id);
        // TODO
        updateUI();
    }

    void updateUI()
    {
        // TODO
    }

    public void selectBlock(int id)
    {
        selectedBlock = id;
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
        selectedBlock = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            // levelWarning.SetActive(false);
            // levelWarning.GetComponent<FlashingAnim>().SetAnimated(false);
        }
    }

    public bool metRequirements()
    {
        for (int i = 0; i < 3; ++i)
        {
            if (nutrition[i] < maxNutrition[i])
            {
                return false;
            }
        }
        return true;
    }

    public void toNextLvl()
    {
        if (!metRequirements())
        {
            levelWarning.SetActive(true);
            // levelWarning.GetComponent<FlashingAnim>().SetAnimated(true);
            // AudioSFXManager.Instance.PlayAudio("bad");
            timer = warningTimer;
            return;
        }
        // DOTween.KillAll();
        // GameManager.StoreNutritionInfo(size, nutrition);
        // ChangeScene.LoadNextSceneStatic();
    }

    public bool checkBlockPosition(Vector3 pos, BlockType blockType)
    {
        // TODO
        return true;
    }

    public void updateBlock(int id, Vector3 position, string blockType)
    {
        removeBlock(id);
        addBlock(id, position, blockType);
    }

    public void addBlock(int id, Vector3 position, string blockType)
    {
        // TODO
    }

    public void removeBlock(int id)
    {
        // TODO remove from list
    }

    public Vector3Int snapToGrid(Vector3 world) {
        Vector3 og = world;
        return new Vector3Int(Mathf.RoundToInt(og.x), Mathf.RoundToInt(og.y) - 1);
    }
}