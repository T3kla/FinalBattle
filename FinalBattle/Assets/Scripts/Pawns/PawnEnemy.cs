using System.Threading;
using Cysharp.Threading.Tasks;

public class PawnEnemy : Pawn
{

    private Tile targetTile = null;

    protected override async UniTask TurnMove(CancellationToken ct)
    {
        await base.TurnMove(ct);

        var accessibleTiles = Pathfinder.GetTilesInMovingRange(classSO, tile);

        // Ask user to select a tile
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.MoveEnemy);

        targetTile = ChooseTarget();

        await UniTask.Delay(1000);
        if (ct.IsCancellationRequested) return;

        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.None);

        // Walk each tile
        var path = Pathfinder.FindPath(classSO, tile, targetTile);
        await WalkPath(ct, path);

        // Update references
        tile.pawn = null;
        targetTile.pawn = this;
        tile = path[path.Count - 1];
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

    protected override void OnSomePawnClicked(Pawn pawn)
    {
        if (pawn != this)
            return;

        var accessibleTiles = Pathfinder.GetTilesInMovingRange(classSO, tile);
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
