using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Pawn : MonoBehaviour
{

    [Header(" · Variables")]
    public int health = 1;
    public int mana = 1;

    [Header(" · Assignables")]
    public GameSO gameSO = null;
    public MapSO mapSO = null;
    public ClassSO classSO = null;
    public Transform weaponSocket = null;

    [Header(" · ReadOnly")]
    [ReadOnly] public Tile tile = null;

    protected virtual void Start()
    {
        if (mapSO)
            TeleportToClosestTile(mapSO);

        if (classSO)
            AssignClass(classSO);
    }

    protected virtual void AssignClass(ClassSO cls)
    {
        health = cls.health;
        mana = cls.mana;

        // Destroy weapon
        if (weaponSocket && weaponSocket.childCount > 0)
            foreach (Transform child in weaponSocket)
                Destroy(child.gameObject);

        // Attach weapon
        if (cls.weapon && weaponSocket)
            Instantiate(cls.weapon, weaponSocket);
    }

    protected virtual void MoveTo(in List<Tile> path)
    {

    }

    protected virtual void TeleportToClosestTile(MapSO map)
    {
        var coord = new Coord(transform.position / map.tileSize);
        var tiles = new List<Tile>(map.tiles.Values);

        tiles.Sort((a, b) => (a.coord - coord).CompareTo((b.coord - coord)));

        for (int i = 0; i < tiles.Count; i++)
            if (!tiles[i].pawn)
            {
                this.tile = tiles[i];
                tiles[i].pawn = this;
                transform.position = tiles[i].transform.position;
                break;
            }
    }

}
