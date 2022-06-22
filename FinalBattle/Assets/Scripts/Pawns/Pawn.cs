using System.Collections.Generic;
using System.Linq;
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
    public Transform modelSocket = null;

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

        // Destroy model
        if (modelSocket && modelSocket.childCount > 0)
            foreach (Transform child in modelSocket)
                Destroy(child.gameObject);

        // Attach model
        if (cls.model && modelSocket)
        {
            var model = Instantiate(cls.model.transform, modelSocket).GetComponent<Model>();

            if (model.RightWeaponSocket && cls.weaponR)
                Instantiate(cls.weaponR, model.RightWeaponSocket);
            if (model.LeftWeaponSocket && cls.weaponL)
                Instantiate(cls.weaponL, model.LeftWeaponSocket);
        }
    }

    protected virtual void TeleportToClosestTile(MapSO map)
    {
        var coord = new Coord(transform.position / map.tileSize);
        var tiles = new List<Tile>(map.tiles.Values);

        tiles.Sort((a, b) => (a.coord - coord).CompareTo((b.coord - coord)));

        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].pawn)
                continue;

            this.tile = tiles[i];
            tiles[i].pawn = this;
            transform.position = tiles[i].transform.position;
            break;
        }
    }

    protected virtual void MoveTo(in List<Tile> path)
    {
        // TODO: Convert to awaitable
        for (int i = 0; i < path.Count; i++)
        {
            Vector3.Lerp(transform.position, path[i].transform.position, 1);
        }
    }

    protected virtual void Attack(Pawn target)
    {
        List<Tile> tilesInRange = null;

        if (tilesInRange.FirstOrDefault(t => t.coord.x == target.tile.coord.x && t.coord.z == target.tile.coord.z) != null)
        {
            // TODO: Attack animation
            // TODO: Dodge stuff
            target.health -= (classSO.attack - classSO.defence);
        }
    }

    protected virtual void Wait()
    {
        // TODO: Create buttons to choose direction and pass turn
    }

}
