using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PawnPlayer : Pawn
{

    public override void OnPointerClick(PointerEventData pointerEventData)
    {
        foreach(KeyValuePair<Coord, Tile> t in Map.Tiles)
        {
            t.Value.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = (Material)Resources.Load("Mat_Tile_copy", typeof(Material));
        }
        List<Tile> accessibleTiles = Pathfinder.GetTilesInMovingRange(classSO, tile);
        foreach(Tile t in accessibleTiles)
        {
            t.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.green;
        }
    }

}
