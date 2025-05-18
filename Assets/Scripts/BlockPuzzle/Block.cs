using System;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;

public class Block : MonoBehaviour
{
    BlockLevelManager levelManager;
    BlockGrid grid;
    new SpriteRenderer renderer;

    public int id {get; set;}

    public BlockType blockType {get; set;}

    public bool isOnGrid {get; set;}
    
    UnityEngine.Vector3 resetPosition;
    UnityEngine.Vector2 difference = UnityEngine.Vector2.zero;

    int orientation = 0;

    float scalefact;

    float xoffset;
    float yoffset;

    public Sprite appleSprite;
    public Sprite baconSprite;
    public Sprite bananaSprite;
    public Sprite beanSprite;
    public Sprite breadSprite;
    public Sprite bread2Sprite;
    public Sprite bread3Sprite;
    public Sprite bread4Sprite;
    public Sprite broccoliSprite;
    public Sprite cabbageSprite;
    public Sprite carrotSprite;
    public Sprite cheeseSprite;
    public Sprite chickenLegSprite;
    public Sprite cornSprite;
    public Sprite cucumberSprite;
    public Sprite eggSprite;
    public Sprite eggplantSprite;
    public Sprite fishSprite;
    public Sprite fishSliceSprite;
    public Sprite hamSprite;
    public Sprite lettuceSprite;
    public Sprite mushroomSprite;
    public Sprite noodleSprite;
    public Sprite oatsSprite;
    public Sprite riceSprite;
    public Sprite tofuSprite;
    public Sprite tomatoSprite;

    bool isEnabled = true;
    bool pauseDragging = false;
    bool selected = false;

    public void initBlock(int id, BlockType type, BlockLevelManager levelManager, BlockGrid grid, int scaling) {
        // GetComponent<FlashingAnim>().SetAnimated(false);

        this.id = id;
        isOnGrid = false;
        blockType = type;
        this.levelManager = levelManager;
        this.grid = grid;
        resetPosition = transform.position;

        renderer = GetComponent<SpriteRenderer>();
        switch (type.name) {
            case "apple":
                renderer.sprite = appleSprite;
                break;
            case "bacon":
                renderer.sprite = baconSprite;
                break;
            case "banana":
                renderer.sprite = bananaSprite;
                break;
            case "beans":
                renderer.sprite = beanSprite;
                break;
            case "bread":
                renderer.sprite = breadSprite;
                break;
            case "bread2":
                renderer.sprite = bread2Sprite;
                break;
            case "bread3":
                renderer.sprite = bread3Sprite;
                break;
            case "bread4":
                renderer.sprite = bread4Sprite;
                break;
            case "broccoli":
                renderer.sprite = broccoliSprite;
                break;
            case "cabbage":
                renderer.sprite = cabbageSprite;
                break;
            case "carrot":
                renderer.sprite = carrotSprite;
                break;
            case "cheese":
                renderer.sprite = cheeseSprite;
                break;
            case "chicken leg":
                renderer.sprite = chickenLegSprite;
                break;
            case "corn":
                renderer.sprite = cornSprite;
                break;
            case "cucumber":
                renderer.sprite = cucumberSprite;
                break;
            case "egg":
                renderer.sprite = eggSprite;
                break;
            case "eggplant":
                renderer.sprite = eggplantSprite;
                break;
            case "fish":
                renderer.sprite = fishSprite;
                break;
            case "fish slice":
                renderer.sprite = fishSliceSprite;
                break;
            case "ham":
                renderer.sprite = hamSprite;
                break;
            case "lettuce":
                renderer.sprite = lettuceSprite;
                break;
            case "mushroom":
                renderer.sprite = mushroomSprite;
                break;
            case "noodle":
                renderer.sprite = noodleSprite;
                break;
            case "oats":
                renderer.sprite = oatsSprite;
                break;
            case "rice":
                renderer.sprite = riceSprite;
                break;
            case "tofu":
                renderer.sprite = tofuSprite;
                break;
            case "tomato":
                renderer.sprite = tomatoSprite;
                break;
            default:
                renderer.sprite = appleSprite;
                break;
        }


        if (scaling == 3) {
            transform.localScale = new UnityEngine.Vector3(1.5f,1.5f,1);
            scalefact = 0.75f;
            xoffset = 0.5f;
            yoffset = 0.75f;
        } else {
            scalefact = 0.5f;
            xoffset = 0;
            yoffset = 0.5f;
        }

        UnityEngine.Vector2 S = renderer.sprite.bounds.size;
        gameObject.GetComponent<BoxCollider2D>().size = S;
    }

    public void setEnabled(bool enabled) {
        isEnabled = enabled;
    }

    private void OnMouseDown() {
        pauseDragging = false;
        if (!isEnabled) {
            return;
        }
        if (isOnGrid) {
            removeFromGrid();
        }
        makeTransparent();
        grid.clearTileMap();
        difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        levelManager.selectBlock(id);
    }

