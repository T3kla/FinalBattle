using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game", menuName = "ScriptableObjects/Game", order = 1)]
public class GameSO : ScriptableObject
{

    [NonSerialized] public List<PawnPlayer> pawnsPlayer = null;
    [NonSerialized] public List<PawnEnemy> pawnsEnemy = null;
    [NonSerialized] public Camera camera;

    [Header(" · Details")]
    // [SerializeField] private float _tileHeight; public float tileHeight => _tileHeight;

    [Header(" · Debug")]
    public Field<bool> initialized;
    [ReadOnly] public Pawn currentPawn;

}
