using System.Collections.Generic;
using UnityEngine;
using static Logger;

public class Map : MonoBehaviour
{

    [SerializeField] public float tileSize = 2f;
    [SerializeField] public float tileHeight = 0.5f;

    public Dictionary<Coord, Tile> map = null;

    private void Awake()
    {
        var tiles = GameObject.FindObjectsOfType<Tile>();
        map = new Dictionary<Coord, Tile>(tiles.Length);

        Log($"Found {tiles.Length} tiles");

        // Add tiles to map
        foreach (var tile in tiles)
        {
            var coord = new Coord(tile.transform.position / tileSize);

            if (map.ContainsKey(coord))
                Destroy(tile.gameObject);
            else
            {
                tile.coord = coord;
                tile.height = (short)(tile.transform.position.y / tileHeight);
                map.Add(coord, tile);
            }
        }

        // Add references to each tile
        foreach (var tile in map)
        {
            var coord = tile.Key;

            if (map.TryGetValue(new Coord(coord.x + 1, coord.z), out Tile right))
                tile.Value.right = right;
            if (map.TryGetValue(new Coord(coord.x - 1, coord.z), out Tile left))
                tile.Value.left = left;
            if (map.TryGetValue(new Coord(coord.x, coord.z + 1), out Tile forward))
                tile.Value.forward = forward;
            if (map.TryGetValue(new Coord(coord.x, coord.z - 1), out Tile back))
                tile.Value.back = back;
        }
    }

    private void Update()
    {

    }

}
