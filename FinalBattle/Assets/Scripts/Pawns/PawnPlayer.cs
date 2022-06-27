using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

[SelectionBase]
[CanEditMultipleObjects]
public class PawnPlayer : Pawn
{

    protected override async UniTask TurnMove(CancellationToken ct)
    {
        await base.TurnMove(ct);

        Tile targetTile = null;
        var accessibleTiles = Pathfinder.GetTilesInMovingRange(classSO, tileUnder);

        // Ask user to select a tile
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.MovePlayer);
        void OnTileClicked(Tile tile) => targetTile = tile;
        Tile.OnTileClicked += OnTileClicked;
        await UniTask.WaitUntil(() => targetTile != null && accessibleTiles.Contains(targetTile));
        Tile.OnTileClicked -= OnTileClicked;
        if (ct.IsCancellationRequested) return;
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.None);

        // Walk each tile
        await WalkPath(ct, Pathfinder.FindPath(classSO, tileUnder, targetTile));

        // Update references
        tileUnder.pawn = null;
        targetTile.pawn = this;
        tileUnder = targetTile;
    }

    protected override async UniTask TurnAttack(CancellationToken ct)
    {
        await base.TurnAttack(ct);

        Tile targetTile = null;
        var accessibleTiles = Pathfinder.GetTilesInAttackRange(classSO, tileUnder);

        // Select tile
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.AttackPlayer);
        if (accessibleTiles.Any(t => t.pawn as PawnEnemy != null))
        {
            void OnTileClicked(Tile tile) => targetTile = tile;
            Tile.OnTileClicked += OnTileClicked;
            await UniTask.WaitUntil(() => targetTile != null && accessibleTiles.Contains(targetTile));
            Tile.OnTileClicked -= OnTileClicked;
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
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.MovePlayer);
    }

}
