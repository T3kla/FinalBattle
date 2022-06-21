using System.Collections.Generic;
using UnityEngine;
using static Logger;

[SelectionBase]
public class Pawn : MonoBehaviour
{


    [Header("Variables")]
    public int health = 1;
    public int mana = 1;
    public int attack = 1;
    public int defence = 1;
    public int speed = 1;

    [Header("Assignables")]
    public ClassSO classSO = null;
    public Transform weaponSocket = null;

    [Header("ReadOnly")]
    [ReadOnly] public Tile tile = null;

    private Map map = null;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        // TODO: search for closes tile move there, get reference

        var map = GameObject.FindObjectOfType<Map>();

        if (map)
        {
            var coord = new Coord(transform.position / map.tileSize);

            // Search closest tile
            var tiles = new List<Tile>(map.map.Values);
            tiles.Sort((a, b) => (a.coord - coord).CompareTo((b.coord - coord)));
            tiles[0].pawn = this;
            tile = tiles[0];
            transform.position = tile.transform.position;
        }
    }

    protected virtual void AssignClass(ClassSO classSO)
    {
        health = classSO.health;
        mana = classSO.mana;
        attack = classSO.attack;
        defence = classSO.defence;
        speed = classSO.speed;

        if (classSO.weapon && weaponSocket)
            Instantiate(classSO.weapon, weaponSocket);
    }

    protected virtual void MoveTo(in List<Tile> path)
    {

    }

}
