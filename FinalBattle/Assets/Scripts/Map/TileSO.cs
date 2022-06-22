using UnityEngine;

[CreateAssetMenu(fileName = "Tile", menuName = "ScriptableObjects/Tile", order = 1)]
public class TileSO : ScriptableObject
{

    [Header(" · VisualAids")]
    [SerializeField] private Transform _vaAttackEnemy; public Transform vaAttackEnemy => _vaAttackEnemy;
    [SerializeField] private Transform _vaAttackPlayer; public Transform vaAttackPlayer => _vaAttackPlayer;
    [SerializeField] private Transform _vaMoveEnemy; public Transform vaMoveEnemy => _vaMoveEnemy;
    [SerializeField] private Transform _vaMovePlayer; public Transform vaMovePlayer => _vaMovePlayer;

    public Transform GetAidPrefab(ETileVisualAid aid) => aid switch
    {
        ETileVisualAid.Default => _vaMovePlayer,
        ETileVisualAid.MovePlayer => _vaMovePlayer,
        ETileVisualAid.MoveEnemy => _vaMoveEnemy,
        ETileVisualAid.AttackPlayer => _vaAttackPlayer,
        ETileVisualAid.AttackEnemy => _vaAttackEnemy,
        _ => null,
    };

}
