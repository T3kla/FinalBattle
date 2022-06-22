using System;
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

            if (model.RightWeaponSocket)
                Instantiate(cls.weapon, model.RightWeaponSocket);
        }
    }

    protected virtual void MoveTo(in List<Tile> path)
    {
        for (int i = 0; i < path.Count; i++)
        {
            Vector3.Lerp(transform.position, path[i].transform.position, 1);
        }
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

    private List<Tile> GetTilesInMovingRange_Recursive(int depth, Tile currentTile, List<Tile> tiles)
    {
        if (depth < classSO.speed)
        {
            foreach (Tile adjacentTile in currentTile.GetAdjacentTiles())
            {
                // we can continue if we've already checked or if it's occupied
                if (tiles.FirstOrDefault(l => l.coord.x == adjacentTile.coord.x && l.coord.z == adjacentTile.coord.z) != null
                    || adjacentTile.pawn != null)
                {
                    continue;
                }

                //if we can jump there
                if (Math.Abs(adjacentTile.height - currentTile.height) > classSO.jump)
                {
                    tiles.Add(currentTile);
                    GetTilesInMovingRange_Recursive(depth + 1, adjacentTile, tiles);
                }
            }
        }
        return tiles;
    }
    public List<Tile> GetTilesInMovingRange()
    {
        return GetTilesInMovingRange_Recursive(0, tile, new List<Tile>());
    }

    private List<Tile> GetTilesInAttackRange_Recursive(int rangeDepth, Tile currentTile, List<Tile> tiles)
    {
        if (rangeDepth > 0)
        {
            foreach (Tile adjacentTile in currentTile.GetAdjacentTiles())
            {
                // if we've already been here, no need to repeat
                if (tiles.FirstOrDefault(l => l.coord.x == adjacentTile.coord.x && l.coord.z == adjacentTile.coord.z) != null)
                {
                    continue;
                }
                //if we can attack that high too
                if (rangeDepth - adjacentTile.height > 0)
                {
                    tiles.Add(currentTile);
                    GetTilesInMovingRange_Recursive(rangeDepth - 1, adjacentTile, tiles);
                }
            }
        }
        return tiles;
    }
    public List<Tile> GetTilesInAttackRange()
    {
        return GetTilesInMovingRange_Recursive(classSO.range + tile.height, tile, new List<Tile>());
    }

    protected virtual void Attack(Pawn target)
    {
        List<Tile> tilesInRange = null;
        //List<Tiles> tilesInRange = GetTilesInAttackRange();
        if (tilesInRange.FirstOrDefault(t => t.coord.x == target.tile.coord.x && t.coord.z == target.tile.coord.z) != null)
        {
            // TODO: Attack animation
            // TODO: Dodge stuff
            target.health -= (classSO.attack - classSO.defence);
        }
    }
}
