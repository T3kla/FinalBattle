using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[SelectionBase]
[CanEditMultipleObjects]
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

        var accessibleTiles = Pathfinder.GetTilesInAttackRange(classSO, tile);

        // Select tile
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.AttackPlayer);
        void OnTileClicked(Tile tile) => targetTile = tile;
        Tile.OnTileClicked += OnTileClicked;
        await UniTask.WaitUntil(() => targetTile != null && accessibleTiles.Contains(targetTile));
        Tile.OnTileClicked -= OnTileClicked;
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.None);
        if (ct.IsCancellationRequested) return;

        // Attack
        var targetPawn = targetTile.pawn;
        if (targetPawn)
        {
            var damage = classSO.attack + Random.Range(-2, 2);
            await Attack(ct, targetTile, damage);
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
        var posPla = Game.Players.FindIndex(p => p.title.Equals(this.title));
        Game.Enemies.RemoveAt(posPla);

        var posIni = Game.Initiative.FindIndex(p => p.title.Equals(this.title));
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
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.MovePlayer);
    }

}
