using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class PawnEnemy : Pawn
{

    private Tile targetTile = null;

    protected override async UniTask TurnMove(CancellationToken ct)
    {
        await base.TurnMove(ct);

        var accessibleTiles = Pathfinder.GetTilesInMovingRange(classSO, tile);

        // Ask user to select a tile
        foreach (var tile in accessibleTiles)
            tile.SetVisualAid(ETileVisualAid.MoveEnemy);

        targetTile = ChooseTarget();

        await UniTask.Delay(1000);
        if (ct.IsCancellationRequested) return;

        foreach (var tile in accessibleTiles)
            tile.SetVisualAid(ETileVisualAid.None);

        // Walk each tile
        List<Tile> path = Pathfinder.FindPath(classSO, tile, targetTile);
        foreach (Tile tile in path)
        {
            var cur = 0f;
            var dur = 0.5f;

            while (true)
            {
                await UniTask.WaitForEndOfFrame(this);
                if (ct.IsCancellationRequested) return;

                cur += Time.deltaTime;
                var nor = cur / dur;

                transform.position = Vector3.Lerp(transform.position, tile.transform.position, nor);

                if (nor > dur)
                    break;
            }
        }

        // Update references
        tile.pawn = null;
        targetTile.pawn = this;
        tile = targetTile;
        targetTile = null;
    }

    protected override async UniTask TurnAttack(CancellationToken ct)
    {
        await base.TurnAttack(ct);
    }

    protected override async UniTask TurnWait(CancellationToken ct)
    {
        await base.TurnWait(ct);
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
