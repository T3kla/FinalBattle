using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using static Logger;

public class PawnEnemy : Pawn
{

    protected override async UniTask TurnMove()
    {
        await base.TurnMove();
    }

    protected override async UniTask TurnAttack()
    {
        await base.TurnAttack();
    }

    protected override async UniTask TurnWait()
    {
        await base.TurnWait();
    }

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
        await UniTask.Delay(0);

        foreach (KeyValuePair<Coord, Tile> t in Map.Tiles)
        {
            t.Value.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = (Material)Resources.Load("Mat_Tile_copy", typeof(Material));
        }
        List<Tile> accessibleTiles = Pathfinder.GetTilesInMovingRange(classSO, tile);
        foreach (Tile t in accessibleTiles)
        {
            List<Tile> attackTiles = Pathfinder.GetTilesInAttackRange(classSO, t);
            foreach (Tile t2 in attackTiles)
            {
                t2.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
    }
}
