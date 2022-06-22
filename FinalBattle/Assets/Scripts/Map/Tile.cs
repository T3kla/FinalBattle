using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Tile : MonoBehaviour
{

    [Header(" · Position")]
    public Coord coord = new Coord();
    public short height = 0;

    [Header(" · Details")]
    public ETileType type = ETileType.Ground;

    [Header(" · Adjacents")]
    public Tile forward = null;
    public Tile back = null;
    public Tile right = null;
    public Tile left = null;

    [Header(" · ReadOnly")]
    [ReadOnly] public Pawn pawn = null;

    public List<Tile> GetAdjacentTiles()
    {
        List<Tile> adjacent = new List<Tile> { forward, back, right, left };
        adjacent.RemoveAll(t => (t == null));
        return adjacent;
    }

}