    private void OnMouseDrag() {
        if (!isEnabled || pauseDragging) {
            return;
        }
        transform.position = (UnityEngine.Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition) - difference;
        grid.drawDropShadow(getSpriteTopLeft(), blockType);
    }

    private void OnMouseUp() {
        if (!isEnabled || pauseDragging) {
            pauseDragging = false;
            return;
        }
        int status = grid.checkBlockPosition(id, getSpriteTopLeft(), blockType);
        if (status == -1) {
            transform.position = resetPosition;
            makeOpaque();
        } else {
            makeTransparent();
        }
    }

    public UnityEngine.Vector3 getSpriteTopLeft() {
        switch (orientation) {
            case 0:
                return GetComponent<Renderer>().transform.TransformPoint(new UnityEngine.Vector3(renderer.sprite.bounds.min.x, renderer.sprite.bounds.max.y, 0));
            case 1:
                return GetComponent<Renderer>().transform.TransformPoint(new UnityEngine.Vector3(renderer.sprite.bounds.min.x, renderer.sprite.bounds.min.y, 0));
            case 2:
                return GetComponent<Renderer>().transform.TransformPoint(new UnityEngine.Vector3(renderer.sprite.bounds.max.x, renderer.sprite.bounds.min.y, 0));
            case 3:
                return GetComponent<Renderer>().transform.TransformPoint(new UnityEngine.Vector3(renderer.sprite.bounds.max.x, renderer.sprite.bounds.max.y, 0));
            default:
                return GetComponent<Renderer>().transform.TransformPoint(new UnityEngine.Vector3(renderer.sprite.bounds.min.x, renderer.sprite.bounds.max.y, 0));
        };
    }

    void removeFromGrid() {
        isOnGrid = false;
        grid.removeBlock(id);
        levelManager.playerRemoveBlock(id);
        grid.drawDropShadow(getSpriteTopLeft(), blockType);
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

    public void placeBlock() {
        int status = grid.checkBlockPosition(id, getSpriteTopLeft(), blockType);
        if (status == 0) {
            pauseDragging = true;
            makeOpaque();

            if (isOnGrid) {
                grid.updateBlock(id, getSpriteTopLeft(), blockType);
            } else {
                isOnGrid = true;
                grid.addBlock(id, getSpriteTopLeft(), blockType);
                levelManager.playerAddBlock(id);
                levelManager.deselectBlock();
            }
            placeBlockAt(grid.snapToGrid(getSpriteTopLeft()) * scalefact);
        }
    }

    public void placeBlockAt(UnityEngine.Vector3 position) {
        // AudioSFXManager.Instance.PlayAudio("thump");
        transform.position = position
        + new UnityEngine.Vector3(
            blockType.shape[0].Length / (2.0f / scalefact) + xoffset,
            - blockType.shape.Length / (2.0f / scalefact) + yoffset
        );
    }

    public void hintColor() {
        float H, S;
        Color.RGBToHSV(renderer.color, out H, out S, out _);

        renderer.color = Color.HSVToRGB(H, S, 1.5f);
        makeTransparent();
    }

    public void flip(bool isHorizontal) {
        if (isOnGrid) {
            return;
        }
        bool[][] oldShape = blockType.shape;

        bool[][] shape = new bool[oldShape.Length][];
        for (int i = 0; i < oldShape.Length; ++i) {
            shape[i] = new bool[oldShape[0].Length];
        }

        if (isHorizontal) {
            for (int i = 0; i < shape.Length; ++i) {
                for (int j = 0; j < shape[0].Length; ++j) {
                    shape[i][shape[0].Length - j - 1] = oldShape[i][j];
                }
            }
        } else {
            for (int i = 0; i < shape.Length; ++i) {
                for (int j = 0; j < shape[0].Length; ++j) {
                    shape[shape.Length - i - 1][j] = oldShape[i][j];
                }
            }
        }

        if (orientation % 2 == 1) {
            isHorizontal = !isHorizontal;
        }

        blockType.shape = shape;
        if (isHorizontal) {
            renderer.flipX = !renderer.flipX;
        } else {
            renderer.flipY = !renderer.flipY;
        }
        grid.drawDropShadow(getSpriteTopLeft(), blockType);
    }

    public void rotate() {
        if (isOnGrid) {
            return;
        }

        bool[][] oldShape = blockType.shape;

        int rows = oldShape.Length, cols = oldShape[0].Length;

        bool[][] shape = new bool[cols][];
        for (int i = 0; i < cols; ++i) {
            shape[i] = new bool[rows];
            for (int j = 0; j < rows; ++j) {
                shape[i][j] = oldShape[rows - j - 1][i];
            }
        }

        blockType.shape = shape;
        orientation = (orientation + 1) % 4;
        transform.Rotate(0, 0, -90f);
        grid.drawDropShadow(getSpriteTopLeft(), blockType);
    }
}