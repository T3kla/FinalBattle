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

    private Map map = null;

    protected virtual void Start()
    {
        if (mapSO && mapSO.initialized.runtime)
            TeleportToClosestTile(map);

        if (classSO)
            AssignClass(classSO);
    }

    protected virtual void AssignClass(ClassSO classSO)
    {
        health = classSO.health;
        mana = classSO.mana;

        // Destroy weapon
        if (weaponSocket && weaponSocket.childCount > 0)
            foreach (Transform child in weaponSocket)
                Destroy(child.gameObject);

        // Attach weapon
        if (classSO.weapon && weaponSocket)
            Instantiate(classSO.weapon, weaponSocket);
    }

    protected virtual void MoveTo(in List<Tile> path)
    {

    }

    protected virtual void TeleportToClosestTile(in Map map)
    {
        var coord = new Coord(transform.position / mapSO.tileSize);
        var tiles = new List<Tile>(mapSO.tiles.Values);

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
