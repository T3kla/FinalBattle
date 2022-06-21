using System.Collections.Generic;
using UnityEngine;
using static Logger;

public class Pawn : MonoBehaviour
{

    public ClassSO classSO;

    public string title;
    public int health;
    public int mana;
    public int attack;
    public int defence;
    public int speed;

    protected virtual void Awake()
    {
        title = classSO.title;
        health = classSO.health;
        mana = classSO.mana;
        attack = classSO.attack;
        defence = classSO.defence;
        speed = classSO.speed;
    }

    protected virtual void MoveTo(in List<Tile> path)
    {

    }

}
