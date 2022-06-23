using System.Threading;
using Cysharp.Threading.Tasks;

public class PawnPlayer : Pawn
{

    private Tile targetTile = null;

    protected override async UniTask TurnMove(CancellationToken ct)
    {
        await base.TurnMove(ct);

        var accessibleTiles = Pathfinder.GetTilesInMovingRange(classSO, tile);

        // Ask user to select a tile
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.MovePlayer);

        void OnTileClicked(Tile tile) => targetTile = tile;

        Tile.OnTileClicked += OnTileClicked;

        await UniTask.WaitUntil(() => targetTile != null && accessibleTiles.Contains(targetTile));
        Tile.OnTileClicked -= OnTileClicked;

        if (ct.IsCancellationRequested) return;

        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.None);

        // Walk each tile
        await WalkPath(ct, Pathfinder.FindPath(classSO, tile, targetTile));

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

}
