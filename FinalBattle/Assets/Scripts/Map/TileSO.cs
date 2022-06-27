using TBox;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile", menuName = "ScriptableObjects/Tile", order = 1)]
public class TileSO : GlobalScriptableObject<TileSO>
{

    [Header(" · VisualAids")]
    [SerializeField] private Material _vaAttackEnemy; public Material vaAttackEnemy => _vaAttackEnemy;
    [SerializeField] private Material _vaAttackPlayer; public Material vaAttackPlayer => _vaAttackPlayer;
    [SerializeField] private Material _vaMoveEnemy; public Material vaMoveEnemy => _vaMoveEnemy;
    [SerializeField] private Material _vaMovePlayer; public Material vaMovePlayer => _vaMovePlayer;

    public Material GetAidPrefab(ETileVisualAid aid) => aid switch
    {
        ETileVisualAid.Default => _vaMovePlayer,
        ETileVisualAid.MovePlayer => _vaMovePlayer,
        ETileVisualAid.MoveEnemy => _vaMoveEnemy,
        ETileVisualAid.AttackPlayer => _vaAttackPlayer,
        ETileVisualAid.AttackEnemy => _vaAttackEnemy,
        _ => null,
    };

}
