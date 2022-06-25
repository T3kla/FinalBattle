using TBox;
using UnityEngine;

[CreateAssetMenu(fileName = "Game", menuName = "ScriptableObjects/Game", order = 1)]
public class GameSO : GlobalScriptableObject<GameSO>
{

    [Header(" · Camera")]
    [SerializeField] private float _camLookSpeed = 3; public float camLookSpeed => _camLookSpeed;
    [SerializeField] private float _camMoveSpeed = 3; public float camMoveSpeed => _camMoveSpeed;
    [SerializeField] private float _camDistance = 10; public float camDistance => _camDistance;

    [Header(" · Factions")]
    [SerializeField] private Material _enemyDye = null; public Material enemyDye => _enemyDye;
    [SerializeField] private Material _playerDye = null; public Material playerDye => _playerDye;

    [Header(" · Floating Text")]
    [SerializeField] private FloatingText _floatingText = null; public FloatingText floatingText => _floatingText;
    [SerializeField] private AnimationCurve _ftMovePattern = null; public AnimationCurve ftMovePattern => _ftMovePattern;
    [SerializeField] private AnimationCurve _ftRotationPattern = null; public AnimationCurve ftRotationPattern => _ftRotationPattern;
    [SerializeField] private float _ftMoveStrength = 1f; public float ftMoveStrength => _ftMoveStrength;
    [SerializeField] private float _ftRotationStrength = 1f; public float ftRotationStrength => _ftRotationStrength;
    [SerializeField] private float _ftDuration = 1f; public float ftDuration => _ftDuration;

}
