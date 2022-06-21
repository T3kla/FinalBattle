using UnityEngine;

[SelectionBase]
public class Tile : MonoBehaviour
{

    public Coord coord;
    public short height;
    public Tile forward;
    public Tile back;
    public Tile right;
    public Tile left;

}
