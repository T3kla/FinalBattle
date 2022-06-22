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

        Tile.OnTileClicked += OnTileClicked;
        await UniTask.WaitUntil(() => targetTile != null);
        Tile.OnTileClicked -= OnTileClicked;

        // TODO: Hide range of movement

        List<Tile> path = Pathfinder.FindPath(classSO, tile, targetTile);

        foreach (Tile tile in path)
        {
            float cur = 0f, dur = 0.35f;

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

        tile = targetTile;
    }

    protected override async UniTask TurnAttack()
    {
        await base.TurnAttack();

        // List<Tile> tilesInRange = null;

        // if (tilesInRange.FirstOrDefault(t => t.coord.x == target.tile.coord.x && t.coord.z == target.tile.coord.z) != null)
        // {
        //     // TODO: Attack animation
        //     // TODO: Dodge stuff
        //     target.health -= (classSO.attack - classSO.defence);
        // }
    }

    protected override async UniTask TurnWait()
    {
        await base.TurnWait();
    }

    public override void OnPointerClick(PointerEventData pointerEventData)
    {
        foreach (KeyValuePair<Coord, Tile> t in Map.Tiles)
        {
            t.Value.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = (Material)Resources.Load("Mat_Tile_copy", typeof(Material));
        }
        List<Tile> accessibleTiles = Pathfinder.GetTilesInMovingRange(classSO, tile);
        foreach (Tile t in accessibleTiles)
        {
            t.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }

    private void OnTileClicked(Tile tile)
    {
        targetTile = tile;
    }

}
