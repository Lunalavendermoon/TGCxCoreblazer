using Unity.Mathematics;
using UnityEngine;

public class Block : MonoBehaviour
{
//     BlockLevelManager levelManager;
//     BlockGrid grid;
//     new SpriteRenderer renderer;

//     public int id {get; set;}

//     public BlockType blockType {get; set;}

//     public bool isOnGrid {get; set;}
    
//     Vector3 resetPosition;

//     public Sprite bigSquare;
//     public Sprite smallSquare;
//     public Sprite bigTriangle;
//     public Sprite bigTriangle2;
//     public Sprite bigTriangle3;
//     public Sprite bigTriangle4;
//     public Sprite smallTriangle;
//     public Sprite smallTriangle2;
//     public Sprite smallTriangle3;
//     public Sprite smallTriangle4;
//     public Sprite bigCircle;
//     public Sprite quarterCircle;
//     public Sprite quarterCircle2;
//     public Sprite quarterCircle3;
//     public Sprite quarterCircle4;

//     bool isEnabled = true;
//     bool selected = false;
//     bool isDragging = false;

//     private Vector3 screenPoint;
//     private Vector3 offset;

//     public void initBlock(int id, BlockType type, BlockLevelManager levelManager, BlockGrid grid)
//     {
//         // GetComponent<FlashingAnim>().SetAnimated(false);

//         this.id = id;
//         isOnGrid = false;
//         blockType = type;
//         this.levelManager = levelManager;
//         this.grid = grid;
//         resetPosition = transform.position;

//         renderer = GetComponent<SpriteRenderer>();
//         switch (type.name)
//         {
//             case "bigSquare":
//                 renderer.sprite = bigSquare;
//                 break;
//             case "smallSquare":
//                 renderer.sprite = smallSquare;
//                 break;
//             case "bigTriangle":
//                 renderer.sprite = bigTriangle;
//                 break;
//             case "bigTriangle2":
//                 renderer.sprite = bigTriangle2;
//                 break;
//             case "bigTriangle3":
//                 renderer.sprite = bigTriangle3;
//                 break;
//             case "bigTriangle4":
//                 renderer.sprite = bigTriangle4;
//                 break;
//             case "smallTriangle":
//                 renderer.sprite = smallTriangle;
//                 break;
//             case "smallTriangle2":
//                 renderer.sprite = smallTriangle2;
//                 break;
//             case "smallTriangle3":
//                 renderer.sprite = smallTriangle3;
//                 break;
//             case "smallTriangle4":
//                 renderer.sprite = smallTriangle4;
//                 break;
//             case "bigCircle":
//                 renderer.sprite = bigCircle;
//                 break;
//             case "quarterCircle":
//                 renderer.sprite = quarterCircle;
//                 break;
//             case "quarterCircle2":
//                 renderer.sprite = quarterCircle2;
//                 break;
//             case "quarterCircle3":
//                 renderer.sprite = quarterCircle3;
//                 break;
//             case "quarterCircle4":
//                 renderer.sprite = quarterCircle4;
//                 break;
//             default:
//                 renderer.sprite = bigSquare;
//                 break;
//         }

//         renderer.flipX = type.hflipped;
//         renderer.flipY = type.vflipped;

//         Vector2 S = renderer.sprite.bounds.size;
//         gameObject.GetComponent<BoxCollider2D>().size = S;
//     }

//     public void setEnabled(bool enabled)
//     {
//         isEnabled = enabled;
//     }

//     private void OnMouseDown()
//     {
//         if (!isEnabled)
//         {
//             return;
//         }
//         if (isOnGrid)
//         {
//             removeFromGrid();
//         }
//         isDragging = true;
//         levelManager.selectBlock(id);
        
//         screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

//         offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

//     }

//     void OnMouseDrag()
//     {
//         Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

//         Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
//         transform.position = curPosition;
//     }

//     private void OnMouseUp()
//     {
//         if (!isEnabled)
//         {
//             return;
//         }
//         isDragging = false;

//         Vector3 pos = getSpriteTopLeft();
//         // Vector3 pos = transform.position;

//         if (grid.checkBlockPosition(pos, blockType))
//         {
//             makeOpaque();

//             if (isOnGrid)
//             {
//                 grid.updateBlock(id, pos, blockType.name);
//             }
//             else
//             {
//                 isOnGrid = true;
//                 grid.addBlock(id, pos, blockType.name);
//                 levelManager.playerAddBlock(id);
//                 levelManager.deselectBlock();
//             }
//             placeBlockAt(grid.snapToGrid(pos));
//         }
//         else
//         {
//             transform.position = resetPosition;
//             makeOpaque();
//         }
//     }

//     public Vector3 getSpriteTopLeft() {
//         return GetComponent<Renderer>().transform.TransformPoint(new Vector3(renderer.sprite.bounds.min.x, renderer.sprite.bounds.max.y, 0));
//     }

//     void removeFromGrid() {
//         isOnGrid = false;
//         grid.removeBlock(id);
//         levelManager.playerRemoveBlock(id);
//     }

//     void makeTransparent() {
//         if (selected == false)
//         {
//             // AudioSFXManager.Instance.PlayAudio("pop");
//             selected = true;
//         }
//         Color col = renderer.color;
//         col.a = 0.8f;
//         renderer.color = col;
//     }

//     void makeOpaque() {
//         if (selected == true)
//         {
//             // AudioSFXManager.Instance.PlayAudio("pop");
//             selected = false;
//         }
//         Color col = renderer.color;
//         col.a = 1;
//         renderer.color = col;
//     }

//     public void placeBlockAt(Vector3 position) {
//         // AudioSFXManager.Instance.PlayAudio("thump");
//         Vector2 S = renderer.sprite.bounds.size;
//         transform.position = position + new Vector3(blockType.width / 2.0f, (blockType.height % 2) * (blockType.height / 2.0f));
//     }

//     public void hintColor() {
//         float H, S;
//         Color.RGBToHSV(renderer.color, out H, out S, out _);

//         renderer.color = Color.HSVToRGB(H, S, 1.5f);
//         makeTransparent();
//     }
}