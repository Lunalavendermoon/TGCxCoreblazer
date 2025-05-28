using System.Collections.Generic;
using UnityEngine;

public class BlockHint : MonoBehaviour
{
    public BlockLevelManager levelManager;

    public GameObject blockPrefab;

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

        block.GetComponent<Block2>().initBlock(id, type, script, canvas);
        block.GetComponent<Block2>().placeBlockAt(position);

        block.GetComponent<Block2>().hintColor();

        // shouldn't be draggable
        block.GetComponent<Block2>().setEnabled(false);

        // hide for now
        block.SetActive(false);

        blocks.Add(id, block);
    }


    public void showBlock(int id)
    {
        blocks[id].SetActive(true);
        GameObject obj = blocks[id];
        obj.transform.SetAsLastSibling(); // bring to front
        // TODO flashing animation on obj?
    }

    public void hideBlock(int id)
    {
        blocks[id].SetActive(false);
        // TODO deactivate flashing animation?
    }
}