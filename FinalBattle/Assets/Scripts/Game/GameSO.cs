using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game", menuName = "ScriptableObjects/Game", order = 1)]
public class GameSO : ScriptableObject
{

    [Header(" · Initiative")]
    [SerializeField] private bool _randomInitiative = false; public bool randomInitiative => _randomInitiative;

    [Header(" · Camera")]
    [SerializeField] private float _camLookSpeed = 3; public float camLookSpeed => _camLookSpeed;
    [SerializeField] private float _camMoveSpeed = 3; public float camMoveSpeed => _camMoveSpeed;
    [SerializeField] private float _camDistance = 10; public float camDistance => _camDistance;

    [Header(" · Debug")]
    [ReadOnly] public string currentPawnTitle = "";
    [ReadOnly] public Pawn currentPawn = null;

}
