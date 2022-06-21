using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game", menuName = "ScriptableObjects/Game", order = 1)]
public class GameSO : ScriptableObject
{

    [NonSerialized] public List<Pawn> pawns = null;

    // [Header(" · Details")]
    // [SerializeField] private float _tileSize; public float tileSize => _tileSize;
    // [SerializeField] private float _tileHeight; public float tileHeight => _tileHeight;

    [Header(" · Debug")]
    public Field<bool> initialized;

}
