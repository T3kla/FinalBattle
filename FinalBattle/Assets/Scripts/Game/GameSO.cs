using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game", menuName = "ScriptableObjects/Game", order = 1)]
public class GameSO : ScriptableObject
{

    [NonSerialized] public List<PawnPlayer> pawnsPlayer = null;
    [NonSerialized] public List<PawnEnemy> pawnsEnemy = null;
    [NonSerialized] public Camera camera = null;

    [Header(" · Initiative")]
    [SerializeField] private bool _randomInitiative = false; public bool randomInitiative => _randomInitiative;

    [Header(" · Camera")]
    [SerializeField] private float _cameraLookSpeed = 3; public float cameraLookSpeed => _cameraLookSpeed;
    [SerializeField] private float _cameraMoveSpeed = 3; public float cameraMoveSpeed => _cameraMoveSpeed;
    [SerializeField] private float _cameraDistance = 10; public float cameraDistance => _cameraDistance;

    [Header(" · Debug")]
    public Field<bool> initialized = null;
    [ReadOnly] public Pawn currentPawn = null;

}
