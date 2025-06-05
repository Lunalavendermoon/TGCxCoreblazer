using UnityEngine;

public class BlockPuzzleManager : MonoBehaviour
{
    public BlockLevelManager levelManager;
    public UIInputHandler bossmansBossman;

    public Sprite Bird;
    public Sprite LittleGirl;
    public Sprite ElderlyGardener;
    public Sprite Robot;
    public Sprite Sapling1;
    public Sprite Sapling2;
    public Sprite Storyteller1;
    public Sprite Storyteller2;

    private int lvlCount = 0;
    private string level;

    public void loadLevel(string nm)
    {
        level = nm;
        levelManager.initLevel(getSolnByName(), getImgByName());
    }

    string[] getSolnByName()
    {
        switch (level)
        {
            case "Bird":
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
            case "Little Girl":
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
            case "Elderly Gardener":
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
            case "Robot":
                // TODO what's the soln
                return new string[] {
                    "smallSquareFF,0,0",
                    "smallSquareFF,0,0",
                    "smallTriangle3FF,0,0",
                    "bigSquareFF,0,0",
                    "bigSquareFF,0,0",
                    "bigSquareFF,0,0",
                    "bigSquareFF,0,0",
                    "smallTriangle4FF,0,0",
                    "smallTriangleFF,0,0"
                };
            case "Sapling":
                if (lvlCount == 0)
                {
                    // TODO what's the soln
                    return new string[] {
                        "bigSquareFF,0,0",
                        "bigTriangle4FF,0,0",
                        "bigTriangleFF,0,0",
                        "bigSquareFF,0,0",
                        "bigSquareFF,0,0",
                        "smallSquareFF,0,0"
                    };
                }
                else
                {
                    // TODO what's the soln
                    return new string[] {
                        "bigSquareFF,0,0",
                        "bigSquareFF,0,0",
                        "bigSquareFF,0,0",
                        "bigSquareFF,0,0",
                        "bigSquareFF,0,0",
                        "smallSquareFF,0,0",
                        "smallSquareFF,0,0",
                        "smallTriangleFF,0,0",
                        "bigTriangleFF,0,0",
                        "bigTriangle3FF,0,0",
                        "bigTriangle4FF,0,0"
                    };
                }
            case "Storyteller":
                if (lvlCount == 0)
                {
                    // TODO what's the soln
                    return new string[] {
                        "bigSquareFF,0,0",
                        "bigSquareFF,0,0",
                        "bigSquareFF,0,0",
                        "bigSquareFF,0,0",
                        "bigTriangleFF,0,0",
                        "bigTriangle2FF,0,0",
                        "bigTriangle3FF,0,0",
                        "bigTriangle4FF,0,0",
                        "smallSquareFF,0,0",
                        "smallSquareFF,0,0"
                    };
                }
                else
                {
                    // TODO what's the soln
                    return new string[] {
                        "bigSquareFF,0,0",
                        "bigSquareFF,0,0",
                        "bigSquareFF,0,0",
                        "bigSquareFF,0,0",
                        "smallSquareFF,0,0",
                        "smallTriangle,0,0",
                        "bigTriangle,0,0",
                        "bigTriangle2,0,0",
                        "bigTriangle2,0,0",
                        "bigTriangle3,0,0",
                        "bigTriangle4,0,0"
                    };
                }
            default:
                return new string[] { };
        }
    }

    Sprite getImgByName()
    {
        switch (level)
        {
            case "Bird":
                return Bird;
            case "Little Girl":
                return LittleGirl;
            case "Elderly Gardener":
                return ElderlyGardener;
            case "Robot":
                return Robot;
            case "Sapling":
                return (lvlCount == 0) ? Sapling1 : Sapling2;
            case "Storyteller":
                return (lvlCount == 0) ? Storyteller1 : Storyteller2;
            default:
                return null;
        }
    }

    public void endLevel()
    {
        if (lvlCount == 0 && (level == "Sapling" || level == "Storyteller"))
        {
            ++lvlCount;
            loadLevel(level);
        }
        else
        {
            lvlCount = 0;
            bossmansBossman.EndBlockPuzzle();
        }
    }
}
