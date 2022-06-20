using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Class", menuName = "ScriptableObjects/Class", order = 1)]
public class ClassSO : ScriptableObject
{
    private string title; public string Title { get => title; private set => title = value; }
    public int health;
    public int mana;
    public int attack;
    public int defense;
    public int speed;
}
