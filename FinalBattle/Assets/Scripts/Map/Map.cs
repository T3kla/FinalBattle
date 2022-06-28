using System.Collections.Generic;
using UnityEngine;
using static TBox.Logger;

public class Map : MonoBehaviour
{

    public static Dictionary<Coord, Tile> Tiles = new Dictionary<Coord, Tile>();

    public MapSO mapSO = null;

    private void Awake()
    {
        if (!mapSO)
        {
            LogWarn("MapSO is null");
            return;
        }

        var tiles = GameObject.FindObjectsOfType<Tile>();

        if (tiles.Length > 0)
        {
            Tiles = new Dictionary<Coord, Tile>(tiles.Length);
            Log($"Initializing map with {tiles.Length} tiles");
        }
        else
        {
            LogWarn("No tiles found");
        }

        // Add tiles to map
        foreach (var tile in tiles)
        {
            var coord = new Coord(tile.transform.position / mapSO.tileSize);

            if (Tiles.ContainsKey(coord))
            {
                LogWarn($"Destroyed overlapped tile in {coord}");
                Destroy(tile.gameObject);
            }
            else
            {
                tile.coord = coord;
                tile.height = (short)(tile.transform.position.y / mapSO.tileHeight);
                Tiles.Add(coord, tile);
            }
        }

        // Add references to adjacent tiles
        foreach (var tile in Tiles)
        {
            var coord = tile.Key;

            if (Tiles.TryGetValue(new Coord(coord.x + 1, coord.z), out Tile right))
                tile.Value.right = right;
            if (Tiles.TryGetValue(new Coord(coord.x - 1, coord.z), out Tile left))
                tile.Value.left = left;
            if (Tiles.TryGetValue(new Coord(coord.x, coord.z + 1), out Tile forward))
                tile.Value.forward = forward;
            if (Tiles.TryGetValue(new Coord(coord.x, coord.z - 1), out Tile back))
                tile.Value.back = back;
        }
    }

}
