using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Pathfinder
{

    public static int ComputeHScore(Tile _start, Tile _target)
    {
        return Mathf.Abs(_target.coord.x - _start.coord.x) + Mathf.Abs(_target.coord.z - _start.coord.z);
    }

    public static List<Tile> FindPath(ClassSO pawnClass, Tile origin, Tile target)
    {
        Node endNode = null;
        List<Tile> path = null;
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        var g = 0;
        var currentNode = new Node(origin);

        currentNode.h = ComputeHScore(origin, target);
        currentNode.f = currentNode.h;
        openList.Add(currentNode);

        while (openList.Count != 0)
        {
            var lowest = openList.Min(l => l.f);
            currentNode = openList.First(l => l.f == lowest);

            closedList.Add(currentNode); // add the current square to the closed list
            openList.Remove(currentNode); // remove it from the open list

            endNode = closedList.FirstOrDefault(l => l.tile.coord.x == target.coord.x && l.tile.coord.z == target.coord.z);

            if (endNode != null)
                break;

            g = currentNode.g + 1;

            var adjacentsList = currentNode.tile.GetAdjacentTiles();

            foreach (var tile in adjacentsList)
            {
                // Too high or low
                if (Mathf.Abs(tile.height - currentNode.tile.height) > pawnClass.jump)
                    continue;

                // Tile is occupied
                if (tile.pawn != null)
                    continue;

                // It's already in the closed list
                if (closedList.FirstOrDefault(l => l.tile.coord.x == tile.coord.x && l.tile.coord.z == tile.coord.z) != null)
                    continue;

                // it's not on the open list, add it
                var node = openList.FirstOrDefault(l => l.tile.coord.x == tile.coord.x && l.tile.coord.z == tile.coord.z);
                if (node == null)
                {
                    var adjacentNode = new Node(tile);
                    adjacentNode.g = g;
                    adjacentNode.h = ComputeHScore(tile, target);
                    adjacentNode.f = adjacentNode.h + adjacentNode.g;
                    adjacentNode.parent = currentNode;
                    openList.Insert(0, adjacentNode);
                }
                else // if it's on both, we check which one is better
                {
                    if (g + node.h >= node.f)
                        continue;

                    node.g = g;
                    node.f = node.g + node.h;
                    node.parent = currentNode;
                }
            }
        }

        if (endNode == null)
            return null;

        //Then we only return the first SPEED number of tiles, ignoring the starting tile
        currentNode = endNode.parent;

        while (currentNode != null)
        {
            path.Insert(0, currentNode.tile);
            currentNode = currentNode.parent;
        }

        return path.GetRange(1, pawnClass.speed + 1);
    }

}