using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[SelectionBase]
[CanEditMultipleObjects]
public class PawnEnemy : Pawn
{

    private Tile targetTile = null;

    protected override async UniTask TurnMove(CancellationToken ct)
    {
        // If we can attack from here, we skip movement
        var attackTiles = Pathfinder.GetTilesInAttackRange(classSO, tile);
        foreach (Tile aT in attackTiles)
        {
            if (aT.pawn != null) return;
        }

        await base.TurnMove(ct);

        var accessibleTiles = Pathfinder.GetTilesInMovingRange(classSO, tile);

        // Ask user to select a tile
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.MoveEnemy);
        targetTile = ChooseTarget();
        await UniTask.Delay(1000);
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.None);
        if (ct.IsCancellationRequested) return;

        var path = Pathfinder.FindPath(classSO, tile, targetTile);

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

        var accessibleTiles = Pathfinder.GetTilesInAttackRange(classSO, tile);

        // Select tile
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.MoveEnemy);
        targetTile = ChooseTarget();
        await UniTask.Delay(1000);
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.None);
        if (ct.IsCancellationRequested) return;

        // Attack
        var targetPawn = targetTile.pawn;
        if (targetPawn)
        {
            var damage = classSO.attack + Random.Range(-2, 2);
            Attack(ct, targetTile, damage).Forget();
            await UniTask.Delay(100);
            targetPawn.ReceiveDamage(damage).Forget();
        }

        await UniTask.Delay(1000);
    }

    protected override async UniTask TurnWait(CancellationToken ct)
    {
        await base.TurnWait(ct);
    }
    protected override async UniTask Death()
    {
        await UniTask.Delay(1000);
        var posEne = Game.Enemies.FindIndex(e => e.title.Equals(this.title));
        Game.Enemies.RemoveAt(posEne);

        var posIni = Game.Initiative.FindIndex(e => e.title.Equals(this.title));
        if (posIni < Game.InitiativeTracker)
            Game.InitiativeTracker--;
        Game.Initiative.RemoveAt(posIni);
        this.tile.pawn = null;
        this.tile = null;
        //Destroy(this);
    }


    // Useful methods

    public override void ShowTilesInMovingRange()
    {
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
