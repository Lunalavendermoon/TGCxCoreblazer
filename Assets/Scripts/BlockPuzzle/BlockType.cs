public class BlockType
{
    public string name { get; }
    public int width { get; }
    public int height { get; }

    public BlockType(string n, int w, int h)
    {
        name = n;
        width = w;
        height = h;
    }

    public static BlockType bigSquare()
    {
        return new BlockType("bigSquare", 2, 2);
    }

    public static BlockType smallSquare()
    {
        return new BlockType("smallSquare", 1, 1);
    }

    public static BlockType bigTriangle()
    {
        return new BlockType("bigTriangle", 2, 2);
    }

    public static BlockType smallTriangle()
    {
        return new BlockType("smallTriangle", 1, 1);
    }

    public static BlockType bigCircle()
    {
        return new BlockType("bigCircle", 2, 2);
    }

    public static BlockType quarterCircle()
    {
        return new BlockType("quarterCircle", 1, 1);
    }
}