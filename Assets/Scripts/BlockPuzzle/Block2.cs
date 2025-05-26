using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor.Tilemaps;

public class Block2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    BlockLevelManager levelManager;
    new Image renderer;

    public int id {get; set;}

    public BlockType blockType {get; set;}

    public bool isOnGrid {get; set;}
    
    Vector3 resetPosition;

    public Sprite bigSquare;
    public Sprite smallSquare;
    public Sprite bigTriangle;
    public Sprite bigTriangle2;
    public Sprite bigTriangle3;
    public Sprite bigTriangle4;
    public Sprite smallTriangle;
    public Sprite smallTriangle2;
    public Sprite smallTriangle3;
    public Sprite smallTriangle4;
    public Sprite bigCircle;
    public Sprite quarterCircle;
    public Sprite quarterCircle2;
    public Sprite quarterCircle3;
    public Sprite quarterCircle4;

    bool isEnabled = true;
    bool selected = false;

    private Vector3 screenPoint;
    private Vector3 offset;

    private RectTransform rectTransform;
    private Canvas canvas;

    public void initBlock(int id, BlockType type, BlockLevelManager levelManager, Canvas canvas)
    {
        // GetComponent<FlashingAnim>().SetAnimated(false);
        rectTransform = GetComponent<RectTransform>();
        this.canvas = canvas;

        this.id = id;
        isOnGrid = false;
        blockType = type;
        this.levelManager = levelManager;
        resetPosition = transform.position;

        renderer = GetComponent<Image>();
        renderer.sprite = spriteByName(type.name);

        rectTransform.sizeDelta = new Vector2(type.width, type.height) * BlockLevelManager.pixelsPerUnit;

        rectTransform.localScale = new Vector3(
            rectTransform.localScale.x * (type.hflipped ? -1 : 1), rectTransform.localScale.y * (type.vflipped ? -1 : 1),
            rectTransform.localScale.z);
    }

    public Sprite spriteByName(string name) {
        switch (name)
        {
            case "bigSquare":
                return bigSquare;
            case "smallSquare":
                return smallSquare;
            case "bigTriangle":
                return bigTriangle;
            case "bigTriangle2":
                return bigTriangle2;
            case "bigTriangle3":
                return bigTriangle3;
            case "bigTriangle4":
                return bigTriangle4;
            case "smallTriangle":
                return smallTriangle;
            case "smallTriangle2":
                return smallTriangle2;
            case "smallTriangle3":
                return smallTriangle3;
            case "smallTriangle4":
                return smallTriangle4;
            case "bigCircle":
                return bigCircle;
            case "quarterCircle":
                return quarterCircle;
            case "quarterCircle2":
                return quarterCircle2;
            case "quarterCircle3":
                return quarterCircle3;
            case "quarterCircle4":
                return quarterCircle4;
            default:
                return bigSquare;
        }
    }

    public void setEnabled(bool enabled)
    {
        isEnabled = enabled;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isEnabled)
        {
            return;
        }
        if (isOnGrid)
        {
            removeFromGrid();
        }
        levelManager.selectBlock(id);

        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        transform.SetAsLastSibling(); // bring to front
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move the UI element
        rectTransform.anchoredPosition3D += new Vector3(eventData.delta.x, eventData.delta.y, 0) / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isEnabled)
        {
            return;
        }

        Vector3 pos = getSpriteTopLeft();

        if (levelManager.checkBlockPosition(pos, blockType))
        {
            makeOpaque();

            Vector3Int snap = levelManager.snapToGrid(pos, blockType) + BlockLevelManager.blockOffset;

            if (isOnGrid)
            {
                levelManager.updateBlock(id, blockType, snap);
            }
            else
            {
                isOnGrid = true;
                levelManager.addBlock(blockType, id, snap);
                levelManager.deselectBlock();
            }
            placeBlockAt(snap);
        }
        else
        {
            transform.position = resetPosition;
            makeOpaque();
        }

        if (levelManager.metRequirements())
        {
            Debug.Log("Correct solution! Yippee!");
            // TODO exit da minigame
        }
    }

    public Vector3 getSpriteTopLeft()
    {
        Vector3 pos = rectTransform.anchoredPosition3D;
        return new Vector3(pos.x - blockType.width * BlockLevelManager.pixelsPerUnit / 2,
                            pos.y - blockType.height * BlockLevelManager.pixelsPerUnit / 2, 0);
    }

    void removeFromGrid() {
        isOnGrid = false;
        levelManager.removeBlock(blockType, id);
    }

    void makeTransparent() {
        if (selected == false)
        {
            // AudioSFXManager.Instance.PlayAudio("pop");
            selected = true;
        }
        Color col = renderer.color;
        col.a = 0.8f;
        renderer.color = col;
    }

    void makeOpaque() {
        if (selected == true)
        {
            // AudioSFXManager.Instance.PlayAudio("pop");
            selected = false;
        }
        Color col = renderer.color;
        col.a = 1;
        renderer.color = col;
    }

    public void placeBlockAt(Vector3 position)
    {
        // AudioSFXManager.Instance.PlayAudio("thump");
        rectTransform.anchoredPosition3D = new Vector3(position.x + blockType.width * BlockLevelManager.pixelsPerUnit / 2,
                                                        position.y - blockType.height * BlockLevelManager.pixelsPerUnit / 2, 0);
    }

    public void hintColor()
    {
        float H, S;
        Color.RGBToHSV(renderer.color, out H, out S, out _);

        renderer.color = Color.HSVToRGB(H, S, 1.5f);
        makeTransparent();
    }
}