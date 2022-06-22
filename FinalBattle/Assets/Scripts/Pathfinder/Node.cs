using UnityEngine;

public class Node
{
    public Tile tile = null;
    public int f = 0;
    public int g = 0;
    public int h = 0;
    public Node parent = null;

    public Node(Tile tile = null)
        => this.tile = tile;
}
