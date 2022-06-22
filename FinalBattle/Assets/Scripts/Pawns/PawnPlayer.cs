using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class PawnPlayer : Pawn
{

    private Tile targetTile = null;

    protected override async UniTask TurnMove()
    {
        // TODO: Show range of movement
        {
            foreach (KeyValuePair<Coord, Tile> t in Map.Tiles)
                t.Value.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = (Material)Resources.Load("Mat_Tile_copy", typeof(Material));

            var accessibleTiles = Pathfinder.GetTilesInMovingRange(classSO, tile);

            foreach (Tile t in accessibleTiles)
                t.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.green;
        }

        Tile.OnTileClicked += OnTileClicked;
        await UniTask.WaitUntil(() => targetTile != null);
        Tile.OnTileClicked -= OnTileClicked;

        // TODO: Hide range of movement
        {
            foreach (KeyValuePair<Coord, Tile> t in Map.Tiles)
                t.Value.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = (Material)Resources.Load("Mat_Tile_copy", typeof(Material));
        }

        // Walk each tile
        List<Tile> path = Pathfinder.FindPath(classSO, tile, targetTile);
        foreach (Tile tile in path)
        {
            var cur = 0f;
            var dur = 0.35f;

            while (true)
            {
                await UniTask.WaitForEndOfFrame(this);

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
    }

    protected override async UniTask TurnAttack()
    {
        await base.TurnAttack();
    }

    protected override async UniTask TurnWait()
    {
        await base.TurnWait();
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
