using System.Threading;
using Cysharp.Threading.Tasks;

public class PawnEnemy : Pawn
{

    private Tile targetTile = null;

    protected override async UniTask TurnMove(CancellationToken ct)
    {
        await base.TurnMove(ct);

        var accessibleTiles = Pathfinder.GetTilesInMovingRange(@class, tile);

        // Ask user to select a tile
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.MoveEnemy);

        targetTile = ChooseTarget();

        await UniTask.Delay(1000);
        if (ct.IsCancellationRequested) return;

        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.None);

        var path = Pathfinder.FindPath(@class, tile, targetTile);

        // If last tile in path contains the targeted player, omit it
        while (path?.Count >= 1 && path?[path.Count - 1].pawn != null)
        {
            path.RemoveAt(path.Count - 1);

            if (path?.Count >= 1)
                targetTile = path[path.Count - 1];
        }

        await WalkPath(ct, path);

        // Update references
        var oldTile = tile;
        var newTile = path.Count - 1 >= 0 ? path[path.Count - 1] : tile;

        if (oldTile != null) { oldTile.pawn = null; tile = null; }
        if (newTile != null) { newTile.pawn = this; tile = newTile; }

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

    // Useful methods

    public override void ShowTilesInMovingRange()
    {
        var accessibleTiles = Pathfinder.GetTilesInMovingRange(@class, tile);
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.MoveEnemy);
    }

    private Tile ChooseTarget()
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

}
