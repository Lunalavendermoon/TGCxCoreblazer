using Unity.Mathematics;
using UnityEngine;

public class Block : MonoBehaviour
{
    BlockLevelManager levelManager;
    BlockGrid grid;
    new SpriteRenderer renderer;

    public int id {get; set;}

    public string blockType {get; set;}

    public bool isOnGrid {get; set;}
    
    Vector3 resetPosition;

    public Sprite squareSprite;

    bool isEnabled = true;
    bool selected = false;
    bool isDragging = false;

    private Vector3 screenPoint;
    private Vector3 offset;

    public void initBlock(int id, string type, BlockLevelManager levelManager, BlockGrid grid)
    {
        // GetComponent<FlashingAnim>().SetAnimated(false);

        this.id = id;
        isOnGrid = false;
        blockType = type;
        this.levelManager = levelManager;
        this.grid = grid;
        resetPosition = transform.position;

        renderer = GetComponent<SpriteRenderer>();
        switch (type)
        {
            case "square":
                renderer.sprite = squareSprite;
                break;
            default:
                renderer.sprite = squareSprite;
                break;
        }

        Vector2 S = renderer.sprite.bounds.size;
        gameObject.GetComponent<BoxCollider2D>().size = S;
    }

    public void setEnabled(bool enabled)
    {
        isEnabled = enabled;
    }

    private void OnMouseDown()
    {
        if (!isEnabled)
        {
            return;
        }
        if (isOnGrid)
        {
            removeFromGrid();
        }
        isDragging = true;
        levelManager.selectBlock(id);
        
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }

    private void OnMouseUp()
    {
        if (!isEnabled)
        {
            return;
        }
        isDragging = false;

        int status = grid.checkBlockPosition(id, getSpriteTopLeft(), blockType);

        if (status == 0)
        {
            makeOpaque();

            if (isOnGrid)
            {
                grid.updateBlock(id, getSpriteTopLeft(), blockType);
            }
            else
            {
                isOnGrid = true;
                grid.addBlock(id, getSpriteTopLeft(), blockType);
                levelManager.playerAddBlock(id);
                levelManager.deselectBlock();
            }
            placeBlockAt(grid.snapToGrid(getSpriteTopLeft()));
        }
        else if (status == -1)
        {
            transform.position = resetPosition;
            makeOpaque();
        }
        else
        {
            makeTransparent();
        }
    }

    public Vector3 getSpriteTopLeft() {
        return GetComponent<Renderer>().transform.TransformPoint(new Vector3(renderer.sprite.bounds.min.x, renderer.sprite.bounds.max.y, 0));
    }

    void removeFromGrid() {
        isOnGrid = false;
        grid.removeBlock(id);
        levelManager.playerRemoveBlock(id);
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

    public void placeBlockAt(Vector3 position) {
        // AudioSFXManager.Instance.PlayAudio("thump");
        Vector2 S = renderer.sprite.bounds.size;
        transform.position = position + new Vector3(S.x / 2.0f, -S.y / 2.0f);
    }

    public void hintColor() {
        float H, S;
        Color.RGBToHSV(renderer.color, out H, out S, out _);

        renderer.color = Color.HSVToRGB(H, S, 1.5f);
        makeTransparent();
    }
}