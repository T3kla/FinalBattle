using UnityEngine;

[CreateAssetMenu(fileName = "Class", menuName = "ScriptableObjects/Class", order = 1)]
public class ClassSO : ScriptableObject
{

    public string title;

    [Header("Variables")]
    public int health;
    public int mana;
    public int attack;
    public int defence;
    public int speed;
    public int jump;

    [Header("Assignables")]
    public Transform weapon;

}
