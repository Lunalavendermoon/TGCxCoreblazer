using System;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockHint : MonoBehaviour
{
    public float hintTimer;

    public GameObject helpButton;
    public GameObject levelManager;

    float timer = 0;

    public GameObject blockPrefab;
    public GameObject hintTextObject;

    TextMeshProUGUI hintText;

    int curId = 0;

    Dictionary<int, GameObject> blocks = new Dictionary<int, GameObject>();

    void Start()
    {
        hintText = hintTextObject.GetComponent<TextMeshProUGUI>();
        hintText.text = "";
    }

    public void initBlock(int id, BlockType type, Vector3 position, bool hflip, bool vflip, int rot, BlockLevelManager script, BlockGrid grid, int scalefact) {
        GameObject block = Instantiate(blockPrefab, position, Quaternion.identity);
        block.GetComponent<Block>().initBlock(id, type, script, grid, scalefact);

        SpriteRenderer renderer = block.GetComponent<SpriteRenderer>();
        if (hflip) {
            renderer.flipX = !renderer.flipX;
        }
        if (vflip) {
            renderer.flipY = !renderer.flipY;
        }

        for (int i = 0; i < rot; ++i) {
            block.transform.Rotate(0, 0, -90f);
        }

        // make sure block is in correct position after rotating
        block.GetComponent<Block>().placeBlockAt(position);
        block.GetComponent<Block>().hintColor();
        block.GetComponent<Renderer>().sortingOrder = 29999;

        // shouldn't be draggable
        block.GetComponent<Block>().setEnabled(false);

        // hide for now
        block.SetActive(false);

        blocks.Add(id, block);
    }

    void Update()
    {
        if (timer > 0.0f) {
            timer -= Time.deltaTime;
            return;
        }
        if (curId != 0) {
            GameObject obj;
            // if (GameManager.blockPositionArray.ContainsKey(curId)) {
            //     blocks[curId].SetActive(false);
            //     obj = blocks[curId];
            // } else {
            //     hintText.text = "";
            //     obj = hintTextObject;
            // }
            // obj.GetComponent<FlashingAnim>().SetAnimated(false);
            helpButton.GetComponent<HelpButton>().ButtonClickable(true);
            curId = 0;
        }
    }

    public void showBlock(int id) {
        timer = hintTimer;
        helpButton.GetComponent<HelpButton>().ButtonClickable(false);
        curId = id;
        GameObject obj;
        if (id == -1 || levelManager.GetComponent<BlockLevelManager>().metRequirements()) {
            hintText.text = "Click <color=#ffd666>Next Phase</color> to move on!";
            obj = hintTextObject;
        } else {
            // if (GameManager.blockPositionArray.ContainsKey(curId)) {
            //     blocks[curId].SetActive(true);
            //     obj = blocks[curId];
            // } else {
            //     string foodName = blocks[curId].GetComponent<Block>().blockType.displayName;
            //     hintText.text = "<color=#ffd666>" +
            //         char.ToUpperInvariant(foodName[0]) + foodName.Substring(1, foodName.Length - 1) +
            //         "</color> should not be on the grid!";
            //     obj = hintTextObject;
            // }
        }
        // obj.GetComponent<FlashingAnim>().SetAnimated(true);
    }
}