using UnityEngine;

[CreateAssetMenu(fileName = "Class", menuName = "ScriptableObjects/Class", order = 1)]
public class ClassSO : ScriptableObject
{

    public string title;
    public int health;
    public int mana;
    public int attack;
    public int defence;
    public int speed;

}
