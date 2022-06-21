using System.Collections.Generic;
using UnityEngine;
using static Logger;

public class Pawn : MonoBehaviour
{

    public ClassSO classSO;

    [Header("Variables")]
    public string title;
    public int health;
    public int mana;
    public int attack;
    public int defence;
    public int speed;

    [Header("Assignables")]
    public Transform weaponSocket;

    [Header("Debug")]
    [ReadOnly] public Tile tile;

    protected virtual void Awake()
    {
        title = classSO.title;
        health = classSO.health;
        mana = classSO.mana;
        attack = classSO.attack;
        defence = classSO.defence;
        speed = classSO.speed;

        if (classSO.weapon && weaponSocket)
            Instantiate(classSO.weapon, weaponSocket);
    }

    protected virtual void Start()
    {
        // TODO: search for closes tile move there, get reference

    }

    protected virtual void MoveTo(in List<Tile> path)
    {

    }

}
