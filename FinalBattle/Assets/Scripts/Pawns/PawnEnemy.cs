using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[SelectionBase]
[CanEditMultipleObjects]
public class PawnEnemy : Pawn
{

    protected override async UniTask TurnMove(CancellationToken ct)
    {
        // If we can attack from here, we skip movement
        var attackTiles = Pathfinder.GetTilesInAttackRange(classSO, tileUnder);
        foreach (Tile aT in attackTiles)
        {
            if (aT.pawn != null && !aT.pawn.title.Equals(this.title)) return;
        }

        await base.TurnMove(ct);

        Tile targetTile = null;
        var accessibleTiles = Pathfinder.GetTilesInMovingRange(classSO, tileUnder);

        // Select tile
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.MoveEnemy);
        targetTile = ChooseTarget();
        await UniTask.Delay(700);
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.None);
        if (ct.IsCancellationRequested) return;

        var path = Pathfinder.FindPath(classSO, tileUnder, targetTile);

        // If last tile in path contains the targeted player, omit it
        while (path?.Count >= 1 && path?[path.Count - 1].pawn != null)
        {
            path.RemoveAt(path.Count - 1);

            if (path?.Count >= 1)
                targetTile = path[path.Count - 1];
        }

        await WalkPath(ct, path);

        // Update references
        var oldTile = tileUnder;
        var newTile = path.Count - 1 >= 0 ? path[path.Count - 1] : tileUnder;

        if (oldTile != null) { oldTile.pawn = null; tileUnder = null; }
        if (newTile != null) { newTile.pawn = this; tileUnder = newTile; }
    }

    protected override async UniTask TurnAttack(CancellationToken ct)
    {
        await base.TurnAttack(ct);

        Tile targetTile = null;
        var accessibleTiles = Pathfinder.GetTilesInAttackRange(classSO, tileUnder);

        // Select tile
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.MoveEnemy);
        if (accessibleTiles.Any(t => t.pawn as PawnPlayer != null))
        {
            targetTile = ChooseTarget();
            await UniTask.Delay(700);
            Tile.SetVisualAid(accessibleTiles, ETileVisualAid.None);
            if (ct.IsCancellationRequested) return;
        }

        // Attack
        if (targetTile?.pawn)
        {
            var damage = classSO.attack + Random.Range(-2, 2);
            await Attack(ct, targetTile, damage);
            targetTile.pawn.ReceiveDamage(damage).Forget();
        }

        await UniTask.Delay(500);
    }

    protected override async UniTask TurnWait(CancellationToken ct)
    {
        await base.TurnWait(ct);
    }

    // Useful methods

    public override void ShowTilesInMovingRange()
    {
        var accessibleTiles = Pathfinder.GetTilesInMovingRange(classSO, tileUnder);
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.MoveEnemy);
    }

    private Tile ChooseTarget()
    {
        Tile target = null;
        var maxScore = int.MaxValue;

        foreach (PawnPlayer pawnPlayer in Game.Players)
        {
            var score = Pathfinder.ComputeHScore(tileUnder, pawnPlayer.tileUnder);

            if (score < maxScore)
            {
                target = pawnPlayer.tileUnder;
                maxScore = score;
            }
        }

        return target;
    }

}
