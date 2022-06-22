using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Logger;

public class PawnEnemy : Pawn
{

    protected virtual Tile ChooseTarget()
    {
        Tile target = null;

        var maxScore = int.MaxValue;

        foreach (PawnPlayer pawnPlayer in Game.Players)
        {
            var score = Pathfinder.ComputeHScore(tile, pawnPlayer.tile);

            if (score < maxScore)
            {
                target = pawnPlayer.tile;
                maxScore = score;
            }
        }

        return target;
    }


    public override async void OnPointerClick(PointerEventData pointerEventData)
    {
        foreach (KeyValuePair<Coord, Tile> t in Map.Tiles)
        {
            t.Value.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = (Material)Resources.Load("Mat_Tile_copy", typeof(Material));
        }
        List<Tile> accessibleTiles = Pathfinder.GetTilesInMovingRange(classSO, tile);
        foreach (Tile t in accessibleTiles)
        {
            List<Tile> attackTiles = Pathfinder.GetTilesInAttackRange(classSO, t);
            foreach(Tile t2 in attackTiles)
            {
                t2.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }

        // For testing
        Tile target = ChooseTarget();
        List<Tile> path = Pathfinder.FindPath(classSO, tile, target);

        Log($"Start in {tile.coord.x}, {tile.coord.z}.");
        foreach (Tile t in path)
        {
            await MoveTo(t);
            //Task movement = MoveTo(t);
            Log($"Then {t.coord.x}, {t.coord.z}.");
        }
    }
}
