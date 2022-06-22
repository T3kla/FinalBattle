using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Collections;

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

    #region AStar pathfinding
    protected class Node
    {
        public Tile tile;
        public int f = 0;
        public int g = 0;
        public int h = 0;
        public Node parent = null;

        public Node(Tile _tile)
        {
            tile = _tile;
        }
    }
    protected int ComputeHScore(Tile _start, Tile _target)
    {
        return Math.Abs(_target.coord.x - _start.coord.x) + Math.Abs(_target.coord.z - _start.coord.z);
    }
    protected virtual List<Tile> FindPath(Tile target)
    {
        //Then we do A* to find the best path
        Node endNode = null;
        List<Tile> path = null;
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        int g = 0;

        Node currentNode = new Node(tile);
        currentNode.h = ComputeHScore(tile, target);
        currentNode.f = currentNode.h;
        openList.Add(currentNode);


        while (openList.Count != 0)
        {
            var lowest = openList.Min(l => l.f);
            currentNode = openList.First(l => l.f == lowest);

            // add the current square to the closed list
            closedList.Add(currentNode);
            // remove it from the open list
            openList.Remove(currentNode);

            endNode = closedList.FirstOrDefault(l => l.tile.coord.x == target.coord.x && l.tile.coord.z == target.coord.z);
            if (endNode != null)
            {
                break;
            }

            g = currentNode.g + 1;
            foreach (var adjacentTile in currentNode.tile.GetAdjacentTiles())
            {
                // We continue if:
                if (Math.Abs(adjacentTile.height - currentNode.tile.height) > classSO.jump // you can't jump there
                    || adjacentTile.pawn != null // tile is occupied
                    || closedList.FirstOrDefault(l => l.tile.coord.x == adjacentTile.coord.x
                                                   && l.tile.coord.z == adjacentTile.coord.z) != null) // it's already on the closed list
                {
                    continue;
                }

                // it's not on the open list, add it
                var find = openList.FirstOrDefault(l => l.tile.coord.x == adjacentTile.coord.x && l.tile.coord.z == adjacentTile.coord.z);
                if (find == null)
                {
                    Node adjacentNode = new Node(adjacentTile);
                    adjacentNode.g = g;
                    adjacentNode.h = ComputeHScore(adjacentTile, target);
                    adjacentNode.f = adjacentNode.h + adjacentNode.g;
                    adjacentNode.parent = currentNode;
                    openList.Insert(0, adjacentNode);
                }
                // if it's on both, we check which one is better
                else
                {
                    if (g + find.h < find.f)
                    {
                        find.g = g;
                        find.f = find.g + find.h;
                        find.parent = currentNode;
                    }
                }
            }
        }
        if (endNode == null)
        {
            return null;
        }

        //Then we only return the first SPEED number of tiles, ignoring the starting tile
        currentNode = endNode.parent;
        while (currentNode != null)
        {
            path.Insert(0, currentNode.tile);
            currentNode = currentNode.parent;
        }

        return path.GetRange(1, classSO.speed + 1);
    }
    #endregion

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
                if(tiles.FirstOrDefault(l => l.coord.x == adjacentTile.coord.x && l.coord.z == adjacentTile.coord.z) != null
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
