using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid : MonoBehaviour
{
    public Tilemap tilemap;

    public Tile blank;
    public Tile dropshadow;

    int[][] gridArray;
    int[][] solutionArray;

    int rows, cols;

    int xoffset, yoffset;

    int currentDay;

    public void initGrid(int[][] gridArray, int xoffset, int yoffset, int[][] solutionArray, int day) {
        this.gridArray = gridArray;
        this.solutionArray = solutionArray;
        rows = gridArray.Length;
        cols = gridArray[0].Length;
        this.xoffset = xoffset;
        this.yoffset = yoffset;
        currentDay = day;
        
        tilemap.ClearAllTiles();
        clearTileMap();
    }

    public void clearTileMap() {
        for (int i = 0; i < gridArray.Length; ++i) {
            for (int j = 0; j < gridArray[i].Length; ++j) {
                Vector3Int cell = arrayToCell(new Vector3Int(i, j, 0));
                switch (gridArray[i][j]) {
                    case -1:
                        tilemap.SetTile(cell, null);
                        break;
                    case -2:
                        tilemap.SetTile(cell, null);
                        break;
                    default:
                        tilemap.SetTile(cell, blank);
                        break;
                }
            }
        }
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

    public void drawDropShadow(Vector3 position, BlockType blockType) {
        clearTileMap();
        Vector3Int off = worldToArray(position);
        bool[][] shape = blockType.shape;
        for (int i = 0; i < shape.Length; ++i) {
            for (int j = 0; j < shape[i].Length; ++j) {
                if (!shape[i][j]) {
                    continue;
                }
                if (off.x + i >= rows || off.y + j >= cols || off.x + i < 0 || off.y + j < 0) {
                    continue;
                }
                int g = gridArray[off.x + i][off.y + j];
                if (g >= 0) {
                    Vector3Int cell = arrayToCell(new Vector3Int(off.x + i, off.y + j, 0));
                    tilemap.SetTile(cell, dropshadow);
                }
            }
        }
    }

    public int checkBlockPosition(int id, Vector3 position, BlockType blockType) {
        Vector3Int off = worldToArray(position);
        bool[][] shape = blockType.shape;
        for (int i = 0; i < shape.Length; ++i) {
            for (int j = 0; j < shape[i].Length; ++j) {
                if (!shape[i][j]) {
                    continue;
                }
                if (off.x + i >= rows || off.y + j >= cols || off.x + i < 0 || off.y + j < 0) {
                    return -1;
                }
                int g = gridArray[off.x + i][off.y + j];
                if (g == -2 || g == -1 || (g > 0 && g != id)) {
                    return -2;
                }
            }
        }
        return 0;
    }

    public void addBlock(int id, Vector3 position, BlockType blockType) {
        Vector3Int off = worldToArray(position);
        bool[][] shape = blockType.shape;
        for (int i = 0; i < shape.Length; ++i) {
            for (int j = 0; j < shape[i].Length; ++j) {
                if (shape[i][j]) {
                    gridArray[off.x + i][off.y + j] = id;
                }
            }
        }
    }

    public void removeBlock(int id) {
        for (int i = 0; i < gridArray.Length; ++i) {
            for (int j = 0; j < gridArray[i].Length; ++j) {
                if (gridArray[i][j] == id) {
                    gridArray[i][j] = 0;
                }
            }
        }
    }

    public void updateBlock(int id, UnityEngine.Vector3 position, BlockType blockType) {
        removeBlock(id);
        addBlock(id, position, blockType);
    }

    public int getFirstMismatch() {
        for (int i = 0; i < gridArray.Length; ++i) {
            for (int j = 0; j < gridArray[i].Length; ++j) {
                if (gridArray[i][j] != solutionArray[i][j]) {
                    return gridArray[i][j] == 0 ? solutionArray[i][j] : gridArray[i][j];
                }
            }
        }
        return -1;
    }
}