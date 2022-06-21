using UnityEngine;

[CreateAssetMenu(fileName = "Class", menuName = "ScriptableObjects/Class", order = 1)]
public class ClassSO : ScriptableObject
{

    [SerializeField] private string _title; public string title => _title;

    [Header("Variables")]
    [SerializeField] public int _health; public int health => health;
    [SerializeField] public int _mana; public int mana => mana;
    [SerializeField] public int _attack; public int attack => attack;
    [SerializeField] public int _defence; public int defence => defence;
    [SerializeField] public int _speed; public int speed => speed;
    [SerializeField] public int _jump; public int jump => jump;

    [Header("Assignables")]
    public Transform _model; public Transform model => model;
    public Transform _weapon; public Transform weapon => weapon;

}
