using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockGrid : MonoBehaviour
{
    public Tilemap tilemap;

    public Tile blank;
    public Tile dropshadow;

    int xoffset, yoffset;
    int rows, cols;

    int currentDay;

    Dictionary<int, Vector2> solution;

    public void initGrid(Dictionary<int, Vector2> solution, int rows, int cols, int xoffset, int yoffset, int day)
    {
        this.xoffset = xoffset;
        this.yoffset = yoffset;
        this.rows = rows;
        this.cols = cols;
        currentDay = day;

        // map Block ID -> top-left corner's relative offset from the block w/ lowest ID
        this.solution = solution;

        tilemap.ClearAllTiles();
    }

    public Vector3 snapToGrid(Vector3 world) {
        Vector3Int og = tilemap.WorldToCell(world);
        return new Vector3(og.x, og.y);
    }

    public Vector3Int worldToArray(Vector3 world) {
        Vector3Int conv = tilemap.WorldToCell(world);
        return new Vector3Int(yoffset - conv.y, conv.x - xoffset);
    }

    public Vector3Int arrayToCell(Vector3Int grid) {
        return new Vector3Int(xoffset + grid.y, yoffset - grid.x);
    }

    public Vector3 arrayToWorld(int row, int col) {
        Vector3 og = tilemap.CellToWorld(arrayToCell(new Vector3Int(row, col)));
        return new Vector3(og.x - (currentDay == 1 ? 0.5f : 0), og.y);
    }

    public int checkBlockPosition(int id, Vector3 position, string blockType) {
        Vector3Int off = worldToArray(position);
        // TODO calculate
        // -1 = out of bounds, -2 = intersection with obstacle
        return 0;
    }

    public void addBlock(int id, Vector3 position, string blockType)
    {
        Vector3Int off = worldToArray(position);
        // TODO add to list
    }

    public void removeBlock(int id)
    {
        // TODO remove from list
    }

    public void updateBlock(int id, Vector3 position, string blockType) {
        removeBlock(id);
        addBlock(id, position, blockType);
    }

    public int getFirstMismatch() {
        // TODO iterate through blocks and find mismatch
        return -1;
    }
}