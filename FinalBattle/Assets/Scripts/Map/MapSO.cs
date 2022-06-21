using System;
using System.Collections.Generic;
using UnityEngine;
using static Logger;

[CreateAssetMenu(fileName = "Map", menuName = "ScriptableObjects/Map", order = 1)]
public class MapSO : ScriptableObject
{

    [NonSerialized] public Dictionary<Coord, Tile> tiles = null;

    [Header(" · Details")]
    [SerializeField] private float _tileSize; public float tileSize => _tileSize;
    [SerializeField] private float _tileHeight; public float tileHeight => _tileHeight;

    [Header(" · Debug")]
    public Field<bool> initialized;

}
