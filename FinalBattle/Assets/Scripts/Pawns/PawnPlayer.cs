using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class PawnPlayer : Pawn
{

    private Tile targetTile = null;

    protected override async UniTask TurnMove(CancellationToken ct)
    {
        await base.TurnMove(ct);

        var accessibleTiles = Pathfinder.GetTilesInMovingRange(classSO, tile);

        // Ask user to select a tile
        foreach (var tile in accessibleTiles)
            tile.SetVisualAid(ETileVisualAid.MovePlayer);

        Tile.OnTileClicked += OnTileClicked;

        await UniTask.WaitUntil(() => targetTile != null && accessibleTiles.Contains(targetTile));
        if (ct.IsCancellationRequested) return;

        Tile.OnTileClicked -= OnTileClicked;

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

    public override void OnPointerClick(PointerEventData pointerEventData)
    {
        // Show tiles to move

    }

    private void OnTileClicked(Tile tile)
    {
        targetTile = tile;
    }

}
