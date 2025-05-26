using System;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockHint : MonoBehaviour
{
    public BlockLevelManager levelManager;

    public GameObject blockPrefab;

    int curId = 0;

    Dictionary<int, GameObject> blocks = new Dictionary<int, GameObject>();

    void Start()
    {
        // hintText = hintTextObject.GetComponent<TextMeshProUGUI>();
        // hintText.text = "";
    }

    public void initLevel()
    {
        blocks.Clear();
    }

    public void initBlock(int id, BlockType type, Vector3 position, BlockLevelManager script, Canvas canvas)
    {
        GameObject block = Instantiate(blockPrefab, position, Quaternion.identity, canvas.transform);
        Vector3 blockpos = block.GetComponent<RectTransform>().anchoredPosition3D;
        blockpos.z = 0f;
        block.GetComponent<RectTransform>().anchoredPosition3D = blockpos;

        block.GetComponent<Block2>().initBlock(id, type, script, canvas);

        block.GetComponent<Block2>().hintColor();

        // shouldn't be draggable
        block.GetComponent<Block2>().setEnabled(false);

        // hide for now
        block.SetActive(false);

        blocks.Add(id, block);
    }


    public void showBlock(int id)
    {
        curId = id;
        blocks[curId].SetActive(true);
        GameObject obj = blocks[curId];
        obj.transform.SetAsLastSibling(); // bring to front
        // TODO flashing animation on obj?
    }

    public void hideBlock(int id)
    {
        // TODO
    }
}