using System.Threading;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;

public class PawnPlayer : Pawn
{

    private Tile targetTile = null;

    protected override async UniTask TurnMove(CancellationToken ct)
    {
        await base.TurnMove(ct);

        var accessibleTiles = Pathfinder.GetTilesInMovingRange(@class, tile);

        // Ask user to select a tile
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.MovePlayer);
        void OnTileClicked(Tile tile) => targetTile = tile;
        Tile.OnTileClicked += OnTileClicked;
        await UniTask.WaitUntil(() => targetTile != null && accessibleTiles.Contains(targetTile));
        Tile.OnTileClicked -= OnTileClicked;
        if (ct.IsCancellationRequested) return;
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.None);

        // Walk each tile
        await WalkPath(ct, Pathfinder.FindPath(@class, tile, targetTile));

        // Update references
        tile.pawn = null;
        targetTile.pawn = this;
        tile = targetTile;
        targetTile = null;
    }

    protected override async UniTask TurnAttack(CancellationToken ct)
    {
        await base.TurnAttack(ct);

        var accessibleTiles = Pathfinder.GetTilesInAttackRange(@class, tile);

        // Ask user to select a tile
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.AttackPlayer);
        void OnTileClicked(Tile tile) => targetTile = tile;
        Tile.OnTileClicked += OnTileClicked;
        await UniTask.WaitUntil(() => targetTile != null && accessibleTiles.Contains(targetTile));
        Tile.OnTileClicked -= OnTileClicked;
        if (ct.IsCancellationRequested) return;
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.None);

        // Attack
        var targetPawn = targetTile.pawn;
        if (targetPawn)
        {
            var damage = @class.attack + Random.Range(-2, 2);
            Attack(ct, targetTile, damage).Forget();
            await UniTask.Delay(100);
            targetPawn.ReceiveDamage(damage).Forget();
        }
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
        var accessibleTiles = Pathfinder.GetTilesInMovingRange(@class, tile);
        Tile.SetVisualAid(accessibleTiles, ETileVisualAid.MovePlayer);
    }

}
