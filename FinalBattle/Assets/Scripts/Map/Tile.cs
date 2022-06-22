using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[SelectionBase]
public class Tile : MonoBehaviour, IPointerClickHandler
{

    public static List<Tile> Each = new List<Tile>(200);
    public static Action<Tile> OnTileClicked = null;

    [Header(" · Assignables")]
    public TileSO tileSO = null;
    public Transform visualAidSocket = null;

    [Header(" · Position")]
    public Coord coord = new Coord();
    public short height = 0;

    [Header(" · Details")]
    public ETileType type = ETileType.Ground;

    [Header(" · Debug")]
    [ReadOnly] public Tile forward = null;
    [ReadOnly] public Tile back = null;
    [ReadOnly] public Tile right = null;
    [ReadOnly] public Tile left = null;
    [ReadOnly] public Pawn pawn = null;

    private void Awake() => Each.Add(this);
    private void OnDestroy() => Each.Remove(this);

    public List<Tile> GetAdjacentTiles()
    {
        List<Tile> adjacent = new List<Tile> { forward, back, right, left };
        adjacent.RemoveAll(t => (t == null));
        return adjacent;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        OnTileClicked?.Invoke(this);
    }

    public void SetVisualAid(ETileVisualAid aid)
    {
        var children = visualAidSocket.childCount;

        for (int i = 0; i < children; i++)
            Destroy(visualAidSocket.GetChild(i).gameObject);

        var prefab = tileSO.GetAidPrefab(aid);

        if (prefab)
            Instantiate(prefab, visualAidSocket);
    }

}
