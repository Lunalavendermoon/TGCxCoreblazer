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

    public static BlockType square()
    {
        return new BlockType("square", 2, 2);
    }
}