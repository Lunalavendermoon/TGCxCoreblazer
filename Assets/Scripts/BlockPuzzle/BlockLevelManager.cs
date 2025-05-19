using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;
using System.Numerics;
// using DG.Tweening;

public class BlockLevelManager : MonoBehaviour
{
    int day;
    public HelpUIManager helpUiManager;
    public BlockHint hintManager;
    public GameObject blockPrefab;

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

    int[] nutrition = {0,0,0};
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

        float ycarb = 2.5f;

        grid.initGrid(
            new Dictionary<int, UnityEngine.Vector2>(), 9, 8, 0, 5, day
        );

        // BLOCK ID MUST BE 1 OR GREATER
        // int id = 1;
        // foreach (BlockType b in blocksToSpawn) {
        //     spawnBlock(id, b, id - 1, ycarb);
        //     id++;
        // }
        spawnBlock(1, BlockType.square(), 0, 2.5f);
        spawnBlock(2, BlockType.square(), 1, 2.5f);
        spawnBlock(3, BlockType.square(), 2, 2.5f);

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
            block.GetComponent<Block>().setEnabled(!status);
        }
    }

    public void showHint() {
        int id = grid.getFirstMismatch();
        hintManager.showBlock(id);
    }
    
    void spawnBlock(int id, BlockType type, int count, float yoffset) {
        int x = count % 5;
        int y = count / 5;
        UnityEngine.Vector3 position = new UnityEngine.Vector3(-7.0f + 1.2f * x, yoffset - 1.7f * y);
        UnityEngine.Vector3 jitter = new UnityEngine.Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f));
        GameObject block = Instantiate(blockPrefab, position + jitter, UnityEngine.Quaternion.identity);
        block.GetComponent<Block>().initBlock(id, type, this, grid);
        blocks.Add(id, block);

        object[] transforms = new object[] {false, false, 0, 0, 0};
        // if (GameManager.blockPositionArray.ContainsKey(id)) {
        //     transforms = GameManager.blockPositionArray[id];
        // }

        UnityEngine.Vector3 hintPos = grid.arrayToWorld((int)transforms[3], (int)transforms[4]);
        // idk why these blocks specifically are broken but i guess i have to hardcode it now..
        hintManager.initBlock(
            id, type, hintPos, (bool)transforms[0], (bool)transforms[1], (int)transforms[2], this, grid
        );
    }

    Block getBlock(int id) {
        GameObject block = blocks[id];
        return block.GetComponent<Block>();
    }

    public void playerAddBlock(int id) {
        blocks[id].GetComponent<Renderer>().sortingOrder = 0;
        Block block = getBlock(id);
        // TODO
        updateUI();
    }

    public void playerRemoveBlock(int id) {
        Block block = getBlock(id);
        // TODO
        updateUI();
    }

    void updateUI()
    {
        // TODO
    }

    public void selectBlock(int id) {
        selectedBlock = id;
        blocks[id].GetComponent<Renderer>().sortingOrder = orderCount++;
        // this will probably never happen but yknow, just in case lol
        if (orderCount == 30000) {
            int minOrder = orderCount+1, maxOrder = 0;
            foreach (var value in blocks.Values) {
                if (value.GetComponent<Renderer>().sortingOrder == 0) {
                    continue;
                }
                minOrder = Math.Min(value.GetComponent<Renderer>().sortingOrder, minOrder);
            }
            foreach (var value in blocks.Values) {
                value.GetComponent<Renderer>().sortingOrder = minOrder;
                maxOrder = Math.Max(value.GetComponent<Renderer>().sortingOrder, maxOrder);
            }
            orderCount = maxOrder + 1;
        }
    }

    public void deselectBlock() {
        selectedBlock = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0.0f) {
            timer -= Time.deltaTime;
        } else {
            // levelWarning.SetActive(false);
            // levelWarning.GetComponent<FlashingAnim>().SetAnimated(false);
        }
    }

    public bool metRequirements() {
        for (int i = 0; i < 3; ++i) {
            if (nutrition[i] < maxNutrition[i]) {
                return false;
            }
        }
        return true;
    }

    public void toNextLvl() {
        if (!metRequirements()) {
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
}