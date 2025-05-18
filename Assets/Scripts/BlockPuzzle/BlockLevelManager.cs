using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System;
using System.Numerics;
using DG.Tweening;

public class BlockLevelManager : MonoBehaviour
{
    public int day;
    public GameObject helpUiManager;
    public GameObject hintManager;
    public GameObject blockPrefab;

    public GameObject gameGrid;

    // 2.0f for big cell, 4.0f for small cell
    public int scalefact;

    public TMP_Text vegText;
    public TMP_Text proteinText;
    public TMP_Text carbText;

    public new Camera camera;
    public TMP_Text blockLabel;

    public GameObject sizeBar;


    public GameObject levelWarning;
    public float warningTimer;
    float timer;

    Grid grid;

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
        GameManager.LoadBlockData(day);

        blockLabel.text = "";

        maxSize = GameManager.blockMaxSize;
        maxNutrition = GameManager.blockMaxGroupSize;

        grid = gameGrid.GetComponent<Grid>();

        BlockType[] blocksToSpawn = GameManager.blockSpawnList;

        float ycarb = 2.5f;

        grid.initGrid(
            GameManager.blockGridArray, GameManager.blockXOffset, GameManager.blockYOffset, GameManager.blockSolutionArray, day
        );
        
        // BLOCK ID MUST BE 1 OR GREATER
        int id = 1;
        foreach (BlockType b in blocksToSpawn) {
            spawnBlock(id, b, id - 1, ycarb);
            id++;
        }

        updateUI();

        if (day == 1) {
            helpUiManager.GetComponent<HelpUIManager>().startTutorialDay1();
        } else if (day == 3) {
            helpUiManager.GetComponent<HelpUIManager>().startTutorialDay3();
        }
    }

    public void setPopupStatus(bool status) {
        if (status == popupIsOpen) {
            return;
        }
        popupIsOpen = status;
        deselectBlock();
        foreach (var block in blocks.Values) {
            block.GetComponent<Block>().setEnabled(!status);
        }
    }

    public void showHint() {
        int id = grid.getFirstMismatch();
        hintManager.GetComponent<BlockHintScript>().showBlock(id);
    }
    
    void spawnBlock(int id, BlockType type, int count, float yoffset) {
        int x = count % 5;
        int y = count / 5;
        UnityEngine.Vector3 position = new UnityEngine.Vector3(-7.0f + 1.2f * x, yoffset - 1.7f * y);
        if (scalefact == 3) {
            x = count < 12 ? count % 3 : (count - 12) % 2;
            y = count < 12 ? count / 3 : 4 + (count - 12) / 2;
            position = new UnityEngine.Vector3(-6.0f + 1.7f * x, yoffset - 1.3f * y);
        }
        UnityEngine.Vector3 jitter;
        if (count == 4 && scalefact == 4) {
            jitter = UnityEngine.Vector3.zero;
        } else {
            jitter = new UnityEngine.Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f));
        }
        GameObject block = Instantiate(blockPrefab, position + jitter, UnityEngine.Quaternion.identity);
        block.GetComponent<Block>().initBlock(id, type, this, grid, scalefact);
        blocks.Add(id, block);

        object[] transforms = new object[] {false, false, 0, 0, 0};
        if (GameManager.blockPositionArray.ContainsKey(id)) {
            transforms = GameManager.blockPositionArray[id];
        }

        UnityEngine.Vector3 hintPos = grid.arrayToWorld((int)transforms[3], (int)transforms[4]);
        // idk why these blocks specifically are broken but i guess i have to hardcode it now..
        if (day == 3 && (int)transforms[2] != 0) {
            hintPos += new UnityEngine.Vector3(-0.25f, -0.25f);
            if (type.name.Equals("eggplant")) {
                hintPos += new UnityEngine.Vector3(-0.25f, -0.25f);
            }
        }
        hintManager.GetComponent<BlockHintScript>().initBlock(
            id, type, hintPos, (bool)transforms[0], (bool)transforms[1], (int)transforms[2], this, grid, scalefact
        );
    }

    Block getBlock(int id) {
        GameObject block = blocks[id];
        return block.GetComponent<Block>();
    }

    public void playerAddBlock(int id) {
        blocks[id].GetComponent<Renderer>().sortingOrder = 0;
        Block block = getBlock(id);
        size += block.blockType.size;
        switch (block.blockType.foodGroup) {
            case "veg":
                nutrition[0] += block.blockType.size;
                break;
            case "carb":
                nutrition[1] += block.blockType.size;
                break;
            case "protein":
                nutrition[2] += block.blockType.size;
                break;
        }
        updateUI();
    }

    public void playerRemoveBlock(int id) {
        Block block = getBlock(id);
        size -= block.blockType.size;
        switch (block.blockType.foodGroup) {
            case "veg":
                nutrition[0] -= block.blockType.size;
                break;
            case "carb":
                nutrition[1] -= block.blockType.size;
                break;
            case "protein":
                nutrition[2] -= block.blockType.size;
                break;
        }
        updateUI();
    }

    void updateUI() {
        sizeBar.transform.localScale = new UnityEngine.Vector3(0.93f * Mathf.Min(((float)size) / maxSize, 1), 0.93f, 1);
        vegText.SetText(nutrition[0] + "/" + maxNutrition[0]);
        carbText.SetText(nutrition[1] + "/" + maxNutrition[1]);
        proteinText.SetText(nutrition[2] + "/" + maxNutrition[2]);
    }

    public void selectBlock(int id) {
        selectedBlock = id;
        blockLabel.text = getBlock(id).blockType.displayName;
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
        blockLabel.text = "";
        selectedBlock = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0.0f) {
            timer -= Time.deltaTime;
        } else {
            levelWarning.SetActive(false);
            levelWarning.GetComponent<FlashingAnim>().SetAnimated(false);
        }
        if (selectedBlock != -1) {
            UnityEngine.Vector3 anchor = getBlock(selectedBlock).getSpriteTopLeft();
            UnityEngine.Vector3 center = blocks[selectedBlock].transform.position;
            blockLabel.transform.position = camera.WorldToScreenPoint(new UnityEngine.Vector3(center.x, anchor.y + 0.1f, center.z));
            // space = confirm placement
            // R = rotate CW
            // D,F = flip horiz,vert
            if (Input.GetKeyDown(KeyCode.Space)) {
                getBlock(selectedBlock).placeBlock();
            } else if (Input.GetKeyDown(KeyCode.R)) {
                getBlock(selectedBlock).rotate();
            } else if (Input.GetKeyDown(KeyCode.H)) {
                getBlock(selectedBlock).flip(true);
            } else if (Input.GetKeyDown(KeyCode.V)) {
                getBlock(selectedBlock).flip(false);
            }
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
            levelWarning.GetComponent<FlashingAnim>().SetAnimated(true);
            AudioSFXManager.Instance.PlayAudio("bad");
            timer = warningTimer;
            return;
        }
        DOTween.KillAll();
        GameManager.StoreNutritionInfo(size, nutrition);
        ChangeScene.LoadNextSceneStatic();
    }
}