using UnityEngine;

[SelectionBase]
public class Tile : MonoBehaviour
{

    [Header("Position")]
    public Coord coord = new Coord();
    public short height = 0;

    [Header("Adjacents")]
    public Tile forward = null;
    public Tile back = null;
    public Tile right = null;
    public Tile left = null;

    [Header("ReadOnly")]
    [ReadOnly] public Pawn pawn = null;

}
