using UnityEngine;

[CreateAssetMenu(fileName = "Class", menuName = "ScriptableObjects/Class", order = 1)]
public class ClassSO : ScriptableObject
{

    [SerializeField] private string _title; public string title => _title;

    [Header(" · Variables")]
    [SerializeField] private int _health; public int health => _health;
    [SerializeField] private int _mana; public int mana => _mana;
    [SerializeField] private int _attack; public int attack => _attack;
    [SerializeField] private int _defence; public int defence => _defence;
    [SerializeField] private int _speed; public int speed => _speed;
    [SerializeField] private int _jump; public int jump => _jump;
    [SerializeField] private int _range; public int range => _range;
    [SerializeField] private int _evasion; public int evasion => _evasion;
    [SerializeField] private int _backstabDmgMultiplier; public int backstabDmgMultiplier => _backstabDmgMultiplier;

    [Header(" · Assignables")]
    private Transform _model; public Transform model => _model;
    private Transform _weapon; public Transform weapon => _weapon;

}
