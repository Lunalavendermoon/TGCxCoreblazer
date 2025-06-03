using UnityEngine;

public class BlockPuzzleManager : MonoBehaviour
{
    public BlockLevelManager levelManager;

    [SerializeField] public Sprite bird;
    public Sprite child;
    public Sprite gardener;
    public Sprite robot;
    public Sprite sapling1;
    public Sprite sapling2;
    public Sprite storyteller1;
    public Sprite storyteller2;

    private int lvlCount = 0;
    private string level;

    void Start()
    {
        // TODO connect to main game
        loadLevel("bird");
    }

    public void loadLevel(string nm)
    {
        level = nm;
        levelManager.initLevel(getSolnByName(), getImgByName());
    }

    string[] getSolnByName()
    {
        switch (level)
        {
            case "bird":
                return new string[] {
                    "smallTriangleFF,2,2",
                    "smallTriangleFF,2,3",
                    "smallTriangleFF,2,4",
                    "smallTriangleFF,4,1",
                    "bigTriangle4FF,3,3",
                    "smallTriangle3FF,4,3",
                    "bigSquareFF,3,2",
                    "bigSquareFF,3,3",
                };
            case "child":
                return new string[] {
                    "smallTriangleFF,2,2",
                    "smallTriangleTF,2,4",
                    "smallSquareFF,3,3",
                    "smallTriangleFF,4,2",
                    "smallSquareFF,4,3",
                    "smallTriangleTF,4,4",
                    "bigSquareFF,5,2",
                    "bigTriangle3FF,4,4",
                    "smallSquareFF,5,2",
                    "smallSquareFF,6,2"
                };
            case "gardener":
                return new string[] {
                    // TODO what's the solution
                    "smallSquareFF,1,2",
                    "bigTriangle4FF,1,4",
                    "smallTriangleFF,2,1",
                    "smallSquareFF,3,1",
                    "bigSquareFF,0,0",
                    "bigSquareFF,0,0",
                    "bigSquareFF,0,0",
                    "smallSquareFF,0,0",
                    "smallSquareFF,0,0",
                    "smallSquareFF,0,0",
                    "smallTriangleFF,0,0",
                    "smallTriangle4FF,0,0"
                };
            case "robot":
                // return robot;
            case "sapling":
                // return (lvlCount == 0) ? sapling1 : sapling2;
            case "storyteller":
                // return (lvlCount == 0) ? storyteller1 : storyteller2;
            default:
                return new string[] { };
        }
    }

    Sprite getImgByName()
    {
        switch (level)
        {
            case "bird":
                return bird;
            case "child":
                return child;
            case "gardener":
                return gardener;
            case "robot":
                return robot;
            case "sapling":
                return (lvlCount == 0) ? sapling1 : sapling2;
            case "storyteller":
                return (lvlCount == 0) ? storyteller1 : storyteller2;
            default:
                return null;
        }
    }

    public void endLevel()
    {
        if (lvlCount == 0 && (level == "sapling" || level == "storyteller"))
        {
            ++lvlCount;
            loadLevel(level);
        }
        else
        {
            Debug.Log("Return to main game");
            // TODO return to main game
        }
    }
}
